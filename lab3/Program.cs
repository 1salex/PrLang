using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

public class Program
{
    private static readonly Purchases purchases = new Purchases();

    public static void Main(string[] args)
    {
        while (true)
        {
            show(); 
            Console.Write("Menu: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    HandleAdd();
                    break;
                case "2":
                    HandleDel();
                    break;
                case "3":
                    HandleSearch();
                    break;
                case "4":
                    HandleExit();
                    return; 
                default:
                    Console.WriteLine("Invalid input. Please choose a number from 1 to 4");
                    break;
            }

            Console.WriteLine("\nEnter any key to return to menu");
            Console.ReadKey();
        }
    }
    private static void show()
    {
        Console.Clear();
        Console.WriteLine("Options:");
        Console.WriteLine("1. Add new purches info");
        Console.WriteLine("2. Remove purchase by it's number");
        Console.WriteLine("3. Print purchases in the specified range of dates");
        Console.WriteLine("4. Save and exit");
        Console.WriteLine("");
    }

    private static void HandleAdd()
    {
        Console.Clear();
        Console.WriteLine("Add new purches to the shopping list");

        Console.Write("Enter name: ");
        string name = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name value can't be empty");
            return;
        }

        Console.Write("Enter comment: ");
        string comment = Console.ReadLine() ?? "";

        Console.Write("Enter amount of credits spent: ");
        int crspent = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter date of purchase (dd.mm.yyyy): ");
        DateTime datePurchase = TakeDate(Console.ReadLine() ?? ""); 

        var newItem = new Item { Name = name, Comment = comment, CreditSpent = crspent, DateOfPurchase = datePurchase };

        if (purchases.AddItem(newItem))
        {
            Console.WriteLine("\nPurchase added");
        }
        else
        {
            Console.WriteLine("\nThis purchase have already been added");
        }
    }
    private static DateTime TakeDate(string input)
{
    DateTime datePurchase;

    while (!DateTime.TryParseExact(input, "dd.MM.yyyy", null,
        System.Globalization.DateTimeStyles.None, out datePurchase))
    {
        Console.Write("Invalid format of date. Please enter the date like dd.MM.yyyy: ");
        input = Console.ReadLine() ?? "";
    }

    return datePurchase;
}

    private static void HandleDel()
    {
        Console.Clear();
        Console.WriteLine("Removing purchase by it's number");

        Console.Write("Enter number: ");
        int number = Convert.ToInt32(Console.ReadLine());

        if (purchases.DeleteItem(number))
        {
            Console.WriteLine($"\nItem '{number}' removed");
        }
        else
        {
            Console.WriteLine($"\nThere is no puchase by this number");
        }
    }

    private static void HandleSearch()
{
    Console.Clear();
    Console.WriteLine("Print purchases in the specified range of dates");

    Console.WriteLine("Enter start date (dd.MM.yyyy): ");
    DateTime startDate = TakeDate(Console.ReadLine() ?? "");

    Console.WriteLine("Enter end date (dd.MM.yyyy): ");
    DateTime endDate = TakeDate(Console.ReadLine() ?? "");

    // switch end and start date to generalise the search
    if (startDate > endDate)
    {
        var temp = startDate;
        startDate = endDate;
        endDate = temp;
    }

    var results = purchases.GetItemsByDateRange(startDate, endDate);

    Console.WriteLine($"\nPurchases from {startDate:dd.MM.yyyy} to {endDate:dd.MM.yyyy}:");

    if (results.Any())
    {
        foreach (var item in results)
        {
            Console.WriteLine(item);
            Console.WriteLine("-------------------------------");
        }
    }
    else
    {
        Console.WriteLine("No purchases found in this date range.");
    }
}

    private static void HandleExit()
    {
        Console.WriteLine("\nSaving changes");
        purchases.Save();
        Console.WriteLine("Succesfully saved");
    }
}