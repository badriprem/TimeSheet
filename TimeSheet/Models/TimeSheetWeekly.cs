using System.ComponentModel.DataAnnotations;

namespace ProjectTimeSheet.Models
{
    public class TimeSheetWeekly
    {
        [Display(Name = "User")]
        public string User { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Employee Id")]
        public int EmpID { get; set; }

        [Display(Name = "Week 1")]
        public decimal Week1 { get; set; }

        [Display(Name = "Week 2")]
        public decimal Week2 { get; set; }

        [Display(Name = "Week 3")]
        public decimal Week3 { get; set; }

        [Display(Name = "Week 4")]
        public decimal Week4 { get; set; }

        [Display(Name = "Week 5")]
        public decimal Week5 { get; set; }
    }
}