using Easy_Password_Validator;
using Easy_Password_Validator.Models;

namespace Easy_Password_Validator_Test;

/// <summary>
/// Unit tests run on the <see cref="PasswordValidatorService"/>
/// </summary>
public class PasswordValidatorTests
{
    [Fact]
    public void CheckConstructorThrows()
    {
        Assert.Throws<ArgumentNullException>("passwordRequirements", () => new PasswordValidatorService(null));
    }

    [Theory]
    [InlineData("")]
    public void CheckPasswordWithDefaultsThrows(string value)
    {
        var requirements = new PasswordRequirements();
        var validator = new PasswordValidatorService(requirements);

        Assert.Throws<ArgumentNullException>("password", () => validator.TestAndScore(value));
    }

    [Theory]
    [InlineData("Eight910")]
    public void CheckPasswordWithDefaultsTrue(string value)
    {
        var requirements = new PasswordRequirements();
        var validator = new PasswordValidatorService(requirements);
        var result = validator.TestAndScore(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("password")]
    [InlineData("p@sSw0rD")]
    [InlineData("qwert001")]
    [InlineData("sevnn77")]
    [InlineData("aflaflafl")]
    [InlineData("xxxx6780")]
    public void CheckPasswordWithDefaultsFalse(string value)
    {
        var requirements = new PasswordRequirements();
        var validator = new PasswordValidatorService(requirements);
        var result = validator.TestAndScore(value);

        Assert.False(result, string.Join(' ', validator.FailureMessages));
    }

    [Theory]
    [InlineData("Nine.10!")]
    public void CheckPasswordWithRequirementsTrue(string value)
    {
        var requirements = new PasswordRequirements() 
        {
            RequireDigit = true,
            RequireLowercase = true,
            RequirePunctuation = true,
            RequireUppercase = true,
            UseLength = false,
            UsePattern = false,
            UseRepeat = false,
            UseUnique = false,
            UseBadList = false,
            MinScore = 0
        };

        var validator = new PasswordValidatorService(requirements);
        var result = validator.TestAndScore(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("Nine.ten!")]
    [InlineData("NINE.10!")]
    [InlineData("nine.10!")]
    [InlineData("Nine10Eleven")]
    public void CheckPasswordWithRequirementsFalse(string value)
    {
        var requirements = new PasswordRequirements()
        {
            RequireDigit = true,
            RequireLowercase = true,
            RequirePunctuation = true,
            RequireUppercase = true,
            UseLength = false,
            UsePattern = false,
            UseRepeat = false,
            UseUnique = false,
            UseBadList = false,
            MinScore = 0
        };

        var validator = new PasswordValidatorService(requirements);
        var result = validator.TestAndScore(value);

        Assert.False(result, string.Join(' ', validator.FailureMessages));
    }

    [Theory]
    [InlineData("N0t.2.Bad!")]
    [InlineData("This one is 100x better!")]
    public void CheckPasswordScoreOnlyTrue(string value)
    {
        var requirements = new PasswordRequirements()
        {
            MinScore = 100,
            MinLength = 1,
            MinUniqueCharacters = 1,
            MaxRepeatSameCharacter = 10,
            MaxNeighboringCharacter = 10,
        };

        var validator = new PasswordValidatorService(requirements);
        var result = validator.TestAndScore(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("score low")]
    [InlineData("11111qwerty999888")]
    [InlineData("!_!")]
    public void CheckPasswordScoreOnlyFalse(string value) 
    {
        var requirements = new PasswordRequirements()
        {
            MinScore = 100,
            MinLength = 1,
            MinUniqueCharacters = 1,
            MaxRepeatSameCharacter = 10,
            MaxNeighboringCharacter = 10,
        };

        var validator = new PasswordValidatorService(requirements);
        var result = validator.TestAndScore(value);

        Assert.False(result, string.Join(' ', validator.FailureMessages));
    }

    [Theory]
    [InlineData("N3eD.éNtr0py!")]
    [InlineData("This one is 100x better!")]
    public void CheckPasswordEntropyOnlyTrue(string value)
    {
        var requirements = new PasswordRequirements()
        {
            MinScore = 0,
            MinLength = 1,
            MinUniqueCharacters = 1,
            MaxRepeatSameCharacter = 10,
            MaxNeighboringCharacter = 10,
            MinEntropy = 3.5F,
            UseEntropy = true
        };

        var validator = new PasswordValidatorService(requirements);
        var result = validator.TestAndScore(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("score low")]
    [InlineData("11111qwerty999888")]
    [InlineData("!_!")]
    [InlineData("N3eD éNtr0py")]
    public void CheckPasswordEntropyOnlyFalse(string value)
    {
        var requirements = new PasswordRequirements()
        {
            MinScore = 0,
            MinLength = 1,
            MinUniqueCharacters = 1,
            MaxRepeatSameCharacter = 10,
            MaxNeighboringCharacter = 10,
            MinEntropy = 3.5F,
            UseEntropy = true
        };

        var validator = new PasswordValidatorService(requirements);
        var result = validator.TestAndScore(value);

        Assert.False(result, string.Join(' ', validator.FailureMessages));
    }

    private static readonly List<string> UserData =
    [
        "First",
        "Last",
        "Company",
        "123 Fake St",
        "Anytown",
        "first.last@company.com"
    ];

    [Theory]
    [InlineData("123 Fake St first.last")]
    public void CheckPasswordWithUserDataTrue(string value)
    {
        var requirements = new PasswordRequirements();
        var validator = new PasswordValidatorService(requirements);
        var result = validator.TestAndScore(value, UserData);

        Assert.True(result);
    }

    [Theory]
    [InlineData("123 fake st")]
    [InlineData("first.last")]
    public void CheckPasswordWithUserDataFalse(string value)
    {
        var requirements = new PasswordRequirements();
        var validator = new PasswordValidatorService(requirements);
        var result = validator.TestAndScore(value, UserData);

        Assert.False(result, string.Join(' ', validator.FailureMessages));
    }
}
