using System;

namespace InformationHiding.Parsing
{
    public class DayOfWeekService : IDayOfWeek
    {
        public string GetDayOfWeek(string date)
        {
            DateTime myDate;

            if (DateTime.TryParse(date, out myDate))
            {
                return myDate.DayOfWeek.ToString();
            }
            else
            {
                return "Invalid Date";
            }
        }
    }
}
