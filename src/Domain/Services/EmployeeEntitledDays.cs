using Domain.Common.Constant;
using Domain.Common.Exceptions;

namespace Domain.Services;

public static class EmployeeEntitledDays
{
    public static decimal CalculateEntitledDays(DateOnly hireDate, short leaveYear)
    {
        if (hireDate.Year > leaveYear)
            throw new DomainException("Invalid Leave year.");

        if (leaveYear > hireDate.Year)
            return EmployeeLeaveBalanceConstant.EntitledLeaveDaysInYear;

        var currentMonthInHireDate = hireDate.Month;

        var entitledDays = (EmployeeLeaveBalanceConstant.TotalMonthInYear
            - currentMonthInHireDate 
            + (hireDate.Day == 1 ? 1 : 0))
            * EmployeeLeaveBalanceConstant.EntitledLeaveDaysInMonth;

        return entitledDays;
    }
}