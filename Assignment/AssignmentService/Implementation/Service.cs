using AssignmentEntity.DataModels;
using AssignmentEntity.ViewModel;
using AssignmentRepo.Interface;
using AssignmentService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentService.Implementation
{
    public class Service : IService
    {
        private readonly IRepo _repo;

        public Service(IRepo repo)
        {
            _repo = repo;
        }

        public List<City> GetCity()
        {
            return _repo.GetCityList();
        }

        public void AddTask(AddTaskViewModel vm)
        {
            Category category = _repo.GetCategoryByName(vm.Category);
            if (category.Id == 0 && category.Name == null)
            {
                category.Name = vm.Category;
                _repo.AddCategory(category);
                category = _repo.GetCategoryByName(vm.Category);
            }
            City city = _repo.GetCityById(vm.SelectedCityId);
            AssignmentEntity.DataModels.Task task = new()
            {
                TaskName = vm.TaskName,
                Assignee = vm.Assignee,
                CategoryId = category.Id,
                Description = vm.TaskDescription,
                DueDate = vm.DueDate,
                Category = category.Name,
            };
            if(city.CityName != null)
            {
                task.City = city.CityName;
            }
            _repo.AddTask(task);
        }

        public List<AssignmentEntity.DataModels.Task> GetAllTask()
        {
            return _repo.GetAllTask();
        }
        public AddTaskViewModel GetTaskData(int id)
        {
            AssignmentEntity.DataModels.Task task = _repo.GetTaskData(id);
            City city = _repo.GetCityByName(task.City);
            AddTaskViewModel vm = new()
            {
                TaskName = task.TaskName,
                Assignee = task.Assignee,
                TaskDescription = task.Description,
                DueDate = (DateOnly)task.DueDate,
                City = _repo.GetCityList(),
                Category = task.Category,
                SelectedCityId = city.Id,
                TaskId = task.Id
            };
            return vm;
        }
        public void EditTask(AddTaskViewModel vm, int id)
        {
            AssignmentEntity.DataModels.Task task = _repo.GetTaskById(id);
            Category category = _repo.GetCategoryByName(vm.Category);
            if (category.Id == 0 && category.Name == null)
            {
                category.Name = vm.Category;
                _repo.AddCategory(category);
                category = _repo.GetCategoryByName(vm.Category);
            }
            task.TaskName = vm.TaskName;
            task.Assignee = vm.Assignee;
            task.CategoryId = category.Id;
            task.Description = vm.TaskDescription;
            task.DueDate = vm.DueDate;
            City city = _repo.GetCityById(vm.SelectedCityId);
            if (city.CityName != null)
            {
                task.City = city.CityName;
            }
            task.Category = category.Name;
            _repo.UpdateTask(task);
        }
        public void DeleteTask(int id)
        {
            AssignmentEntity.DataModels.Task task = _repo.GetTaskById(id);
            _repo.RemoveTask(task);
        }




    }
}
