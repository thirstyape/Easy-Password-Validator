using Easy_Password_Validator.Interfaces;

using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks to see whether the password contains any uppercase letters
    /// </summary>
    public class TestUppercase : IPasswordTest
    {
        public TestUppercase(IPasswordRequirements passwordRequirements)
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

            // Check for uppercase
            var uppercases = password.Count(char.IsUpper);

            // Adjust score
            ScoreModifier = uppercases;

            // Return result
            var pass = Settings.RequireUppercase == false || uppercases > 0;

            if (pass == false)
                FailureMessage = "Must have at least one uppercase letter in password";

            return pass;
        }
    }
}
