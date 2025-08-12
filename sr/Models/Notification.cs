namespace sr.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string url { get; set; }
        public List<NotificationApplicationUser> NotificationApplicationUsers { get; set; }
    }



}
