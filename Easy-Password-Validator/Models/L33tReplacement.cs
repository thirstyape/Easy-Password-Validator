using Easy_Password_Validator.Enums;

namespace Easy_Password_Validator.Models
{
    /// <summary>
    /// Model to create lists of l33t speak decodings
    /// </summary>
    public class L33tReplacement
    {
        /// <summary>
        /// The regular character used in words
        /// </summary>
        public string PlainText { get; set; }

        /// <summary>
        /// The l33t edition of the character
        /// </summary>
        public string L33tEncoded { get; set; }

        /// <summary>
        /// The level of l33t speak that uses this replacement
        /// </summary>
        public L33tLevel Level { get; set; }

        /// <summary>
        /// Specifies the order to execute replacements where lower numbers execute first (useful in cases such as M => /\/\ and A => /\)
        /// </summary>
        public int RunOrder { get; set; } = 100;
    }
}
