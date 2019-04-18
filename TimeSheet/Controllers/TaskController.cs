using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace ProjectTimeSheet.Controllers
{
    public class TaskController : Controller
    {

        public DataAccess dataAccess = null;

        public TaskController()
        {
            dataAccess = new DataAccess(ConfigurationManager.ConnectionStrings["DbConnection"].ToString());
        }

        // GET: Task
        public ActionResult Index()
        {
            return View(dataAccess.GetListofTaskInfo());
        }

        // GET: Task/Create
        public ActionResult Create()
        {
            ViewBag.Departments = dataAccess.GetListofDepartmentInfo().Select(n => new SelectListItem() { Value = n.DepartmentId.ToString(), Text = n.Name });
            return View();
        }

        // POST: Task/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                dataAccess.InsertTask(
                    new Models.Task()
                    {
                        Name = Request.Form["Name"],
                        DepartmentId = Request.Form["Department"],
                    });
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Task/Edit/5
        public ActionResult Edit(int id)
        {
            var taskinfo = dataAccess.GetTaskInfobyId(id);
            ViewBag.Departments = dataAccess.GetListofDepartmentInfo().Select(n => new SelectListItem() { Value = n.DepartmentId.ToString(), Text = n.Name, Selected = (n.DepartmentId == Convert.ToInt32(taskinfo.Department)) });
            return View(taskinfo);
        }

        // POST: Task/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                dataAccess.UpdateTask(
                    new Models.Task()
                    {
                        TaskId = id,
                        Name = Request.Form["Name"],
                        DepartmentId = Request.Form["Department"]
                    });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Task/Delete/5
        public ActionResult Delete(int id)
        {
            return View(dataAccess.GetTaskInfobyId(id));
        }

        // POST: Task/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                dataAccess.DeleteTaskbyID(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
