using Easy_Password_Validator.Interfaces;

using System.Collections.Generic;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks to see whether a password meets the minimum string length requirements
    /// </summary>
    public class TestLength : IPasswordTest
    {
        /// <summary>
        /// Prepares test for use
        /// </summary>
        /// <param name="passwordRequirements">Object containing current settings</param>
        public TestLength(IPasswordRequirements passwordRequirements)
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
        public IEnumerable<string> BadList { get; set; }

        /// <inheritdoc/>
        public bool TestAndScore(string password)
        {
            // Reset
            FailureMessage = null;
            ScoreModifier = 0;

            // Adjust score
            ScoreModifier = password.Length * 5;

            // Return result
            var pass = password.Length >= Settings.MinLength;

            if (pass == false)
                FailureMessage = $"Password must be at least {Settings.MinLength} characters long";

            return pass;
        }
    }
}
