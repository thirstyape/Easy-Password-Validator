using Easy_Password_Validator.Models;
using System.Collections.Generic;
using System.Linq;

namespace Easy_Password_Validator
{
    /// <summary>
    /// Class to detect keyboard or user-supplied character patterns in a string
    /// </summary>
    public static class PatternMapService
    {
        /// <summary>
        /// Checks two characters to see whether they are neighbors (using Qwerty map)
        /// </summary>
        /// <param name="key">The regular or shift key to lookup</param>
        /// <param name="neighbor">The neighbor to look for</param>
        public static bool IsNeighbor(char key, char neighbor)
        {
            return IsNeighbor(key, neighbor, QwertyMap);
        }

        /// <summary>
        /// Checks two characters to see whether they are neighbors
        /// </summary>
        /// <param name="key">The regular or shift key to lookup</param>
        /// <param name="neighbor">The neighbor to look for</param>
        /// <param name="map">A custom map to use</param>
        public static bool IsNeighbor(char key, char neighbor, List<PatternMapItem> map)
        {
            return map.Any(x =>
                (x.RegularKey == key || x.ShiftKey == key) &&
                x.NeighborKeys.Contains(neighbor)
            );
        }

        /// <summary>
        /// Returns the specified map item provided it exists (using Qwerty map)
        /// </summary>
        /// <param name="key">The regular or shift key to return</param>
        public static PatternMapItem GetMapItem(char key)
        {
            return GetMapItem(key, QwertyMap);
        }

        /// <summary>
        /// Returns the specified map item provided it exists
        /// </summary>
        /// <param name="key">The regular or shift key to return</param>
        /// <param name="map">A custom map to use</param>
        public static PatternMapItem GetMapItem(char key, List<PatternMapItem> map)
        {
            return map.SingleOrDefault(x => x.RegularKey == key || x.ShiftKey == key);
        }

        /// <summary>
        /// Checks a string and returns any found patterns (using Qwerty map)
        /// </summary>
        /// <param name="test">The string to check</param>
        public static IEnumerable<string> GetPatterns(string test)
        {
            return GetPatterns(test, QwertyMap);
        }

        /// <summary>
        /// Checks a string and returns any found patterns
        /// </summary>
        /// <param name="test">The string to check</param>
        /// <param name="map">A custom map to use</param>
        public static IEnumerable<string> GetPatterns(string test, List<PatternMapItem> map)
        {
            var patterns = new List<string>();
            var current = string.Empty;

            for (var i = 0; i < test.Length - 1; i++)
            {
                var pass = IsNeighbor(test[i], test[i + 1], map);

                if (pass && current.Length == 0)
                    current = test[i].ToString();

                if (pass)
                    current += test[i + 1];

                if (pass == false && current.Length > 1)
                    patterns.Add(current);

                if (pass == false)
                    current = string.Empty;
            }

            if (current.Length > 1)
                patterns.Add(current);

            return patterns;
        }

        /// <summary>
        /// Checks to see whether the specified string is a pattern (using Qwerty map)
        /// </summary>
        /// <param name="test">The string to check</param>
        public static bool IsPattern(string test)
        {
            return IsPattern(test, QwertyMap);
        }

        /// <summary>
        /// Checks to see whether the specified string is a pattern
        /// </summary>
        /// <param name="test">The string to check</param>
        /// <param name="map">A custom map to use</param>
        public static bool IsPattern(string test, List<PatternMapItem> map)
        {
            var result = true;
            var last = default(char);

            foreach (var c in test)
            {
                if (last != default(char) && IsNeighbor(c, last, map) == false)
                    return false;

                last = c;
            }

            return result;
        }

        /// <summary>
        /// The default QWERTY keyboard pattern map
        /// </summary>
        public static readonly List<PatternMapItem> QwertyMap = new List<PatternMapItem>()
        {
            new PatternMapItem() { RegularKey = '`', ShiftKey = '~', NeighborKeys = new char[] { '1', '!' } },
            new PatternMapItem() { RegularKey = '1', ShiftKey = '!', NeighborKeys = new char[] { '`', '2', 'q', '~', '@', 'Q' } },
            new PatternMapItem() { RegularKey = '2', ShiftKey = '@', NeighborKeys = new char[] { '1', '3', 'q', 'w', '!', '#', 'Q', 'W' } },
            new PatternMapItem() { RegularKey = '3', ShiftKey = '#', NeighborKeys = new char[] { '2', '4', 'w', 'e', '@', '$', 'W', 'E' } },
            new PatternMapItem() { RegularKey = '4', ShiftKey = '$', NeighborKeys = new char[] { '3', '5', 'e', 'r', '#', '%', 'E', 'R' } },
            new PatternMapItem() { RegularKey = '5', ShiftKey = '%', NeighborKeys = new char[] { '4', '6', 'r', 't', '$', '^', 'R', 'T' } },
            new PatternMapItem() { RegularKey = '6', ShiftKey = '^', NeighborKeys = new char[] { '5', '7', 't', 'y', '%', '&', 'T', 'Y' } },
            new PatternMapItem() { RegularKey = '7', ShiftKey = '&', NeighborKeys = new char[] { '6', '8', 'y', 'u', '^', '*', 'Y', 'U' } },
            new PatternMapItem() { RegularKey = '8', ShiftKey = '*', NeighborKeys = new char[] { '7', '9', 'u', 'i', '&', '(', 'U', 'I' } },
            new PatternMapItem() { RegularKey = '9', ShiftKey = '(', NeighborKeys = new char[] { '8', '0', 'i', 'o', '*', ')', 'I', 'O' } },
            new PatternMapItem() { RegularKey = '0', ShiftKey = ')', NeighborKeys = new char[] { '9', '-', 'o', 'p', '(', '_', 'O', 'P' } },
            new PatternMapItem() { RegularKey = '-', ShiftKey = '_', NeighborKeys = new char[] { '0', '=', 'p', '[', ')', '+', 'P', '{' } },
            new PatternMapItem() { RegularKey = '=', ShiftKey = '+', NeighborKeys = new char[] { '-', '[', ']', '_', '{', '}' } },

            new PatternMapItem() { RegularKey = 'q', ShiftKey = 'Q', NeighborKeys = new char[] { '1', '2', 'w', 'a', '!', '@', 'W', 'A' } },
            new PatternMapItem() { RegularKey = 'w', ShiftKey = 'W', NeighborKeys = new char[] { '2', '3', 'q', 'e', 'a', 's', '@', '#', 'Q', 'E', 'A', 'S' } },
            new PatternMapItem() { RegularKey = 'e', ShiftKey = 'E', NeighborKeys = new char[] { '3', '4', 'w', 'r', 's', 'd', '#', '$', 'W', 'R', 'S', 'D' } },
            new PatternMapItem() { RegularKey = 'r', ShiftKey = 'R', NeighborKeys = new char[] { '4', '5', 'e', 't', 'd', 'f', '$', '%', 'E', 'T', 'D', 'F' } },
            new PatternMapItem() { RegularKey = 't', ShiftKey = 'T', NeighborKeys = new char[] { '5', '6', 'r', 'y', 'f', 'g', '%', '^', 'R', 'Y', 'F', 'G' } },
            new PatternMapItem() { RegularKey = 'y', ShiftKey = 'Y', NeighborKeys = new char[] { '6', '7', 't', 'u', 'g', 'h', '^', '&', 'T', 'U', 'G', 'H' } },
            new PatternMapItem() { RegularKey = 'u', ShiftKey = 'U', NeighborKeys = new char[] { '7', '8', 'y', 'i', 'h', 'j', '&', '*', 'Y', 'I', 'H', 'J' } },
            new PatternMapItem() { RegularKey = 'i', ShiftKey = 'I', NeighborKeys = new char[] { '8', '9', 'u', 'o', 'j', 'k', '*', '(', 'U', 'O', 'J', 'K' } },
            new PatternMapItem() { RegularKey = 'o', ShiftKey = 'O', NeighborKeys = new char[] { '9', '0', 'i', 'p', 'k', 'l', '(', ')', 'I', 'P', 'K', 'L' } },
            new PatternMapItem() { RegularKey = 'p', ShiftKey = 'P', NeighborKeys = new char[] { '0', '-', 'o', '[', 'l', ';', ')', '_', 'O', '{', 'L', ':' } },
            new PatternMapItem() { RegularKey = '[', ShiftKey = '{', NeighborKeys = new char[] { '-', '=', 'p', ']', ';', '\'', '_', '+', 'P', '}', ':', '"' } },
            new PatternMapItem() { RegularKey = ']', ShiftKey = '}', NeighborKeys = new char[] { '=', '[', '\\', '\'', '+', '{', '|', '"' } },
            new PatternMapItem() { RegularKey = '\\', ShiftKey = '|', NeighborKeys = new char[] { ']', '}' } },

            new PatternMapItem() { RegularKey = 'a', ShiftKey = 'A', NeighborKeys = new char[] { 'q', 'w', 's', 'z', 'Q', 'W', 'S', 'Z' } },
            new PatternMapItem() { RegularKey = 's', ShiftKey = 'S', NeighborKeys = new char[] { 'w', 'e', 'a', 'd', 'z', 'x', 'W', 'E', 'A', 'D', 'Z', 'X' } },
            new PatternMapItem() { RegularKey = 'd', ShiftKey = 'D', NeighborKeys = new char[] { 'e', 'r', 's', 'f', 'x', 'c', 'E', 'R', 'S', 'F', 'X', 'C' } },
            new PatternMapItem() { RegularKey = 'f', ShiftKey = 'F', NeighborKeys = new char[] { 'r', 't', 'd', 'g', 'c', 'v', 'R', 'T', 'D', 'G', 'C', 'V' } },
            new PatternMapItem() { RegularKey = 'g', ShiftKey = 'G', NeighborKeys = new char[] { 't', 'y', 'f', 'h', 'v', 'b', 'T', 'Y', 'F', 'H', 'V', 'B' } },
            new PatternMapItem() { RegularKey = 'h', ShiftKey = 'H', NeighborKeys = new char[] { 'y', 'u', 'g', 'j', 'b', 'n', 'Y', 'U', 'G', 'J', 'B', 'N' } },
            new PatternMapItem() { RegularKey = 'j', ShiftKey = 'J', NeighborKeys = new char[] { 'u', 'i', 'h', 'k', 'n', 'm', 'U', 'I', 'H', 'K', 'N', 'M' } },
            new PatternMapItem() { RegularKey = 'k', ShiftKey = 'K', NeighborKeys = new char[] { 'i', 'o', 'j', 'l', 'm', ',', 'I', 'O', 'J', 'L', 'M', '<' } },
            new PatternMapItem() { RegularKey = 'l', ShiftKey = 'L', NeighborKeys = new char[] { 'o', 'p', 'k', ';', ',', '.', 'O', 'P', 'K', ':', '<', '>' } },
            new PatternMapItem() { RegularKey = ';', ShiftKey = ':', NeighborKeys = new char[] { 'p', '[', 'l', '\'', '.', '/', 'P', '{', 'L', '"', '>', '?' } },
            new PatternMapItem() { RegularKey = '\'', ShiftKey = '"', NeighborKeys = new char[] { '[', ']', ';', '/', '{', '}', ':', '?' } },

            new PatternMapItem() { RegularKey = 'z', ShiftKey = 'Z', NeighborKeys = new char[] { 'a', 's', 'x', 'A', 'S', 'X' } },
            new PatternMapItem() { RegularKey = 'x', ShiftKey = 'X', NeighborKeys = new char[] { 's', 'd', 'z', 'c', ' ', 'S', 'D', 'Z', 'C' } },
            new PatternMapItem() { RegularKey = 'c', ShiftKey = 'C', NeighborKeys = new char[] { 'd', 'f', 'x', 'v', ' ', 'D', 'F', 'X', 'V' } },
            new PatternMapItem() { RegularKey = 'v', ShiftKey = 'V', NeighborKeys = new char[] { 'f', 'g', 'c', 'b', ' ', 'F', 'G', 'C', 'B' } },
            new PatternMapItem() { RegularKey = 'b', ShiftKey = 'B', NeighborKeys = new char[] { 'g', 'h', 'v', 'n', ' ', 'G', 'H', 'V', 'N' } },
            new PatternMapItem() { RegularKey = 'n', ShiftKey = 'N', NeighborKeys = new char[] { 'h', 'j', 'b', 'm', ' ', 'H', 'J', 'B', 'M' } },
            new PatternMapItem() { RegularKey = 'm', ShiftKey = 'M', NeighborKeys = new char[] { 'j', 'k', 'n', ',', ' ', 'J', 'K', 'N', '<' } },
            new PatternMapItem() { RegularKey = ',', ShiftKey = '<', NeighborKeys = new char[] { 'k', 'l', 'm', '.', ' ', 'K', 'L', 'M', '>' } },
            new PatternMapItem() { RegularKey = '.', ShiftKey = '>', NeighborKeys = new char[] { 'l', ';', ',', '/', 'L', ':', '<', '?' } },
            new PatternMapItem() { RegularKey = '/', ShiftKey = '?', NeighborKeys = new char[] { ';', '\'', '.', ':', '"', '>' } },

            new PatternMapItem() { RegularKey = ' ', ShiftKey = ' ', NeighborKeys = new char[] { 'x', 'c', 'v', 'b', 'n', 'm', ',', 'X', 'C', 'V', 'B', 'N', 'M', '<' } }
        };

        /// <summary>
        /// The default QWERTZ keyboard pattern map
        /// </summary>
        public static readonly List<PatternMapItem> QwertzMap = new List<PatternMapItem>()
        {
            new PatternMapItem() { RegularKey = '^', ShiftKey = 'º', NeighborKeys = new char[] { '1', '!' } },
            new PatternMapItem() { RegularKey = '1', ShiftKey = '!', NeighborKeys = new char[] { '^', '2', 'q', 'º', '"', 'Q' } },
            new PatternMapItem() { RegularKey = '2', ShiftKey = '"', NeighborKeys = new char[] { '1', '3', 'q', 'w', '!', '§', 'Q', 'W' } },
            new PatternMapItem() { RegularKey = '3', ShiftKey = '§', NeighborKeys = new char[] { '2', '4', 'w', 'e', '"', '$', 'W', 'E' } },
            new PatternMapItem() { RegularKey = '4', ShiftKey = '$', NeighborKeys = new char[] { '3', '5', 'e', 'r', '§', '%', 'E', 'R' } },
            new PatternMapItem() { RegularKey = '5', ShiftKey = '%', NeighborKeys = new char[] { '4', '6', 'r', 't', '$', '&', 'R', 'T' } },
            new PatternMapItem() { RegularKey = '6', ShiftKey = '&', NeighborKeys = new char[] { '5', '7', 't', 'z', '%', '/', 'T', 'Z' } },
            new PatternMapItem() { RegularKey = '7', ShiftKey = '/', NeighborKeys = new char[] { '6', '8', 'z', 'u', '&', '(', 'Z', 'U' } },
            new PatternMapItem() { RegularKey = '8', ShiftKey = '(', NeighborKeys = new char[] { '7', '9', 'u', 'i', '/', ')', 'U', 'I' } },
            new PatternMapItem() { RegularKey = '9', ShiftKey = ')', NeighborKeys = new char[] { '8', '0', 'i', 'o', '(', '=', 'I', 'O' } },
            new PatternMapItem() { RegularKey = '0', ShiftKey = '=', NeighborKeys = new char[] { '9', 'ß', 'o', 'p', ')', '?', 'O', 'P' } },
            new PatternMapItem() { RegularKey = 'ß', ShiftKey = '?', NeighborKeys = new char[] { '0', '´', 'p', 'ü', '=', '`', 'P', 'Ü' } },
            new PatternMapItem() { RegularKey = '´', ShiftKey = '`', NeighborKeys = new char[] { 'ß', 'ü', '+', '?', 'Ü', '*' } },

            new PatternMapItem() { RegularKey = 'q', ShiftKey = 'Q', NeighborKeys = new char[] { '1', '2', 'w', 'a', '!', '"', 'W', 'A' } },
            new PatternMapItem() { RegularKey = 'w', ShiftKey = 'W', NeighborKeys = new char[] { '2', '3', 'q', 'e', 'a', 's', '"', '§', 'Q', 'E', 'A', 'S' } },
            new PatternMapItem() { RegularKey = 'e', ShiftKey = 'E', NeighborKeys = new char[] { '3', '4', 'w', 'r', 's', 'd', '§', '$', 'W', 'R', 'S', 'D' } },
            new PatternMapItem() { RegularKey = 'r', ShiftKey = 'R', NeighborKeys = new char[] { '4', '5', 'e', 't', 'd', 'f', '$', '%', 'E', 'T', 'D', 'F' } },
            new PatternMapItem() { RegularKey = 't', ShiftKey = 'T', NeighborKeys = new char[] { '5', '6', 'r', 'z', 'f', 'g', '%', '&', 'R', 'Z', 'F', 'G' } },
            new PatternMapItem() { RegularKey = 'z', ShiftKey = 'Z', NeighborKeys = new char[] { '6', '7', 't', 'u', 'g', 'h', '&', '/', 'T', 'U', 'G', 'H' } },
            new PatternMapItem() { RegularKey = 'u', ShiftKey = 'U', NeighborKeys = new char[] { '7', '8', 'z', 'i', 'h', 'j', '/', '(', 'Z', 'I', 'H', 'J' } },
            new PatternMapItem() { RegularKey = 'i', ShiftKey = 'I', NeighborKeys = new char[] { '8', '9', 'u', 'o', 'j', 'k', '(', ')', 'U', 'O', 'J', 'K' } },
            new PatternMapItem() { RegularKey = 'o', ShiftKey = 'O', NeighborKeys = new char[] { '9', '0', 'i', 'p', 'k', 'l', ')', '=', 'I', 'P', 'K', 'L' } },
            new PatternMapItem() { RegularKey = 'p', ShiftKey = 'P', NeighborKeys = new char[] { '0', 'ß', 'o', 'ü', 'l', 'ö', '=', '?', 'O', 'Ü', 'L', 'Ö' } },
            new PatternMapItem() { RegularKey = 'ü', ShiftKey = 'Ü', NeighborKeys = new char[] { 'ß', '´', 'p', '+', 'ö', 'ä', '?', '`', 'P', '*', 'Ö', 'Ä' } },
            new PatternMapItem() { RegularKey = '+', ShiftKey = '*', NeighborKeys = new char[] { '´', 'ü', 'ä', '#', '`', 'Ü', 'Ä', '\'' } },

            new PatternMapItem() { RegularKey = 'a', ShiftKey = 'A', NeighborKeys = new char[] { 'q', 'w', 's', '>', 'Q', 'W', 'S', '<' } },
            new PatternMapItem() { RegularKey = 's', ShiftKey = 'S', NeighborKeys = new char[] { 'w', 'e', 'a', 'd', '>', 'y', 'W', 'E', 'A', 'D', '<', 'Y' } },
            new PatternMapItem() { RegularKey = 'd', ShiftKey = 'D', NeighborKeys = new char[] { 'e', 'r', 's', 'f', 'y', 'x', 'E', 'R', 'S', 'F', 'Y', 'X' } },
            new PatternMapItem() { RegularKey = 'f', ShiftKey = 'F', NeighborKeys = new char[] { 'r', 't', 'd', 'g', 'x', 'c', 'R', 'T', 'D', 'G', 'X', 'C' } },
            new PatternMapItem() { RegularKey = 'g', ShiftKey = 'G', NeighborKeys = new char[] { 't', 'z', 'f', 'h', 'c', 'v', 'T', 'Z', 'F', 'H', 'C', 'V' } },
            new PatternMapItem() { RegularKey = 'h', ShiftKey = 'H', NeighborKeys = new char[] { 'z', 'u', 'g', 'j', 'v', 'b', 'Z', 'U', 'G', 'J', 'V', 'B' } },
            new PatternMapItem() { RegularKey = 'j', ShiftKey = 'J', NeighborKeys = new char[] { 'u', 'i', 'h', 'k', 'b', 'n', 'U', 'I', 'H', 'K', 'B', 'N' } },
            new PatternMapItem() { RegularKey = 'k', ShiftKey = 'K', NeighborKeys = new char[] { 'i', 'o', 'j', 'l', 'n', 'm', 'I', 'O', 'J', 'L', 'N', 'M' } },
            new PatternMapItem() { RegularKey = 'l', ShiftKey = 'L', NeighborKeys = new char[] { 'o', 'p', 'k', 'ö', 'm', ',', 'O', 'P', 'K', 'Ö', 'M', ';' } },
            new PatternMapItem() { RegularKey = 'ö', ShiftKey = 'Ö', NeighborKeys = new char[] { 'p', 'ü', 'l', 'ä', ',', '.', 'P', 'Ü', 'L', 'Ä', ';', ':' } },
            new PatternMapItem() { RegularKey = 'ä', ShiftKey = 'Ä', NeighborKeys = new char[] { 'ü', '+', 'ö', '#', '.', '-', 'Ü', '*', 'Ö', '\'', ':', '_' } },
            new PatternMapItem() { RegularKey = '#', ShiftKey = '\'', NeighborKeys = new char[] { '+', 'ä', '-', '*', 'Ä', '_' } },

            new PatternMapItem() { RegularKey = '<', ShiftKey = '>', NeighborKeys = new char[] { 'a', 's', 'y', 'A', 'S', 'Y' } },
            new PatternMapItem() { RegularKey = 'y', ShiftKey = 'Y', NeighborKeys = new char[] { 's', 'd', '<', 'x', ' ', 'S', 'D', '>', 'X' } },
            new PatternMapItem() { RegularKey = 'x', ShiftKey = 'X', NeighborKeys = new char[] { 'd', 'f', 'y', 'c', ' ', 'D', 'F', 'Y', 'C' } },
            new PatternMapItem() { RegularKey = 'c', ShiftKey = 'C', NeighborKeys = new char[] { 'f', 'g', 'x', 'v', ' ', 'F', 'G', 'X', 'V' } },
            new PatternMapItem() { RegularKey = 'v', ShiftKey = 'V', NeighborKeys = new char[] { 'g', 'h', 'c', 'b', ' ', 'G', 'H', 'C', 'B' } },
            new PatternMapItem() { RegularKey = 'b', ShiftKey = 'B', NeighborKeys = new char[] { 'h', 'j', 'v', 'n', ' ', 'H', 'J', 'V', 'N' } },
            new PatternMapItem() { RegularKey = 'n', ShiftKey = 'N', NeighborKeys = new char[] { 'j', 'k', 'b', 'm', ' ', 'J', 'K', 'B', 'M' } },
            new PatternMapItem() { RegularKey = 'm', ShiftKey = 'M', NeighborKeys = new char[] { 'k', 'l', 'n', ',', ' ', 'K', 'L', 'N', ';' } },
            new PatternMapItem() { RegularKey = ',', ShiftKey = ';', NeighborKeys = new char[] { 'l', 'ö', 'm', '.', 'L', 'Ö', 'M', ':' } },
            new PatternMapItem() { RegularKey = '.', ShiftKey = ':', NeighborKeys = new char[] { 'ö', 'ä', ',', '-', 'Ö', 'Ä', ';', '_' } },
            new PatternMapItem() { RegularKey = '-', ShiftKey = '_', NeighborKeys = new char[] { 'ä', '#', '.', 'Ä', '\'', ':' } },

            new PatternMapItem() { RegularKey = ' ', ShiftKey = ' ', NeighborKeys = new char[] { 'x', 'c', 'v', 'b', 'n', 'm', ',', 'X', 'C', 'V', 'B', 'N', 'M', ';' } }
        };

        /// <summary>
        /// The default AZERTY keyboard pattern map
        /// </summary>
        public static readonly List<PatternMapItem> AzertyMap = new List<PatternMapItem>()
        {
            new PatternMapItem() { RegularKey = '²', ShiftKey = '²', NeighborKeys = new char[] { '1', '&' } },
            new PatternMapItem() { RegularKey = '1', ShiftKey = '&', NeighborKeys = new char[] { '²', '2', 'a', 'é', 'A' } },
            new PatternMapItem() { RegularKey = '2', ShiftKey = 'é', NeighborKeys = new char[] { '1', '3', 'a', 'z', '&', '"', 'A', 'Z' } },
            new PatternMapItem() { RegularKey = '3', ShiftKey = '"', NeighborKeys = new char[] { '2', '4', 'z', 'e', 'é', '\'', 'Z', 'E' } },
            new PatternMapItem() { RegularKey = '4', ShiftKey = '\'', NeighborKeys = new char[] { '3', '5', 'e', 'r', '"', '(', 'E', 'R' } },
            new PatternMapItem() { RegularKey = '5', ShiftKey = '(', NeighborKeys = new char[] { '4', '6', 'r', 't', '\'', '-', 'R', 'T' } },
            new PatternMapItem() { RegularKey = '6', ShiftKey = '-', NeighborKeys = new char[] { '5', '7', 't', 'y', '(', 'è', 'T', 'Y' } },
            new PatternMapItem() { RegularKey = '7', ShiftKey = 'è', NeighborKeys = new char[] { '6', '8', 'y', 'u', '-', '_', 'Y', 'U' } },
            new PatternMapItem() { RegularKey = '8', ShiftKey = '_', NeighborKeys = new char[] { '7', '9', 'u', 'i', 'è', 'ç', 'U', 'I' } },
            new PatternMapItem() { RegularKey = '9', ShiftKey = 'ç', NeighborKeys = new char[] { '8', '0', 'i', 'o', '_', 'à', 'I', 'O' } },
            new PatternMapItem() { RegularKey = '0', ShiftKey = 'à', NeighborKeys = new char[] { '9', 'º', 'o', 'p', 'ç', ')', 'O', 'P' } },
            new PatternMapItem() { RegularKey = 'º', ShiftKey = ')', NeighborKeys = new char[] { '0', '=', 'p', '^', 'à', '+', 'P', '¨' } },
            new PatternMapItem() { RegularKey = '+', ShiftKey = '=', NeighborKeys = new char[] { 'º', '^', '$', ')', '¨', '£' } },

            new PatternMapItem() { RegularKey = 'a', ShiftKey = 'A', NeighborKeys = new char[] { '1', '2', 'z', 'q', '&', 'é', 'Z', 'Q' } },
            new PatternMapItem() { RegularKey = 'z', ShiftKey = 'Z', NeighborKeys = new char[] { '2', '3', 'a', 'e', 'q', 's', 'é', '"', 'A', 'E', 'Q', 'S' } },
            new PatternMapItem() { RegularKey = 'e', ShiftKey = 'E', NeighborKeys = new char[] { '3', '4', 'z', 'r', 's', 'd', '"', '\'', 'Z', 'R', 'S', 'D' } },
            new PatternMapItem() { RegularKey = 'r', ShiftKey = 'R', NeighborKeys = new char[] { '4', '5', 'e', 't', 'd', 'f', '\'', '(', 'E', 'T', 'D', 'F' } },
            new PatternMapItem() { RegularKey = 't', ShiftKey = 'T', NeighborKeys = new char[] { '5', '6', 'r', 'y', 'f', 'g', '(', '-', 'R', 'Y', 'F', 'G' } },
            new PatternMapItem() { RegularKey = 'y', ShiftKey = 'Y', NeighborKeys = new char[] { '6', '7', 't', 'u', 'g', 'h', '-', 'è', 'T', 'U', 'G', 'H' } },
            new PatternMapItem() { RegularKey = 'u', ShiftKey = 'U', NeighborKeys = new char[] { '7', '8', 'y', 'i', 'h', 'j', 'è', '_', 'Y', 'I', 'H', 'J' } },
            new PatternMapItem() { RegularKey = 'i', ShiftKey = 'I', NeighborKeys = new char[] { '8', '9', 'u', 'o', 'j', 'k', '_', 'ç', 'U', 'O', 'J', 'K' } },
            new PatternMapItem() { RegularKey = 'o', ShiftKey = 'O', NeighborKeys = new char[] { '9', '0', 'i', 'p', 'k', 'l', 'ç', 'à', 'I', 'P', 'K', 'L' } },
            new PatternMapItem() { RegularKey = 'p', ShiftKey = 'P', NeighborKeys = new char[] { '0', 'º', 'o', '^', 'l', 'm', 'à', ')', 'O', '¨', 'L', 'M' } },
            new PatternMapItem() { RegularKey = '^', ShiftKey = '¨', NeighborKeys = new char[] { 'º', '+', 'p', '$', 'm', 'ù', ')', '=', 'P', '£', 'M', '%' } },
            new PatternMapItem() { RegularKey = '$', ShiftKey = '£', NeighborKeys = new char[] { '+', '^', 'ù', '*', '=', '¨', '%', 'µ' } },

            new PatternMapItem() { RegularKey = 'q', ShiftKey = 'Q', NeighborKeys = new char[] { 'a', 'z', 's', '<', 'w', 'A', 'Z', 'S', '>', 'W' } },
            new PatternMapItem() { RegularKey = 's', ShiftKey = 'S', NeighborKeys = new char[] { 'z', 'e', 'q', 'd', 'w', 'x', 'Z', 'E', 'Q', 'D', 'W', 'X' } },
            new PatternMapItem() { RegularKey = 'd', ShiftKey = 'D', NeighborKeys = new char[] { 'e', 'r', 's', 'f', 'x', 'c', 'E', 'R', 'S', 'F', 'X', 'C' } },
            new PatternMapItem() { RegularKey = 'f', ShiftKey = 'F', NeighborKeys = new char[] { 'r', 't', 'd', 'g', 'c', 'v', 'R', 'T', 'D', 'G', 'C', 'V' } },
            new PatternMapItem() { RegularKey = 'g', ShiftKey = 'G', NeighborKeys = new char[] { 't', 'y', 'f', 'h', 'v', 'b', 'T', 'Y', 'F', 'H', 'V', 'B' } },
            new PatternMapItem() { RegularKey = 'h', ShiftKey = 'H', NeighborKeys = new char[] { 'y', 'u', 'g', 'j', 'b', 'n', 'Y', 'U', 'G', 'J', 'B', 'N' } },
            new PatternMapItem() { RegularKey = 'j', ShiftKey = 'J', NeighborKeys = new char[] { 'u', 'i', 'h', 'k', 'n', ',', 'U', 'I', 'H', 'K', 'N', '?' } },
            new PatternMapItem() { RegularKey = 'k', ShiftKey = 'K', NeighborKeys = new char[] { 'i', 'o', 'j', 'l', ',', ';', 'I', 'O', 'J', 'L', '?', '.' } },
            new PatternMapItem() { RegularKey = 'l', ShiftKey = 'L', NeighborKeys = new char[] { 'o', 'p', 'k', 'm', ';', ':', 'O', 'P', 'K', 'M', '.', '/' } },
            new PatternMapItem() { RegularKey = 'm', ShiftKey = 'M', NeighborKeys = new char[] { 'p', '^', 'l', 'ù', ':', '!', 'P', '¨', 'L', '%', '/', '§' } },
            new PatternMapItem() { RegularKey = 'ù', ShiftKey = '%', NeighborKeys = new char[] { '^', '$', 'm', '*', '!', '¨', '£', 'M', 'µ', '§' } },
            new PatternMapItem() { RegularKey = '*', ShiftKey = 'µ', NeighborKeys = new char[] { '$', 'ù', '£', '%' } },

            new PatternMapItem() { RegularKey = '<', ShiftKey = '>', NeighborKeys = new char[] { 'q', 'w', 'Q', 'W' } },
            new PatternMapItem() { RegularKey = 'w', ShiftKey = 'W', NeighborKeys = new char[] { 'q', 's', '<', 'x', ' ', 'Q', 'S', '>', 'X' } },
            new PatternMapItem() { RegularKey = 'x', ShiftKey = 'X', NeighborKeys = new char[] { 's', 'd', 'w', 'c', ' ', 'S', 'D', 'W', 'C' } },
            new PatternMapItem() { RegularKey = 'c', ShiftKey = 'C', NeighborKeys = new char[] { 'd', 'f', 'x', 'v', ' ', 'D', 'F', 'X', 'V' } },
            new PatternMapItem() { RegularKey = 'v', ShiftKey = 'V', NeighborKeys = new char[] { 'f', 'g', 'c', 'b', ' ', 'F', 'G', 'C', 'B' } },
            new PatternMapItem() { RegularKey = 'b', ShiftKey = 'B', NeighborKeys = new char[] { 'g', 'h', 'v', 'n', ' ', 'G', 'H', 'V', 'N' } },
            new PatternMapItem() { RegularKey = 'n', ShiftKey = 'N', NeighborKeys = new char[] { 'h', 'j', 'b', ',', ' ', 'H', 'J', 'B', '?' } },
            new PatternMapItem() { RegularKey = ',', ShiftKey = '?', NeighborKeys = new char[] { 'j', 'k', 'n', ';', ' ', 'J', 'K', 'N', '.' } },
            new PatternMapItem() { RegularKey = ';', ShiftKey = '.', NeighborKeys = new char[] { 'k', 'l', ',', ':', 'K', 'L', '?', '/' } },
            new PatternMapItem() { RegularKey = ':', ShiftKey = '/', NeighborKeys = new char[] { 'l', 'm', ';', '!', 'L', 'M', '.', '§' } },
            new PatternMapItem() { RegularKey = '!', ShiftKey = '§', NeighborKeys = new char[] { 'm', 'ù', ':', 'M', '%', '/' } },

            new PatternMapItem() { RegularKey = ' ', ShiftKey = ' ', NeighborKeys = new char[] { 'w', 'x', 'c', 'v', 'b', 'n', ',', 'W', 'X', 'C', 'V', 'B', 'N', '?' } }
        };

        /// <summary>
        /// The default NumPad keyboard pattern map
        /// </summary>
        public static readonly List<PatternMapItem> NumPadMap = new List<PatternMapItem>() 
        {
            new PatternMapItem() { RegularKey = '/', ShiftKey = '/', NeighborKeys = new char[] { '*', '8' } },
            new PatternMapItem() { RegularKey = '*', ShiftKey = '*', NeighborKeys = new char[] { '/', '-', '9' } },
            new PatternMapItem() { RegularKey = '-', ShiftKey = '-', NeighborKeys = new char[] { '*', '+' } },
            new PatternMapItem() { RegularKey = '1', ShiftKey = '1', NeighborKeys = new char[] { '4', '2', '0' } },
            new PatternMapItem() { RegularKey = '2', ShiftKey = '2', NeighborKeys = new char[] { '5', '1', '3', '0' } },
            new PatternMapItem() { RegularKey = '3', ShiftKey = '3', NeighborKeys = new char[] { '6', '2', '.' } },
            new PatternMapItem() { RegularKey = '4', ShiftKey = '4', NeighborKeys = new char[] { '7', '5', '1' } },
            new PatternMapItem() { RegularKey = '5', ShiftKey = '5', NeighborKeys = new char[] { '8', '4', '6', '2' } },
            new PatternMapItem() { RegularKey = '6', ShiftKey = '6', NeighborKeys = new char[] { '9', '5', '+', '3' } },
            new PatternMapItem() { RegularKey = '7', ShiftKey = '7', NeighborKeys = new char[] { '8', '4' } },
            new PatternMapItem() { RegularKey = '8', ShiftKey = '8', NeighborKeys = new char[] { '/', '7', '9', '5' } },
            new PatternMapItem() { RegularKey = '9', ShiftKey = '9', NeighborKeys = new char[] { '*', '8', '+', '6' } },
            new PatternMapItem() { RegularKey = '0', ShiftKey = '0', NeighborKeys = new char[] { '1', '2', '.' } },
            new PatternMapItem() { RegularKey = '+', ShiftKey = '+', NeighborKeys = new char[] { '-', '9', '6' } },
            new PatternMapItem() { RegularKey = '.', ShiftKey = '.', NeighborKeys = new char[] { '0', '3' } }
        };
    }
}
