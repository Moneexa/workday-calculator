using System;
using Xunit;

namespace WorkdayCalender.Tests;

public class NormalDatesUnitTests
{
    private static WorkDayCalender CreateDefaultWorkdayCalender()
    {
        var workdayCalender = new WorkDayCalender();
        workdayCalender.SetWorkdayStartAndStop(
        new DateTime(2004, 1, 1, 8, 0, 0),
        new DateTime(2004, 1, 1, 16, 0, 0));
        workdayCalender.SetHoliday(new DateTime(2004, 5, 17, 0, 0, 0));
        workdayCalender.SetRecurringHoliday(new DateTime(2004, 5, 27, 0, 0, 0));
        return workdayCalender;
    }

    [Fact]
    public void GetWorkDayIncrement_WithNegativeIncrementStartingAfterWorkday_ReturnsCorrectPreviousDate()
    {
        // Arrange
        var workdayCalender = CreateDefaultWorkdayCalender();
        DateTime start = new(2004, 5, 24, 18, 3, 0);
        float increment = -6.7470217f;

        // Act
        var result = workdayCalender.GetWorkDayIncrement(start, increment);

        // Assert
        Assert.Equal(new DateTime(2004, 5, 13, 10, 1, 0), result);
    }

    [Fact]
    public void GetWorkDayIncrement_WithNegativeIncrementStartingBeforeWorkday_ReturnsCorrectPreviousDate()
    {
        // Arrange
        var workdayCalender = CreateDefaultWorkdayCalender();

        DateTime start = new(2004, 5, 24, 7, 3, 0);
        float increment = -6.7470217f;

        // Act
        var result = workdayCalender.GetWorkDayIncrement(start, increment);

        // Assert
        Assert.Equal(new DateTime(2004, 05, 12, 10, 1, 0), result);
    }


    [Fact]


    public void GetWorkDayIncrement_WithLargePositiveIncrementSpanningMultipleMonths_ReturnsCorrectFutureDateTime()
    {
        // Arrange
        var workdayCalender = CreateDefaultWorkdayCalender();

        DateTime start = new(2004, 5, 24, 19, 3, 0);
        float increment = 44.723656f;

        // Act
        var result = workdayCalender.GetWorkDayIncrement(start, increment);

        // Assert
        Assert.Equal(new DateTime(2004, 07, 27, 13, 47, 0), result);
    }

    [Fact]
    public void GetWorkDayIncrement_WithInvalidTime_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var workdayCalender = CreateDefaultWorkdayCalender();
        float increment = 44.723656f;

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(
        () => workdayCalender.GetWorkDayIncrement(new DateTime(2004, 5, 24, 24, 100, 0), increment));
    }
}