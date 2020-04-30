# Easy Password Validator

This project was created to provide an easy to use and configurable password validation library. If the default configuration is sufficient for your needs the library can be used out of the box without further setup. However, if you have specific validation needs you can alter the library configuration settings and also provide custom validation methods.

## Getting Started

These instuctions can be used to acquire and implement the library.

### Installation

To use this library either clone a copy of the repository or check out the NuGet package

### Usage

There are two main usage methods for this library: score checking and validation testing. The score checking methods will run tests on the provided password and return its total score. The validation testing methods will run tests on the provided password and return whether it passed or failed the tests.

**Score Checking**

```
PasswordValidatorService.CheckScore("");
```

**Validation Testing**

```
PasswordValidatorService.TestPassword("");
```

## Authors

* **Nathanael Frey**

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

* Parts of this library have been inspired by [zxcvbn](https://github.com/dropbox/zxcvbn)
* Other parts have been inspired by [The Password Meter](http://www.passwordmeter.com)
* Thank you to Daniel Miessler for the [SecLists](https://github.com/danielmiessler/SecLists/tree/master/Passwords) project