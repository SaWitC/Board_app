using Board.Api.Features.Board.CreateBoard;
using Board.Application.DTOs;
using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.Board.UpdateBoard;

public class UpdateBoardValidator : Validator<UpdateBoardRequest>
{
    public UpdateBoardValidator()
    {
        RuleFor(i => i.Title).NotNull().NotEmpty().MaximumLength(20);
        RuleFor(i => i.Description).MaximumLength(100);
        RuleForEach(i => i.BoardUsers).NotNull().NotEmpty().SetValidator(new BoardUserValidator());
        RuleForEach(i => i.BoardColumns).NotNull().NotEmpty().SetValidator(new UpdateBoardColumnValidator());
    }

    public class UpdateBoardColumnValidator : AbstractValidator<BoardColumnDto>
    {
        public UpdateBoardColumnValidator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty();
            RuleFor(c => c.Title).NotNull().NotEmpty().MaximumLength(200);
            RuleFor(c => c.Description).NotNull().NotEmpty().MaximumLength(10000);
        }
    }
}
