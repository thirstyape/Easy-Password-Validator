using Easy_Password_Validator.Enums;
using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Tests;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator
{
    /// <summary>
    /// Main class to analyse passwords via requirement checks and scoring
    /// </summary>
    public class PasswordValidatorService
    {
        private readonly List<IPasswordTest> PasswordTests;
        private readonly List<TestBadList> BadListTests;

        /// <summary>
        /// Prepares the validator service for use analysing passwords
        /// </summary>
        /// <param name="passwordRequirements">The parameters to analyse passwords with</param>
        public PasswordValidatorService(IPasswordRequirements passwordRequirements)
        {
            Settings = passwordRequirements;
            FailureMessages = new List<string>();

            PasswordTests = new List<IPasswordTest>
            {
                new TestLength(passwordRequirements),
                new TestUnique(passwordRequirements),
                new TestRepeat(passwordRequirements),
                new TestPattern(passwordRequirements),
                new TestDigit(passwordRequirements),
                new TestLowercase(passwordRequirements),
                new TestUppercase(passwordRequirements),
                new TestPunctuation(passwordRequirements)
            };

            BadListTests = new List<TestBadList>
            {
                new TestBadList("/xato-10k"),
                new TestBadList("/xato-100k")
            };
        }

        /// <summary>
        /// The resulting score of an analysed password
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// The configuration settings to use when analysing passwords
        /// </summary>
        public IPasswordRequirements Settings { get; set; }

        /// <summary>
        /// Contains a listing of any reasons a password failed analysis
        /// </summary>
        public IList<string> FailureMessages { get; private set; }

        /// <summary>
        /// Runs scoring and validation on the specified password
        /// </summary>
        /// <param name="password">The password to test</param>
        /// <param name="userInformation">An optional list containing user information to compare against the password</param>
        /// <exception cref="ArgumentNullException"></exception>
        public bool TestAndScore(string password, IEnumerable<string> userInformation = null)
        {
            // Input validation
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "Must provide password to analyse");

            // Reset
            Score = 0;
            FailureMessages.Clear();

            // Get l33t variants
            var l33t = L33tDecoderService.Decode(password, L33tLevel.Basic);

            // Run general tests
            RunPasswordTests(password, false);

            foreach (var variant in l33t)
                if (Settings.ExitOnFailure == false || FailureMessages.Count == 0)
                    RunPasswordTests(variant, true);

            // Run list tests
            if (userInformation != null)
                BadListTests.Add(new TestBadList(userInformation));

            // NOTE: this is where you check password and l33t, and reversed password and reversed l33t

            // Remove duplicate failure messages (l33t variants may cause this)
            FailureMessages = FailureMessages.Distinct().ToList();

            // Return result
            if (Settings.MinScore > 0)
                return Score >= Settings.MinScore && FailureMessages.Count == 0;
            else
                return FailureMessages.Count == 0;
        }

        /// <summary>
        /// Adds a custom password test to the list of tests that will be run against provided passwords
        /// </summary>
        /// <param name="test">The test to add</param>
        public void AddTest(IPasswordTest test)
        {
            if (test is TestBadList testBadList)
                BadListTests.Add(testBadList);
            else
                PasswordTests.Add(test);
        }

        /// <summary>
        /// Runs each loaded password test against the provided password
        /// </summary>
        /// <param name="password">The password to test</param>
        /// <param name="isL33t">Specifies whether this password is a l33t variant (scoring does not occur)</param>
        private void RunPasswordTests(string password, bool isL33t)
        {
            foreach (var test in PasswordTests)
            {
                var pass = test.TestAndScore(password, isL33t);

                if (pass == false)
                    FailureMessages.Add(test.FailureMessage);

                if (pass == false && Settings.ExitOnFailure)
                    break;

                Score += test.ScoreModifier;
            }
        }
    }
}
