# Easy Password Validator

[![MIT](https://img.shields.io/github/license/thirstyape/Easy-Password-Validator)](https://github.com/thirstyape/Easy-Password-Validator/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Easy.Password.Validator.svg)](https://www.nuget.org/packages/Easy.Password.Validator/)

This project was created to provide an easy to use and configurable password validation library. If the default configuration is sufficient for your needs the library can be used out of the box without further setup. However, if you have specific validation needs you can alter the library configuration settings and also provide custom validation methods.

There are two parts to this library: score checking and validation testing. Testing a password will perform both actions. The score checking will provide an overall score to a password which is the sum of all test scores. The validation testing will return whether a password passed or failed the tests.

The default implementation will check for the following:

* Contains digits
* Contains uppercase letters
* Contains lowercase letters
* Contains punctuation marks
* Checks password length
* Checks number of unique characters
* Checks for Qwerty keyboard patterns (ex. asDFr$)
* Checks for repeat characters (ex. tttttt)
* Checks password entropy (disabled by default)
* Checks if password is in top 100,000 bad password list
* Checks if decoded l33t versions of password are in top 10,000 bad password list

These checks will result in a pass or fail value being returned based on the provided password requirements. The score is not altered by the requirements.

## Getting Started

These instuctions can be used to acquire and implement the library.

### Installation

To use this library either clone a copy of the repository or check out the [NuGet package](https://www.nuget.org/packages/Easy.Password.Validator/)

### Usage

**Basic Example**

The following example provides a complete use case. This example makes use of the most basic configuration.

```
Console.WriteLine("Enter a username:");
var username = Console.ReadLine();

Console.WriteLine("Enter a password to test:");
var password = Console.ReadLine();

var passwordValidator = new PasswordValidatorService(new PasswordRequirements());

var pass = passwordValidator.TestAndScore(password, new string[] { username });

if (pass)
    Console.WriteLine($"Password passed validation with score: {passwordValidator.Score}");
else
    Console.WriteLine($"Password failed validation with score: {passwordValidator.Score}");

if (pass == false)
    foreach (var message in passwordValidator.FailureMessages)
        Console.WriteLine(message);
```

**Usage as a service in Web API**

The following example displays how to register the ```PasswordValidatorService``` as an injectable service.

Modify Startup.cs as follows:

```
public void ConfigureServices(IServiceCollection services) {
    // Your other startup items

    // Register password validator
    services.AddTransient(service => new PasswordValidatorService(new PasswordRequirements()));
}
```

Once that has been done you can simply pull a copy into your controllers as follows:

```
public class MyCustomController : ControllerBase {
    private readonly PasswordValidatorService PasswordValidator;

    public MyCustomController(PasswordValidatorService passwordValidatorService) {
        PasswordValidator = passwordValidatorService;
    }
}
```

**Usage in Blazor WebAssembly**

Since Blazor WebAssembly is not able to use asynchronous code in a synchronous manner, an extra step is required if use of the bad lists is desired. Note the call to ```.Initialize()```, this will perform the bad list loading with the ```await``` keyword.

```
Console.WriteLine("Enter a password to test:");
var password = Console.ReadLine();

var passwordValidator = new PasswordValidatorService(new PasswordRequirements());
await passwordValidator.Initialize();

var pass = passwordValidator.TestAndScore(password);

if (pass)
    Console.WriteLine($"Password passed validation with score: {passwordValidator.Score}");
else
    Console.WriteLine($"Password failed validation with score: {passwordValidator.Score}");
```

**Using custom configuration**

In the previous example, the call to ```new PasswordRequirements()``` was done inline in the service setup. However, it can be prepared beforehand and the validator will use different settings or you can create your own using ```IPasswordRequirements```.

```
var requirements = new PasswordRequirements()
{
    MinLength = 4,
    ExitOnFailure = true,
    RequireDigit = true,
    MinScore = 50
};

var passwordValidator = new PasswordValidatorService(requirements);
```

**Adding A Custom Tester**

The system also supports adding your own password tests that will run with the built-in ones. This is done by using ```IPasswordTest```.

The sample tester class.

```
private class TestWhiteSpace : IPasswordTest
{
    public int ScoreModifier { get; set; }
    public string FailureMessage { get; set; }
    public IPasswordRequirements Settings { get; set; }
    public IEnumerable<string> BadList { get; set; }

    public bool TestAndScore(string password)
    {
        // Reset
        FailureMessage = null;
        ScoreModifier = 0;

        // Check for digits
        var whitespace = password.Count(char.IsWhiteSpace);

        // Adjust score
        ScoreModifier = whitespace * 2;

        // Return result
        var pass = whitespace > 0;

        if (pass == false)
            FailureMessage = "Must have at least one space in password";

        return pass;
    }
}
```

Adding to validator service.

```
var passwordValidator = new PasswordValidatorService(new PasswordRequirements());

passwordValidator.AddTest(new TestWhiteSpace());
```

**Adding A Custom Pattern**

The pattern matcher checks for Qwerty keyboard patterns by default. You may add another instance of the pattern matcher with your own custom patterns (for example alphabetical order checking).

```
var requirements = new PasswordRequirements();
var passwordValidator = new PasswordValidatorService(requirements);

var pattern = new List<PatternMapItem>()
{
    new PatternMapItem() { RegularKey = 'a', ShiftKey = 'A', NeighborKeys = new char[] { 'b', 'B' } },
    new PatternMapItem() { RegularKey = 'b', ShiftKey = 'B', NeighborKeys = new char[] { 'a', 'A', 'c', 'C' } },
    new PatternMapItem() { RegularKey = 'c', ShiftKey = 'C', NeighborKeys = new char[] { 'b', 'B', 'd', 'D' } },
    new PatternMapItem() { RegularKey = 'd', ShiftKey = 'D', NeighborKeys = new char[] { 'c', 'C', 'e', 'E' } } // And so on
};

var test = new TestPattern(requirements, pattern);

passwordValidator.AddTest(test);
```

**Using A Custom L33T Dictionary**

If desired, a custom l33t decoding dictionary may be used, or you may extend or modify the built in dictionary.

```
var passwordValidator = new PasswordValidatorService(new PasswordRequirements());
var l33TReplacements = L33tDecoderService.GetReplacements(L33tLevel.Advanced).ToList();

l33TReplacements.Add(new L33tReplacement() { PlainText = "h", L33tEncoded = "|~|", RunOrder = 15 });
l33TReplacements.Add(new L33tReplacement() { PlainText = "h", L33tEncoded = "]~[", RunOrder = 15 });

passwordValidator.UpdateL33tReplacements(l33TReplacements);
```

The run order for l33t replacements has been laid out as follows, and you may select any run order for your custom replacements.

* 10 ```L33tLevel.Advanced```, not contained in any other replacements
* 20 ```L33tLevel.Advanced```, contained in another replacement at RunOrder 10
* 30 ```L33tLevel.Advanced```, contained in at least one other replacement at RunOrder 10 or 20
* 40 ```L33tLevel.Intermediate```, not contained in any other replacements
* 50 ```L33tLevel.Intermediate```, contained in at least one other replacement at RunOrder 10 or 40
* 60 ```L33tLevel.Intermediate```, contained in at least one other replacement at RunOrder 10, 20, 40, or 50
* 70 ```L33tLevel.Basic```, not contained in any other replacements
* 80 ```L33tLevel.Basic```, contained in at least one other replacement at RunOrder 10, 40, or 70
* 90 ```L33tLevel.Basic```, contained in at least one other replacement at RunOrder 10, 20, 40, 50, 70, or 80

**Using error messages in another language**

Error messages are provided using .RESX files and are currently available in the following languages:

* English (en, en-CA, en-US)
* French (fr)
* German (de)
* Italian (it)
* Romanian (ro)

By default error messages will be returned based on the language of the operating system (defaults to English if specified language is not available). To choose a specific language enter the language code in the ```.TestAndScore()``` method. Language codes are either 2 or 5 characters in length (ex. en, en-US, de, de-DE).

```
Console.WriteLine("Enter a username:");
var username = Console.ReadLine();

Console.WriteLine("Enter a password to test:");
var password = Console.ReadLine();

var passwordValidator = new PasswordValidatorService(new PasswordRequirements());

var pass = passwordValidator.TestAndScore(password, new string[] { username }, "de");
```

To add another language you must use the Git repository, the NuGet package does not support adding languages. To do so add a .RESX file with the proper language code and update the ```.CheckValidLanguage()``` method in ```PasswordValidatorService.cs```. To simplify the creation of the .RESX files you can use [Zeta Resource Editor](https://www.zeta-resource-editor.com/index.html) with the .zeproj file included in the repository.

If you do add support for a language that is not bundled here, please let me know or fork the repository so I can update the master branch and NuGet package. Looking for high-quality translations only, i.e. no copy paste from translation services.

## Authors

* **Nathanael Frey**

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

Parts of this library have been inspired by:
* [zxcvbn](https://github.com/dropbox/zxcvbn)
* [The Password Meter](http://www.passwordmeter.com)

Thank you to:
* Daniel Miessler for the [SecLists](https://github.com/danielmiessler/SecLists) project
* [Kmg Design](https://www.iconfinder.com/kmgdesignid) for the project icon
* [Uwe Keim](https://github.com/UweKeim) for help with translation system and German translation