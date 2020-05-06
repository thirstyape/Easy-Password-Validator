using Easy_Password_Validator.Interfaces;

using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks to see whether the password contains any punctuation marks
    /// </summary>
    public class TestPunctuation : IPasswordTest
    {
        public TestPunctuation(IPasswordRequirements passwordRequirements)
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

            // Check for punctuation
            var punctuations = password.Count(char.IsPunctuation);

            // Adjust score
            ScoreModifier = punctuations * 5;

            // Return result
            var pass = Settings.RequirePunctuation == false || punctuations > 0;

            if (pass == false)
                FailureMessage = "Must have at least one punctuation mark in password";

            return pass;
        }
    }
}
