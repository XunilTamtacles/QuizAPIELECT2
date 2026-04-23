using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using QuizAPIELECT2.Models;
using QuizAPIELECT2.Models;
using static QuizAPIELECT2.Utils.MockDataBase;


namespace QuizAPIELECT2.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        [EnableRateLimiting("tasksPolicy")]
        public IActionResult GetAll()
        {
            return Ok(MockData.Tasks);
        }

       
        [HttpGet("{taskId}")]
        [Authorize(Roles = "Admin,User")]
        [EnableRateLimiting("tasksPolicy")]
        public IActionResult GetById(int taskId)
        {
            var item = MockData.Tasks.FirstOrDefault(x => x.Id == taskId);

            if (item == null)
                return NotFound("No task found.");

            return Ok(item);
        }

       
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        [EnableRateLimiting("tasksPolicy")]
        public IActionResult Add(CreateTasksRequest input)
        {
            int nextId = MockData.Tasks.Count > 0
                ? MockData.Tasks.Max(x => x.Id) + 1
                : 1;

            var task = new TasksItem
            {
                Id = nextId,
                Title = input.Title,
                IsCompleted = input.IsCompleted
            };

            MockData.Tasks.Add(task);

            return Ok(task);
        }

        [HttpPut("{taskId}")]
        [Authorize(Roles = "Admin,User")]
        [EnableRateLimiting("tasksPolicy")]
        public IActionResult Edit(int taskId, UpdateTaskRequest input)
        {
            var item = MockData.Tasks.FirstOrDefault(x => x.Id == taskId);

            if (item == null)
                return NotFound("Task not found.");

            item.Title = input.Title;
            item.IsCompleted = input.IsCompleted;

            return Ok(item);
        }

        
        [HttpDelete("{taskId}")]
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("tasksPolicy")]
        public IActionResult Remove(int taskId)
        {
            var item = MockData.Tasks.FirstOrDefault(x => x.Id == taskId);

            if (item == null)
                return NotFound("Task not found.");

            MockData.Tasks.Remove(item);

            return Ok("Task removed successfully.");
        }
    }
}