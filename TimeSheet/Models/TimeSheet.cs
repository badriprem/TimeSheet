using System;

namespace ProjectTimeSheet.Models
{
    public class TimeSheet
    {
        public int TimeSheetId { get; set; }

        public int UserId { get; set; }

        public int TaskId { get; set; }

        public DateTime Date { get; set; }

        public decimal Hours { get; set; }

        public int Day { get; set; }
    }
}