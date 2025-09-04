namespace Board.Application.Commands.BoardItems.CreateBoardItem;

	using FluentValidation;

	public class CreateBoardItemValidator : AbstractValidator<CreateBoardItemCommand>
	{
		public CreateBoardItemValidator()
		{
			RuleFor(i => i.Title).NotNull().NotEmpty().MaximumLength(20);
			RuleFor(i => i.Description).MaximumLength(100);
			RuleFor(i => i.Priority).IsInEnum();
		}
	}
