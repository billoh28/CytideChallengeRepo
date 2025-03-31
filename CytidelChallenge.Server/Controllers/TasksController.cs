using CytidelChallenge.Server.Model;
using CytidelChallenge.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CytidelChallenge.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {

        private readonly ILogger<TasksController> _logger;
        private ITaskService _taskService;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger, ITokenService tokenService, IUserService userService)
        {
            _logger = logger;
            _taskService = taskService;
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<TaskRecord> GetTasks()
        {
            var results = new List<TaskRecord>();

            results = _taskService.GetAllTasks();

            return results;
        }

        [HttpGet("{taskId}")]
        [Authorize]
        public TaskRecord GetTaskSpecific(long taskId)
        {
            var result = _taskService.GetTask(taskId);

            return result;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateTask([FromBody] TaskRecord record)
        {
            _taskService.CreateTask(record);

            return Ok();
        }

        [HttpPut("{taskId}")]
        [Authorize]
        public IActionResult UpdateTask([FromBody] TaskRecord record)
        {
            _taskService.UpdateTask(record);

            return Ok();
        }

        [HttpDelete("{taskId}")]
        [Authorize]
        public IActionResult DeleteTask(long taskId)
        {
            _taskService.DeleteTask(taskId);

            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            var token = "";

            try
            {
                var user = _userService.ValidateUser(loginModel.Username, loginModel.Password);

                if (user == null)
                    return Unauthorized();

                token = _tokenService.GenerateToken(
                    user.Id.ToString(),
                    user.Username,
                    user.Roles
                );
            }
            catch (Exception ex) 
            {
                throw ex;
            }

            return Ok(new { token });
        }
    }
}
