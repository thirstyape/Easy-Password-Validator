using Easy_Password_Validator.Enums;
using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Models;
using Easy_Password_Validator.Tests;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;

namespace Easy_Password_Validator
{
    /// <summary>
    /// Main class to analyse passwords via requirement checks and scoring
    /// </summary>
    public class PasswordValidatorService
    {
        private readonly List<IPasswordTest> PasswordTests;
        private TestBadList Top10kBadList;
        private TestBadList Top100kBadList;
        private bool BadListsLoaded;

        private IEnumerable<L33tReplacement> CustomReplacements;

        /// <summary>
        /// Prepares the validator service for use analysing passwords
        /// </summary>
        /// <param name="passwordRequirements">The parameters to analyse passwords with</param>
        public PasswordValidatorService(IPasswordRequirements passwordRequirements)
        {
            // Prepare tests
            Settings = passwordRequirements;
            FailureMessages = new List<string>();

            PasswordTests = new List<IPasswordTest>()
            {
                new TestLength(passwordRequirements),
                new TestUnique(passwordRequirements),
                new TestRepeat(passwordRequirements),
                new TestPattern(passwordRequirements),
                new TestDigit(passwordRequirements),
                new TestLowercase(passwordRequirements),
                new TestUppercase(passwordRequirements),
                new TestPunctuation(passwordRequirements),
                new TestEntropy(passwordRequirements)
            };

            // Load lists
            LoadBadLists();
        }

        /// <summary>
        /// The resulting score of an analysed password
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// The configuration settings to use when analysing passwords
        /// </summary>
        public IPasswordRequirements Settings { get; private set; }

        /// <summary>
        /// Contains a listing of any reasons a password failed analysis
        /// </summary>
        public IList<string> FailureMessages { get; private set; }

        /// <summary>
        /// Runs scoring and validation on the specified password
        /// </summary>
        /// <param name="password">The password to test</param>
        /// <param name="userInformation">An optional list containing user information to compare against the password</param>
        /// <param name="languageCode">An optional language code used for error text</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public bool TestAndScore(string password, IEnumerable<string> userInformation = null, string languageCode = null)
        {
            // Input validation
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "Must provide password to analyse");

            // Reset
            FailureMessages.Clear();
            Score = 0;

            // Update error output language
            if (string.IsNullOrEmpty(languageCode) == false)
            {
                if (CheckValidLanguage(languageCode) == false)
                    throw new ArgumentException("Provided language code is not supported", nameof(languageCode));

                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(languageCode);
            }

            // Get l33t variants
            IEnumerable<string> l33t;

            if (CustomReplacements == null)
                l33t = L33tDecoderService.Decode(password, L33tLevel.Advanced);
            else
                l33t = L33tDecoderService.Decode(password, L33tLevel.Custom, CustomReplacements);

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
        /// Updates the collection of l33t replacements that will be used to decode l33t based passwords
        /// </summary>
        /// <param name="l33TReplacements">The replacements to use</param>
        public void UpdateL33tReplacements(IEnumerable<L33tReplacement> l33TReplacements)
        {
            CustomReplacements = l33TReplacements;
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

            if (BadListsLoaded)
            {
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
        /// Loads the badlists into memory
        /// </summary>
        private void LoadBadLists()
        {
            BadListsLoaded = LoadLocalBadLists() || LoadRemoteBadLists();
        }

        /// <summary>
        /// Attempts to load locally stored copies of the badlists
        /// </summary>
        private bool LoadLocalBadLists()
        {
            try
            {
                // Prepare directory names
                var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                var local10k = Path.Combine(directory, "BadLists\\top-10k-passwords.txt");
                var local100k = Path.Combine(directory, "BadLists\\top-100k-passwords.txt");

                // Load local copy
                if (File.Exists(local10k))
                    Top10kBadList = new TestBadList(local10k);

                if (File.Exists(local100k))
                    Top100kBadList = new TestBadList(local100k);

                if (Top10kBadList != null && Top100kBadList != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to load remote copies of the badlists
        /// </summary>
        private bool LoadRemoteBadLists()
        {
            // Prepare directory names
            var appdata10k = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy-Password-Validator\\BadLists\\top-10k-passwords.txt");
            var appdata100k = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy-Password-Validator\\BadLists\\top-100k-passwords.txt");

            var remote10k = "https://raw.githubusercontent.com/thirstyape/Easy-Password-Validator/master/BadLists/top-10k-passwords.txt";
            var remote100k = "https://raw.githubusercontent.com/thirstyape/Easy-Password-Validator/master/BadLists/top-100k-passwords.txt";

            // Load remote copy
            try
            {
                using (var client = new WebClient())
                {
                    if (Top10kBadList == null)
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(appdata10k));
                        client.DownloadFile(remote10k, appdata10k);
                        Top10kBadList = new TestBadList(appdata10k);
                    }

                    if (Top100kBadList == null)
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(appdata100k));
                        client.DownloadFile(remote100k, appdata100k);
                        Top100kBadList = new TestBadList(appdata100k);
                    }
                }

                if (Top10kBadList != null && Top100kBadList != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
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

        /// <summary>
        /// Checks to see that the user provided language code is supported
        /// </summary>
        /// <param name="languageCode">The code to check</param>
        private bool CheckValidLanguage(string languageCode)
        {
            if (languageCode.Length == 2)
            {
                switch (languageCode.ToLower())
                {
                    case "de":
                    case "en":
                        return true;
                    default:
                        return false;
                };
            }
            else if (languageCode.Length == 5 && languageCode[2] == '-')
            {
                return CheckValidLanguage(languageCode.Substring(0, 2));
            }
            else
            {
                return false;
            }
        }
    }
}
