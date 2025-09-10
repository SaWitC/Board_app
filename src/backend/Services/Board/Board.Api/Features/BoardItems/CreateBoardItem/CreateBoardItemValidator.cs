using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.BoardItems.CreateBoardItem;

public class CreateBoardItemValidator : Validator<CreateBoardItemRequest>
{
    public CreateBoardItemValidator()
    {
        RuleFor(i => i.Title).NotNull().NotEmpty().MaximumLength(500);
        RuleFor(i => i.Description).MaximumLength(1000000);
        RuleFor(i => i.Priority).IsInEnum();
    }
}
