using System.Data;
using FluentValidation;

namespace BoardAppApi.Features.BoardItems.CreateBoard;

public class CreateBoardItemValidator : AbstractValidator<CreateBoardItemCommand>
{
    public CreateBoardItemValidator()
    {
        RuleFor(i => i.Title).NotNull().NotEmpty().MaximumLength(20);
        RuleFor(i => i.Description).MaximumLength(100);
        RuleFor(i => i.Priority).IsInEnum();
    }
}


