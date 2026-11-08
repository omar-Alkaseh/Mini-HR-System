using Domain.Common.Constant;
using Domain.Common.Exceptions;

namespace Domain.Models;

public sealed class EmployeeLeave
{
    public int EmployeeLeaveId { get; private set; }

    public int EmployeeId { get; private set; }

    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }

    public int Days { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public int RecordedByUserId { get; private set; }

    public Employee Employee { get; private set; } = null!;
    public User RecordedByUser { get; private set; } = null!;

    private EmployeeLeave() { }

    public static EmployeeLeave Create(
        int employeeId,
        DateOnly startDate,
        DateOnly endDate,
        int days,
        DateTime nowUtc,
        int recordedByUserId)
    {
        if (startDate == default || endDate == default)
            throw new DomainException("Start date and/or End date required.");

        if (startDate > endDate)
            throw new DomainException("Start date must be less than End date.");

        if (startDate.Year != endDate.Year)
            throw new DomainException("Leave must be within the same year.");

        if (employeeId <= 0)
            throw new DomainException("Invalid Employee Id.");

        if (recordedByUserId <= 0)
            throw new DomainException("Invalid User Id.");

        if (days is < 1 or > 30)
            throw new DomainException("Number of days must be between 1 and 30.");

        return new EmployeeLeave
        {
            EmployeeId = employeeId,
            StartDate = startDate,
            EndDate = endDate,
            Days = days,
            CreatedAt = nowUtc,
            RecordedByUserId = recordedByUserId
        };
    }   

    public static int CalculateLeaveDays(DateOnly startDate, DateOnly endDate)
    {
        int numberOfLeaveDays = 0;

        while(startDate <= endDate)
        {
            if (!WeekendConstants.IsWeekend(startDate))
                numberOfLeaveDays ++;
            
            startDate = startDate.AddDays(1);
        }

        return numberOfLeaveDays;
    }
}