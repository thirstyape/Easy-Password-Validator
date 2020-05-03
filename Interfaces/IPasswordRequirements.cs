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
        public int MinLength { get; set; }

        /// <summary>
        /// Specifies the minimum amount of unique characters required in a password
        /// </summary>
        public int MinUniqueCharacters { get; set; }

        /// <summary>
        /// Specifies the maximum amount of times a single character may appear beside itself in a password
        /// </summary>
        public int MaxConsecutiveSameCharacter { get; set; }

        /// <summary>
        /// Specifies the maximum amount of characters neighboring one another on the keyboard that may appear in a password
        /// </summary>
        public int MaxNeighboringCharacter { get; set; }

        /// <summary>
        /// Specifies whether a password must contain at least one digit
        /// </summary>
        public bool RequireDigit { get; set; }

        /// <summary>
        /// Specifies whether a password must contain at least one lowercase letter
        /// </summary>
        public bool RequireLowercase { get; set; }

        /// <summary>
        /// Specifies whether a password must contain at least one uppercase letter
        /// </summary>
        public bool RequireUppercase { get; set; }

        /// <summary>
        /// Specifies whether a password must contain at least one punctuation mark
        /// </summary>
        public bool RequirePunctuation { get; set; }
    }
}
