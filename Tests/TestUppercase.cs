using Easy_Password_Validator.Interfaces;

using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks to see whether the password contains any uppercase letters
    /// </summary>
    public class TestUppercase : IPasswordTest
    {
        public int ScoreModifier { get; set; }
        public IPasswordRequirements Settings { get; set; }
        public IEnumerable<string> BadList { get; set; }

        public bool RunTest(string password, bool isL33t)
        {
            var uppercases = password.Count(char.IsUpper);

            return uppercases > 0;
        }
    }
}
