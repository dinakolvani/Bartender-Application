namespace BartenderApp.Models
{
    public enum OrdersStatus
    {
        Placed,     // order was created
        InProgress, // bartender is making it
        Completed,  // finished
        Canceled    // canceled by user/bartender
    }
}