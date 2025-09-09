using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.BoardItems.UpdateBoardItem;

public class UpdateBoardItemValidator : Validator<UpdateBoardItemRequest>
{
    public UpdateBoardItemValidator()
    {
        RuleFor(i => i.Title).NotNull().NotEmpty().MaximumLength(20);
        RuleFor(i => i.Description).MaximumLength(100);
        RuleFor(i => i.Priority).IsInEnum();
    }
}
