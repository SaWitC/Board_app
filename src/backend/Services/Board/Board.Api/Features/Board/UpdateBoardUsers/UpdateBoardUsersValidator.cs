using Board.Api.Features.Board.CreateBoard;
using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.Board.UpdateBoardUsers;

public class UpdateBoardUsersValidator : Validator<UpdateBoardUsersRequest>
{
    public UpdateBoardUsersValidator()
    {
        RuleForEach(x => x.BoardUsers)
            .NotNull()
            .NotEmpty()
            .SetValidator(new BoardUserValidator());
    }
}
