using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Models;
using Easy_Password_Validator.Properties;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks to see whether a password meets the minimum entropy requirements
    /// </summary>
    public class TestEntropy : IPasswordTest
    {
        /// <summary>
        /// Prepares test for use
        /// </summary>
        /// <param name="passwordRequirements">Object containing current settings</param>
        public TestEntropy(IPasswordRequirements passwordRequirements)
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
        public IEnumerable<string> BadList { get; set; }

        /// <inheritdoc/>
        public List<PatternMapItem> PatternMap { get; set; }

        /// <inheritdoc/>
        public bool TestAndScore(string password)
        {
            // Reset
            FailureMessage = null;
            ScoreModifier = 0;

            // Check for inactive
            if (Settings.UseEntropy == false || Settings.MinEntropy == 0)
                return true;

            // Do work
            var grouped = password.GroupBy(c => c).ToDictionary(c => c.Key, c => c.Count());
            var entropy = 0.0;
            var log2 = Math.Log(2);

            foreach (var item in grouped)
            {
                var frequency = (float)item.Value / password.Length;
                entropy -= frequency * (Math.Log(frequency) / log2);
            }

            // Adjust score
            ScoreModifier = (int)Math.Truncate(Math.Pow(2, entropy));

            // Return result
            var pass = Settings.MinEntropy <= entropy;

            if (pass == false)
                FailureMessage = string.Format(Resources.FailedEntropy, Settings.MinEntropy);

            return pass;
        }
    }
}
