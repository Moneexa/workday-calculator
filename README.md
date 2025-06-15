 # workday-calender Running Project

- This project is developed using C#.
- The project uses XUnit dependancy for defining unit tests for written classes.
- To run this project make sure you have .NET SDK 9.0 version installed.

Clone the project using command:

`git clone https://gitlab.com/14besemsyed/workday-calender.git`

Go inside the project using command

`cd workday-calender/Workdaycalender`

Upon running the project, you will see the output in the terminal. The visible output comes from calling the GetIncrementDay function inside Program.cs
Use following command to run the project.

`dotnet run`

You will see different unit test files inside WorkdayCalculator.tests folder. To run all the tests, use following command

`dotnet test`



# Work Day Calender Algorithm Overview

This document breaks down the `GetWorkDayIncrement` workflow into digestible pieces: key components, helper methods, and the main algorithm steps.

---

## 📦 Key Components

| Component               | Type                | Purpose                                                               |
|-------------------------|---------------------|-----------------------------------------------------------------------|
| `_workdayStart`         | `DateTime`          | Time when each business day begins (e.g., 08:00:00)                   |
| `_workDayStop`          | `DateTime`          | Time when each business day ends (e.g., 16:00:00)                     |
| `_holiday`              | `Holiday`           | Tracks fixed and recurring holidays/weekends to skip non-work days   |
| `DayOffset`             | `enum`              | Direction of day adjustment: `Next = +1`, `Prev = -1`                 |

---

## 🛠️ Helper Methods

| Method                                         | Inputs                        | Outputs              | Purpose                                                                 |
|-----------------------------------------------|-------------------------------|----------------------|-------------------------------------------------------------------------|
| `SetWorkdayStartAndStop(start, stop)`         | `DateTime start, stop`        | —                    | Configure business hours                                                |
| `ConvertNonBusinessTimeToBusinessHours(date)` | `DateTime currentDate`        | `DateTime`           | Clamp any time before/after business hours to the nearest boundary     |
| `IsWorkDay(date)`                             | `DateTime currentDate`        | `bool`               | Checks if the date is not a weekend/holiday (inside `Holiday` class)   |
| `AdjustHoursForOverAndEarlyWork(date, dir)`   | `DateTime currentDate`, `DayOffset dayOffset` | `DateTime` | Carries over extra seconds to next/prev workday, skipping holidays     |

---

## 🚀 Main Algorithm: `GetWorkDayIncrement(startWorkDay, workDaysIncrement)`

1. **Handle Zero Increment**  
   - If `workDaysIncrement == 0`, return the original `startWorkDay`.

2. **Determine Direction & Remaining Whole Days**  
   - **`dayOffset`** = `Next` if increment ≥ 0, else `Prev`.  
   - **`remainingDays`** = integer part of `workDaysIncrement`.  

3. **Skip Non-Work Days**  

    Calculate the end business day by incrementing/decrementing whole number.

   ```csharp
   while (remainingDays != 0) {
     currentDate = currentDate.AddDays((int) dayOffset);
     if (_holiday.IsWorkDay(currentDate))
       remainingDays -= (int) dayOffset;
   }
4. **Add Seconds to the reached day**

    . Adjust the clock of the day. 

    . Calculate fraction of the work hours from the  workdayIncrement. Multiple this fraction with 8 hours and  3600.

    . Add or subtract these seconds from the end day. 
  
5. **Pass on the remaining time to the next/prev day**  

   We check if added time has exceeded/preceeded business hour or not.
   If yes then pass remaining time to next/prev day.