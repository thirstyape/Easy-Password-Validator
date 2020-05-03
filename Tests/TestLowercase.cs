using Easy_Password_Validator.Interfaces;

using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks to see whether the password contains any lowercase letters
    /// </summary>
    public class TestLowercase : IPasswordTest
    {
        public int ScoreModifier { get; set; }
        public IPasswordRequirements Settings { get; set; }
        public IEnumerable<string> BadList { get; set; }

        public bool RunTest(string password, bool isL33t)
        {
            var lowercases = password.Count(char.IsLower);

            return lowercases > 0;
        }
    }
}
