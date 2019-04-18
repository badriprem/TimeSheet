using System.ComponentModel.DataAnnotations;

namespace ProjectTimeSheet.Models
{
    public class User
    {
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please Enter User Name")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Full Name")]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please Enter Emaployee Id")]
        [Display(Name = "Employee Id")]
        public int EmpId { get; set; }

        [Required(ErrorMessage = "Please Enter Email Id")]
        [Display(Name = "Email ID")]
        public string EmailId { get; set; }
    }
}