using Board.Api.Resources;
using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.BoardItems.UpdateBoardItem;

public class UpdateBoardItemValidator : Validator<UpdateBoardItemRequest>
{
    public UpdateBoardItemValidator()
    {
        RuleFor(i => i.Title)
            .Cascade(CascadeMode.Continue)
            .NotNull().NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Title)))
            .MaximumLength(500)
            .WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Title), 500));
        RuleFor(i => i.Description)
            .Cascade(CascadeMode.Continue)
            .MaximumLength(1000000)
            .WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Description), 1000000));
        RuleFor(i => i.Priority)
            .Cascade(CascadeMode.Continue)
            .IsInEnum();
        RuleFor(i => i.TaskType)
            .Cascade(CascadeMode.Continue)
            .IsInEnum();
        RuleFor(i => i.BoardColumnId)
            .Cascade(CascadeMode.Continue)
            .NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.BoardColumnId)));
    }
}
