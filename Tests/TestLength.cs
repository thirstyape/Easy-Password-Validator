using Easy_Password_Validator.Interfaces;

using System.Collections.Generic;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks to see whether a password meets the minimum string length requirements
    /// </summary>
    public class TestLength : IPasswordTest
    {
        public TestLength(IPasswordRequirements passwordRequirements)
        {
            Settings = passwordRequirements;
        }

        public int ScoreModifier { get; set; }
        public IPasswordRequirements Settings { get; set; }
        public IEnumerable<string> BadList { get; set; }

        public bool RunTest(string password, bool isL33t)
        {
            return password.Length >= Settings.MinLength;
        }
    }
}
