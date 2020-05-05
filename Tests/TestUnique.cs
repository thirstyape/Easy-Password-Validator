using Easy_Password_Validator.Interfaces;

using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks to see whether a password meets the minimum unique characters requirements
    /// </summary>
    public class TestUnique : IPasswordTest
    {
        public TestUnique(IPasswordRequirements passwordRequirements)
        {
            Settings = passwordRequirements;
        }

        public int ScoreModifier { get; set; }
        public string FailureMessage { get; set; }
        public IPasswordRequirements Settings { get; set; }
        public IEnumerable<string> BadList { get; set; }

        public bool TestAndScore(string password, bool isL33t)
        {
            // Count unique chars
            var unique = password.GroupBy(x => x).Count();

            // Adjust score
            if (isL33t == false)
                ScoreModifier = unique * 2;

            // Return result
            var pass = unique >= Settings.MinUniqueCharacters;

            if (pass == false)
                FailureMessage = $"Must have at least {Settings.MinUniqueCharacters} unique characters in password";

            return pass;
        }
    }
}
