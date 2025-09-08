using FastEndpoints;
using FluentValidation;

namespace Board.Api.Features.Board.UpdateBoard;

public class UpdateBoardValidator : Validator<UpdateBoardRequest>
	{
		public UpdateBoardValidator()
		{
			RuleFor(i => i.Title).NotNull().NotEmpty().MaximumLength(20);
			RuleFor(i => i.Description).MaximumLength(100);
		}
	}
