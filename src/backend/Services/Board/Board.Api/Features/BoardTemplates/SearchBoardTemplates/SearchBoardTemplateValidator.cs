using Board.Api.Resources;
using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.BoardTemplates.SearchBoardTemplates;

public class SearchBoardTemplateValidator : Validator<SearchBoardTemplatesRequest>
{
    public SearchBoardTemplateValidator()
    {
        RuleFor(x => x.SearchTerm)
            .Cascade(CascadeMode.Continue)
            .NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.SearchTerm)))
            .MinimumLength(3);
    }
}


