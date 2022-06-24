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

//Test for lab 3.
Product p1 = new Product(null, 1, "MILK1");
Product p2 = new Product("milk", 1, null);
Product p3 = new Product("milk", -1, "MILK1");

VendingMachine machine3 = new VendingMachine(null);

machine.StockItem(cola, -1);

machine.VendItem(null, new List<int> { 5 });
machine.VendItem("cola",new List<int> {});


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
        try
        {
            if (barcode == null)
            {
                throw new ArgumentNullException();
            }
            Barcode = barcode;
        }
        catch(ArgumentNullException ex)
        {
            Console.WriteLine("Exception: "+ex.Message);
        }
        SerialNumber = MachineNumber;
        MachineNumber++;
        MoneyFloat = new Dictionary<int, int>();
        foreach (int denomination in allDenomination)
        {
            MoneyFloat.Add(denomination, 0);
        }
        Inventory = new Dictionary<Product, int>();
        
    }

    public string StockItem(Product product, int quantity)
    {
        try
        {   
            if(quantity <= 0)
            {
                throw new Exception("The quantity of product should not be a negative number.");
            }
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
        catch(Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }
        return null;
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
        try
        {
            if (code == null)
            {
                throw new Exception("Please enter code.");
            }
            if(money.Count == 0)
            {
                throw new Exception("Please enter right amount of money");
            }
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
            return "Error, no item in this machine.";
        }
        catch(Exception ex)
        {
            Console.WriteLine("Exception: "+ex.Message);
        }
        return null;
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
        try
        {
            if (name == null)
            {
                throw new Exception("Name of product should not be null.");
            }
            if (code == null)
            {
                throw new Exception("Code of product should not be null.");
            }
            if (price <= 0)
            {
                throw new Exception("Price of product should be a positive number.");
            }
            Name = name;
            Price = price;
            Code = code;
            
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception: "+e.Message);
        }
    }   
}