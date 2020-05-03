using Easy_Password_Validator.Interfaces;

namespace Easy_Password_Validator.Models
{
    /// <summary>
    /// Default implementation of <see cref="IPasswordRequirements"/>
    /// </summary>
    public class PasswordRequirements : IPasswordRequirements
    {
        public int MinLength { get; set; } = 6;
        public int MinUniqueCharacters { get; set; }
        public int MaxConsecutiveSameCharacter { get; set; }
        public int MaxNeighboringCharacter { get; set; }
        public bool RequireDigit { get; set; } = true;
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequirePunctuation { get; set; }
    }
}
