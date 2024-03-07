using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Properties;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
	/// <summary>
	/// Checks to see whether the password contains any punctuation marks
	/// </summary>
	public class TestPunctuation : IPasswordTest
    {
        /// <summary>
        /// Prepares test for use
        /// </summary>
        /// <param name="passwordRequirements">Object containing current settings</param>
        public TestPunctuation(IPasswordRequirements passwordRequirements)
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

            // Check for punctuation
            var punctuations = password.Count(char.IsPunctuation);

            // Adjust score
            ScoreModifier = punctuations * 5;

            // Return result
            var pass = Settings.RequirePunctuation == false || punctuations > 0;

            if (pass == false)
                FailureMessage = Resources.FailedPunctuation;

            return pass;
        }
    }
}
