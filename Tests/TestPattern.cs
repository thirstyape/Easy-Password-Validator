using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Models;

using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks to see whether a password meets the maximum neighboring characters requirements
    /// </summary>
    public class TestPattern : IPasswordTest
    {
        public TestPattern(IPasswordRequirements passwordRequirements, List<PatternMapItem> map = null)
        {
            Settings = passwordRequirements;

            PatternMap = map ?? PatternMapService.QwertyMap;
        }

        public int ScoreModifier { get; set; }
        public string FailureMessage { get; set; }
        public IPasswordRequirements Settings { get; set; }
        public IEnumerable<string> BadList { get; set; }
        public List<PatternMapItem> PatternMap { get; set; }

        public bool TestAndScore(string password, bool isL33t)
        {
            // Reset
            ScoreModifier = 0;

            // Check for inactive
            if (Settings.MaxNeighboringCharacter < 1)
                return true;

            // Do work
            var patterns = PatternMapService.GetPatterns(password, PatternMap);

            // Adjust score
            if (isL33t == false)
                ScoreModifier = patterns.Count(x => x.Length > Settings.MaxNeighboringCharacter) * -3;

            // Return result
            var pass = patterns.Any(x => x.Length > Settings.MaxNeighboringCharacter);

            if (pass == false)
                FailureMessage = $"Can have a maximum of {Settings.MaxNeighboringCharacter} characters that neighbor each other on the keyboard";

            return pass;
        }
    }
}
