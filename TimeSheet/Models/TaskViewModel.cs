using System.ComponentModel.DataAnnotations;

namespace ProjectTimeSheet.Models
{
    public class TaskViewModel
    {
        [Display(Name = "Task ID")]
        public int TaskId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }
    }
}