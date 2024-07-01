namespace Together.Shared.Extensions;

public static class DateTimeExtensions
{
    public static DateOnly ToDateOnly(this DateTime input) => new(input.Year, input.Month, input.Day);
}