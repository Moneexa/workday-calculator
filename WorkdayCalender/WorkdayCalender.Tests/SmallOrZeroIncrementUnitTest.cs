using System;
using Xunit;
namespace WorkdayCalender.Tests
{
    public class WorkdayBoundsTests
    {
        private WorkDayCalender CreateDefaultWorkdayCalender()
        {
            var workdayCalender = new WorkDayCalender();
            workdayCalender.SetWorkdayStartAndStop(
                new DateTime(2025, 1, 1, 8, 0, 0),
                new DateTime(2025, 1, 1, 16, 0, 0)
            );
            return workdayCalender;
        }
        [Fact]
        public void GetWorkDayIncrement_AtStartTimeWithZeroIncrement_ReturnsSameTime()
        {
            // Arrange
            var workdayCalender = CreateDefaultWorkdayCalender();
            var start = new DateTime(2025, 6, 10, 8, 0, 0);
            float increment = 0.0f;

            // Act
            var result = workdayCalender.GetWorkDayIncrement(start, increment);

            // Assert: remains at configured start time
            Assert.Equal(start, result);
        }

        [Fact]
        public void GetWorkDayIncrement_AtStopTimeWithZeroIncrement_ReturnsSameTime()
        {
            // Arrange
            var workdayCalender = CreateDefaultWorkdayCalender();
            var start = new DateTime(2025, 6, 10, 16, 0, 0);
            float increment = 0.0f;

            // Act
            var result = workdayCalender.GetWorkDayIncrement(start, increment);

            // Assert: remains at configured start time
            Assert.Equal(start, result);
        }

        [Fact]
        public void GetWorkDayIncrement_AtStopTimeWithSmallIncrement_MovesToNextWorkdayStart()
        {
            // Arrange
            var workdayCalender = CreateDefaultWorkdayCalender();
            var atStop = new DateTime(2025, 6, 10, 16, 0, 0);
            float increment = 0.01f;

            // Act
            var result = workdayCalender.GetWorkDayIncrement(atStop, increment);

            // Assert: moves to next workdayCalender's start
            Assert.Equal(new DateTime(2025, 6, 11, 8, 4, 0), result);
        }
    }
}
