using Easy_Password_Validator.Interfaces;

using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks to see whether a password meets the maximum consecutive characters requirements
    /// </summary>
    public class TestRepeat : IPasswordTest
    {
        public TestRepeat(IPasswordRequirements passwordRequirements)
        {
            Settings = passwordRequirements;
        }

        public int ScoreModifier { get; set; }
        public string FailureMessage { get; set; }
        public IPasswordRequirements Settings { get; set; }
        public IEnumerable<string> BadList { get; set; }

        public bool TestAndScore(string password, bool isL33t)
        {
            // Check for inactive
            if (Settings.MaxRepeatSameCharacter < 1)
                return true;

            // Group consecutive letters
            var repeats = password.Select((c, i) => password.Substring(i).TakeWhile(x => x == c));

            // Adjust score
            if (isL33t == false)
                ScoreModifier = repeats.Count(x => x.Count() > Settings.MaxRepeatSameCharacter) * -3;

            // Return result
            var pass = repeats.Any(x => x.Count() > Settings.MaxRepeatSameCharacter) == false;

            if (pass == false)
                FailureMessage = $"Can have a maximum of {Settings.MaxRepeatSameCharacter} adjacent repeat characters";

            return pass;
        }
    }
}
