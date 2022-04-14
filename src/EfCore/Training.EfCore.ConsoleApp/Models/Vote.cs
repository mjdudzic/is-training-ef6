namespace Training.EfCore.ConsoleApp.Models
{
    public partial class Vote
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int? UserId { get; set; }
        public int? BountyAmount { get; set; }
        public int VoteTypeId { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] RowVersion { get; set; } = null!;

        public virtual User? User { get; set; }
        public virtual VoteType VoteType { get; set; } = null!;
    }
}
