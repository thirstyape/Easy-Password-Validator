using Easy_Password_Validator.Enums;
using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Models;
using Easy_Password_Validator.Tests;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Easy_Password_Validator
{
	/// <summary>
	/// Main class to analyse passwords via requirement checks and scoring
	/// </summary>
	public class PasswordValidatorService
	{
		private readonly List<IPasswordTest> PasswordTests;
		private readonly List<TestBadList> BadListTests;

		private IEnumerable<L33tReplacement> CustomReplacements;
		private readonly string BadListDirectory;
		private readonly bool LoadRemoteBadLists;

		private const string Remote10k = "https://raw.githubusercontent.com/thirstyape/Easy-Password-Validator/master/BadLists/top-10k-passwords.txt";
		private const string Remote100k = "https://raw.githubusercontent.com/thirstyape/Easy-Password-Validator/master/BadLists/top-100k-passwords.txt";

		private string InstallDirectory => Path.GetDirectoryName(Assembly.GetAssembly(typeof(PasswordValidatorService)).Location);

		/// <summary>
		/// Prepares the validator service for use analysing passwords
		/// </summary>
		/// <param name="passwordRequirements">The parameters to analyse passwords with</param>
		/// <param name="badListDirectory">A custom directory containing the Top 10K and Top 100K bad list files</param>
		/// <param name="loadRemoteBadLists">Specifies whether to load bad lists over HTTP when missing</param>
		/// <param name="patternMap">A custom pattern map to use with the keyboard pattern test</param>
		/// <exception cref="ArgumentNullException"></exception>
		public PasswordValidatorService(IPasswordRequirements passwordRequirements, string badListDirectory = null, bool loadRemoteBadLists = true, List<PatternMapItem> patternMap = null)
		{
			// Configure class
			LoadRemoteBadLists = loadRemoteBadLists;
			Settings = passwordRequirements ?? throw new ArgumentNullException(nameof(passwordRequirements), "Must provide password requirements object");
			FailureMessages = new List<string>();

			if (string.IsNullOrWhiteSpace(badListDirectory) == false)
				BadListDirectory = badListDirectory;
			else if (string.IsNullOrWhiteSpace(InstallDirectory) == false)
				BadListDirectory = Path.Combine(InstallDirectory, "BadLists");
			else
				BadListDirectory = "";

			// Prepare tests
			PasswordTests = new List<IPasswordTest>()
			{
				new TestLength(passwordRequirements),
				new TestUnique(passwordRequirements),
				new TestRepeat(passwordRequirements),
				new TestPattern(passwordRequirements, patternMap),
				new TestDigit(passwordRequirements),
				new TestLowercase(passwordRequirements),
				new TestUppercase(passwordRequirements),
				new TestPunctuation(passwordRequirements),
				new TestEntropy(passwordRequirements)
			};

			BadListTests = new List<TestBadList>();

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
			if (string.IsNullOrWhiteSpace(languageCode) == false)
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
			if (userInformation != null && userInformation.Any())
			{
				var existing = BadListTests.FirstOrDefault(x => x.ListType == BadListTypes.UserInformation);

				if (existing != null)
					existing.BadList = userInformation;
				else
					BadListTests.Add(new TestBadList(Settings, userInformation) { ListType = BadListTypes.UserInformation, TestL33tVariants = true });
			}

			RunBadListTests(password, false);

			foreach (var variant in l33t)
				if (Settings.ExitOnFailure == false || FailureMessages.Count == 0)
					RunBadListTests(variant, true);

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
			if (test == null)
				return;

			test.Settings = Settings;

			if (test is TestBadList badList)
				BadListTests.Add(badList);
			else
				PasswordTests.Add(test);
		}

		/// <summary>
		/// Updates the password requirements to use in each test
		/// </summary>
		/// <param name="passwordRequirements">The parameters to analyse passwords with</param>
		/// <exception cref="ArgumentNullException"></exception>
		public void UpdatePasswordRequirements(IPasswordRequirements passwordRequirements)
		{
			Settings = passwordRequirements ?? throw new ArgumentNullException(nameof(passwordRequirements), "Must provide password requirements object");

			foreach (var test in PasswordTests)
				test.Settings = Settings;

			foreach (var list in BadListTests)
				list.Settings = Settings;
		}

		/// <summary>
		/// Updates the collection of l33t replacements that will be used to decode l33t based passwords
		/// </summary>
		/// <param name="l33TReplacements">The replacements to use</param>
		/// <exception cref="ArgumentNullException"></exception>
		public void UpdateL33tReplacements(IEnumerable<L33tReplacement> l33TReplacements)
		{
			CustomReplacements = l33TReplacements ?? throw new ArgumentNullException(nameof(l33TReplacements), "Must provide L33T replacements collection");
		}

		/// <summary>
		/// Performs tasks required to configure the class
		/// </summary>
		/// <remarks>
		/// This should only be required when using Blazor WebAssembly as it cannot download the bad lists synchronously. Calling this method from other platforms will have no negative effects.
		/// </remarks>
		public async Task Initialize()
		{
			// Skip on non-WebAssembly
			if (RuntimeInformation.OSDescription.Equals("Browser", StringComparison.OrdinalIgnoreCase) == false)
				return;

			// Load temp copy
			try
			{
				using (var client = new HttpClient())
				{
					if (BadListTests.Any(x => x.ListType == BadListTypes.Top10K) == false)
					{
						var temp10k = await client.GetStringAsync(Remote10k);
						BadListTests.Add(new TestBadList(Settings, temp10k.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)) { ListType = BadListTypes.Top10K, TestL33tVariants = true });
					}

					if (BadListTests.Any(x => x.ListType == BadListTypes.Top100K) == false)
					{
						var temp100k = await client.GetStringAsync(Remote100k);
						BadListTests.Add(new TestBadList(Settings, temp100k.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)) { ListType = BadListTypes.Top100K });
					}
				}
			}
			catch { }
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
		private void RunBadListTests(string password, bool isL33t)
		{
			var reversed = Reverse(password);
			var current = isL33t ? BadListTests.Where(x => x.TestL33tVariants) : BadListTests.AsEnumerable();

			foreach (var list in current)
			{
				RunTest(password, list);
				RunTest(reversed, list);
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
			// Skip on WebAssembly
			if (RuntimeInformation.OSDescription.Equals("Browser", StringComparison.OrdinalIgnoreCase))
				return;

			// Load embedded copy
			var embedded10k = Path.Combine(BadListDirectory, "top-10k-passwords.txt");
			var embedded100k = Path.Combine(BadListDirectory, "top-100k-passwords.txt");

			try
			{
				if (File.Exists(embedded10k))
					BadListTests.Add(new TestBadList(Settings, embedded10k) { ListType = BadListTypes.Top10K, TestL33tVariants = true });

				if (File.Exists(embedded100k))
					BadListTests.Add(new TestBadList(Settings, embedded100k) { ListType = BadListTypes.Top100K });

				if (BadListTests.Count == 2)
					return;
			}
			catch { }

			// Load appdata copy
			var appdata10k = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy-Password-Validator", "BadLists", "top-10k-passwords.txt");
			var appdata100k = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy-Password-Validator", "BadLists", "top-100k-passwords.txt");

			try
			{
				if (BadListTests.Any(x => x.ListType == BadListTypes.Top10K) == false && File.Exists(appdata10k))
					BadListTests.Add(new TestBadList(Settings, appdata10k) { ListType = BadListTypes.Top10K, TestL33tVariants = true });

				if (BadListTests.Any(x => x.ListType == BadListTypes.Top100K) == false && File.Exists(appdata100k))
					BadListTests.Add(new TestBadList(Settings, appdata100k) { ListType = BadListTypes.Top100K });

				if (BadListTests.Count == 2)
					return;
			}
			catch { }

			// Load temp copy
			string temp10k = null;
			string temp100k = null;

			if (LoadRemoteBadLists == false)
				return;

			try
			{
				using (var client = new HttpClient())
				{
					if (BadListTests.Any(x => x.ListType == BadListTypes.Top10K) == false)
					{
						var task = Task.Run(async () => await client.GetStringAsync(Remote10k));
						task.Wait();

						temp10k = task.Result;
						BadListTests.Add(new TestBadList(Settings, temp10k.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)) { ListType = BadListTypes.Top10K, TestL33tVariants = true });
					}

					if (BadListTests.Any(x => x.ListType == BadListTypes.Top100K) == false)
					{
						var task = Task.Run(async () => await client.GetStringAsync(Remote100k));
						task.Wait();

						temp100k = task.Result;
						BadListTests.Add(new TestBadList(Settings, temp100k.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)) { ListType = BadListTypes.Top100K });
					}
				}
			}
			catch { }

			// Save temp copy
			try
			{
				if (string.IsNullOrWhiteSpace(temp10k) == false)
				{
					Directory.CreateDirectory(Path.GetDirectoryName(appdata10k));
					File.WriteAllText(appdata10k, temp10k);
				}

				if (string.IsNullOrWhiteSpace(temp100k) == false)
				{
					Directory.CreateDirectory(Path.GetDirectoryName(appdata100k));
					File.WriteAllText(appdata100k, temp100k);
				}
			}
			catch { }
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
					case "fr":
					case "it":
					case "ro":
					case "pl":
					case "zh":
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
