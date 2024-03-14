using Easy_Password_Validator.Enums;
using Easy_Password_Validator.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Easy_Password_Validator_GUI;

/// <summary>
/// Implementation of <see cref="IPasswordRequirements"/> with data annotations and attributes.
/// </summary>
public class AnnotatedPasswordRequirements : IPasswordRequirements
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public AnnotatedPasswordRequirements() { }

    /// <summary>
    /// Constructor to clone existing requirements
    /// </summary>
    /// <param name="requirements"></param>
    public AnnotatedPasswordRequirements(IPasswordRequirements requirements)
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
    [Display(Name = "Minimum Length", Description = "Specifies the minimum length requirement for a password.")]
    [Range(1, 100)]
    public int MinLength { get; set; } = 8;

    /// <inheritdoc/>
    [Display(Name = "Minimum Unique Characters", Description = "Specifies the minimum amount of unique characters required in a password.")]
    [Range(1, 100)]
    public int MinUniqueCharacters { get; set; } = 4;

    /// <inheritdoc/>
    [Display(Name = "Maximum Repeat Same Character", Description = "Specifies the maximum amount of times a single character may appear beside itself in a password.")]
    [Range(1, 100)]
    public int MaxRepeatSameCharacter { get; set; } = 3;

    /// <inheritdoc/>
    [Display(Name = "Maximum Neighbouring Character", Description = "Specifies the maximum amount of characters neighbouring one another on the keyboard that may appear in a password.")]
    [Range(1, 100)]
    public int MaxNeighboringCharacter { get; set; } = 4;

    /// <inheritdoc/>
    [Display(Name = "Require Digit", Description = "Specifies whether a password must contain at least one digit.")]
    public bool RequireDigit { get; set; }

    /// <inheritdoc/>
    [Display(Name = "Require Lowercase", Description = "Specifies whether a password must contain at least one lowercase letter.")]
    public bool RequireLowercase { get; set; }

    /// <inheritdoc/>
    [Display(Name = "Require Uppercase", Description = "Specifies whether a password must contain at least one uppercase letter.")]
    public bool RequireUppercase { get; set; }

    /// <inheritdoc/>
    [Display(Name = "Require Punctuation", Description = "Specifies whether a password must contain at least one punctuation mark.")]
    public bool RequirePunctuation { get; set; }

    /// <inheritdoc/>
    [Display(Name = "Minimum Score", Description = "Specifies the minimum total score required for a password to be considered valid.")]
    [Range(1, 1_000)]
    public int MinScore { get; set; } = 50;

    /// <inheritdoc/>
    [Display(Name = "Minimum Entropy", Description = "Specifies the minimum entropy score required for a password to be considered valid.")]
    [Range(0.1D, 100D)]
    public float MinEntropy { get; set; } = 2.5F;

    /// <inheritdoc/>
    [Display(Name = "Use Entropy", Description = "Specifies whether to perform the entropy checking test.")]
    public bool UseEntropy { get; set; }

    /// <inheritdoc/>
    [Display(Name = "Exit On Failure", Description = "Specifies whether to stop execution on the first failure or continue and report after all tests complete.")]
    public bool ExitOnFailure { get; set; }

    /// <inheritdoc/>
    [Display(Name = "Use Length", Description = "Specifies whether to perform the length test.")]
    public bool UseLength { get; set; } = true;

    /// <inheritdoc/>
    [Display(Name = "Use Digit", Description = "Specifies whether to perform the digit test.")]
    public bool UseDigit { get; set; } = true;

    /// <inheritdoc/>
    [Display(Name = "Use Lowercase", Description = "Specifies whether to perform the lowercase test.")]
    public bool UseLowercase { get; set; } = true;

    /// <inheritdoc/>
    [Display(Name = "Use Uppercase", Description = "Specifies whether to perform the uppercase test.")]
    public bool UseUppercase { get; set; } = true;

    /// <inheritdoc/>
    [Display(Name = "Use Pattern", Description = "Specifies whether to perform the pattern test.")]
    public bool UsePattern { get; set; } = true;

    /// <inheritdoc/>
    [Display(Name = "Use Punctuation", Description = "Specifies whether to perform the punctuation test.")]
    public bool UsePunctuation { get; set; } = true;

    /// <inheritdoc/>
    [Display(Name = "Use Repeat", Description = "Specifies whether to perform the repeat test.")]
    public bool UseRepeat { get; set; } = true;

    /// <inheritdoc/>
    [Display(Name = "Use Unique", Description = "Specifies whether to perform the unique test.")]
    public bool UseUnique { get; set; } = true;

    /// <inheritdoc/>
    [Display(Name = "Use Bad List", Description = "Specifies whether to perform the bad list test.")]
    public bool UseBadList { get; set; } = true;

    /// <inheritdoc/>
    [Display(Name = "Keyboard Style", Description = "Sets the keyboard layout to use with the pattern test.")]
    public PatternMapTypes KeyboardStyle { get; set; } = PatternMapTypes.Qwerty;
}

