using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Properties;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
	/// <summary>
	/// Checks to see whether a password meets the maximum consecutive characters requirements
	/// </summary>
	public class TestRepeat : IPasswordTest
    {
        /// <summary>
        /// Prepares test for use
        /// </summary>
        /// <param name="passwordRequirements">Object containing current settings</param>
        public TestRepeat(IPasswordRequirements passwordRequirements)
        {
            Settings = passwordRequirements;
        }

        /// <inheritdoc/>
        public int ScoreModifier { get; set; }

        /// <inheritdoc/>
        public string FailureMessage { get; set; }

        /// <inheritdoc/>
        public IPasswordRequirements Settings { get; set; }

        /// <inheritdoc/>
        public bool TestAndScore(string password)
        {
            // Reset
            FailureMessage = null;
            ScoreModifier = 0;

            // Check for inactive
            if (Settings.MaxRepeatSameCharacter < 1)
                return true;

            // Group consecutive letters
            var repeats = new List<string>();
            var current = string.Empty;

            for (var i = 0; i < password.Length - 1; i++)
            {
                var same = password[i] == password[i + 1];

                if (same && current.Length == 0)
                    current = password[i].ToString();

                if (same)
                    current += password[i + 1];

                if (same == false && current.Length > 1)
                    repeats.Add(current);

                if (same == false)
                    current = string.Empty;
            }

            if (current.Length > 1)
                repeats.Add(current);

            // Adjust score
            ScoreModifier = repeats.Sum(x => -(int)Math.Pow(2, x.Length));

            // Return result
            var pass = repeats.Any(x => x.Length > Settings.MaxRepeatSameCharacter) == false;

            if (pass == false)
                FailureMessage = string.Format(Resources.FailedRepeat, Settings.MaxRepeatSameCharacter);

            return pass;
        }
    }
}
