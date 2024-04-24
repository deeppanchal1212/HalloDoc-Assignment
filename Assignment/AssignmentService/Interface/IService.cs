using AssignmentEntity.DataModels;
using AssignmentEntity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentService.Interface
{
    public interface IService
    {
        List<City> GetCity();
        void AddTask(AddTaskViewModel vm);
        List<AssignmentEntity.DataModels.Task> GetAllTask();
        AddTaskViewModel GetTaskData(int id);
        void EditTask(AddTaskViewModel vm, int id);
        void DeleteTask(int id);
    }
}
