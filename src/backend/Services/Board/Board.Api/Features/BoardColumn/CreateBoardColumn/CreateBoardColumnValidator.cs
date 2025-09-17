using Board.Api.Resources;
using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.BoardColumn.CreateBoardColumn;

public class CreateBoardColumnValidator : Validator<CreateBoardItemRequest>
{
    public CreateBoardColumnValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Title)))
            .MaximumLength(200)
            .WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Title), 200));
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Description)))
            .MaximumLength(10000)
            .WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Description), 10000));
    }
}
