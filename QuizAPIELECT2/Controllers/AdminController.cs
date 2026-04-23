using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizAPIELECT2.Models;
using QuizAPIELECT2.Utils;
using static QuizAPIELECT2.Utils.MockDataBase;

namespace QuizAPIELECT2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
    
        [HttpGet("tasks")]
        public IActionResult GetAllTasks()
        {
            return Ok(MockData.Tasks);
        }

        
        [HttpPut("tasks/{id}")]
        public IActionResult UpdateTask(int id, TasksItem updated)
        {
            var task = MockData.Tasks.FirstOrDefault(x => x.Id == id);

            if (task == null)
                return NotFound("Task not found");

            task.Title = updated.Title;
            task.IsCompleted = updated.IsCompleted;

            return Ok(task);
        }

       
        [HttpDelete("tasks/{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = MockData.Tasks.FirstOrDefault(x => x.Id == id);

            if (task == null)
                return NotFound("Task not found");

            MockData.Tasks.Remove(task);

            return Ok("Task deleted successfully");
        }
    }
}