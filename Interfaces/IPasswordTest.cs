using System.Collections.Generic;

namespace Easy_Password_Validator.Interfaces
{
    /// <summary>
    /// Interface to define requirements for tests that will be run against passwords
    /// </summary>
    public interface IPasswordTest
    {
        /// <summary>
        /// Provided the amount the password score should be changed by after running the test
        /// </summary>
        public int ScoreModifier { get; set; }

        /// <summary>
        /// Container to pass validator configuration to tester
        /// </summary>
        public IPasswordRequirements Settings { get; set; }

        /// <summary>
        /// Contains an optional listing of invalid passwords to compare against
        /// </summary>
        public IEnumerable<string> BadList { get; set; }

        /// <summary>
        /// Executes the test on the provided password
        /// </summary>
        /// <param name="password">The password to run the test on</param>
        /// <param name="isL33t">Specifies whether this password is a l33t variant</param>
        public bool RunTest(string password, bool isL33t);
    }
}
