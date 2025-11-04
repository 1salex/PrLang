public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public int CreditSpent { get; set; } = 0;
    public DateTime DateOfPurchase { get; set; } = DateTime.Now;

    public override string ToString()
    {
        return $"{Id} \n" +
                $"{Name} \n" +
               $"Comment: {Comment}\n" +
               $"Credit spent: {CreditSpent}\n" +
               $"Date of purchase: {DateOfPurchase}\n";
    }
}