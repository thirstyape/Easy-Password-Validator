using Easy_Password_Validator.Interfaces;

using System.Collections.Generic;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks to see whether a password meets the minimum string length requirements
    /// </summary>
    public class TestLength : IPasswordTest
    {
        public TestLength(IPasswordRequirements passwordRequirements)
        {
            Settings = passwordRequirements;
        }

        public int ScoreModifier { get; set; }
        public string FailureMessage { get; set; }
        public IPasswordRequirements Settings { get; set; }
        public IEnumerable<string> BadList { get; set; }

        public bool TestAndScore(string password, bool isL33t)
        {
            // Reset
            ScoreModifier = 0;

            // Adjust score
            if (isL33t == false)
                ScoreModifier = password.Length * 3;

            // Return result
            var pass = password.Length >= Settings.MinLength;

            if (pass == false)
                FailureMessage = $"Password must be at least {Settings.MinLength} characters long";

            return pass;
        }
    }
}
