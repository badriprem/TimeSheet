using System.ComponentModel.DataAnnotations;

namespace ProjectTimeSheet.Models
{
    public class UserViewModel
    {
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Employee ID")]
        public int EmpId { get; set; }

        [Display(Name = "Email Id")]
        public string EmailId { get; set; }
    }
}