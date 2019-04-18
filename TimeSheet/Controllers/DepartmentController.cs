using ProjectTimeSheet.Models;
using System.Configuration;
using System.Web.Mvc;

namespace ProjectTimeSheet.Controllers
{
    public class DepartmentController : Controller
    {

        public DataAccess dataAccess = null;

        public DepartmentController()
        {
            dataAccess = new DataAccess(ConfigurationManager.ConnectionStrings["DbConnection"].ToString());
        }
        // GET: Department
        public ActionResult Index()
        {
            return View(dataAccess.GetListofDepartmentInfo());
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                dataAccess.InsertDepartment(new Department { Name = Request.Form["name"] });
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Department/Edit/5
        public ActionResult Edit(int id)
        {
            return View(dataAccess.GetDepartmentbyID(id));
        }

        // POST: Department/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                dataAccess.InsertDepartment(new Department { DepartmentId = id, Name = Request.Form["name"] });
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Department/Delete/5
        public ActionResult Delete(int id)
        {
            return View(dataAccess.GetDepartmentbyID(id));
        }

        // POST: Department/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                dataAccess.DeleteDepartmentbyID(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
