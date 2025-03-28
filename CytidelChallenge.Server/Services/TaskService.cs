using CytidelChallenge.Server.Model;

namespace CytidelChallenge.Server.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskDbContext _context;

        public TaskService()
        {
            string databasePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MyApp",
                "tasks.sqlite"
            );

            string? directory = Path.GetDirectoryName(databasePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _context = new TaskDbContext(databasePath);
            _context.Database.EnsureCreated();
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

        /*public List<TaskRecord> GetTasksByStatus(Status status)
        {
            return _context.Tasks
                .Where(t => t.Status == status)
                .ToList();
        }

        public List<TaskRecord> GetTasksByPriority(Priority priority)
        {
            return _context.Tasks
                .Where(t => t.Priority == priority)
                .ToList();
        }*/

        public void CreateTask(TaskRecord task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public void UpdateTask(TaskRecord task)
        {
            var existingTask = _context.Tasks.Find(task.TaskId);

            if (existingTask == null)
            {
                throw new InvalidOperationException($"Task with ID {task.TaskId} not found.");
            }

            _context.Entry(existingTask).CurrentValues.SetValues(task);
            _context.SaveChanges();
        }

        public void DeleteTask(long taskId)
        {
            var task = _context.Tasks.Find(taskId);

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
