var workdayCalender = new WorkDayCalender();
Console.WriteLine("Welcome to the Workday Calender!");
Console.WriteLine("Type 'exit' to quit.\n");
workdayCalender.SetWorkdayStartAndStop(
new DateTime(2004, 1, 1, 8, 0, 0),
new DateTime(2004, 1, 1, 16, 0, 0));
workdayCalender.SetHoliday(new(2004, 5, 28, 0, 0, 0));
workdayCalender.SetRecurringHoliday(new(2004, 5, 27, 0, 0, 0));
workdayCalender.SetRecurringHoliday(new(2004, 5, 31, 0, 0, 0));
workdayCalender.SetRecurringHoliday(new(2004, 6, 1, 0, 0, 0));
DateTime start = new(2004, 5, 22, 8, 3, 0);
float increment = 5.5f;
Console.WriteLine(start.ToString("dd-MM-yyyy HH:mm") + " with the addition of " +
increment + " working days is " +
workdayCalender.GetWorkDayIncrement(start, increment).ToString("dd-MM-yyyy HH:mm"));