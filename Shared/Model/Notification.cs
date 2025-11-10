namespace James.Shared.Model
{
    public class Notification
    {
        public Notification()
        {
        }
        public Notification(string title, DateTime createdDate, string content)
        {
            Title = title;
            CreatedDate = createdDate;
            Content = content;
        }
        public string Title { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; } = null!;
    }
}
