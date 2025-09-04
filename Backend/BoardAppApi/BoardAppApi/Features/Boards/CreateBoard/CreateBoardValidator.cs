using FluentValidation;

namespace BoardAppApi.Features.Boards.CreateBoard;

public class CreateBoardValidator : AbstractValidator<CreateBoardCommand>
{
    public CreateBoardValidator()
    {
        RuleFor(i => i.Title).NotNull().NotEmpty().MaximumLength(20);
        RuleFor(i => i.Description).MaximumLength(100);
    }
}


