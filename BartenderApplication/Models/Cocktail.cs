namespace BartenderApp.Models
{
    public class Cocktail
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string? Description { get; set; }
    }
}