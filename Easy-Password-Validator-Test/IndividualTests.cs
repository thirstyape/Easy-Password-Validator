using Easy_Password_Validator.Enums;
using Easy_Password_Validator.Interfaces;
using Easy_Password_Validator.Models;
using Easy_Password_Validator.Tests;

namespace Easy_Password_Validator_Test;

/// <summary>
/// Unit tests run directly on the implementations of <see cref="IPasswordTest"/>
/// </summary>
public class IndividualTests
{
    [Fact]
    public void CheckBadListThrowsArgument()
    {
        var requirements = new PasswordRequirements() { UseBadList = true };

        Assert.Throws<ArgumentException>("fileName", () => new TestBadList(requirements, ""));
    }

    [Fact]
    public void CheckBadListThrowsArgumentNull()
    {
        var requirements = new PasswordRequirements() { UseBadList = true };

        Assert.Throws<ArgumentNullException>("badList", () => new TestBadList(requirements, (IEnumerable<string>?)null));
    }

    [Theory]
    [InlineData("p@ssw0rd")]
    public void CheckBadListTrue(string value)
    {
        var requirements = new PasswordRequirements() { UseBadList = true };
        var test = new TestBadList(requirements, ".\\BadLists\\top-10k-passwords.txt");
        var result = test.TestAndScore(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("password")]
    public void CheckBadListFalse(string value)
    {
        var requirements = new PasswordRequirements() { UseBadList = true };
        var test = new TestBadList(requirements, ".\\BadLists\\top-10k-passwords.txt");
        var result = test.TestAndScore(value);

        Assert.False(result, test.FailureMessage);
    }

    [Theory]
    [InlineData("hasdigit1")]
    public void RequireDigitTrue(string value)
    {
        var requirements = new PasswordRequirements() { UseDigit = true, RequireDigit = true };
        var test = new TestDigit(requirements);
        var result = test.TestAndScore(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("nodigit")]
    public void RequireDigitFalse(string value)
    {
        var requirements = new PasswordRequirements() { UseDigit = true, RequireDigit = true };
        var test = new TestDigit(requirements);
        var result = test.TestAndScore(value);

        Assert.False(result, test.FailureMessage);
    }

    [Theory]
    [InlineData("Need2.0", 2.0F)]
    [InlineData("Need2.6!", 2.6F)]
    public void MinimumEntropyTrue(string value, float minimum)
    {
        var requirements = new PasswordRequirements() { UseEntropy = true, MinEntropy = minimum };
        var test = new TestEntropy(requirements);
        var result = test.TestAndScore(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("", 0F)]
    [InlineData("", 0.1F)]
    [InlineData("Need2", 2F)]
    [InlineData("Need2.6", 2.6F)]
    public void MinimumEntropyFalse(string value, float minimum)
    {
        var requirements = new PasswordRequirements() { UseEntropy = true, MinEntropy = minimum };
        var test = new TestEntropy(requirements);
        var result = test.TestAndScore(value);

        Assert.False(result, test.FailureMessage);
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData("Need5", 5)]
    public void MinimumLengthTrue(string value, int minimum)
    {
        var requirements = new PasswordRequirements() { UseLength = true, MinLength = minimum };
        var test = new TestLength(requirements);
        var result = test.TestAndScore(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("", 1)]
    [InlineData("Need6", 6)]
    public void MinimumLengthFalse(string value, int minimum) 
    {
        var requirements = new PasswordRequirements() { UseLength = true, MinLength = minimum };
        var test = new TestLength(requirements);
        var result = test.TestAndScore(value);

        Assert.False(result, test.FailureMessage);
    }

    [Theory]
    [InlineData("haslowercase")]
    public void RequireLowercaseTrue(string value)
    {
        var requirements = new PasswordRequirements() { UseLowercase = true, RequireLowercase = true };
        var test = new TestLowercase(requirements);
        var result = test.TestAndScore(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("NOLOWERCASE")]
    public void RequireLowercaseFalse(string value)
    {
        var requirements = new PasswordRequirements() { UseLowercase = true, RequireLowercase = true };
        var test = new TestLowercase(requirements);
        var result = test.TestAndScore(value);

        Assert.False(result, test.FailureMessage);
    }

    [Fact]
    public void MaximumPatternThrows()
    {
        var requirements = new PasswordRequirements() { UsePattern = true, MaxNeighboringCharacter = 0, KeyboardStyle = PatternMapTypes.Custom };

        Assert.Throws<ArgumentNullException>("map", () => new TestPattern(requirements));
    }

    [Theory]
    [InlineData("", 1, PatternMapTypes.Qwertz)]
    [InlineData("Max4 qwer", 4, PatternMapTypes.Qwerty)]
    [InlineData("Max5 QwEr$", 5, PatternMapTypes.Qwerty)]
    [InlineData("Max4 azer", 4, PatternMapTypes.Azerty)]
    public void MaximumPatternTrue(string value, int maximum, PatternMapTypes mapType)
    {
        var requirements = new PasswordRequirements() { UsePattern = true, MaxNeighboringCharacter = maximum, KeyboardStyle = mapType };
        var test = new TestPattern(requirements);
        var result = test.TestAndScore(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("", 0, PatternMapTypes.Qwertz)]
    [InlineData("Max3 qwer", 3, PatternMapTypes.Qwerty)]
    [InlineData("Max4 QwEr$", 4, PatternMapTypes.Qwerty)]
    [InlineData("Max3 azer", 3, PatternMapTypes.Azerty)]
    public void MaximumPatternFalse(string value, int maximum, PatternMapTypes mapType)
    {
        var requirements = new PasswordRequirements() { UsePattern = true, MaxNeighboringCharacter = maximum, KeyboardStyle = mapType };
        var test = new TestPattern(requirements);
        var result = test.TestAndScore(value);

        Assert.False(result, test.FailureMessage);
    }

    [Theory]
    [InlineData("haspunctuation!")]
    [InlineData("hassymbol$")]
    public void RequirePunctuationTrue(string value)
    {
        var requirements = new PasswordRequirements() { UsePunctuation = true, RequirePunctuation = true };
        var test = new TestPunctuation(requirements);
        var result = test.TestAndScore(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("nopunctuation")]
    [InlineData("nosymbol")]
    public void RequirePunctuationFalse(string value) 
    {
        var requirements = new PasswordRequirements() { UsePunctuation = true, RequirePunctuation = true };
        var test = new TestPunctuation(requirements);
        var result = test.TestAndScore(value);

        Assert.False(result, test.FailureMessage);
    }

    [Theory]
    [InlineData("", 1)]
    [InlineData("Maaax3", 3)]
    [InlineData("MMMaaaxxx3", 3)]
    public void MaximumRepeatTrue(string value, int maximum)
    {
        var requirements = new PasswordRequirements() { UseRepeat = true, MaxRepeatSameCharacter = maximum };
        var test = new TestRepeat(requirements);
        var result = test.TestAndScore(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData("Maaaax3", 3)]
    [InlineData("MMMaaaxxxx3", 3)]
    public void MaximumRepeatFalse(string value, int maximum) 
    {
        var requirements = new PasswordRequirements() { UseRepeat = true, MaxRepeatSameCharacter = maximum };
        var test = new TestRepeat(requirements);
        var result = test.TestAndScore(value);

        Assert.False(result, test.FailureMessage);
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData("Need4", 4)]
    [InlineData("NeEd5", 5)]
    public void MinimumUniqueTrue(string value, int minimum)
    {
        var requirements = new PasswordRequirements() { UseUnique = true, MinUniqueCharacters = minimum };
        var test = new TestUnique(requirements);
        var result = test.TestAndScore(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("", 1)]
    [InlineData("Need5", 5)]
    [InlineData("NeEd6", 6)]
    public void MinimumUniqueFalse(string value, int minimum)
    {
        var requirements = new PasswordRequirements() { UseUnique = true, MinUniqueCharacters = minimum };
        var test = new TestUnique(requirements);
        var result = test.TestAndScore(value);

        Assert.False(result, test.FailureMessage);
    }

    [Theory]
    [InlineData("HASUPPERCASE")]
    public void RequireUppercaseTrue(string value)
    {
        var requirements = new PasswordRequirements() { UseUppercase = true, RequireUppercase = true };
        var test = new TestUppercase(requirements);
        var result = test.TestAndScore(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("nouppercase")]
    public void RequireUppercaseFalse(string value)
    {
        var requirements = new PasswordRequirements() { UseUppercase = true, RequireUppercase = true };
        var test = new TestUppercase(requirements);
        var result = test.TestAndScore(value);

        Assert.False(result, test.FailureMessage);
    }
}
