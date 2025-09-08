namespace Board.Application.Commands.BoardColumns.CreateBoardColumn;

	using FluentValidation;

	public class CreateBoardColumnValidator : AbstractValidator<CreateBoardColumnCommand>
	{
		public CreateBoardColumnValidator()
		{
			RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
			RuleFor(x => x.Description).NotEmpty().MaximumLength(10000);
		}
	} 