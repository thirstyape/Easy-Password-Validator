using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Properties;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
	/// <summary>
	/// Checks to see whether the password contains any digits
	/// </summary>
	public class TestDigit : IPasswordTest
    {
        /// <summary>
        /// Prepares test for use
        /// </summary>
        /// <param name="passwordRequirements">Object containing current settings</param>
        public TestDigit(IPasswordRequirements passwordRequirements)
        {
            Settings = passwordRequirements;
        }

        /// <inheritdoc/>
        public int ScoreModifier { get; set; }

        /// <inheritdoc/>
        public string FailureMessage { get; set; }

        /// <inheritdoc/>
        public IPasswordRequirements Settings { get; set; }

        /// <inheritdoc/>
        public bool TestAndScore(string password)
        {
            // Reset
            FailureMessage = null;
            ScoreModifier = 0;

            // Check for inactive
            if (Settings.UseDigit == false)
                return true;

            // Check for digits
            var digits = password.Count(char.IsDigit);

            // Adjust score
            ScoreModifier = digits * 3;

            // Return result
            var pass = Settings.RequireDigit == false || digits > 0;

            if (pass == false)
                FailureMessage = Resources.FailedDigit;

            return pass;
        }
    }
}
