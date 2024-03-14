using easy_blazor_bulma;
using Easy_Password_Validator;
using Microsoft.AspNetCore.Components;

namespace Easy_Password_Validator_GUI.Components.Pages;

public partial class Home : ComponentBase
{
	private readonly PageModel InputModel = new();
	private readonly AnnotatedPasswordRequirements Settings = new();
	private PasswordValidatorService? PasswordValidator;

	/// <inheritdoc/>
	protected override void OnInitialized()
	{
		PasswordValidator = new(Settings);
	}

	private string PasswordMeterCssClass
	{
		get
		{
			var css = "is-medium mt-2";

            if (InputModel.CurrentScore == 0 && string.IsNullOrWhiteSpace(InputModel.CurrentPassword))
                css += ' ' + BulmaColorHelper.GetColorCss(BulmaColors.Default);
            else if (string.IsNullOrWhiteSpace(InputModel.PasswordFailedMessage) == false || InputModel.CurrentScore < Settings.MinScore)
				css += ' ' + BulmaColorHelper.GetColorCss(BulmaColors.Red);
			else if (InputModel.CurrentScore >= Settings.MinScore && InputModel.CurrentScore < (int)Math.Round(Settings.MinScore * 1.5))
				css += ' ' + BulmaColorHelper.GetColorCss(BulmaColors.Yellow);
			else if (InputModel.CurrentScore >= (int)Math.Round(Settings.MinScore * 1.5) && InputModel.CurrentScore < (Settings.MinScore * 2))
				css += ' ' + BulmaColorHelper.GetColorCss(BulmaColors.Green);
			else
				css += ' ' + BulmaColorHelper.GetColorCss(BulmaColors.Blue);

			return css;
		}
	}

	private string ScoreCssClass
	{
		get
		{
			var css = "title";

			if (InputModel.CurrentScore == 0 && string.IsNullOrWhiteSpace(InputModel.CurrentPassword))
                css += ' ' + BulmaColorHelper.GetTextCss(BulmaColors.Default);
            else if (string.IsNullOrWhiteSpace(InputModel.PasswordFailedMessage) == false || InputModel.CurrentScore < Settings.MinScore)
                css += ' ' + BulmaColorHelper.GetTextCss(BulmaColors.Red);
            else if (InputModel.CurrentScore >= Settings.MinScore && InputModel.CurrentScore < (int)Math.Round(Settings.MinScore * 1.5))
                css += ' ' + BulmaColorHelper.GetTextCss(BulmaColors.Yellow);
            else if (InputModel.CurrentScore >= (int)Math.Round(Settings.MinScore * 1.5) && InputModel.CurrentScore < (Settings.MinScore * 2))
                css += ' ' + BulmaColorHelper.GetTextCss(BulmaColors.Green);
            else
                css += ' ' + BulmaColorHelper.GetTextCss(BulmaColors.Blue);

            return css;
		}
	}

	private void ValidatePassword()
	{
		if (PasswordValidator == null || string.IsNullOrWhiteSpace(InputModel.CurrentPassword))
		{
			InputModel.PasswordFailedMessage = null;
			InputModel.CurrentScore = 0;

			return;
		}
		else
		{
			PasswordValidator.UpdatePasswordRequirements(Settings);

			InputModel.PasswordFailedMessage = PasswordValidator.TestAndScore(InputModel.CurrentPassword) == false ?
				string.Join(' ', PasswordValidator.FailureMessages) :
				null;

			InputModel.CurrentScore = PasswordValidator.Score;
		}
	}

	private class PageModel
	{
		public string? CurrentPassword { get; set; }
		public int CurrentScore { get; set; }
		public string? PasswordFailedMessage { get; set; }
	}
}
