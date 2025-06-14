public class Holiday
{
    readonly List<DateTime> holiday = []; //this should take care of the whole date with year
    readonly List<DateTime> recurringHoliday = []; // this should not care about year, since it will repeat each year
    public void SetHoliday(DateTime date)
    {

        DateTime newDate = new(date.Year, date.Month, date.Day);
        holiday.Add(newDate.Date);

    }
    public void SetRecurringHoliday(DateTime date)
    {
        if (date.Day != 29 && date.ToString("MMM") != "Feb")
        {
            DateTime newDate = new(1, date.Month, date.Day);
            recurringHoliday.Add(newDate);

        }
    }
    public bool IsHoliday(DateTime currentDate, bool? isHoliday = null, bool? isRecurringHoliday = null)
    {
        bool holidayCheck = isHoliday ?? holiday.Exists(h => h.Date == currentDate.Date);
        bool recurringHolidayCheck = isRecurringHoliday ?? recurringHoliday.Exists(h => h.Day == currentDate.Day && h.Month == currentDate.Month);
        return holidayCheck || recurringHolidayCheck;
    }
    public bool IsWorkDay(DateTime currentDate)
    {
        DayOfWeek currentDayWithWeekNumber = currentDate.DayOfWeek;
        bool isWeekend = currentDayWithWeekNumber == DayOfWeek.Saturday || currentDayWithWeekNumber == DayOfWeek.Sunday;
        bool isHoliday = IsHoliday(currentDate);

        return !(isWeekend || isHoliday);
    }


}