using FluentValidation;

namespace BoardAppApi.Features.BoardItems.UpdateBoardItem;

public class UpdateBoardItemValidator : AbstractValidator<UpdateBoardItemCommand>
{
    public UpdateBoardItemValidator()
    {
        RuleFor(i => i.Title).NotNull().NotEmpty().MaximumLength(20);
        RuleFor(i => i.Description).MaximumLength(100);
        RuleFor(i => i.Priority).IsInEnum();
    }
}


