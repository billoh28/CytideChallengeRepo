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
            var result = _taskService.GetTask(taskId);

            return result;
        }

        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskRecord record)
        {
            _taskService.CreateTask(record);

            return Ok();
        }

        [HttpPut("{taskId}")]
        public IActionResult UpdateTask([FromBody] TaskRecord record)
        {
            _taskService.UpdateTask(record);

            return Ok();
        }

        [HttpDelete("{taskId}")]
        public IActionResult DeleteTask(long taskId)
        {
            _taskService.DeleteTask(taskId);

            return Ok();
        }
    }
}
