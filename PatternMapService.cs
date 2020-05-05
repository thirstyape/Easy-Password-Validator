using Easy_Password_Validator.Models;

using System;
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
            return test.Select((c, i) => test.Substring(i)
                .TakeWhile(x => IsNeighbor(c, x, map))
                .ToString()
            );
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
        /// The default Qwerty keyboard pattern map
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
    }
}
