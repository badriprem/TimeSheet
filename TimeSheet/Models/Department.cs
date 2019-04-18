using System.ComponentModel.DataAnnotations;

namespace ProjectTimeSheet.Models
{
    public class Department
    {
        [Display(Name = "Department Id")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please Enter Department Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}