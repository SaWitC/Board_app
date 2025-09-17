using Board.Api.Resources;
using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.BoardItems.UpdateBoardItem;

public class UpdateBoardItemValidator : Validator<UpdateBoardItemRequest>
{
    public UpdateBoardItemValidator()
    {
        RuleFor(i => i.Title)
            .NotNull().NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Title)))
            .MaximumLength(500)
            .WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Title), 500));
        RuleFor(i => i.Description)
            .MaximumLength(1000000)
            .WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Description), 1000000));
        RuleFor(i => i.Priority)
            .IsInEnum();
        RuleFor(i => i.TaskType)
            .IsInEnum();
        RuleFor(i => i.BoardColumnId)
            .NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.BoardColumnId)));
    }
}
