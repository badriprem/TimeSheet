using System.Configuration;
using System.Web.Mvc;

namespace ProjectTimeSheet.Controllers
{
    public class AdminController : Controller
    {
        public DataAccess dataAccess = null;

        public AdminController()
        {
            dataAccess = new DataAccess(ConfigurationManager.ConnectionStrings["DbConnection"].ToString());
        }
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.usercount = dataAccess.GetCountofUser();
            ViewBag.deptcount = dataAccess.GetCountofDepartment();
            ViewBag.taskcount = dataAccess.GetCountofTask();
            return View();
        }

        public ActionResult TimeSheetWeekly()
        {
            return PartialView(dataAccess.GetTimeSheetWeeklyDetails("092018"));
        }


    }
}