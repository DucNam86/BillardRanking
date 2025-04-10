namespace BillardAPI.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int ballDie { get; set; } = 0;
        public int Wins { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
