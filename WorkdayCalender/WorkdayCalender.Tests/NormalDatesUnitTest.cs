using System;
using Xunit;

namespace WorkdayCalender.Tests;

public class NormalDatesUnitTests
{
    private static WorkDayCalender CreateDefaultWorkday()
    {
        var workday = new WorkDayCalender();
        workday.SetWorkdayStartAndStop(
        new DateTime(2004, 1, 1, 8, 0, 0),
        new DateTime(2004, 1, 1, 16, 0, 0));
        workday.SetHoliday(new DateTime(2004, 5, 17, 0, 0, 0));
        workday.SetRecurringHoliday(new DateTime(2004, 5, 27, 0, 0, 0));
        return workday;
    }

    [Fact]
    public void Negative_Increment()
    {
        // Arrange
        var workday = CreateDefaultWorkday();
        DateTime start = new(2004, 5, 24, 18, 3, 0);
        float increment = -6.7470217f;

        // Act
        var result = workday.GetWorkDayIncrement(start, increment);

        // Assert
        Assert.Equal(new DateTime(2004, 05, 13, 10, 01, 0), result);
    }

    [Fact]
    public void Negative_Increment_WithEarly_Hour()
    {
        // Arrange
        var workday = CreateDefaultWorkday();

        DateTime start = new(2004, 5, 24, 7, 3, 0);
        float increment = -6.7470217f;

        // Act
        var result = workday.GetWorkDayIncrement(start, increment);

        // Assert
        Assert.Equal(new DateTime(2004, 05, 12, 10, 01, 0), result);
    }


    [Fact]


    public void Long_Positive_Increment()
    {
        // Arrange
        var workday = CreateDefaultWorkday();

        DateTime start = new(2004, 5, 24, 19, 3, 0);
        float increment = 44.723656f;

        // Act
        var result = workday.GetWorkDayIncrement(start, increment);

        // Assert
        Assert.Equal(new DateTime(2004, 07, 27, 13, 47, 0), result);
    }

    [Fact]
    public void Unusual_Time()
    {
        // Arrange
        var workday = new WorkDayCalender();
        float increment = 44.723656f;

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(
        () => workday.GetWorkDayIncrement(new DateTime(2004, 5, 24, 24, 100, 0), increment));
    }
}