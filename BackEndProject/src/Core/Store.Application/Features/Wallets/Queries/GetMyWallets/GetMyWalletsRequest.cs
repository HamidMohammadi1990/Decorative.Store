using Store.Common.Models;

namespace Edition.Application.Features.Wallets.Queries;

public record GetMyWalletsRequest : IRequest<OperationResult<List<GetMyWalletResponse>>>;
