using CytidelChallenge.Server.Model;
using CytidelChallenge.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CytidelChallenge.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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
        public IEnumerable<TaskRecord> GetTasks()
        {
            _logger.LogInformation("HTTPGet GetTasks");

            var results = new List<TaskRecord>();

            results = _taskService.GetAllTasks();

            return results;
        }

        [HttpGet("{taskId}")]
        public TaskRecord GetTaskSpecific(long taskId)
        {
            _logger.LogInformation("HTTPGet GetTasks");

            var result = _taskService.GetTask(taskId);

            return result;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult CreateTask([FromBody] TaskRecord record)
        {
            _logger.LogInformation("HTTPPost CreateTask");

            _taskService.CreateTask(record);

            return Ok();
        }

        [HttpPut("{taskId}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateTask([FromBody] TaskRecord record)
        {
            _logger.LogInformation("HTTPPost UpdateTask");

            _taskService.UpdateTask(record);

            return Ok();
        }

        [HttpDelete("{taskId}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteTask(long taskId)
        {
            _logger.LogInformation("HTTPDelete DeleteTask");

            _taskService.DeleteTask(taskId);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            _logger.LogInformation("HTTPPost Login");

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
