using Board.Api.Resources;
using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.BoardColumn.UpdateBoardColumn;

public class UpdateBoardColumnValidator : Validator<UpdateBoardColumnRequest>
{
    public UpdateBoardColumnValidator()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Continue)
            .NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Id)));
        RuleFor(x => x.Title)
            .Cascade(CascadeMode.Continue)
            .NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Title)))
            .MaximumLength(200)
            .WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Title), 200));
        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Continue)
            .NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Description)))
            .MaximumLength(10000)
            .WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Description), 10000));
    }
}
