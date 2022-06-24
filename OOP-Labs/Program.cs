Product cola = new Product("cola", 3, "Drink1");
VendingMachine machine = new VendingMachine("Machine 1");
Console.WriteLine(machine.StockItem(cola, 2));

Console.WriteLine(machine.StockFloat(1, 10));

List<int> userMoney1 = new List<int> { 7 };
Console.WriteLine(machine.VendItem("Drink1", userMoney1));

List<int> userMoney2 = new List<int> { 7 };
Console.WriteLine(machine.VendItem("Drink", userMoney2));

List<int> userMoney3 = new List<int> { 15 };
Console.WriteLine(machine.VendItem("Drink1", userMoney3));

List<int> userMoney4 = new List<int> { 1 };
Console.WriteLine(machine.VendItem("Drink1", userMoney4));

Console.WriteLine(machine.StockItem(cola, 5));

List<int> userMoney5 = new List<int> { 1 };
Console.WriteLine(machine.VendItem("Drink1", userMoney5));

//Test for lab 2.
Console.WriteLine(machine.getSerialNumber());
VendingMachine machine2 = new VendingMachine("Machine 2");
Console.WriteLine(machine2.getSerialNumber());

class VendingMachine
{
    public static int MachineNumber { get; set; } = 1;
    private int SerialNumber { get; set; }   
    private Dictionary<int, int> MoneyFloat { get; set; }
    private Dictionary<Product, int> Inventory { get; set; }

    private readonly List<int> allDenomination = new List<int> { 20, 10, 5, 2, 1 };
    private readonly string Barcode;

    public VendingMachine(string barcode)
    {
        SerialNumber = MachineNumber;
        MachineNumber++;
        MoneyFloat = new Dictionary<int, int>();
        foreach (int denomination in allDenomination)
        {
            MoneyFloat.Add(denomination, 0);
        }
        Inventory = new Dictionary<Product, int>();
        Barcode = barcode;
    }

    public string StockItem(Product product, int quantity)
    {
        if (Inventory.ContainsKey(product))
        {
            Inventory[product] += quantity;
        }
        else
        {
            Inventory.Add(product, quantity);
        }
        return $"{product.Name}\n" +
            $"Code: {product.Code}\n" +
            $"Price: {product.Price}\n" +
            $"Quantity:{Inventory[product]}";
    }

    public string StockFloat(int moneyDenomination, int quantity)
    {
        string floatInVentory = "All money in the machine:\n";
        if (MoneyFloat.ContainsKey(moneyDenomination))
        {
            MoneyFloat[moneyDenomination] += quantity;
        }
        else
        {
            MoneyFloat.Add(moneyDenomination, quantity);
        }
        foreach(KeyValuePair<int,int> money in MoneyFloat)
        {
            floatInVentory += $"{money.Key}: {money.Value}\n";
        }
        return floatInVentory;
    }

    public string VendItem(string code, List<int> money)
    {
        foreach(KeyValuePair<Product,int> product in Inventory)
        {
            if(product.Key.Code == code)
            {
                Product wantedProduct = product.Key;
                if (Inventory[wantedProduct] > 0)
                {
                    int insertedMoney = 0;
                    foreach(int coin in money)
                    {
                        insertedMoney += coin;
                    }
                    if(insertedMoney > wantedProduct.Price)
                    {
                        int returnedMoney = insertedMoney - wantedProduct.Price;
                        int finalReturnMoney = returnedMoney;
                        Dictionary<int, int> copyMoneyFloat = new Dictionary<int, int>(MoneyFloat);
                        Inventory[wantedProduct]--;
                        //Try to change.                        
                        foreach(int denomination in allDenomination)
                        {
                            while(returnedMoney >= denomination && MoneyFloat[denomination] > 0)
                            {
                                returnedMoney -= denomination;
                                MoneyFloat[denomination]--;
                            }
                        }
                        if(returnedMoney == 0)
                        {
                            //Success to puchase.
                            Inventory[wantedProduct]--;
                            return $"Please enjoy your {wantedProduct.Name} and take your change of ${finalReturnMoney}.";
                        }
                        else
                        {
                            //Fail to change, recover the money list to the original version.
                            MoneyFloat = copyMoneyFloat;
                            return "Error: insufficient money to change.";
                        }
                    }
                    else
                    {
                        return "Error: insufficient money provided.";
                    }
                }
                else
                {
                    return "Error: Item is out of stock.";
                }
            }
            else
            {
                return $"Error, no item with code {code}.";
            }
        }
        return "Erro, no item in this machine.";
    }

    public int getSerialNumber()
    {
        return SerialNumber;
    }
}

class Product
{
    public string Name { get; set; }
    public int Price { get; set; }
    public string Code { get; set; }

    public Product(string name, int price, string code)
    {
        Name = name;
        Price = price;
        Code = code;
    }   
}