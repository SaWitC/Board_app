using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.BoardItems.UpdateBoardItem;

public class UpdateBoardItemValidator : Validator<UpdateBoardItemRequest>
{
    public UpdateBoardItemValidator()
    {
        RuleFor(i => i.Title).NotNull().NotEmpty().MaximumLength(500);
        RuleFor(i => i.Description).MaximumLength(1000000);
        RuleFor(i => i.Priority).IsInEnum();
    }
}
