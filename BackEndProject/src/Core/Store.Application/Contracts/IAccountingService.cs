using Edition.Application.Models.Dtos;
using Edition.Application.Models.Services;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Entities;

namespace Edition.Application.Contracts;

public interface IAccountingService
{
    Task<OperationResult<LogOutTokenResponseDto>> BlockTokenAsync(CheckTokenRequestDto request);
    Task<OperationResult<bool>> IsTokenBlockedAsync(CheckTokenRequestDto request);
    OperationResult<AccessTokenResponse> GenerateTokenAsync(User user, Guid sessionId);
    Task<OperationResult<AccessTokenResponse>> IssueTokenPairAsync(User user, UserSessionContext sessionContext, Guid? sessionId = null, CancellationToken cancellationToken = default);
    Task<OperationResult<AccessTokenResponse>> RefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<NotificationSendResult> SendActivationCodeToPhoneAsync(string phoneNumber);
    Task<NotificationSendResult> SendActivationCodeToEmailAsync(string email);
    List<ForgetPasswordOptionDto> GetForgetPasswordOptionsByUser(User user);
    Task<bool> HasPermissionAsync(int userId, PermissionType permissionType);
}
