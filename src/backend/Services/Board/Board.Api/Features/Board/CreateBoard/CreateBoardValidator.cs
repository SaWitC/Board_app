using Board.Api.Resources;
using Board.Application.DTOs;
using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.Board.CreateBoard;

public class CreateBoardValidator : Validator<CreateBoardRequest>
{
    public CreateBoardValidator()
    {
        //RuleFor(i => i.Title)
        //    .NotNull().NotEmpty().WithMessage(x => localizer["FieldIsRequired", x.Title])
        //    .MaximumLength(20).WithMessage(x => localizer["MaxLengthExceeded", x.Title, 20]);

        RuleFor(i => i.Title)
            .NotNull().NotEmpty().WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Title)))
            .MaximumLength(20).WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Title), 20));

        RuleFor(i => i.Description).MaximumLength(100)
            .WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Description), 100));

        RuleForEach(i => i.BoardUsers).NotNull().NotEmpty().SetValidator(new BoardUserValidator());
        //TODO ADD CHECK FOR OWNER
        RuleForEach(i => i.BoardColumns).NotNull().NotEmpty().SetValidator(new BoardColumnValidator());
    }
}

public class BoardUserValidator : AbstractValidator<BoardUserDto>
{
    public BoardUserValidator()
    {
        RuleFor(c => c.Role).NotNull().NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Role)));
        RuleFor(c => c.Email).NotNull().NotEmpty().MaximumLength(100)
            .WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Email), 100));
    }
}

public class BoardColumnValidator : AbstractValidator<BoardColumnDto>
{
    public BoardColumnValidator()
    {
        RuleFor(c => c.Title).NotNull().NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Title)))
            .MaximumLength(200).WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Title), 200));
        RuleFor(c => c.Description).NotNull().NotEmpty().WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Description)))
            .MaximumLength(10000).WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Description), 10000));
    }
}
