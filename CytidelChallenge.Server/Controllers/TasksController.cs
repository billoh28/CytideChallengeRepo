using CytidelChallenge.Server.Model;
using CytidelChallenge.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CytidelChallenge.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TasksController : ControllerBase
    {

        private readonly ILogger<TasksController> _logger;
        private ITaskService _taskService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<TaskUser> _userManager;
        private readonly SignInManager<TaskUser> _signInManager;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger, ITokenService tokenService, UserManager<TaskUser> userManager, SignInManager<TaskUser> signInManager)
        {
            _logger = logger;
            _taskService = taskService;
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
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
        public IActionResult CreateTask([FromBody] TaskRecord record)
        {
            _logger.LogInformation("HTTPPost CreateTask");

            _taskService.CreateTask(record);

            return Ok();
        }

        [HttpPut("{taskId}")]
        public IActionResult UpdateTask([FromBody] TaskRecord record)
        {
            _logger.LogInformation("HTTPPost UpdateTask");

            _taskService.UpdateTask(record);

            return Ok();
        }

        [HttpDelete("{taskId}")]
        public IActionResult DeleteTask(long taskId)
        {
            _logger.LogInformation("HTTPDelete DeleteTask");

            _taskService.DeleteTask(taskId);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var token = "";

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password,
                                                                 isPersistent: false, lockoutOnFailure: true);
            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid login credentials" });

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null)
            {
                token = _tokenService.GenerateToken(
                        user.Id.ToString(),
                        user.UserName ?? "",
                        new List<string>()
                    );
            }

            return Ok(new { token });
        }
    }
}
