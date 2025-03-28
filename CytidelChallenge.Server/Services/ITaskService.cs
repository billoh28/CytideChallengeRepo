using CytidelChallenge.Server.Model;

namespace CytidelChallenge.Server.Services
{
    public interface ITaskService
    {
        public TaskRecord GetTask(long taskId);

        public List<TaskRecord> GetAllTasks();

        public void CreateTask(TaskRecord task);

        public void UpdateTask(TaskRecord task);

        public void DeleteTask(long taskId);
    }
}
