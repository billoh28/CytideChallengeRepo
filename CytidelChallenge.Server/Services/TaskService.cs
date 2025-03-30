using CytidelChallenge.Server.Model;

namespace CytidelChallenge.Server.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskDbContext _context;

        public TaskService(TaskDbContext context)
        {
            _context = context;
        }

        public TaskRecord GetTask(long taskId)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId == taskId);

            if (task == null)
            {
                throw new InvalidOperationException($"Task with ID {taskId} not found.");
            }

            return task;
        }

        public List<TaskRecord> GetAllTasks()
        {
            return _context.Tasks.ToList();
        }

        public void CreateTask(TaskRecord task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public void UpdateTask(TaskRecord task)
        {
            var existingTask = _context.Tasks.Where(t => t.TaskId == task.TaskId).FirstOrDefault();

            if (existingTask == null)
            {
                throw new InvalidOperationException($"Task with ID {task.TaskId} not found.");
            }

            _context.Entry(existingTask).CurrentValues.SetValues(task);
            _context.SaveChanges();
        }

        public void DeleteTask(long taskId)
        {
            var task = _context.Tasks.Where(t => t.TaskId == taskId).FirstOrDefault();

            if (task == null)
            {
                throw new InvalidOperationException($"Task with ID {taskId} not found.");
            }

            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
