using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Models;

using System;
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

        public bool TestAndScore(string password)
        {
            // Reset
            FailureMessage = null;
            ScoreModifier = 0;

            // Check for inactive
            if (Settings.MaxNeighboringCharacter < 1)
                return true;

            // Do work
            var patterns = PatternMapService.GetPatterns(password, PatternMap);

            // Adjust score
            ScoreModifier = patterns.Sum(x => -(int)Math.Pow(2, x.Length));

            // Return result
            var pass = patterns.Any(x => x.Length > Settings.MaxNeighboringCharacter) == false;

            if (pass == false)
                FailureMessage = $"Can have a maximum of {Settings.MaxNeighboringCharacter} characters that neighbor each other on the keyboard";

            return pass;
        }
    }
}
