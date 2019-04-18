using ProjectTimeSheet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace ProjectTimeSheet
{
    public class DataAccess
    {
        private string _connectionString { get; set; }

        public DataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region TimeSheet

        public List<TimeSheet> GetTimeSheetDetails(int userID, string monthYear)
        {
            List<TimeSheet> timeSheets = new List<TimeSheet>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT TSD.* FROM TIMESHEETDETAILS TSD 
                                                   LEFT OUTER JOIN USER U ON  U.USERID=TSD.USERID
                                                   WHERE U.USERID = @SEARCH and MONTHYEAR=@monthYear";
                    command.Parameters.AddWithValue("@SEARCH", userID);
                    command.Parameters.AddWithValue("@monthYear", monthYear);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        timeSheets.Add(new TimeSheet()
                        {
                            TaskId = Convert.ToInt32(reader["TaskID"].ToString()),
                            Date = Convert.ToDateTime(reader["Updated_Date"].ToString()),
                            Hours = Convert.ToDecimal(reader["Hours"].ToString()),
                            Day = Convert.ToInt32(reader["Day"].ToString()),
                            UserId = Convert.ToInt32(reader["UserId"].ToString()),
                        });
                    }
                }
            }
            return timeSheets;
        }

        public List<TimeSheet> GetTimeSheetDetails(int userId, int day, int taskId, string monthYear)
        {
            List<TimeSheet> timeSheets = new List<TimeSheet>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT TSD.* FROM TIMESHEETDETAILS TSD 
                                                   LEFT OUTER JOIN USER U ON  U.USERID=TSD.USERID
                                                   WHERE U.USERID = @SEARCH AND TSD.TASKID=@TASKID AND DAY = @DAY and MONTHYEAR=@monthYear";
                    command.Parameters.AddWithValue("@SEARCH", userId);
                    command.Parameters.AddWithValue("@TASKID", taskId);
                    command.Parameters.AddWithValue("@DAY", day);
                    command.Parameters.AddWithValue("@monthYear", monthYear);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        timeSheets.Add(new TimeSheet()
                        {
                            TaskId = Convert.ToInt32(reader["TaskID"].ToString()),
                            Date = Convert.ToDateTime(reader["Updated_Date"].ToString()),
                            Hours = Convert.ToDecimal(reader["Hours"].ToString()),
                            Day = Convert.ToInt32(reader["Day"].ToString()),
                            UserId = Convert.ToInt32(reader["UserId"].ToString()),

                        });
                    }
                }
            }
            return timeSheets;
        }

        public void InsertTimeSheetDetails(List<TimeSheet> timeSheets, string monthYear)
        {
            foreach (var item in timeSheets)
            {
                var values = GetTimeSheetDetails(item.UserId, item.Day, item.TaskId, monthYear);
                if (values.Count() == 0)
                {
                    if (item.Hours > 0)
                    {
                        using (var connection = new SQLiteConnection(_connectionString))
                        {
                            connection.Open();
                            using (var command = new SQLiteCommand(connection))
                            {
                                command.CommandText = @"INSERT INTO TimeSheetDetails (UserId,TaskId,Updated_Date,Hours,Day,MonthYear) VALUES (@UserId,@TaskId,@Updated_Date,@Hours,@Day,@MonthYear)";
                                command.Parameters.AddWithValue("@UserId", item.UserId);
                                command.Parameters.AddWithValue("@TaskId", item.TaskId);
                                command.Parameters.AddWithValue("@Updated_Date", item.Date);
                                command.Parameters.AddWithValue("@Hours", item.Hours);
                                command.Parameters.AddWithValue("@Day", item.Day);
                                command.Parameters.AddWithValue("@MonthYear", monthYear);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                else
                {
                    if (values.Where(x => x.Hours == item.Hours).Count() == 0)
                    {
                        using (var connection = new SQLiteConnection(_connectionString))
                        {
                            connection.Open();
                            using (var command = new SQLiteCommand(connection))
                            {
                                command.CommandText = @"UPDATE TimeSheetDetails
                                                           SET Hours = @Hours
                                                           ,Updated_Date=@Updated_Date
                                                           WHERE UserId=@UserId and TaskId=@TaskId and MonthYear=@MonthYear and Day=@Day ";
                                command.Parameters.AddWithValue("@UserId", item.UserId);
                                command.Parameters.AddWithValue("@TaskId", item.TaskId);
                                command.Parameters.AddWithValue("@Updated_Date", item.Date);
                                command.Parameters.AddWithValue("@Hours", item.Hours);
                                command.Parameters.AddWithValue("@Day", item.Day);
                                command.Parameters.AddWithValue("@MonthYear", monthYear);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }


        public List<TimeSheetWeekly> GetTimeSheetWeeklyDetails(string monthYear)
        {
            var weekList = GetWeekList(monthYear);
            List<TimeSheetWeekly> timeSheets = new List<TimeSheetWeekly>();
            foreach (var item in GetListofUsers())
            {
                var userinfo = new TimeSheetWeekly()
                {
                    FullName = item.FullName,
                    User = item.UserName,
                    EmpID = item.EmpId,
                };
                List<int> totalhours = new List<int>();
                int counter = 0;
                foreach (var weekItem in weekList)
                {
                    counter++;
                    using (var connection = new SQLiteConnection(_connectionString))
                    {
                        connection.Open();

                        using (var command = new SQLiteCommand(connection))
                        {
                            command.CommandText = @"SELECT SUM(HOURS) AS TOTAL FROM TIMESHEETDETAILS WHERE MONTHYEAR=@MONTHYEAR AND DAY IN (" + weekItem + ") AND USERID=@USERID";
                            command.Parameters.AddWithValue("@MONTHYEAR", monthYear);
                            command.Parameters.AddWithValue("@USERID", item.UserId);

                            object o = command.ExecuteScalar();
                            if (o != null && !string.IsNullOrEmpty(o.ToString()))
                            {
                                switch (counter)
                                {
                                    case 1:
                                        userinfo.Week1 = Convert.ToDecimal(o.ToString());
                                        break;
                                    case 2:
                                        userinfo.Week2 = Convert.ToDecimal(o.ToString());
                                        break;
                                    case 3:
                                        userinfo.Week3 = Convert.ToDecimal(o.ToString());
                                        break;
                                    case 4:
                                        userinfo.Week4 = Convert.ToDecimal(o.ToString());
                                        break;
                                    case 5:
                                        userinfo.Week5 = Convert.ToDecimal(o.ToString());
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                switch (counter)
                                {
                                    case 1:
                                        userinfo.Week1 = 0;
                                        break;
                                    case 2:
                                        userinfo.Week2 = 0;
                                        break;
                                    case 3:
                                        userinfo.Week3 = 0;
                                        break;
                                    case 4:
                                        userinfo.Week4 = 0;
                                        break;
                                    case 5:
                                        userinfo.Week5 = 0;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
                timeSheets.Add(userinfo);
            }
            return timeSheets;
        }

        private List<string> GetWeekList(string monthYear)
        {
            int intYear = Convert.ToInt32(monthYear.Substring(2, 4));
            int intMonth = Convert.ToInt32(monthYear.Substring(0, 2));
            int daysinMonth = DateTime.DaysInMonth(intYear, intMonth);
            List<string> days = new List<string>();
            List<string> week = new List<string>();
            DateTime oBeginnngOfThisMonth = new DateTime(intYear, intMonth, 1);
            for (int i = 0; i < daysinMonth + 1; i++)
            {
                if (oBeginnngOfThisMonth.AddDays(i).DayOfWeek == DayOfWeek.Monday)
                {
                    week.Add(string.Join(",", days));
                    days.Clear();
                }
                if (daysinMonth >= i + 1)
                {
                    days.Add((i + 1).ToString());
                }
            }
            if (days.Any())
            {
                week.Add(string.Join(",", days));
                days.Clear();
            }
            return week;
        }

        #endregion

        #region Department

        public List<Department> GetListofDepartmentInfo()
        {
            List<Department> departments = new List<Department>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT * FROM DEPARTMENT";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        departments.Add(
                            new Department()
                            {
                                DepartmentId = Convert.ToInt32(reader["DEPTID"].ToString()),
                                Name = reader["NAME"].ToString(),
                            });
                    }
                }
            }
            return departments;
        }

        public int GetCountofDepartment()
        {
            int count = 0;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT COUNT(*) FROM DEPARTMENT";
                    object o = command.ExecuteScalar();
                    if (o != null)
                    {
                        count = Convert.ToInt32(o.ToString());
                    }
                }
            }
            return count;
        }

        public Department GetDepartmentbyID(int id)
        {
            Department department = null;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM Department WHERE DEPTID = @DEPTID;";
                    command.Parameters.AddWithValue("@DEPTID", id);
                    var reader = command.ExecuteReader();
                    department = new Department();
                    while (reader.Read())
                    {
                        department.DepartmentId = Convert.ToInt32(reader["deptID"].ToString());
                        department.Name = reader["name"].ToString();
                    }
                }
            }
            return department;
        }

        public void InsertDepartment(Department department)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"INSERT INTO DEPARTMENT (NAME) VALUES (@NAME);";
                    command.Parameters.AddWithValue("@NAME", department.Name);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateDepartment(Department department)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"UPDATE DEPARTMENT SET NAME = @NAME
                                                   WHERE DEPTID= @DEPTID;";
                    command.Parameters.AddWithValue("@NAME", department.Name);
                    command.Parameters.AddWithValue("@DEPTID", department.DepartmentId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDepartmentbyID(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"Delete FROM Department WHERE DEPTID = @DEPTID;";
                    command.Parameters.AddWithValue("@DEPTID", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region User

        public int GetCountofUser()
        {
            int count = 0;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT COUNT(*) FROM USER";
                    object o = command.ExecuteScalar();
                    if (o != null)
                    {
                        count = Convert.ToInt32(o.ToString());
                    }
                }
            }
            return count;
        }

        public int GetUserID(string userName)
        {
            int id = 0;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT USERID FROM USER WHERE NAME LIKE @SEARCH";
                    command.Parameters.AddWithValue("@SEARCH", "%" + userName + "%");
                    object o = command.ExecuteScalar();
                    if (o != null)
                    {
                        id = Convert.ToInt32(o.ToString());
                    }
                }
            }
            return id;
        }

        public List<User> GetListofUsers()
        {
            List<User> users = new List<User>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT * FROM USER";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        users.Add(
                            new User()
                            {
                                UserId = Convert.ToInt32(reader["UserID"].ToString()),
                                UserName = reader["UserName"].ToString(),
                                DepartmentId = Convert.ToInt32(reader["DeptID"]),
                                EmailId = reader["EmailId"].ToString(),
                                EmpId = Convert.ToInt32(reader["EmpId"].ToString()),
                                FullName = reader["FullName"].ToString(),
                            });
                    }
                }
            }
            return users;
        }

        public List<Task> GetTaskListbyUserName(int userID)
        {
            List<Task> tasks = new List<Task>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT T.TASKID,T.NAME FROM TASK T
                                            JOIN DEPARTMENT D ON D.DEPTID=T.DEPTID
                                            JOIN USER U ON U.DEPTID=T.DEPTID
                                            WHERE U.USERID = @SEARCH
                                            ORDER BY T.TASKID";
                    command.Parameters.AddWithValue("@SEARCH", userID);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        tasks.Add(new Task()
                        {
                            TaskId = Convert.ToInt32(reader["TaskID"].ToString()),
                            Name = reader["Name"].ToString(),
                        });
                    }
                }
            }
            return tasks;
        }

        public List<UserViewModel> GetListofUsersDetails()
        {
            List<UserViewModel> users = new List<UserViewModel>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT U.*,D.NAME FROM USER U
                                                   JOIN DEPARTMENT D ON D.DEPTID=U.DEPTID ";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        users.Add(
                            new UserViewModel()
                            {
                                UserId = Convert.ToInt32(reader["UserID"].ToString()),
                                UserName = reader["UserName"].ToString(),
                                Department = reader["Name"].ToString(),
                                EmailId = reader["EmailId"].ToString(),
                                EmpId = Convert.ToInt32(reader["EmpId"].ToString()),
                                FullName = reader["FullName"].ToString(),
                            });
                    }
                }
            }
            return users;
        }

        public void InsertUser(User user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"INSERT INTO USER
                                                   (USERNAME,DEPTID,EMPID,FULLNAME,EMAILID) VALUES
                                                   (@USERNAME,@DEPTID,@EMPID,@FULLNAME,@EMAILID,@TEAMID);";
                    command.Parameters.AddWithValue("@USERNAME", user.UserName);
                    command.Parameters.AddWithValue("@DEPTID", user.DepartmentId);
                    command.Parameters.AddWithValue("@EMPID", user.EmpId);
                    command.Parameters.AddWithValue("@FULLNAME", user.FullName);
                    command.Parameters.AddWithValue("@TEAMID", 0);
                    if (string.IsNullOrEmpty(user.EmailId))
                    {
                        command.Parameters.AddWithValue("@EMAILID", string.Empty);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@EMAILID", user.EmailId);
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUser(User user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"UPDATE USER SET 
                                                   USERNAME=@USERNAME,
                                                   DEPTID=@DEPTID,
                                                   EMPID=@EMPID,
                                                   FULLNAME=@FULLNAME,
                                                   EMAILID=@EMAILID
                                                   WHERE USERID= @USERID;";
                    command.Parameters.AddWithValue("@USERNAME", user.UserName);
                    command.Parameters.AddWithValue("@DEPTID", user.DepartmentId);
                    command.Parameters.AddWithValue("@EMPID", user.EmpId);
                    command.Parameters.AddWithValue("@FULLNAME", user.FullName);
                    command.Parameters.AddWithValue("@EMAILID", user.EmailId);
                    command.Parameters.AddWithValue("@USERID", user.UserId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUserbyID(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"Delete FROM USER WHERE USERID = @USERID;";
                    command.Parameters.AddWithValue("@USERID", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public User GetUserInfo(int id)
        {
            User user = null;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT * FROM USER WHERE USERID=@USERID";
                    command.Parameters.AddWithValue("@USERID", id);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        user = new User()
                        {
                            UserId = Convert.ToInt32(reader["UserID"].ToString()),
                            UserName = reader["UserName"].ToString(),
                            DepartmentId = Convert.ToInt32(reader["DeptID"]),
                            EmailId = reader["EmailId"].ToString(),
                            EmpId = Convert.ToInt32(reader["EmpId"].ToString()),
                            FullName = reader["FullName"].ToString(),
                        };
                    }
                }
            }
            return user;
        }
        #endregion

        #region Task

        public int GetCountofTask()
        {
            int count = 0;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT COUNT(*) FROM TASK";
                    object o = command.ExecuteScalar();
                    if (o != null)
                    {
                        count = Convert.ToInt32(o.ToString());
                    }
                }
            }
            return count;
        }

        public List<TaskViewModel> GetListofTaskInfo()
        {
            List<TaskViewModel> tasks = new List<TaskViewModel>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT T.TASKID,T.NAME,D.NAME as DEPTNAME FROM TASK T
                                                   JOIN DEPARTMENT D ON D.DEPTID=T.DEPTID";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        tasks.Add(
                            new TaskViewModel()
                            {
                                TaskId = Convert.ToInt32(reader["TASKID"].ToString()),
                                Name = reader["NAME"].ToString(),
                                Department = reader["DEPTNAME"].ToString()
                            });
                    }
                }
            }
            return tasks;
        }

        public void InsertTask(Task task)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"INSERT INTO TASK
                                                   (NAME,DEPTID) VALUES
                                                   (@Name,@DEPTID);";
                    command.Parameters.AddWithValue("@Name", task.Name);
                    command.Parameters.AddWithValue("@DEPTID", task.DepartmentId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTask(Task task)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"UPDATE TASK SET 
                                                   NAME=@NAME,
                                                   DEPTID=@DEPTID
                                                   WHERE TASKID= @TASKID;";
                    command.Parameters.AddWithValue("@NAME", task.Name);
                    command.Parameters.AddWithValue("@DEPTID", Convert.ToInt32(task.DepartmentId));
                    command.Parameters.AddWithValue("@TASKID", task.TaskId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTaskbyID(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"Delete FROM TASK WHERE TASKID = @TASKID;";
                    command.Parameters.AddWithValue("@TASKID", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public TaskViewModel GetTaskInfobyId(int id)
        {
            TaskViewModel task = null;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT * FROM TASK WHERE TASKID=@TASKID";
                    command.Parameters.AddWithValue("@TASKID", id);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        task = new TaskViewModel()
                        {
                            TaskId = Convert.ToInt32(reader["TASKID"].ToString()),
                            Name = reader["NAME"].ToString(),
                            Department = reader["DEPTID"].ToString()
                        };
                    }
                }
            }
            return task;
        }

        #endregion
    }
}