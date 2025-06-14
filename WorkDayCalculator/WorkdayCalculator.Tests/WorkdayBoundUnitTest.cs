using Xunit;

namespace WorkdayCalculator.Tests
{
    public class WorkdayBoundsTests
    {
        private Workday CreateDefaultWorkday()
        {
            var workday = new Workday();
            workday.SetWorkdayStartAndStop(
                new DateTime(2025, 1, 1, 8, 0, 0),
                new DateTime(2025, 1, 1, 16, 0, 0)
            );
            return workday;
        }

        [Fact]
        public void Boundary_At_StartInclusive_NoShift()
        {
            // Arrange
            var workday = CreateDefaultWorkday();
            var start = new DateTime(2025, 6, 10, 8, 0, 0);
            float increment = 0.0f;

            // Act
            var result = workday.GetWorkDayIncrement(start, increment);

            // Assert: remains at configured start time
            Assert.Equal(start, result);
        }

        [Fact]
        public void Boundary_At_StopInclusive_NoShift()
        {
            // Arrange
            var workday = CreateDefaultWorkday();
            var start = new DateTime(2025, 6, 10, 16, 0, 0);
            float increment = 0.0f;

            // Act
            var result = workday.GetWorkDayIncrement(start, increment);

            // Assert: remains at configured start time
            Assert.Equal(start, result);
        }

        [Fact]
        public void Boundary_At_Stop_Shifts_To_NextDayStart()
        {
            // Arrange
            var workday = CreateDefaultWorkday();
            var atStop = new DateTime(2025, 6, 10, 16, 0, 0);
            float increment = 0.01f;

            // Act
            var result = workday.GetWorkDayIncrement(atStop, increment);

            // Assert: moves to next workday's start
            Assert.Equal(new DateTime(2025, 6, 11, 8, 4, 0), result);
        }
        [Fact]
        public void Next_Year_Workday()
        {
            // Arrange
            var workday = CreateDefaultWorkday();
            workday.SetRecurringHoliday(new DateTime(2024, 1, 1));
            workday.SetHoliday(new DateTime(2026, 1, 2));

            var atStop = new DateTime(2025, 12, 31, 16, 0, 0);
            float increment = 0.01f;

            // Act
            var result = workday.GetWorkDayIncrement(atStop, increment);

            // Assert: moves to next workday's start
            Assert.Equal(new DateTime(2026, 1, 5, 8, 4, 0), result);
        }
    }
}
