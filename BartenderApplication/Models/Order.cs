using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BartenderApp.Models
{
    public enum OrderStatus
    {
        Placed = 0,
        InPreparation = 1,
        ReadyForPickup = 2,
        Completed = 3,
        Cancelled = 4
    }


    public class Order
    {
        public int Id { get; set; }


        [Required, StringLength(60)]
        public string CustomerName { get; set; } = string.Empty;


        [Display(Name = "Cocktail")]
        public int CocktailId { get; set; }


        [ForeignKey(nameof(CocktailId))]
        public Cocktail? Cocktail { get; set; }


        public DateTime OrderedAt { get; set; } = DateTime.UtcNow;


        public OrderStatus Status { get; set; } = OrderStatus.Placed;
    }
}