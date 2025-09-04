namespace Board.Application.Commands.UpdateBoard;

	using FluentValidation;

	public class UpdateBoardValidator : AbstractValidator<UpdateBoardCommand>
	{
		public UpdateBoardValidator()
		{
			RuleFor(i => i.Title).NotNull().NotEmpty().MaximumLength(20);
			RuleFor(i => i.Description).MaximumLength(100);
		}
	}
