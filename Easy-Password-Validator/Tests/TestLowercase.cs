﻿using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Properties;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
	/// <summary>
	/// Checks to see whether the password contains any lowercase letters
	/// </summary>
	public class TestLowercase : IPasswordTest
    {
        /// <summary>
        /// Prepares test for use
        /// </summary>
        /// <param name="passwordRequirements">Object containing current settings</param>
        public TestLowercase(IPasswordRequirements passwordRequirements)
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
            if (Settings.UseLowercase == false)
                return true;

            // Check for lowercase
            var lowercases = password.Count(char.IsLower);

            // Adjust score
            ScoreModifier = -lowercases;

            // Return result
            var pass = Settings.RequireLowercase == false || lowercases > 0;

            if (pass == false)
                FailureMessage = Resources.FailedLowercase;

            return pass;
        }
    }
}
