using System.Collections.Generic;

namespace ProjectTimeSheet.Models
{
    public class UserEditViewModel
    {
        public int EmpId { get; set; }

        public List<Department> Departments { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string EmailId { get; set; }
    }
}