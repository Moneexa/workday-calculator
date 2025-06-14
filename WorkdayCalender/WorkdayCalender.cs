using System;
enum DayOffset
{
    Next = 1,
    Prev = -1,
}
public class WorkDayCalender
{
    DateTime _workdayStart = new(); // start time of the working day
    DateTime _workDayStop = new(); // stop time of the working day
    readonly Holiday _holiday = new();

    public void SetHoliday(DateTime date)
    {

        DateTime newDate = new(date.Year, date.Month, date.Day);

        _holiday.SetHoliday(newDate.Date);

    }
    public void SetRecurringHoliday(DateTime date)
    {
        DateTime newDate = new(date.Year, date.Month, date.Day);

        _holiday.SetRecurringHoliday(newDate.Date);
    }
    public void SetWorkdayStartAndStop(DateTime start, DateTime stop)
    {
        _workdayStart = start;
        _workDayStop = stop;
    }
    private bool IsOverWork(DateTime currentDate)
    {
        return currentDate.TimeOfDay.CompareTo(_workDayStop.TimeOfDay) > 0;
    }
    private bool IsEarlyWork(DateTime currentDate)
    {
        return currentDate.TimeOfDay.CompareTo(_workdayStart.TimeOfDay) < 0;
    }

    private DateTime ConvertNonBusinessTimeToBusinessHours(DateTime currentDate)
    {
        return IsOverWork(currentDate) ? new(
                currentDate.Year,
                currentDate.Month,
                currentDate.Day,
                _workDayStop.Hour,
                _workDayStop.Minute,
                _workDayStop.Second
            ) : IsEarlyWork(currentDate) ? new(
                currentDate.Year,
                currentDate.Month,
                currentDate.Day,
                _workdayStart.Hour,
                _workdayStart.Minute,
                _workdayStart.Second
            ) : currentDate;
    }

    private DateTime AdjustHoursForOverAndEarlyWork(DateTime currentDate, DayOffset dayOffset)
    {
        if (!IsOverWork(currentDate) && !IsEarlyWork(currentDate)) return currentDate;
        DateTime nextDay = currentDate.AddDays((int)dayOffset);

        //  business boundary time to subtract the additional time from
        DateTime businessBoundryTime = dayOffset == DayOffset.Next ? _workDayStop : _workdayStart;

        // starting business time to set for the next/previous day
        DateTime nxtPrevDayBoundy = dayOffset == DayOffset.Next ? _workdayStart : _workDayStop;

        TimeSpan extraTimeExceedingBusinessTime = currentDate.Subtract(new DateTime(currentDate.Year, currentDate.Month,
                 currentDate.Day, businessBoundryTime.Hour, businessBoundryTime.Minute, businessBoundryTime.Second));
        DateTime nextDayWithAdjustedBusinessHour = new(nextDay.Year, nextDay.Month, nextDay.Day, nxtPrevDayBoundy.Hour,
                 nxtPrevDayBoundy.Minute, nxtPrevDayBoundy.Second);
        while (!_holiday.IsWorkDay(nextDayWithAdjustedBusinessHour))
        {
            nextDayWithAdjustedBusinessHour = nextDayWithAdjustedBusinessHour.AddDays((int)dayOffset);
        }
        return nextDayWithAdjustedBusinessHour.Add(extraTimeExceedingBusinessTime);
    }

    public DateTime GetWorkDayIncrement(DateTime startWorkDay, float workDaysIncrement)
    {
        if (workDaysIncrement == 0)
        {
            return startWorkDay;
        }
        DateTime currentDate = startWorkDay;
        int totalWorkingHours = _workDayStop.Hour - _workdayStart.Hour;

        // reach the incrementedDate while handling vaccation/weekend/holidays
        int remainingDays = (int)workDaysIncrement;
        DayOffset dayOffset = remainingDays >= 0 ? DayOffset.Next : DayOffset.Prev;
        while (remainingDays != 0)
        {
            currentDate = currentDate.AddDays((int)dayOffset);
            remainingDays = _holiday.IsWorkDay(currentDate) ? remainingDays - (int)dayOffset : remainingDays;
        }

        //set the bounds for time, if the time is above work hours, then set 4:00pm, if before work hours then 8:00am,
        // otherwise return date as it is
        DateTime currentDateWithBusinessHours = ConvertNonBusinessTimeToBusinessHours(currentDate);

        // set the formula for adding seconds as per the increment count
        // take the after decimal part of increment count and multiply it with work hours and 3600.
        float workdayFraction = (float)(Math.Abs(workDaysIncrement) - Math.Abs((int)workDaysIncrement));

        // this product gives the seconds to be added or subtracted from the current date time.
        double secondsToAdd = totalWorkingHours * workdayFraction * 3600;

        DateTime businessDayWithAddedTime = workDaysIncrement > 0 ? currentDateWithBusinessHours.AddSeconds(secondsToAdd)
                 : currentDateWithBusinessHours.AddSeconds(-secondsToAdd);
        DateTime businessDayWithAddedBusinessHours = AdjustHoursForOverAndEarlyWork(businessDayWithAddedTime, dayOffset);

        // Rounding off seconds
        return new(businessDayWithAddedBusinessHours.Year, businessDayWithAddedBusinessHours.Month,
               businessDayWithAddedBusinessHours.Day, businessDayWithAddedBusinessHours.Hour,
               businessDayWithAddedBusinessHours.Minute, 0);
    }
}
