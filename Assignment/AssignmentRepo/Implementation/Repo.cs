using AssignmentEntity.DataContext;
using AssignmentEntity.DataModels;
using AssignmentRepo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentRepo.Implementation
{
    public class Repo : IRepo
    {
        private readonly ApplicationDbContext _context;
        public Repo(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<City> GetCityList()
        {
            return _context.Cities.ToList();
        }

        public Category GetCategoryByName(string category)
        {
            return _context.Categories.FirstOrDefault(u => u.Name.ToLower() == category.ToLower()) ?? new();
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public City GetCityById(int selectedCityId)
        {
            return _context.Cities.FirstOrDefault(u => u.Id == selectedCityId)??new();
        }

        public void AddTask(AssignmentEntity.DataModels.Task task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public List<AssignmentEntity.DataModels.Task> GetAllTask()
        {
            return _context.Tasks.ToList();
        }
        public AssignmentEntity.DataModels.Task GetTaskData(int id)
        {
            return _context.Tasks.FirstOrDefault(u => u.Id == id)?? new();
        }
        public City GetCityByName(string? city)
        {
            return _context.Cities.FirstOrDefault(u => u.CityName == city)??new();
        }
        public AssignmentEntity.DataModels.Task GetTaskById(int id)
        {
            return _context.Tasks.FirstOrDefault(u => u.Id ==id)??new();
        }
        public void UpdateTask(AssignmentEntity.DataModels.Task task)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
        }
        public void RemoveTask(AssignmentEntity.DataModels.Task task)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }


    }
}
