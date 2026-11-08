namespace Domain.Common.Constant;

internal static class EmployeeLeaveBalanceConstant
{
    public const decimal EntitledLeaveDaysInYear = 30m;
    public const int TotalMonthInYear = 12;
    public const decimal EntitledLeaveDaysInMonth = EntitledLeaveDaysInYear / TotalMonthInYear;
}