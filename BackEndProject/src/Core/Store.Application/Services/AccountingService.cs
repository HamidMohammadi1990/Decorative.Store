using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Edition.Application.Contracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Edition.Application.Models.Dtos;
using Edition.Application.Models.Services;
using Edition.Application.Models.Constants;
using Edition.Application.Common.Extensions;
using Edition.Application.Common.Security;
using Edition.Application.Configurations.SMS;
using Edition.Application.Configurations.Email;
using Edition.Application.Common.Caching.Enums;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.Infrastructure;
using Edition.Application.Common.Caching.Abstractions;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Services;

public class AccountingService
    : IAccountingService
{
    private readonly IUnitOfWork uow;
    private readonly ISmsService smsService;
    private readonly IDistributedCache cache;
    private readonly SiteSettings siteSettings;
    private readonly IEmailService emailService;
    private readonly IUserAuthCache userAuthCache;
    private readonly IUserRepository userRepository;
    private readonly IUserSessionService userSessionService;
    private readonly IAuthContextValidator authContextValidator;
    private readonly IPermissionRepository permissionRepository;
    private readonly IRefreshTokenRepository refreshTokenRepository;
    private readonly IOptions<TokenValidationParameters> tokenValidationParameters;
    private readonly IEmailTokenProviderConfiguration emailTokenProviderConfiguration;
    private readonly IPhoneNumberTokenProviderConfiguration phoneNumberTokenProviderConfiguration;

    public AccountingService(
        IUnitOfWork uow,
        ISmsService smsService,
        IDistributedCache cache,
        SiteSettings siteSettings,
        IEmailService emailService,
        IUserAuthCache userAuthCache,
        IUserRepository userRepository,
        IUserSessionService userSessionService,
        IAuthContextValidator authContextValidator,
        IPermissionRepository permissionRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<TokenValidationParameters> tokenValidationParameters,
        IEmailTokenProviderConfiguration emailTokenProviderConfiguration,
        IPhoneNumberTokenProviderConfiguration phoneNumberTokenProviderConfiguration)
    {
        this.uow = uow;
        this.cache = cache;
        this.smsService = smsService;
        this.emailService = emailService;
        this.siteSettings = siteSettings;
        this.userAuthCache = userAuthCache;
        this.userRepository = userRepository;
        this.userSessionService = userSessionService;
        this.authContextValidator = authContextValidator;
        this.permissionRepository = permissionRepository;
        this.refreshTokenRepository = refreshTokenRepository;
        this.tokenValidationParameters = tokenValidationParameters;
        this.emailTokenProviderConfiguration = emailTokenProviderConfiguration;
        this.phoneNumberTokenProviderConfiguration = phoneNumberTokenProviderConfiguration;
    }

    public async Task<OperationResult<LogOutTokenResponseDto>> BlockTokenAsync(CheckTokenRequestDto request)
    {
        var principal = GetPrincipalFromTokenWithoutAlgorithmValidation(request.Token);
        if (principal is null)
            return new LogOutTokenResponseDto(true);

        var expiredDateString = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value;
        var expiredDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiredDateString)).UtcDateTime;
        if (expiredDate < DateTime.UtcNow)
            return new LogOutTokenResponseDto(true);

        var userId = int.Parse(principal.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var jwtId = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        var cacheKey = GetBlockedTokenCacheKey(userId.ToString(), jwtId);
        var cachedToken = await cache.GetAsync<string>(cacheKey, CacheInstanceType.UserTokens);
        if (string.IsNullOrEmpty(cachedToken))
            await cache.SetAsync(cacheKey, request.Token, expiredDate, CacheInstanceType.UserTokens);

        if (TryGetSessionId(principal, out var sessionId))
            await userSessionService.RevokeSessionAsync(sessionId, userId, UserSessionRevokeReason.SignOut);

        var storedToken = await refreshTokenRepository.GetByToken(jwtId);
        if (storedToken is not null)
        {
            storedToken.Invalidate();
            await uow.SaveChangesAsync();
        }

        return new LogOutTokenResponseDto(true);
    }

    public async Task<OperationResult<bool>> IsTokenBlockedAsync(CheckTokenRequestDto request)
    {
        var principal = GetPrincipalFromTokenWithoutAlgorithmValidation(request.Token);
        if (principal is null)
            return false;

        var userId = AuthClaimResolver.GetUserId(principal);
        var jwtId = AuthClaimResolver.GetJwtId(principal);
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(jwtId))
            return false;

        var cacheKey = GetBlockedTokenCacheKey(userId, jwtId);
        return await cache.ExistsAsync(cacheKey, CacheInstanceType.UserTokens);
    }

    public OperationResult<AccessTokenResponse> GenerateTokenAsync(User user, Guid sessionId)
    {
        var jwtId = Guid.NewGuid().ToString();
        var secretKey = Encoding.UTF8.GetBytes(siteSettings.JwtSettings.SecretKey);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);
        var encryptionkey = Encoding.UTF8.GetBytes(siteSettings.JwtSettings.EncryptKey);
        var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = siteSettings.JwtSettings.Issuer,
            Audience = siteSettings.JwtSettings.Audience,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow.AddMinutes(siteSettings.JwtSettings.NotBeforeMinutes),
            Expires = DateTime.UtcNow.AddMinutes(siteSettings.JwtSettings.ExpirationMinutes),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials,
            Subject = new ClaimsIdentity(GenerateClaims(user, sessionId, jwtId))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
        return new AccessTokenResponse(securityToken, jwtId, sessionId);
    }

    public async Task<OperationResult<AccessTokenResponse>> IssueTokenPairAsync(
        User user,
        UserSessionContext sessionContext,
        Guid? sessionId = null,
        CancellationToken cancellationToken = default)
    {
        var effectiveSessionId = sessionId ?? Guid.CreateVersion7();
        var tokenResponse = GenerateTokenAsync(user, effectiveSessionId);
        var jwtId = tokenResponse.Result!.RefreshToken;

        if (sessionId.HasValue)
        {
            var continued = await userSessionService.ContinueSessionAsync(sessionId.Value, user.Id, jwtId, cancellationToken);
            if (continued is null)
                return ErrorModel.Create("SessionExpired");
        }
        else
        {
            await userSessionService.CreateSessionAsync(
                effectiveSessionId,
                user.Id,
                jwtId,
                sessionContext,
                GetRefreshTokenExpirationUtc(),
                cancellationToken);
        }

        var stored = await StoreRefreshTokenAsync(user.Id, jwtId, effectiveSessionId, cancellationToken);
        if (!stored.IsSuccess)
            return OperationResult<AccessTokenResponse>.Fail();

        await userAuthCache.SetSecurityStampAsync(user.Id, user.SecurityStamp, cancellationToken);

        return tokenResponse;
    }

    public async Task<OperationResult<AccessTokenResponse>> RefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var validateToken = GetPrincipalFromToken(token);
        if (validateToken is null)
            return ErrorModel.Create("TokenIsInvalid");

        var expiredDate = validateToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value;
        var expiredDateUtc = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiredDate)).UtcDateTime;
        if (expiredDateUtc > DateTime.UtcNow)
            return ErrorModel.Create("TokenIsNotExpired");

        var storedToken = await refreshTokenRepository.GetByToken(refreshToken);
        if (storedToken is null)
            return ErrorModel.Create("TokenIsInvalid");

        if (storedToken.ExpiredDateOnUtc < DateTime.UtcNow)
            return ErrorModel.Create("TokenIsExpired");

        var jti = validateToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        if (storedToken.JwtId != jti)
            return ErrorModel.Create("TokenIsInvalid");

        var userId = Convert.ToInt32(validateToken.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        if (storedToken.UserId != userId)
            return ErrorModel.Create("TokenIsInvalid");

        if (!TryGetSessionId(validateToken, out var sessionId))
            return ErrorModel.Create("SessionExpired");

        if (!await userSessionService.ValidateSessionAsync(sessionId, userId, jti, cancellationToken))
            return ErrorModel.Create("SessionExpired");

        storedToken.UsedToken();
        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<AccessTokenResponse>();

        var user = await userRepository.FindAsync(userId, cancellationToken);
        if (user is null)
            return ErrorModel.Create("UserNotFound");

        return await IssueTokenPairAsync(
            user,
            new UserSessionContext(null, null, null, DeviceType.Unknown, OperatingSystemType.Unknown),
            sessionId,
            cancellationToken);
    }

    public async Task<NotificationSendResult> SendActivationCodeToPhoneAsync(string phoneNumber)
    {
        var smsSendResult = new NotificationSendResult();
        var cacheKey = $"{phoneNumber}-sent-sms";
        if (await cache.ExistsAsync(cacheKey, CacheInstanceType.Default))
        {
            smsSendResult.AlreadySend = true;
            smsSendResult.SuccessSent = true;

            var cacheTime = cache.TimeToLive(cacheKey, CacheInstanceType.Default);
            var cacheTimeTotalSeconds = Math.Round(cacheTime!.Value.TotalSeconds);
            smsSendResult.Message = ErrorModel.Create("TheSmsHasAleardyBeenSentToYou", cacheTimeTotalSeconds);
        }
        else
        {
            var message = await smsService.GenerateTokenAsync(phoneNumber);
            var sendingSmsStatus = smsService.Send(phoneNumber, message);
            if (sendingSmsStatus)
                await cache.SetAsync(cacheKey, string.Empty, new TimeSpan(0, 0, 0, phoneNumberTokenProviderConfiguration.Duration), CacheInstanceType.Default);
            smsSendResult.SuccessSent = sendingSmsStatus;
            smsSendResult.Message = sendingSmsStatus
                                    ? ErrorModel.Create("VerificationCodeSentSuccessfully")
                                    : ErrorModel.Create("VerificationCodeCouldNotBeSendSuccessfully");
        }
        return smsSendResult;
    }

    public async Task<NotificationSendResult> SendActivationCodeToEmailAsync(string email)
    {
        var emailSendResult = new NotificationSendResult();
        var cacheKey = $"{email}-sent-email";
        if (await cache.ExistsAsync(cacheKey, CacheInstanceType.Default))
        {
            emailSendResult.AlreadySend = true;
            emailSendResult.SuccessSent = true;

            var cacheTime = cache.TimeToLive(cacheKey, CacheInstanceType.Default);
            var cacheTimeTotalSeconds = Math.Round(cacheTime!.Value.TotalSeconds);
            emailSendResult.Message = ErrorModel.Create("TheEmailHasAleardyBeenSentToYou", cacheTimeTotalSeconds);
        }
        else
        {
            var message = await emailService.GenerateTokenAsync(email);
            var sendingEmailStatus = emailService.Send(email, message, "کد تایید  - سازمان چاپ");
            if (sendingEmailStatus)
                await cache.SetAsync(cacheKey, string.Empty, new TimeSpan(0, 0, 0, emailTokenProviderConfiguration.Duration), CacheInstanceType.Default);
            emailSendResult.SuccessSent = sendingEmailStatus;
            emailSendResult.Message = sendingEmailStatus
                                    ? ErrorModel.Create("VerificationCodeSentSuccessfully")
                                    : ErrorModel.Create("VerificationCodeCouldNotBeSendSuccessfully");
        }
        return emailSendResult;
    }

    public List<ForgetPasswordOptionDto> GetForgetPasswordOptionsByUser(User user)
    {
        var optionItems = new List<ForgetPasswordOptionDto>();

        if (user.PhoneNumberConfirmed && !string.IsNullOrEmpty(user.PhoneNumber) && user.PhoneNumber.Length >= 6)
            optionItems.Add(new ForgetPasswordOptionDto
            (
                $"{user.PhoneNumber[..4]}******{user.PhoneNumber[^2..]}",
                ForgetPasswordOptionType.Message
            ));

        if (user.EmailConfirmed && !string.IsNullOrEmpty(user.Email))
        {
            var atIndex = user.Email.IndexOf('@');
            if (atIndex > 1)
            {
                optionItems.Add(new ForgetPasswordOptionDto
                (
                    $"{user.Email[..2]}******{user.Email[atIndex..]}",
                    ForgetPasswordOptionType.Email
                ));
            }
        }

        return optionItems;
    }

    public async Task<bool> HasPermissionAsync(int userId, PermissionType permissionType)
        => await permissionRepository.HasPermissionAsync(userId, permissionType);

    private async Task<OperationResult> StoreRefreshTokenAsync(int userId, string jwtId, Guid sessionId, CancellationToken cancellationToken)
    {
        var refreshToken = RefreshToken.Create(userId, jwtId, GetRefreshTokenExpirationUtc(), sessionId);
        refreshTokenRepository.Add(refreshToken);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        return saveChangesResult.IsSuccess
            ? OperationResult.Success()
            : saveChangesResult;
    }

    private DateTime GetRefreshTokenExpirationUtc()
        => DateTime.UtcNow.AddMinutes(siteSettings.JwtSettings.RefreshTokenExpirationMinutes);

    private static List<Claim> GenerateClaims(User user, Guid sessionId, string jwtId)
    {
        var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;
        return
        [
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, jwtId),
            new(AuthClaimTypes.SessionId, sessionId.ToString()),
            new(securityStampClaimType, user.SecurityStamp)
        ];
    }

    private static bool TryGetSessionId(ClaimsPrincipal principal, out Guid sessionId)
    {
        sessionId = Guid.Empty;
        var claim = principal.Claims.FirstOrDefault(x => x.Type == AuthClaimTypes.SessionId)?.Value;
        return !string.IsNullOrWhiteSpace(claim) && Guid.TryParse(claim, out sessionId);
    }

    private static bool IsJwtValidSecurityAlgorithm(SecurityToken securityToken)
    {
        try
        {
            return securityToken is JwtSecurityToken jwtSecurityToken &&
               jwtSecurityToken
               .Header.Alg
               .Equals(SecurityAlgorithms.HmacSha256,
               StringComparison.InvariantCultureIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    private ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = tokenValidationParameters.Value.CloneWithLifetimeValidation(validateLifetime: false);
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validationToken);
            if (!IsJwtValidSecurityAlgorithm(validationToken))
                return null;
            return principal;
        }
        catch
        {
            return null;
        }
    }

    private ClaimsPrincipal? GetPrincipalFromTokenWithoutAlgorithmValidation(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = tokenValidationParameters.Value.CloneWithLifetimeValidation(validateLifetime: false);
            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }
        catch
        {
            return null;
        }
    }

    private static string GetBlockedTokenCacheKey(string userId, string jwtId)
        => $"BlockedToken|{userId}|{jwtId}";
}
