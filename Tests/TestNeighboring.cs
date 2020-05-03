using Easy_Password_Validator.Interfaces;

using System;
using System.Collections.Generic;

namespace Easy_Password_Validator.Tests
{
    /// <summary>
    /// Checks to see whether a password meets the maximum neighboring characters requirements
    /// </summary>
    public class TestNeighboring : IPasswordTest
    {
        public int ScoreModifier { get; set; }
        public IPasswordRequirements Settings { get; set; }
        public IEnumerable<string> BadList { get; set; }

        public bool RunTest(string password, bool isL33t)
        {
            throw new NotImplementedException();
        }
    }
}
