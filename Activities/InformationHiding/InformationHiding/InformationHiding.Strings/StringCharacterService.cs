using System;

namespace InformationHiding.Strings
{
    public class StringCharacterService: IStringCharacters
    {
        public int GetCharactersInString(string stringToCount)
        {
            if (!String.IsNullOrEmpty(stringToCount))
            {
                return stringToCount.Length;
            }
            else
            {
                return 0;
            }
        }
    }
}
