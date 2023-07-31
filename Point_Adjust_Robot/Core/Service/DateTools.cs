namespace Point_Adjust_Robot.Core.Service
{
    public static class DateTools
    {
        public static DateTime ValidDate(int days)
        {
            return ValidDate(days, DateTime.Now.ToString("dd/MM/yyyy")); ;
        }

        public static DateTime ValidDate()
        {
            return ValidDate(0, DateTime.Now.ToString("dd/MM/yyyy")); ;
        }

        public static DateTime ValidDate(int days, string dateRef)
        {
            var today = DateTime.ParseExact(dateRef, "dd/MM/yyyy", null);

            if (today.DayOfWeek == DayOfWeek.Saturday)
                today = today.AddDays(-1);

            if (today.DayOfWeek == DayOfWeek.Sunday)
                today = today.AddDays(-2);

            var date = today.AddDays(days);

            if (date.DayOfWeek == DayOfWeek.Saturday)
                date = date.AddDays(+2);

            if (date.DayOfWeek == DayOfWeek.Sunday)
                date = date.AddDays(+1);

            return date;
        }

        public static string FormatDate(DateTime date) => date.ToString("dd/MM/yyyy");
    }
}
