using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.BoardTemplates.CreateBoardTemplate;

public class CreateBoardTemplateValidator : Validator<CreateBoardTemplateRequest>
{
    public CreateBoardTemplateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(10000);
        RuleFor(x => x.BoardId).NotEmpty();
    }
}


