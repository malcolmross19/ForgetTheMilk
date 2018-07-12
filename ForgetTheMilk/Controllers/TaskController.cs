using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForgetTheMilk.Models;
using System.Text.RegularExpressions;

namespace ForgetTheMilk.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View(Tasks);
        }

        public static readonly List<Task> Tasks = new List<Task>();

        [HttpPost]
        public ActionResult Add(string task)
        {
            var taskItem = new Task(task);
            Tasks.Add(taskItem);
            return RedirectToAction("Index");
        }
    }

    public class Task
    {
        public Task(string task)
        {
            Description = task;
            var dueDatePattern = new Regex(@"may\s(\d)");
            var hasDueDate = dueDatePattern.IsMatch(task);

            if (hasDueDate)
            {
                var dueDate = dueDatePattern.Match(task);
                var day = Convert.ToInt32(dueDate.Groups[1].Value);
                DueDate = new DateTime(DateTime.Today.Year, 5, day);
            }
        }

        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
