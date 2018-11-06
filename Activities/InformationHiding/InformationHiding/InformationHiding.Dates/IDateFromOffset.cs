namespace InformationHiding.Dates
{
    public interface IDateFromOffset
    {
        /// <summary>
        /// Returns the new date after adding the specified numbers of days
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="offsetDays"></param>
        /// <returns></returns>
        string GetDateFromOffset(string startDate, int offsetDays);
    }
}