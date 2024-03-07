using Easy_Password_Validator.Enums;

namespace Easy_Password_Validator.Interfaces
{
    /// <summary>
    /// Interface to define elements required by built-in password tester
    /// </summary>
    public interface IPasswordRequirements
    {
        /// <summary>
        /// Specifies the minimum length requirement for a password
        /// </summary>
        /// <remarks>
        /// Ignored if <see cref="UseLength"/> is false
        /// </remarks>
        int MinLength { get; set; }

        /// <summary>
        /// Specifies the minimum amount of unique characters required in a password
        /// </summary>
        /// <remarks>
        /// Ignored if <see cref="UseUnique"/> is false
        /// </remarks>
        int MinUniqueCharacters { get; set; }

        /// <summary>
        /// Specifies the maximum amount of times a single character may appear beside itself in a password
        /// </summary>
        /// <remarks>
        /// Ignored if <see cref="UseRepeat"/> is false
        /// </remarks>
        int MaxRepeatSameCharacter { get; set; }

        /// <summary>
        /// Specifies the maximum amount of characters neighboring one another on the keyboard that may appear in a password
        /// </summary>
        /// <remarks>
        /// Ignored if <see cref="UsePattern"/> is false
        /// </remarks>
        int MaxNeighboringCharacter { get; set; }

        /// <summary>
        /// Specifies whether a password must contain at least one digit
        /// </summary>
        /// <remarks>
        /// Ignored if <see cref="UseDigit"/> is false
        /// </remarks>
        bool RequireDigit { get; set; }

        /// <summary>
        /// Specifies whether a password must contain at least one lowercase letter
        /// </summary>
        /// <remarks>
        /// Ignored if <see cref="UseLowercase"/> is false
        /// </remarks>
        bool RequireLowercase { get; set; }

        /// <summary>
        /// Specifies whether a password must contain at least one uppercase letter
        /// </summary>
        /// <remarks>
        /// Ignored if <see cref="UseUppercase"/> is false
        /// </remarks>
        bool RequireUppercase { get; set; }

        /// <summary>
        /// Specifies whether a password must contain at least one punctuation mark
        /// </summary>
        /// <remarks>
        /// Ignored if <see cref="UsePunctuation"/> is false
        /// </remarks>
        bool RequirePunctuation { get; set; }

        /// <summary>
        /// Specifies the minimum total score required for a password to be considered valid
        /// </summary>
        int MinScore { get; set; }

        /// <summary>
        /// Specifies the minimum entropy score required for a password to be considered valid
        /// </summary>
        /// <remarks>
        /// Ignored if <see cref="UseEntropy"/> is false
        /// </remarks>
        float MinEntropy { get; set; }

        /// <summary>
        /// Specifies whether to perform the length test
        /// </summary>
        bool UseLength { get; set; }

        /// <summary>
        /// Specifies whether to perform the digit test
        /// </summary>
        bool UseDigit { get; set; }

        /// <summary>
        /// Specifies whether to perform the lowercase test
        /// </summary>
        bool UseLowercase { get; set; }

        /// <summary>
        /// Specifies whether to perform the uppercase test
        /// </summary>
        bool UseUppercase { get; set; }

        /// <summary>
        /// Specifies whether to perform the pattern matching test
        /// </summary>
        bool UsePattern { get; set; }

        /// <summary>
        /// Specifies whether to perform the punctuation test
        /// </summary>
        bool UsePunctuation { get; set; }

        /// <summary>
        /// Specifies whether to perform the repeat characters test
        /// </summary>
        bool UseRepeat { get; set; }

        /// <summary>
        /// Specifies whether to perform the unique characters test
        /// </summary>
        bool UseUnique { get; set; }

        /// <summary>
        /// Specifies whether to perform the bad list tests
        /// </summary>
        bool UseBadList { get; set; }

        /// <summary>
        /// Specifies whether to perform the entropy checking test
        /// </summary>
        bool UseEntropy { get; set; }

        /// <summary>
        /// Specifies whether to stop execution on the first failure or continue and report after all tests complete
        /// </summary>
        bool ExitOnFailure { get; set; }

        /// <summary>
        /// Sets the keyboard layout to use with the pattern test
        /// </summary>
        PatternMapTypes KeyboardStyle { get; set; }
    }
}
