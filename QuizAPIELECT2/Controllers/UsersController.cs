using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizAPIELECT2.Models;
using QuizAPIELECT2.Utils;
using static QuizAPIELECT2.Utils.MockDataBase;

namespace QuizAPIELECT2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Admin")] 
    public class UsersController : ControllerBase
    {
      
        [HttpGet("tasks")]
        public IActionResult GetTasks()
        {
            return Ok(MockData.Tasks);
        }

        
        [HttpGet("tasks/{id}")]
        public IActionResult GetTask(int id)
        {
            var task = MockData.Tasks.FirstOrDefault(x => x.Id == id);

            if (task == null)
                return NotFound("Task not found");

            return Ok(task);
        }

        
        [HttpPost("tasks")]
        public IActionResult CreateTask(TasksItem task)
        {
            task.Id = MockData.Tasks.Count + 1;
            MockData.Tasks.Add(task);

            return Ok(task);
        }
    }
}