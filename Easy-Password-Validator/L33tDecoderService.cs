using Easy_Password_Validator.Enums;
using Easy_Password_Validator.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator
{
    /// <summary>
    /// Service that converts l33t encoded text to plain text
    /// </summary>
    public static class L33tDecoderService
    {
        /// <summary>
        /// Returns the full collection of l33t replacements for the specified level
        /// </summary>
        /// <param name="level">The dictionary level to return</param>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<L33tReplacement> GetReplacements(L33tLevel level)
        {
            if (level >= L33tLevel.Custom)
                throw new ArgumentException($"Must specify {nameof(L33tLevel)} lower than custom", nameof(level));

            var replacements = BasicReplacements
                .Concat(IntermediateReplacements)
                .Concat(AdvancedReplacements);

            return replacements
                .Where(x => x.Level <= level);
        }

        /// <summary>
        /// Returns the collection of l33t replacements for <see cref="L33tLevel.Basic"/> substitutions
        /// </summary>
        public static IEnumerable<L33tReplacement> BasicReplacements = new List<L33tReplacement>
        {
            new L33tReplacement() { PlainText = "a", L33tEncoded = "4", Level = L33tLevel.Basic, RunOrder = 70 },
            new L33tReplacement() { PlainText = "b", L33tEncoded = "8", Level = L33tLevel.Basic, RunOrder = 70 },
            new L33tReplacement() { PlainText = "c", L33tEncoded = "<", Level = L33tLevel.Basic, RunOrder = 80 }, // See K for full overlap: |<
            new L33tReplacement() { PlainText = "e", L33tEncoded = "3", Level = L33tLevel.Basic, RunOrder = 80 }, // B
            new L33tReplacement() { PlainText = "g", L33tEncoded = "6", Level = L33tLevel.Basic, RunOrder = 70 },
            new L33tReplacement() { PlainText = "i", L33tEncoded = "1", Level = L33tLevel.Basic, RunOrder = 70 },
            new L33tReplacement() { PlainText = "o", L33tEncoded = "0", Level = L33tLevel.Basic, RunOrder = 70 },
            new L33tReplacement() { PlainText = "s", L33tEncoded = "5", Level = L33tLevel.Basic, RunOrder = 70 },
            new L33tReplacement() { PlainText = "t", L33tEncoded = "7", Level = L33tLevel.Basic, RunOrder = 80 }, // Z
            new L33tReplacement() { PlainText = "z", L33tEncoded = "2", Level = L33tLevel.Basic, RunOrder = 80 } // R
        };

        /// <summary>
        /// Returns the collection of l33t replacements for <see cref="L33tLevel.Intermediate"/> substitutions (does not include <see cref="L33tLevel.Basic"/>)
        /// </summary>
        public static IEnumerable<L33tReplacement> IntermediateReplacements = new List<L33tReplacement>
        {
            new L33tReplacement() { PlainText = "a", L33tEncoded = "@", Level = L33tLevel.Intermediate, RunOrder = 40 },
            new L33tReplacement() { PlainText = "c", L33tEncoded = "[", Level = L33tLevel.Intermediate, RunOrder = 50 }, // G, H
            new L33tReplacement() { PlainText = "e", L33tEncoded = "£", Level = L33tLevel.Intermediate, RunOrder = 40 },
            new L33tReplacement() { PlainText = "i", L33tEncoded = "!", Level = L33tLevel.Intermediate, RunOrder = 40 },
            new L33tReplacement() { PlainText = "s", L33tEncoded = "$", Level = L33tLevel.Intermediate, RunOrder = 40 },
            new L33tReplacement() { PlainText = "t", L33tEncoded = "+", Level = L33tLevel.Intermediate, RunOrder = 50 }, // G
            new L33tReplacement() { PlainText = "x", L33tEncoded = "%", Level = L33tLevel.Intermediate, RunOrder = 40 },
            new L33tReplacement() { PlainText = "y", L33tEncoded = "¥", Level = L33tLevel.Intermediate, RunOrder = 40 }
        };

        /// <summary>
        /// Returns the collection of l33t replacements for <see cref="L33tLevel.Advanced"/> substitutions (does not include <see cref="L33tLevel.Basic"/> or <see cref="L33tLevel.Intermediate"/>)
        /// </summary>
        public static IEnumerable<L33tReplacement> AdvancedReplacements = new List<L33tReplacement>
        {
            new L33tReplacement() { PlainText = "a", L33tEncoded = @"/\", Level = L33tLevel.Advanced, RunOrder = 30 }, // M, N, W
            new L33tReplacement() { PlainText = "b", L33tEncoded = "|3", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "c", L33tEncoded = "{", Level = L33tLevel.Advanced, RunOrder = 20 }, // X
            new L33tReplacement() { PlainText = "d", L33tEncoded = "|)", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "e", L33tEncoded = "€", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "f", L33tEncoded = "|=", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "g", L33tEncoded = "[+", Level = L33tLevel.Advanced, RunOrder = 10 },

            new L33tReplacement() { PlainText = "h", L33tEncoded = "|-|", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "h", L33tEncoded = "[-]", Level = L33tLevel.Advanced, RunOrder = 10 },

            new L33tReplacement() { PlainText = "i", L33tEncoded = "|", Level = L33tLevel.Advanced, RunOrder = 35 }, // So many
            new L33tReplacement() { PlainText = "j", L33tEncoded = "_|", Level = L33tLevel.Advanced, RunOrder = 20 }, // U
            new L33tReplacement() { PlainText = "k", L33tEncoded = "|<", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "l", L33tEncoded = "|_", Level = L33tLevel.Advanced, RunOrder = 20 }, // U

            new L33tReplacement() { PlainText = "m", L33tEncoded = @"/\/\", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "m", L33tEncoded = "^^", Level = L33tLevel.Advanced, RunOrder = 10 },

            new L33tReplacement() { PlainText = "n", L33tEncoded = @"|\|", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "n", L33tEncoded = @"/\/", Level = L33tLevel.Advanced, RunOrder = 20 }, // M, W

            new L33tReplacement() { PlainText = "o", L33tEncoded = "()", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "p", L33tEncoded = "|*", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "q", L33tEncoded = "(,)", Level = L33tLevel.Advanced, RunOrder = 10 },

            new L33tReplacement() { PlainText = "r", L33tEncoded = "|2", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "r", L33tEncoded = "|?", Level = L33tLevel.Advanced, RunOrder = 10 },

            new L33tReplacement() { PlainText = "s", L33tEncoded = "§", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "t", L33tEncoded = "-|-", Level = L33tLevel.Advanced, RunOrder = 10 },

            new L33tReplacement() { PlainText = "u", L33tEncoded = "(_)", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "u", L33tEncoded = "|_|", Level = L33tLevel.Advanced, RunOrder = 10 },

            new L33tReplacement() { PlainText = "v", L33tEncoded = @"\/", Level = L33tLevel.Advanced, RunOrder = 30 }, // M, N, W
            new L33tReplacement() { PlainText = "w", L33tEncoded = @"\/\/", Level = L33tLevel.Advanced, RunOrder = 10 },

            new L33tReplacement() { PlainText = "x", L33tEncoded = "><", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "x", L33tEncoded = "}{", Level = L33tLevel.Advanced, RunOrder = 10 },

            new L33tReplacement() { PlainText = "y", L33tEncoded = "`/", Level = L33tLevel.Advanced, RunOrder = 10 },
            new L33tReplacement() { PlainText = "z", L33tEncoded = "7_", Level = L33tLevel.Advanced, RunOrder = 10 }
        };

        /// <summary>
        /// Converts the provided text from l33t format to plain text
        /// </summary>
        /// <param name="l33t">The encoded text to parse</param>
        /// <param name="level">Specifies the replacement level to use</param>
        /// <param name="replacements">The list to use when decoding the provided text with <see cref="L33tLevel.Custom"/></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<string> Decode(string l33t, L33tLevel level, IEnumerable<L33tReplacement> replacements = null)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(l33t))
                throw new ArgumentNullException(nameof(l33t), "Must provide l33t text to decode");

            if (level > L33tLevel.Custom)
                throw new ArgumentException($"Invalid {nameof(L33tLevel)} specified", nameof(level));

            if (level == L33tLevel.Custom && replacements == null)
                throw new ArgumentNullException(nameof(replacements), $"Must provide replacements list when using custom {nameof(L33tLevel)}");

            // Run appropriate decoder
            switch (level)
            {
                case L33tLevel.Basic:
                    return Decode(l33t, BasicReplacements);
                case L33tLevel.Intermediate:
                    return Decode(l33t, BasicReplacements.Concat(IntermediateReplacements));
                case L33tLevel.Advanced:
                    return Decode(l33t, BasicReplacements.Concat(IntermediateReplacements).Concat(AdvancedReplacements));
                case L33tLevel.Custom:
                    return Decode(l33t, replacements);
                default:
                    throw new ArgumentException($"Invalid {nameof(L33tLevel)} specified", nameof(level));
            };
        }

        /// <summary>
        /// Converts the provided text from l33t format to plain text
        /// </summary>
        /// <param name="l33t">The encoded text to parse</param>
        /// <param name="replacements">The list to use when decoding the provided text</param>
        private static IEnumerable<string> Decode(string l33t, IEnumerable<L33tReplacement> replacements)
        {
            // Prepare values for replacement
            var result = new List<string>();

            var replacementsList = replacements
                .OrderBy(x => x.RunOrder)
                .ToList();

            var multiples = replacementsList
                .GroupBy(x => x.L33tEncoded)
                .Where(x => x.Count() > 1)
                .SelectMany(x => x)
                .ToList();

            if (multiples.Count == 0)
                multiples.Add(replacementsList.First());

            // Run replacements
            foreach (var multiple in multiples)
            {
                var current = l33t;

                foreach (var replacement in replacementsList)
                {
                    if (replacement.PlainText == multiple.PlainText && replacement.L33tEncoded != multiple.L33tEncoded)
                        continue;

                    current = current.Replace(replacement.L33tEncoded, replacement.PlainText);
                }

                result.Add(current);
            }

            return result.Distinct().Where(x => x != l33t);
        }
    }
}
