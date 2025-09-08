using Board.Application.DTOs;
using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.Board.CreateBoard;

public class CreateBoardValidator : Validator<CreateBoardRequest>
{
    public CreateBoardValidator()
    {
        RuleFor(i => i.Title).NotNull().NotEmpty().MaximumLength(20);
        RuleFor(i => i.Description).MaximumLength(100);

        RuleForEach(i => i.BoardColumns).SetValidator(new BoardColumnValidator());
    }
}

public class BoardColumnValidator : AbstractValidator<BoardColumnDto>
{
    public BoardColumnValidator()
    {
        RuleFor(c => c.Title).NotNull().NotEmpty().MaximumLength(200);
        RuleFor(c => c.Description).NotNull().NotEmpty().MaximumLength(10000);
    }
}
