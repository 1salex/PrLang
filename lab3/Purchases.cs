using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

public class Purchases
{
    private const string file = "shoplist.txt";
    private SortedDictionary<int, Item> _items;

    public Purchases()
    {
        _items = new SortedDictionary<int, Item>();
        Load();
    }
    private int _nextId = 1;

    public bool AddItem(Item item)
    {
        item.Id = _nextId++;
        return _items.TryAdd(item.Id, item);
    }

    public bool DeleteItem(int id)
    {
        return _items.Remove(id);
    }

    public List<Item> GetItemsByDateRange(DateTime startDate, DateTime endDate)
    {
        return _items.Values
            .Where(item => item.DateOfPurchase.Date >= startDate.Date &&
                           item.DateOfPurchase.Date <= endDate.Date)
            .ToList();
    }
    
    public void Save()
    {
        try
        {
            var items = _items.Values.ToList();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            string jsonString = JsonSerializer.Serialize(items, options);

            File.WriteAllText(file, jsonString, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
        }
    }
    
    private void Load()
    {
        if (!File.Exists(file))
        {
            return;
        }

        try
        {
            string jsonString = File.ReadAllText(file, Encoding.UTF8);

            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return;
            }

            var loadedItems = JsonSerializer.Deserialize<List<Item>>(jsonString);

            _items.Clear();
            if (loadedItems != null)
            {
                foreach (var item in loadedItems)
                {
                    _items.TryAdd(item.Id, item);
                }

                if (loadedItems.Count > 0)
                    _nextId = loadedItems.Max(i => i.Id) + 1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            _items = new SortedDictionary<int, Item>();
        }
    }
} 