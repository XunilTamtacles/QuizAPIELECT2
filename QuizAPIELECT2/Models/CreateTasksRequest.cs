namespace QuizAPIELECT2.Models
{
    public class CreateTasksRequest
    {
        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } 
    }
}
