using Easy_Password_Validator.Interfaces;

using System;
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
        public IPasswordRequirements Settings { get; set; }
        public IEnumerable<string> BadList { get; set; }

        public bool RunTest(string password, bool isL33t)
        {
            return BadList.Contains(password);
        }
    }
}
