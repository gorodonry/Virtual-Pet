using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualPet
{
    public static class Methods
    {
        // Contains all the methods used in the program
        public static string CheckS(string str)
        {
            // Checks whether to add an s after an apostrophe, returns formatted string
            if (str.ToLower().ToCharArray()[str.Length - 1] == char.Parse("s"))
            {
                return str + "'";
            }
            else
            {
                return str + "'s";
            }
        }

        public static string JoinWithAnd(List<string> iterable)
        {
            // Joins a list of strings together with commas and an and
            if (iterable.Count() == 0)
            {
                return "";
            }
            else if (iterable.Count() == 1)
            {
                return iterable[0];
            }
            else if (iterable.Count() == 2)
            {
                return $"{iterable[0]} and {iterable[1]}";
            }
            else
            {
                return $"{string.Join(", ", iterable.GetRange(0, iterable.Count() - 1))}, and {iterable[iterable.Count() - 1]}";
            }
        }

        public static string Capitalise(string str)
        {
            // Makes the first character of a string upper case then returns the string
            char[] chars = str.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return string.Join("", chars);
        }
    }
}
