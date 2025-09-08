namespace Board.Application.Commands.BoardColumns.UpdateBoardColumn;

	using FluentValidation;

	public class UpdateBoardColumnValidator : AbstractValidator<UpdateBoardColumnCommand>
	{
		public UpdateBoardColumnValidator()
		{
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
			RuleFor(x => x.Description).NotEmpty().MaximumLength(10000);
		}
	} 