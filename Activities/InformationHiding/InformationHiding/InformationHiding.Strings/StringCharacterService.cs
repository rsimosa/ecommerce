using System;
using System.Text.RegularExpressions;

namespace InformationHiding.Strings
{
    public class StringCharacterService: IStringCharacters
    {
        public int CountVowelsInString(string stringToCount)
        {
            if (!String.IsNullOrEmpty(stringToCount))
            {
                int vowelCount = Regex.Matches(stringToCount.ToUpper(), "[A|E|I|O|U]").Count;
                return vowelCount;
            }
            else
            {
                return 0;
            }
        }
    }
}
