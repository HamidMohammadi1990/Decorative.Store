using Edition.Application.Contracts;
using Store.Common.Models;

namespace Edition.Application.Features.RefreshTokens.Commands;

public class GenerateRefreshTokenHandler
    (IAccountingService accountingService)
    : IRequestHandler<GenerateRefreshTokenRequest, OperationResult<GenerateRefreshTokenResponse>>
{
    public async Task<OperationResult<GenerateRefreshTokenResponse>> Handle(GenerateRefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var refreshTokenResponse = await accountingService.RefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
        if (!refreshTokenResponse.IsSuccess)
            return refreshTokenResponse.Messages;

        var result = new GenerateRefreshTokenResponse
        {
            ExpiresIn = refreshTokenResponse.Result!.ExpiresIn,
            TokenType = refreshTokenResponse.Result!.TokenType,
            AccessToken = refreshTokenResponse.Result!.AccessToken,
            RefreshToken = refreshTokenResponse.Result.RefreshToken,
            SessionId = refreshTokenResponse.Result.SessionId
        };
        return result;
    }
}