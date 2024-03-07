using Easy_Password_Validator.Enums;
using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Models;
using Easy_Password_Validator.Properties;

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
        /// <summary>
        /// Prepares test for use and allows using custom pattern
        /// </summary>
        /// <param name="passwordRequirements">Object containing current settings</param>
        /// <param name="map">An optional custom pattern mapping to check</param>
        /// <exception cref="ArgumentNullException"></exception>
        public TestPattern(IPasswordRequirements passwordRequirements, List<PatternMapItem> map = null)
        {
            Settings = passwordRequirements;
            PatternMap = map;

            if (Settings.KeyboardStyle == PatternMapTypes.Custom && map == null)
                throw new ArgumentNullException(nameof(map), "Must provide pattern mapping list when using custom layout");
        }

        /// <inheritdoc/>
        public int ScoreModifier { get; set; }

        /// <inheritdoc/>
        public string FailureMessage { get; set; }

        /// <inheritdoc/>
        public IPasswordRequirements Settings { get; set; }

        /// <summary>
        /// Contains the patterns to check for
        /// </summary>
        public List<PatternMapItem> PatternMap { get; set; }

        /// <inheritdoc/>
        public bool TestAndScore(string password)
        {
            // Reset
            FailureMessage = null;
            ScoreModifier = 0;

            // Check for inactive
            if (Settings.UsePattern == false)
                return true;

            // Check for invalid
            if (Settings.MaxNeighboringCharacter < 1)
                return false;

            // Update map
            if (Settings.KeyboardStyle == PatternMapTypes.Qwerty)
                PatternMap = PatternMapService.QwertyMap;
            else if (Settings.KeyboardStyle == PatternMapTypes.Qwertz)
                PatternMap = PatternMapService.QwertzMap;
            else if (Settings.KeyboardStyle == PatternMapTypes.Azerty)
                PatternMap = PatternMapService.AzertyMap;

            // Do work
            var patterns = PatternMapService.GetPatterns(password, PatternMap);

            // Adjust score
            ScoreModifier = patterns.Sum(x => -(int)Math.Pow(2, x.Length));

            // Return result
            var pass = patterns.Any(x => x.Length > Settings.MaxNeighboringCharacter) == false;

            if (pass == false)
                FailureMessage = string.Format(Resources.FailedPattern, Settings.MaxNeighboringCharacter);

            return pass;
        }
    }
}
