using Easy_Password_Validator.Interfaces;

namespace Easy_Password_Validator
{
    /// <summary>
    /// 
    /// </summary>
    public class PasswordValidatorService
    {
        public int Score { get; set; }
        public IPasswordRequirements Settings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        public int CheckScore(string password)
        {
            return Score;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        public bool TestPassword(string password)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        public (int, bool) AnalysePassword(string password)
        {
            var score = CheckScore(password);
            var pass = TestPassword(password);

            return (score, pass);
        }
    }
}
