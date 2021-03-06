﻿using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Properties;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks whether a password is contained within the provided list
    /// </summary>
    public class TestBadList : IPasswordTest
    {
        /// <summary>
        /// Prepares test for use and accepts a list of bad passwords to check
        /// </summary>
        /// <param name="badList">The badlist to use</param>
        public TestBadList(IEnumerable<string> badList)
        {
            BadList = badList;
        }

        /// <summary>
        /// Reads a file containing bad passwords and loads them into the badlist
        /// </summary>
        /// <param name="fileName">The full filename containing the bad password list to use</param>
        /// <exception cref="ArgumentException"></exception>
        public TestBadList(string fileName)
        {
            if (File.Exists(fileName) == false)
                throw new ArgumentException("Specified file does not exist", nameof(fileName));

            BadList = File.ReadAllLines(fileName);
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
        public bool TestAndScore(string password)
        {
            // Reset
            FailureMessage = null;
            ScoreModifier = 0;

            // Check for match
            var match = BadList.Any(x => x.Contains(password, StringComparison.OrdinalIgnoreCase));

            // Adjust score
            ScoreModifier = match ? -50 : 0;

            // Return result
            var pass = match == false;

            if (pass == false)
                FailureMessage = Resources.FailedBadList;

            return pass;
        }
    }
}
