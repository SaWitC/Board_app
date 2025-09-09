using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.BoardColumn.CreateBoardColumn;

public class CreateBoardColumnValidator : Validator<CreateBoardItemRequest>
{
    public CreateBoardColumnValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(10000);
    }
}
