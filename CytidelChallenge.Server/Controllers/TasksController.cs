using CytidelChallenge.Server.Model;
using CytidelChallenge.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace CytidelChallenge.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {

        private readonly ILogger<TasksController> _logger;
        private ITaskService _taskService;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _logger = logger;
            _taskService = taskService;
        }

        [HttpGet]
        public IEnumerable<TaskRecord> GetTasks()
        {
            var results = new List<TaskRecord>();

            results = _taskService.GetAllTasks();

            return results;
        }

        [HttpGet("{taskId}")]
        public TaskRecord GetTaskSpecific(long taskId)
        {
            /*_taskService.CreateTask(new TaskRecord {
                Title = "Testing Data",
                Description = "Testing data for testing DB and that",
                DueDate = DateTime.Now,
                Priority = Priority.Low,
                Status = Status.InProgress
            });*/

            var result = _taskService.GetTask(taskId);

            return result;
        }

        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskRecord record)
        {
            _taskService.CreateTask(record);

            return Ok();
        }
    }
}
