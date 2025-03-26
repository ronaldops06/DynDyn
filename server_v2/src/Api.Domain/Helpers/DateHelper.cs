using System;

namespace Domain.Helpers
{
    public static class DateHelper
    {
        public static Period CalculatePeriod(DateTime? baseDate, int dayStartMonth, int month)
        {
            DateTime monthCalculated = baseDate?.AddMonths(month) ??  DateTime.Today.AddMonths(month);
            
            DateTime startDate = new DateTime(monthCalculated.Year, monthCalculated.Month, dayStartMonth, 0, 0, 0);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            
            var period = new Period
            {
                DateStartMonth = startDate,
                DateEndMonth = endDate
            };

            return period;
        }
    }
}