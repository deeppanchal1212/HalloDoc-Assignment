using AssignmentEntity.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentRepo.Interface
{
    public interface IRepo
    {
        List<City> GetCityList();
        Category GetCategoryByName(string category);
        void AddCategory(Category category);
        City GetCityById(int selectedCityId);
        void AddTask(AssignmentEntity.DataModels.Task task);
        List<AssignmentEntity.DataModels.Task> GetAllTask();
        AssignmentEntity.DataModels.Task GetTaskData(int id);
        City GetCityByName(string? city);
        AssignmentEntity.DataModels.Task GetTaskById(int id);
        void UpdateTask(AssignmentEntity.DataModels.Task task);
        void RemoveTask(AssignmentEntity.DataModels.Task task);
    }
}
