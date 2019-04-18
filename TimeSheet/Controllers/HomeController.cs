using ClosedXML.Excel;
using ProjectTimeSheet.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ProjectTimeSheet.Controllers
{
    public class HomeController : Controller
    {

        public DataAccess dataAccess = null;


        public HomeController()
        {
            dataAccess = new DataAccess(ConfigurationManager.ConnectionStrings["DbConnection"].ToString());
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetUserName(string prefix)
        {
            var user = dataAccess.GetListofUsers();
            var filteredItems = user.Where(item => item.UserName.Contains(prefix) || item.FullName.Contains(prefix)).Select(x => new { val = x.UserId.ToString(), label = x.UserName }).ToList();
            return Json(filteredItems);
        }

        public ActionResult TaskDetails(string userID, string monthYear)
        {
            Session["user"] = userID;
            Session["monthsYears"] = monthYear;
            List<TaskDetailsViewModel> tasks = new List<TaskDetailsViewModel>();
            List<TimeSheet> timesheet = new List<TimeSheet>();

            foreach (var item in dataAccess.GetTaskListbyUserName(Convert.ToInt32(userID)))
            {
                tasks.Add(new TaskDetailsViewModel()
                {
                    TaskId = item.TaskId,
                    TaskName = item.Name
                });
            }
            timesheet = dataAccess.GetTimeSheetDetails(Convert.ToInt32(userID), monthYear);

            foreach (var taskItem in tasks)
            {
                var timesheetInfo = timesheet.Where(x => x.TaskId == taskItem.TaskId).ToList();
                foreach (var item in timesheetInfo)
                {
                    switch (item.Day)
                    {
                        case 1:
                            taskItem.Day1 = item.Hours;
                            break;
                        case 2:
                            taskItem.Day2 = item.Hours;
                            break;
                        case 3:
                            taskItem.Day3 = item.Hours;
                            break;
                        case 4:
                            taskItem.Day4 = item.Hours;
                            break;
                        case 5:
                            taskItem.Day5 = item.Hours;
                            break;
                        case 6:
                            taskItem.Day6 = item.Hours;
                            break;
                        case 7:
                            taskItem.Day7 = item.Hours;
                            break;
                        case 8:
                            taskItem.Day8 = item.Hours;
                            break;
                        case 9:
                            taskItem.Day9 = item.Hours;
                            break;
                        case 10:
                            taskItem.Day10 = item.Hours;
                            break;
                        case 11:
                            taskItem.Day11 = item.Hours;
                            break;
                        case 12:
                            taskItem.Day12 = item.Hours;
                            break;
                        case 13:
                            taskItem.Day13 = item.Hours;
                            break;
                        case 14:
                            taskItem.Day14 = item.Hours;
                            break;
                        case 15:
                            taskItem.Day15 = item.Hours;
                            break;
                        case 16:
                            taskItem.Day16 = item.Hours;
                            break;
                        case 17:
                            taskItem.Day17 = item.Hours;
                            break;
                        case 18:
                            taskItem.Day18 = item.Hours;
                            break;
                        case 19:
                            taskItem.Day19 = item.Hours;
                            break;
                        case 20:
                            taskItem.Day20 = item.Hours;
                            break;
                        case 21:
                            taskItem.Day21 = item.Hours;
                            break;
                        case 22:
                            taskItem.Day22 = item.Hours;
                            break;
                        case 23:
                            taskItem.Day23 = item.Hours;
                            break;
                        case 24:
                            taskItem.Day24 = item.Hours;
                            break;
                        case 25:
                            taskItem.Day25 = item.Hours;
                            break;
                        case 26:
                            taskItem.Day26 = item.Hours;
                            break;
                        case 27:
                            taskItem.Day27 = item.Hours;
                            break;
                        case 28:
                            taskItem.Day28 = item.Hours;
                            break;
                        case 29:
                            taskItem.Day29 = item.Hours;
                            break;
                        case 30:
                            taskItem.Day30 = item.Hours;
                            break;
                        case 31:
                            taskItem.Day31 = item.Hours;
                            break;
                        default:
                            break;
                    }
                }
            }
            var orderList = tasks.OrderBy(x => x.TaskId).ToList();
            return PartialView(orderList);
        }

        [HttpPost]
        public ActionResult SubmitTimeSheet(IEnumerable<TaskDetailsViewModel> result)
        {
            List<TimeSheet> timeSheets = new List<TimeSheet>();
            foreach (var item in result)
            {
                foreach (var propertie in item.GetType().GetProperties().ToList())
                {
                    var timesheet = new TimeSheet()
                    {
                        TaskId = item.TaskId,
                        UserId = Convert.ToInt32(Session["user"].ToString()),
                        Date = DateTime.Now
                    };

                    switch (propertie.Name.ToString())
                    {
                        case "Day1":
                            timesheet.Day = 1;
                            timesheet.Hours = item.Day1;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day2":
                            timesheet.Day = 2;
                            timesheet.Hours = item.Day2;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day3":
                            timesheet.Day = 3;
                            timesheet.Hours = item.Day3;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day4":
                            timesheet.Day = 4;
                            timesheet.Hours = item.Day4;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day5":
                            timesheet.Day = 5;
                            timesheet.Hours = item.Day5;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day6":
                            timesheet.Day = 6;
                            timesheet.Hours = item.Day6; timeSheets.Add(timesheet);
                            break;
                        case "Day7":
                            timesheet.Day = 7;
                            timesheet.Hours = item.Day7;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day8":
                            timesheet.Day = 8;
                            timesheet.Hours = item.Day8;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day9":
                            timesheet.Day = 9;
                            timesheet.Hours = item.Day9;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day10":
                            timesheet.Day = 10;
                            timesheet.Hours = item.Day10;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day11":
                            timesheet.Day = 11;
                            timesheet.Hours = item.Day11;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day12":
                            timesheet.Day = 12;
                            timesheet.Hours = item.Day12;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day13":
                            timesheet.Day = 13;
                            timesheet.Hours = item.Day13;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day14":
                            timesheet.Day = 14;
                            timesheet.Hours = item.Day14;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day15":
                            timesheet.Day = 15;
                            timesheet.Hours = item.Day15;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day16":
                            timesheet.Day = 16;
                            timesheet.Hours = item.Day16;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day17":
                            timesheet.Day = 17;
                            timesheet.Hours = item.Day17;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day18":
                            timesheet.Day = 18;
                            timesheet.Hours = item.Day18;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day19":
                            timesheet.Day = 19;
                            timesheet.Hours = item.Day19;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day20":
                            timesheet.Day = 20;
                            timesheet.Hours = item.Day20;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day21":
                            timesheet.Day = 21;
                            timesheet.Hours = item.Day21;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day22":
                            timesheet.Day = 22;
                            timesheet.Hours = item.Day22;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day23":
                            timesheet.Day = 23;
                            timesheet.Hours = item.Day23;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day24":
                            timesheet.Day = 24;
                            timesheet.Hours = item.Day24;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day25":
                            timesheet.Day = 25;
                            timesheet.Hours = item.Day25;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day26":
                            timesheet.Day = 26;
                            timesheet.Hours = item.Day26;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day27":
                            timesheet.Day = 27;
                            timesheet.Hours = item.Day27;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day28":
                            timesheet.Day = 28;
                            timesheet.Hours = item.Day28;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day29":
                            timesheet.Day = 29;
                            timesheet.Hours = item.Day29;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day30":
                            timesheet.Day = 30;
                            timesheet.Hours = item.Day30;
                            timeSheets.Add(timesheet);
                            break;
                        case "Day31":
                            timesheet.Day = 31;
                            timesheet.Hours = item.Day31;
                            timeSheets.Add(timesheet);
                            break;
                        default:
                            break;
                    }
                }
            }
            dataAccess.InsertTimeSheetDetails(timeSheets, Session["monthsYears"].ToString());
            return PartialView();
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public ActionResult ExporttoExcel(string userID, string monthYear)
        {
            List<TaskDetailsViewModel> tasks = new List<TaskDetailsViewModel>();
            List<TimeSheet> timesheet = new List<TimeSheet>();

            foreach (var item in dataAccess.GetTaskListbyUserName(Convert.ToInt32(userID)))
            {
                tasks.Add(new TaskDetailsViewModel()
                {
                    TaskId = item.TaskId,
                    TaskName = item.Name
                });
            }
            timesheet = dataAccess.GetTimeSheetDetails(Convert.ToInt32(userID), monthYear);

            foreach (var taskItem in tasks)
            {
                var timesheetInfo = timesheet.Where(x => x.TaskId == taskItem.TaskId).ToList();
                foreach (var item in timesheetInfo)
                {
                    switch (item.Day)
                    {
                        case 1:
                            taskItem.Day1 = item.Hours;
                            break;
                        case 2:
                            taskItem.Day2 = item.Hours;
                            break;
                        case 3:
                            taskItem.Day3 = item.Hours;
                            break;
                        case 4:
                            taskItem.Day4 = item.Hours;
                            break;
                        case 5:
                            taskItem.Day5 = item.Hours;
                            break;
                        case 6:
                            taskItem.Day6 = item.Hours;
                            break;
                        case 7:
                            taskItem.Day7 = item.Hours;
                            break;
                        case 8:
                            taskItem.Day8 = item.Hours;
                            break;
                        case 9:
                            taskItem.Day9 = item.Hours;
                            break;
                        case 10:
                            taskItem.Day10 = item.Hours;
                            break;
                        case 11:
                            taskItem.Day11 = item.Hours;
                            break;
                        case 12:
                            taskItem.Day12 = item.Hours;
                            break;
                        case 13:
                            taskItem.Day13 = item.Hours;
                            break;
                        case 14:
                            taskItem.Day14 = item.Hours;
                            break;
                        case 15:
                            taskItem.Day15 = item.Hours;
                            break;
                        case 16:
                            taskItem.Day16 = item.Hours;
                            break;
                        case 17:
                            taskItem.Day17 = item.Hours;
                            break;
                        case 18:
                            taskItem.Day18 = item.Hours;
                            break;
                        case 19:
                            taskItem.Day19 = item.Hours;
                            break;
                        case 20:
                            taskItem.Day20 = item.Hours;
                            break;
                        case 21:
                            taskItem.Day21 = item.Hours;
                            break;
                        case 22:
                            taskItem.Day22 = item.Hours;
                            break;
                        case 23:
                            taskItem.Day23 = item.Hours;
                            break;
                        case 24:
                            taskItem.Day24 = item.Hours;
                            break;
                        case 25:
                            taskItem.Day25 = item.Hours;
                            break;
                        case 26:
                            taskItem.Day26 = item.Hours;
                            break;
                        case 27:
                            taskItem.Day27 = item.Hours;
                            break;
                        case 28:
                            taskItem.Day28 = item.Hours;
                            break;
                        case 29:
                            taskItem.Day29 = item.Hours;
                            break;
                        case 30:
                            taskItem.Day30 = item.Hours;
                            break;
                        case 31:
                            taskItem.Day31 = item.Hours;
                            break;
                        default:
                            break;
                    }
                }
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ToDataTable(tasks));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }
            
        }
    }
}