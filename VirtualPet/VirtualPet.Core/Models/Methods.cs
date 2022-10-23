using System;
using System.Collections.Generic;

namespace VirtualPet.Core.Models
{
    /// <summary>
    /// Static class containing methods used by the entire program.
    /// </summary>
    public static class Methods
    {
        /// <summary>
        /// Provides the correct format for the possessive form of a string.
        /// </summary>
        /// <param name="str">The string possessing something.</param>
        /// <returns>
        /// The string correctly formatted. E.g. "Ryan" becomes "Ryan's", and "Jess" becomes "Jess'".
        /// </returns>
        /// <remarks>
        /// Strings of all lengths are dealth with by the method, including null strings.
        /// </remarks>
        public static string CheckSNeededAfterApostrophe(string str)
        {
            if (str is null)
                return string.Empty;

            if (str.Length == 0)
                return string.Empty;

            if (str.ToLower()[^1] == char.Parse("s"))
                return $"{str}'";

            return $"{str}'s";
        }

        /// <summary>
        /// Joins a list of strings together, separated by commas with an "and" at the end.
        /// </summary>
        /// <param name="iterable">The list of strings to join together.</param>
        /// <returns>
        /// The list as a string with its strings joined together.
        /// E.g. parsing { "one", "two", "three", "four" } will return the string "one, two, three, and four".
        /// </returns>
        /// <remarks>
        /// Lists of all lengths are dealth with by the method, including null entries.
        /// </remarks>
        public static string JoinWithAnd(List<string> iterable)
        {
            if (iterable is null)
                return string.Empty;

            if (iterable.Count == 0)
                return string.Empty;

            if (iterable.Count == 1)
                return iterable[0];

            if (iterable.Count == 2)
                return $"{iterable[0]} and {iterable[1]}";

            return $"{string.Join(", ", iterable.GetRange(0, iterable.Count - 1))}, and {iterable[^1]}";
        }

        /// <summary>
        /// Makes the first character of a string upper case.
        /// </summary>
        /// <param name="str">The string to be capitalised.</param>
        /// <returns>
        /// The string with the first character converted to upper case.
        /// </returns>
        /// <remarks>
        /// String should be trimmed before passed into this method.
        /// </remarks>
        public static string Capitalise(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            if (str.Length == 1)
                return str.ToUpper();

            return char.ToUpper(str[0]).ToString() + string.Join("", new ArraySegment<char>(str.ToCharArray(), 1, str.Length - 1));
        }

        /// <summary>
        /// Randomly selects a string from a list of strings.
        /// </summary>
        /// <param name="collection">The list the random string is chosen from.</param>
        /// <returns>The randomly selected string.</returns>
        public static string RandomChoice(List<string> collection)
        {
            return collection[new Random().Next(collection.Count)];
        }
    }
}
