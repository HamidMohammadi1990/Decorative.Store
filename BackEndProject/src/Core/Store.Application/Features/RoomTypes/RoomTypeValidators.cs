using FluentValidation;
using Edition.Application.Common.Validation;
using Edition.Application.Features.RoomTypes.Queries;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.RoomTypes.Commands;

public class CreateRoomTypeValidator : AbstractValidator<CreateRoomTypeRequest>
{
    public CreateRoomTypeValidator(
        IRoomTypeRepository repository,
        ILanguageRepository languageRepository)
    {
        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (languageId, cancellationToken) =>
                await languageRepository.FindAsync(languageId, cancellationToken) is { IsActive: true })
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(32)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (code, cancellationToken) =>
                !await repository.ExistsCodeAsync(code.Trim(), cancellationToken: cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired)
            .MaximumLength(100)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.ImageFileName)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(120)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0)
            .WithMessage(MessageKeys.InvalidRequest);
    }
}

public class UpdateRoomTypeValidator : AbstractValidator<UpdateRoomTypeRequest>
{
    public UpdateRoomTypeValidator(
        IRoomTypeRepository repository,
        ILanguageRepository languageRepository)
    {
        RuleFor(x => x.Id).MustBeValidEntityId();

        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (languageId, cancellationToken) =>
                await languageRepository.FindAsync(languageId, cancellationToken) is { IsActive: true })
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(32)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (request, code, cancellationToken) =>
                !await repository.ExistsCodeAsync(code.Trim(), request.Id, cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired)
            .MaximumLength(100)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.ImageFileName)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(120)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0)
            .WithMessage(MessageKeys.InvalidRequest);
    }
}

public class DeleteRoomTypeValidator : AbstractValidator<DeleteRoomTypeRequest>
{
    public DeleteRoomTypeValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}

public class GetAllRoomTypeValidator : AbstractValidator<GetAllRoomTypeRequest>
{
    public GetAllRoomTypeValidator()
    {
        RuleFor(x => x.Pagination).NotNull();
    }
}

public class GetRoomTypeValidator : AbstractValidator<GetRoomTypeRequest>
{
    public GetRoomTypeValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
        RuleFor(x => x.LanguageId).GreaterThan(0).WithMessage(MessageKeys.InvalidRequest);
    }
}

public class ListRoomTypesValidator : AbstractValidator<ListRoomTypesRequest>
{
    public ListRoomTypesValidator()
    {
        RuleFor(x => x.LanguageId).GreaterThan(0).WithMessage(MessageKeys.InvalidRequest);
    }
}
