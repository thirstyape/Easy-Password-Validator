using Easy_Password_Validator.Interfaces;

using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks to see whether the password contains any digits
    /// </summary>
    public class TestDigit : IPasswordTest
    {
        public TestDigit(IPasswordRequirements passwordRequirements)
        {
            Settings = passwordRequirements;
        }

        public int ScoreModifier { get; set; }
        public string FailureMessage { get; set; }
        public IPasswordRequirements Settings { get; set; }
        public IEnumerable<string> BadList { get; set; }

        public bool TestAndScore(string password)
        {
            // Reset
            FailureMessage = null;
            ScoreModifier = 0;

            // Check for digits
            var digits = password.Count(char.IsDigit);

            // Adjust score
            ScoreModifier = digits * 3;

            // Return result
            var pass = Settings.RequireDigit == false || digits > 0;

            if (pass == false)
                FailureMessage = "Must have at least one digit in password";

            return pass;
        }
    }
}
