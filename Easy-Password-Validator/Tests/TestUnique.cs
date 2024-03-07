using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Properties;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
	/// <summary>
	/// Checks to see whether a password meets the minimum unique characters requirements
	/// </summary>
	public class TestUnique : IPasswordTest
    {
        /// <summary>
        /// Prepares test for use
        /// </summary>
        /// <param name="passwordRequirements">Object containing current settings</param>
        public TestUnique(IPasswordRequirements passwordRequirements)
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
            if (Settings.UseUnique == false)
                return true;

            // Count unique chars
            var unique = password.GroupBy(x => x).Count();

            // Adjust score
            ScoreModifier = unique * 4;

            // Return result
            var pass = unique >= Settings.MinUniqueCharacters;

            if (pass == false)
                FailureMessage = string.Format(Resources.FailedUnique, Settings.MinUniqueCharacters);

            return pass;
        }
    }
}
