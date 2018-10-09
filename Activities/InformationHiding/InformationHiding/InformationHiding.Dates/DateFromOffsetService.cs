using System;

namespace InformationHiding.Dates
{
    public class DateFromOffsetService: IDateFromOffset
    {
        public string GetDateFromOffset(string startDate, int offsetDays)
        {
            DateTime myDate;
            if (DateTime.TryParse(startDate, out myDate))
            {
                return myDate.AddDays(offsetDays).ToShortDateString();
            }
            else
            {
                return "Invalid Date";
            }
        }
    }
}
