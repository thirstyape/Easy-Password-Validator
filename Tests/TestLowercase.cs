using Easy_Password_Validator.Interfaces;

using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks to see whether the password contains any lowercase letters
    /// </summary>
    public class TestLowercase : IPasswordTest
    {
        public TestLowercase(IPasswordRequirements passwordRequirements)
        {
            Settings = passwordRequirements;
        }

        public int ScoreModifier { get; set; }
        public string FailureMessage { get; set; }
        public IPasswordRequirements Settings { get; set; }
        public IEnumerable<string> BadList { get; set; }

        public bool TestAndScore(string password, bool isL33t)
        {
            // Check for lowercase
            var lowercases = password.Count(char.IsLower);

            // Adjust score
            if (isL33t == false)
                ScoreModifier = lowercases * 2;

            // Return result
            var pass = Settings.RequireLowercase == false || lowercases > 0;

            if (pass == false)
                FailureMessage = "Must have at least one lowercase letter in password";

            return pass;
        }
    }
}
