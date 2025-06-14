using System;
using Xunit;

namespace WorkdayCalender.Tests
{
    public class HolidayHandlingTests
    {
        private static WorkDayCalender CreateDefaultWorkday()
        {
            WorkDayCalender workday = new();
            workday.SetWorkdayStartAndStop(
                new(2025, 1, 1, 8, 0, 0),
                new(2025, 1, 1, 16, 0, 0)
            );
            return workday;
        }

        [Fact]
        public void Long_Holidays()
        {
            // Arrange
            var workday = new WorkDayCalender();
            workday.SetWorkdayStartAndStop(
            new DateTime(2004, 1, 1, 8, 0, 0),
            new DateTime(2004, 1, 1, 16, 0, 0));
            workday.SetHoliday(new(2004, 5, 28, 0, 0, 0));
            workday.SetRecurringHoliday(new(2004, 5, 27, 0, 0, 0));
            workday.SetRecurringHoliday(new(2004, 5, 31, 0, 0, 0));
            workday.SetRecurringHoliday(new(2004, 6, 1, 0, 0, 0));

            DateTime start = new(2004, 5, 24, 8, 3, 0);
            float increment = 5.5f;

            // Act
            var result = workday.GetWorkDayIncrement(start, increment);

            // Assert
            Assert.Equal(new DateTime(2004, 06, 04, 12, 3, 0), result);
        }
        [Fact]
        public void Empty_Holidays_SimpleIncrement()
        {
            // Arrange
            var workday = CreateDefaultWorkday();
            DateTime start = new(2025, 6, 10, 8, 0, 0); // Tuesday
            float increment = 1.0f;

            // Act
            var result = workday.GetWorkDayIncrement(start, increment);

            // Assert: next workday is Wednesday, June 11 at 08:00
            Assert.Equal(new DateTime(2025, 6, 11, 8, 0, 0), result);
        }

        [Fact]
        public void Duplicate_Holidays_Are_Idempotent()
        {
            // Arrange
            var workday = CreateDefaultWorkday();
            var holiday = new DateTime(2025, 6, 12);
            workday.SetHoliday(holiday);
            workday.SetHoliday(holiday);

            var start = new DateTime(2025, 6, 11, 8, 0, 0); // Thursday
            float increment = 1.0f;

            // Act
            var result = workday.GetWorkDayIncrement(start, increment);

            // Assert: skips only 12th once, lands on 13th
            Assert.Equal(new DateTime(2025, 6, 13, 8, 0, 0), result);
        }

        [Fact]
        public void Recurring_LeapDay_Skipped_In_NonLeap()
        {
            // Arrange
            var workday = CreateDefaultWorkday();
            workday.SetRecurringHoliday(new DateTime(2000, 2, 29));

            var start = new DateTime(2024, 2, 28, 8, 0, 0); // 2024 is leap year
            float increment = 2.0f;

            // Act
            var result = workday.GetWorkDayIncrement(start, increment);

            // Assert: 29th is recurring holiday, so lands on March 1st
            Assert.Equal(new DateTime(2024, 3, 1, 8, 0, 0), result);
        }

        [Fact]
        public void SetHoliday_Invalid_Feb29_NonLeap_Throws()
        {
            // Arrange
            var workday = CreateDefaultWorkday();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => workday.SetHoliday(new DateTime(2025, 2, 29))
            );
        }
        [Fact]
        public void Start_Weekend()
        {
            // Arrange
            var workday = new WorkDayCalender();
            workday.SetWorkdayStartAndStop(
            new DateTime(2004, 1, 1, 8, 0, 0),
            new DateTime(2004, 1, 1, 16, 0, 0));
            workday.SetHoliday(new(2004, 5, 28, 0, 0, 0));
            workday.SetRecurringHoliday(new(2004, 5, 27, 0, 0, 0));
            workday.SetRecurringHoliday(new(2004, 5, 31, 0, 0, 0));
            workday.SetRecurringHoliday(new(2004, 6, 1, 0, 0, 0));

            DateTime start = new(2004, 5, 22, 8, 3, 0);
            float increment = 5.5f;

            // Act
            var result = workday.GetWorkDayIncrement(start, increment);

            // Assert
            Assert.Equal(new DateTime(2004, 06, 03, 12, 3, 0), result);
        }

    }
}
