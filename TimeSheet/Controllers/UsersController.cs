using ProjectTimeSheet.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace ProjectTimeSheet.Controllers
{
    public class UsersController : Controller
    {
        public DataAccess dataAccess = null;

        public UsersController()
        {
            dataAccess = new DataAccess(ConfigurationManager.ConnectionStrings["DbConnection"].ToString());
        }
        // GET: Users
        public ActionResult Index()
        {
            return View(dataAccess.GetListofUsersDetails());
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.Departments = dataAccess.GetListofDepartmentInfo().Select(n => new SelectListItem() { Value = n.DepartmentId.ToString(), Text = n.Name });
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                dataAccess.InsertUser(
                    new User
                    {
                        UserName = Request.Form["UserName"],
                        EmailId = Request.Form["EmailId"],
                        EmpId = Convert.ToInt32(Request.Form["EmpId"]),
                        FullName = Request.Form["FullName"],
                        DepartmentId = Convert.ToInt32(Request.Form["DepartmentId"])
                    });

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            var userInfo = dataAccess.GetUserInfo(id);
            ViewBag.Departments = dataAccess.GetListofDepartmentInfo().Select(n => new SelectListItem() { Value = n.DepartmentId.ToString(), Text = n.Name, Selected = (n.DepartmentId == userInfo.DepartmentId) });
            return View(userInfo);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                dataAccess.UpdateUser(
                    new User()
                    {
                        UserId = id,
                        UserName = Request.Form["UserName"],
                        EmailId = Request.Form["EmailId"],
                        EmpId = Convert.ToInt32(Request.Form["EmpId"]),
                        FullName = Request.Form["FullName"],
                        DepartmentId = Convert.ToInt32(Request.Form["DepartmentId"])
                    });

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View(dataAccess.GetUserInfo(id));
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                dataAccess.DeleteUserbyID(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
