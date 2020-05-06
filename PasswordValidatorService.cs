using Easy_Password_Validator.Enums;
using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Tests;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Easy_Password_Validator
{
    /// <summary>
    /// Main class to analyse passwords via requirement checks and scoring
    /// </summary>
    public class PasswordValidatorService
    {
        private readonly List<IPasswordTest> PasswordTests;
        private readonly TestBadList Top10kBadList;
        private readonly TestBadList Top100kBadList;

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

            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Top10kBadList = new TestBadList(Path.Combine(directory, "BadLists\\top-10k-passwords.txt"));
            Top100kBadList = new TestBadList(Path.Combine(directory, "BadLists\\top-100k-passwords.txt"));
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
            FailureMessages.Clear();
            Score = 0;

            // Get l33t variants
            var l33t = L33tDecoderService.Decode(password, L33tLevel.Advanced);

            // Run general tests
            RunPasswordTests(password);

            // Run list tests
            TestBadList userBadList = null;

            if (userInformation != null)
                userBadList = new TestBadList(userInformation);

            RunBadListTests(password, false, userBadList);

            foreach (var variant in l33t)
                if (Settings.ExitOnFailure == false || FailureMessages.Count == 0)
                    RunBadListTests(variant, true, userBadList);

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
            PasswordTests.Add(test);
        }

        /// <summary>
        /// Runs each loaded password test against the provided password
        /// </summary>
        /// <param name="password">The password to test</param>
        private void RunPasswordTests(string password)
        {
            foreach (var test in PasswordTests)
            {
                var pass = RunTest(password, test);

                if (pass == false && Settings.ExitOnFailure)
                    break;
            }
        }

        /// <summary>
        /// Runs each applicable bad list test against the provided password
        /// </summary>
        /// <param name="password">The password to test</param>
        /// <param name="isL33t">Specifies whether this password is a l33t variant</param>
        /// <param name="userBadList">An optional bad list to check against containing user information</param>
        private void RunBadListTests(string password, bool isL33t, TestBadList userBadList = null)
        {
            var reversed = Reverse(password);

            // Test top 10K list
            if (isL33t)
            {
                RunTest(password, Top10kBadList);
                RunTest(reversed, Top10kBadList);
            }

            // Test top 100K list
            if (isL33t == false)
            {
                RunTest(password, Top100kBadList);
                RunTest(reversed, Top100kBadList);
            }

            // Test user list
            if (userBadList != null)
            {
                RunTest(password, userBadList);
                RunTest(reversed, userBadList);
            }
        }

        /// <summary>
        /// Runs a single test on a password and updates the failure message and score
        /// </summary>
        /// <param name="password">The password to test</param>
        /// <param name="test">The test to run on the password</param>
        private bool RunTest(string password, IPasswordTest test)
        {
            var pass = test.TestAndScore(password);

            if (pass == false)
                FailureMessages.Add(test.FailureMessage);

            Score += test.ScoreModifier;

            return pass;
        }

        /// <summary>
        /// Separates a string into its grapheme clusters (or characters)
        /// </summary>
        /// <param name="value">The value to separate</param>
        private IEnumerable<string> GetGraphemeClusters(string value)
        {
            var enumerator = StringInfo.GetTextElementEnumerator(value);

            while (enumerator.MoveNext())
                yield return (string)enumerator.Current;
        }

        /// <summary>
        /// Reverses the provided string
        /// </summary>
        /// <param name="value">The value to reverse</param>
        private string Reverse(string value)
        {
            return string.Join(string.Empty, GetGraphemeClusters(value).Reverse().ToArray());
        }
    }
}
