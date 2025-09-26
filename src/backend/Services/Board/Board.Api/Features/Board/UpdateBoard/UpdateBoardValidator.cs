using Board.Api.Features.Board.CreateBoard;
using Board.Api.Resources;
using Board.Application.DTOs;
using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.Board.UpdateBoard;

public class UpdateBoardValidator : Validator<UpdateBoardRequest>
{
    public UpdateBoardValidator()
    {
        RuleFor(i => i.Title).NotNull().NotEmpty()
            .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Title)))
            .MaximumLength(200).WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Title), 200));

        RuleFor(i => i.Description).MaximumLength(100000)
            .WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Description), 100000));

        RuleForEach(i => i.BoardUsers).NotNull().NotEmpty().SetValidator(new BoardUserValidator());
        RuleForEach(i => i.BoardColumns).NotNull().NotEmpty().SetValidator(new UpdateBoardColumnValidator());
    }

    public class UpdateBoardColumnValidator : AbstractValidator<BoardColumnDto>
    {
        public UpdateBoardColumnValidator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty().WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Id)));
            RuleFor(c => c.Title).NotNull().NotEmpty()
                .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Title)))
                .MaximumLength(200).WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Title), 200));
            RuleFor(c => c.Description).NotNull().NotEmpty()
                .WithMessage(x => string.Format(SharedResources.FieldIsRequired, nameof(x.Description)))
                .MaximumLength(10000).WithMessage(x => string.Format(SharedResources.MaxLengthExceeded, nameof(x.Description), 10000));
        }
    }
}
