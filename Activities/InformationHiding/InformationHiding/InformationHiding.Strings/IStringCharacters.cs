namespace InformationHiding.Strings
{
    public interface IStringCharacters
    {
        /// <summary>
        /// Count the characters in the provided string and return the count
        /// </summary>
        /// <param name="stringToCount"></param>
        /// <returns></returns>
        int GetCharactersInString(string stringToCount);
    }
}