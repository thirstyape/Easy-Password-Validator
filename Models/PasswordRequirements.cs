using Easy_Password_Validator.Interfaces;

namespace Easy_Password_Validator.Models
{
    /// <summary>
    /// Default implementation of <see cref="IPasswordRequirements"/>
    /// </summary>
    public class PasswordRequirements : IPasswordRequirements
    {
        /// <inheritdoc/>
        public int MinLength { get; set; } = 8;

        /// <inheritdoc/>
        public int MinUniqueCharacters { get; set; } = 4;

        /// <inheritdoc/>
        public int MaxRepeatSameCharacter { get; set; } = 3;

        /// <inheritdoc/>
        public int MaxNeighboringCharacter { get; set; } = 4;

        /// <inheritdoc/>
        public bool RequireDigit { get; set; }

        /// <inheritdoc/>
        public bool RequireLowercase { get; set; }

        /// <inheritdoc/>
        public bool RequireUppercase { get; set; }

        /// <inheritdoc/>
        public bool RequirePunctuation { get; set; }

        /// <inheritdoc/>
        public int MinScore { get; set; } = 50;

        /// <inheritdoc/>
        public bool ExitOnFailure { get; set; }
    }
}
