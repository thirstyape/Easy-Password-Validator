using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Properties;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
	/// <summary>
	/// Checks to see whether the password contains any uppercase letters
	/// </summary>
	public class TestUppercase : IPasswordTest
    {
        /// <summary>
        /// Prepares test for use
        /// </summary>
        /// <param name="passwordRequirements">Object containing current settings</param>
        public TestUppercase(IPasswordRequirements passwordRequirements)
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

            // Check for uppercase
            var uppercases = password.Count(char.IsUpper);

            // Adjust score
            ScoreModifier = uppercases;

            // Return result
            var pass = Settings.RequireUppercase == false || uppercases > 0;

            if (pass == false)
                FailureMessage = Resources.FailedUppercase;

            return pass;
        }
    }
}
