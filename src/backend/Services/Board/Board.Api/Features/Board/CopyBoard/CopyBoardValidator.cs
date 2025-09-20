using Board.Api.Resources;
using Board.Application.DTOs;
using FluentValidation;

namespace Board.Api.Features.Board.CopyBoard;

public sealed class CopyBoardValidator : AbstractValidator<CopyBoardRequest>
{
    public CopyBoardValidator()
    {
        RuleFor(i => i.Title)
            .Cascade(CascadeMode.Continue)
            .NotNull().NotEmpty().WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Title)))
            .MaximumLength(200).WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Title), 200));

        RuleFor(i => i.Description)
            .Cascade(CascadeMode.Continue)
            .MaximumLength(100000)
            .WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Description), 100000));

        RuleFor(i => i.TemplateId)
            .Cascade(CascadeMode.Continue)
            .NotNull().NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.TemplateId)));

        RuleForEach(i => i.BoardUsers)
            .Cascade(CascadeMode.Continue)
            .NotNull().NotEmpty().SetValidator(new BoardUserValidator());
    }
}

public class BoardUserValidator : AbstractValidator<BoardUserDto>
{
    public BoardUserValidator()
    {
        RuleFor(c => c.Role)
            .Cascade(CascadeMode.Continue)
            .NotNull().NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Role)));
        RuleFor(c => c.Email)
            .Cascade(CascadeMode.Continue)
            .NotNull().NotEmpty().MaximumLength(100)
            .WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Email), 100));
    }
}
