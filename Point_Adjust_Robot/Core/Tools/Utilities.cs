﻿namespace Point_Adjust_Robot.Core.Tools
{
    public static class Utilities
    {
        public static string GetDifMinutes(string start, string final)
        {
            return GetDifMinutes(DateTime.Parse(start), DateTime.Parse(final));
        }

        public static string GetDifMinutes(DateTime start, DateTime final)
        {
            return GetDifMinutes((final - start).TotalMinutes);
        }

        public static string GetDifMinutes(double decimalMinutes)
        {
            var timeSpan = TimeSpan.FromMinutes(decimalMinutes);

            double day = timeSpan.Days;
            int hh = timeSpan.Hours;
            int mm = timeSpan.Minutes;
            int ss = timeSpan.Seconds;

            string HH = hh < 10 ? $"0{hh}" : $"{hh}";
            string MM = mm < 10 ? $"0{mm}" : $"{mm}";
            string SS = ss < 10 ? $"0{ss}" : $"{ss}";

            return day < 1 ? 
                        (hh < 1 ?
                         $"{MM}:{SS}" :
                         $"{HH}:{MM}:{SS}") : 
                   $"{day}D {HH}:{MM}:{SS}";
        }
    }
}