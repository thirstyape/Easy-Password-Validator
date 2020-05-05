using Easy_Password_Validator.Interfaces;

using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks whether a password is contained within the provided list
    /// </summary>
    public class TestBadList : IPasswordTest
    {
        public TestBadList(IEnumerable<string> badList)
        {
            BadList = badList;
        }

        public int ScoreModifier { get; set; }
        public string FailureMessage { get; set; }
        public IPasswordRequirements Settings { get; set; }
        public IEnumerable<string> BadList { get; set; }

        public bool TestAndScore(string password, bool isL33t)
        {
            // Return result
            var pass = BadList.Any(x => x == password || x.Contains(password)) == false;

            if (pass == false)
                FailureMessage = "Specified password is in list of known bad passwords";

            return pass;
        }
    }
}
