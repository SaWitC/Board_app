using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.BoardTemplates.SearchBoardTemplates;

public class SearchBoardTemplateValidator : Validator<SearchBoardTemplatesRequest>
{
    public SearchBoardTemplateValidator()
    {
        RuleFor(x => x.SearchTerm)
            .NotEmpty()
            .MinimumLength(3);
    }
}


