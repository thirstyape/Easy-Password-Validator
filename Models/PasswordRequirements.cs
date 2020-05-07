using Easy_Password_Validator.Interfaces;

namespace Easy_Password_Validator.Models
{
    /// <summary>
    /// Default implementation of <see cref="IPasswordRequirements"/>
    /// </summary>
    public class PasswordRequirements : IPasswordRequirements
    {
        public int MinLength { get; set; } = 8;
        public int MinUniqueCharacters { get; set; } = 4;
        public int MaxRepeatSameCharacter { get; set; } = 3;
        public int MaxNeighboringCharacter { get; set; } = 4;
        public bool RequireDigit { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequirePunctuation { get; set; }
        public int MinScore { get; set; } = 50;
        public bool ExitOnFailure { get; set; }
    }
}
