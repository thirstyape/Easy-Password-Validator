using Easy_Password_Validator.Enums;
using Easy_Password_Validator.Interfaces;
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
		/// <exception cref="ArgumentNullException"></exception>
		public TestBadList(IEnumerable<string> badList)
		{
            BadList = badList ?? throw new ArgumentNullException(nameof(badList), "Must provide bad list collection");
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

		/// <summary>
		/// Contains a listing of invalid passwords to compare against
		/// </summary>
		public IEnumerable<string> BadList { get; set; }

        /// <summary>
        /// The type of list to be tested
        /// </summary>
        /// <remarks>
        /// Primarily used for loading Top 10K and Top 100K lists
        /// </remarks>
        public BadListTypes ListType { get; set; } = BadListTypes.UserSupplied;

        /// <summary>
        /// Specifies whether to use this list when testing L33T variants of the supplied password
        /// </summary>
        public bool TestL33tVariants { get; set; }

        /// <inheritdoc/>
        public bool TestAndScore(string password)
        {
            // Reset
            FailureMessage = null;
            ScoreModifier = 0;

            // Check for inactive
            if (Settings.UseBadList == false)
                return true;

            // Check for match
            var match = BadList.Any(x => x.IndexOf(password, StringComparison.OrdinalIgnoreCase) >= 0);

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
