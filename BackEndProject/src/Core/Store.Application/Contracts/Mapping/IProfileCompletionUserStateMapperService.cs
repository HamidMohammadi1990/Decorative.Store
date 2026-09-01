using Edition.Application.Features.ProfileCompletion.Queries;
using Store.Domain.Dtos.ProfileCompletion;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IProfileCompletionUserStateMapperService : IMapper
{
    GetAllProfileCompletionUserStateRequestDto Map(GetAllProfileCompletionUserStateRequest model);
    PagedResult<GetAllProfileCompletionUserStateResponse> Map(PagedResult<GetAllProfileCompletionUserStateResponseDto> model);
    GetProfileCompletionUserStateResponse Map(ProfileCompletionUserState model);
}
