using System;
using Xunit;

namespace WorkdayCalender.Tests
{
    public class HolidayHandlingTests
    {
        private static WorkDayCalender CreateDefaultWorkdayCalender()
        {
            WorkDayCalender workDayCalender = new();
            workDayCalender.SetWorkdayStartAndStop(
                new(2025, 1, 1, 8, 0, 0),
                new(2025, 1, 1, 16, 0, 0)
            );
            return workDayCalender;
        }

        [Fact]
        public void GetWorkDayIncrement_WithLongHolidaySequence_ReturnsSkippingAllHolidays()
        {
            // Arrange
            var workdayCalender = CreateDefaultWorkdayCalender();
            workdayCalender.SetHoliday(new(2004, 5, 28, 0, 0, 0));
            workdayCalender.SetRecurringHoliday(new(2004, 5, 27, 0, 0, 0));
            workdayCalender.SetRecurringHoliday(new(2004, 5, 31, 0, 0, 0));
            workdayCalender.SetRecurringHoliday(new(2004, 6, 1, 0, 0, 0));

            DateTime start = new(2004, 5, 24, 8, 3, 0);
            float increment = 5.5f;

            // Act
            var result = workdayCalender.GetWorkDayIncrement(start, increment);

            // Assert
            Assert.Equal(new DateTime(2004, 06, 04, 12, 3, 0), result);
        }
        [Fact]
        public void GetWorkDayIncrement_WithNoHolidaysAndOneDayIncrement_ReturnsNextWorkdayStart()
        {
            // Arrange
            var workdayCalender = CreateDefaultWorkdayCalender();
            DateTime start = new(2025, 6, 10, 8, 0, 0); // Tuesday
            float increment = 1.0f;

            // Act
            var result = workdayCalender.GetWorkDayIncrement(start, increment);

            // Assert: next workday is Wednesday, June 11 at 08:00
            Assert.Equal(new DateTime(2025, 6, 11, 8, 0, 0), result);
        }

        [Fact]
        public void GetWorkDayIncrement_WithDuplicateHolidays_SkipsEachOnlyOnce()
        {
            // Arrange
            var workdayCalender = CreateDefaultWorkdayCalender();
            var holiday = new DateTime(2025, 6, 12);
            workdayCalender.SetHoliday(holiday);
            workdayCalender.SetHoliday(holiday);

            var start = new DateTime(2025, 6, 11, 8, 0, 0); // Thursday
            float increment = 1.0f;

            // Act
            var result = workdayCalender.GetWorkDayIncrement(start, increment);

            // Assert: skips only 12th once, lands on 13th
            Assert.Equal(new DateTime(2025, 6, 13, 8, 0, 0), result);
        }

        [Fact]
        public void GetWorkDayIncrement_WithRecurringFeb29InNonLeapYear_SkipsToMarchFirst()
        {
            // Arrange
            var workdayCalender = CreateDefaultWorkdayCalender();
            workdayCalender.SetRecurringHoliday(new DateTime(2000, 2, 29));

            var start = new DateTime(2024, 2, 28, 8, 0, 0); // 2024 is leap year
            float increment = 2.0f;

            // Act
            var result = workdayCalender.GetWorkDayIncrement(start, increment);

            // Assert: 29th is recurring holiday, so lands on March 1st
            Assert.Equal(new DateTime(2024, 3, 1, 8, 0, 0), result);
        }

        [Fact]
        public void SetHoliday_WithNonLeapYearFeb29_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var workdayCalender = CreateDefaultWorkdayCalender();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => workdayCalender.SetHoliday(new DateTime(2025, 2, 29))
            );
        }
        [Fact]
        public void GetWorkDayIncrement_WithStartOnWeekendAndHolidays_ReturnsCorrectDate()
        {
            // Arrange
            var workdayCalender = CreateDefaultWorkdayCalender();
            workdayCalender.SetHoliday(new(2004, 5, 28, 0, 0, 0));
            workdayCalender.SetRecurringHoliday(new(2004, 5, 27, 0, 0, 0));
            workdayCalender.SetRecurringHoliday(new(2004, 5, 31, 0, 0, 0));
            workdayCalender.SetRecurringHoliday(new(2004, 6, 1, 0, 0, 0));

            DateTime start = new(2004, 5, 22, 8, 3, 0);
            float increment = 5.5f;

            // Act
            var result = workdayCalender.GetWorkDayIncrement(start, increment);

            // Assert
            Assert.Equal(new DateTime(2004, 06, 03, 12, 3, 0), result);
        }
        [Fact]
        public void GetWorkDayIncrement_WithStartOnYearEndWithHolidays_ReturnsNextYearWorkDay()
        {
            // Arrange
            var workdayCalender = CreateDefaultWorkdayCalender();
            workdayCalender.SetRecurringHoliday(new DateTime(2024, 1, 1));
            workdayCalender.SetHoliday(new DateTime(2026, 1, 2));

            var atStop = new DateTime(2025, 12, 31, 16, 0, 0);
            float increment = 0.01f;

            // Act
            var result = workdayCalender.GetWorkDayIncrement(atStop, increment);

            // Assert: moves to next workday's start
            Assert.Equal(new DateTime(2026, 1, 5, 8, 4, 0), result);
        }


    }
}
