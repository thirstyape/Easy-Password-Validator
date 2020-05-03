namespace Easy_Password_Validator
{
    /// <summary>
    /// 
    /// </summary>
    public class PasswordValidatorService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        public int CheckScore(string password)
        {
            return 0;
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
