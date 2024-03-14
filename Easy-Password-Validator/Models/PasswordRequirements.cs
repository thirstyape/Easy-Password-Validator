using Easy_Password_Validator.Enums;
using Easy_Password_Validator.Interfaces;

namespace Easy_Password_Validator.Models
{
    /// <summary>
    /// Default implementation of <see cref="IPasswordRequirements"/>
    /// </summary>
    public class PasswordRequirements : IPasswordRequirements
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public PasswordRequirements() { }

        /// <summary>
        /// Constructor to clone existing requirements
        /// </summary>
        /// <param name="requirements"></param>
        public PasswordRequirements(IPasswordRequirements requirements)
        {
            MinLength = requirements.MinLength;
            MinUniqueCharacters = requirements.MinUniqueCharacters;
            MaxRepeatSameCharacter = requirements.MaxRepeatSameCharacter;
            MaxNeighboringCharacter = requirements.MaxNeighboringCharacter;
            MinScore = requirements.MinScore;
            MinEntropy = requirements.MinEntropy;

            RequireDigit = requirements.RequireDigit;
            RequireLowercase = requirements.RequireLowercase;
            RequireUppercase = requirements.RequireUppercase;
            RequirePunctuation = requirements.RequirePunctuation;

            ExitOnFailure = requirements.ExitOnFailure;
            KeyboardStyle = requirements.KeyboardStyle;

            UseLength = requirements.UseLength;
            UseDigit = requirements.UseDigit;
            UseLowercase = requirements.UseLowercase;
            UseUppercase = requirements.UseUppercase;
            UsePattern = requirements.UsePattern;
            UsePunctuation = requirements.UsePunctuation;
            UseRepeat = requirements.UseRepeat;
            UseUnique = requirements.UseUnique;
            UseBadList = requirements.UseBadList;
            UseEntropy = requirements.UseEntropy;
        }

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
        public float MinEntropy { get; set; } = 2.5F;

        /// <inheritdoc/>
        public bool UseLength { get; set; } = true;

        /// <inheritdoc/>
        public bool UseDigit { get; set; } = true;

        /// <inheritdoc/>
        public bool UseLowercase { get; set; } = true;

        /// <inheritdoc/>
        public bool UseUppercase { get; set; } = true;

        /// <inheritdoc/>
        public bool UsePattern { get; set; } = true;

        /// <inheritdoc/>
        public bool UsePunctuation { get; set; } = true;

        /// <inheritdoc/>
        public bool UseRepeat { get; set; } = true;

        /// <inheritdoc/>
        public bool UseUnique { get; set; } = true;

        /// <inheritdoc/>
        public bool UseBadList { get; set; } = true;

        /// <inheritdoc/>
        public bool UseEntropy { get; set; }

        /// <inheritdoc/>
        public bool ExitOnFailure { get; set; }

        /// <inheritdoc/>
        public PatternMapTypes KeyboardStyle { get; set; } = PatternMapTypes.Qwerty;
    }
}
