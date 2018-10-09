namespace InformationHiding.Parsing
{
    public interface IDayOfWeek
    {
        /// <summary>
        /// Returns the string presentation of the actual day of the
        /// week for the provided date (e.g. “Saturday”)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        string GetDayOfWeek(string date);
    }
}
