using AspNetCore;
using Assignment.Models;
using AssignmentEntity.ViewModel;
using AssignmentService.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService _service;
        public HomeController(IService service)
        {
            _service = service;
        }

        public IActionResult Index(string searchInput)
        {
            return View();
        }

        [HttpPost]
        public IActionResult TableView(string searchInput)
        {
            List<AssignmentEntity.DataModels.Task> vm = _service.GetAllTask();
            if (searchInput != null)
            {
                searchInput = searchInput.Trim();
                if (!string.IsNullOrEmpty(searchInput))
                {
                    vm = vm.Where(u => u.TaskName.ToLower().Contains(searchInput.ToLower())).ToList();
                }
            }
            return PartialView("_TableView",vm);
        }

        public IActionResult AddTask()
        {
            AddTaskViewModel vm = new();
            vm.City = _service.GetCity();
            return PartialView("_AddTask", vm);
        }

        [HttpPost]
        public IActionResult AddTask(AddTaskViewModel vm)
        {
            _service.AddTask(vm);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditTask(int id)
        {
            AddTaskViewModel vm = _service.GetTaskData(id);
            return PartialView("_EditTask", vm);
        }

        [HttpPost]
        public IActionResult SaveEditTask(AddTaskViewModel vm, int id)
        {
            _service.EditTask(vm, id);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteTask(int id)
        {
            _service.DeleteTask(id);
            return RedirectToAction("Index");

        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}