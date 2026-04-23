namespace QuizAPIELECT2.Models
{
    public class TasksItem
    {
        public int Id { get; set; }
        
        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;
    }
}
