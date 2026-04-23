namespace QuizAPIELECT2.Utils
{
    public class MockDataBase
    {
        public  static class MockData
        {
            public static List<Models.Users> Users = new List<Models.Users>
            {
                new Models.Users { Id = 1, Username = "admin", Password = "admin123", Role = "Admin" },
                new Models.Users { Id = 2, Username = "user1", Password = "user123", Role = "User" },
                new Models.Users { Id = 3, Username = "user2", Password = "user123", Role = "User" }
            };

            public static List<Models.TasksItem> Tasks = new List<Models.TasksItem>
            {
                new Models.TasksItem { Id = 1, Title = "Task 1", IsCompleted = false },
                new Models.TasksItem { Id = 2, Title = "Task 2", IsCompleted = true },
                new Models.TasksItem { Id = 3, Title = "Task 3", IsCompleted = false }
            };
        }
    }
}
