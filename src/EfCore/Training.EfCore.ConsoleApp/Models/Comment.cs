namespace Training.EfCore.ConsoleApp.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public int PostId { get; set; }
        public int? Score { get; set; }
        public string Text { get; set; } = null!;
        public int? UserId { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User? User { get; set; }
    }
}
