﻿namespace Easy_Password_Validator.Interfaces
{
	/// <summary>
	/// Interface to define requirements for tests that will be run against passwords
	/// </summary>
	public interface IPasswordTest
    {
        /// <summary>
        /// The amount the password score should be changed by after running the test
        /// </summary>
        int ScoreModifier { get; set; }

        /// <summary>
        /// A message to display to the end user on test failure
        /// </summary>
        string FailureMessage { get; set; }

        /// <summary>
        /// Container to pass validator configuration to tester
        /// </summary>
        IPasswordRequirements Settings { get; set; }

        /// <summary>
        /// Executes the test on the provided password and updates the score modifier
        /// </summary>
        /// <param name="password">The password to run the test on</param>
        bool TestAndScore(string password);
    }
}
