using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.BoardColumn.UpdateBoardColumn;

public class UpdateBoardColumnValidator : Validator<UpdateBoardColumnRequest>
{
    public UpdateBoardColumnValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(10000);
    }
}
