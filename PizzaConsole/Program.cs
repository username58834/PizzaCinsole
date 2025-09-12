using Microsoft.VisualBasic.FileIO;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace PizzaConsole
{
    internal class Program
    {
        static internal List<PizzaClass> pizzas = new List<PizzaClass>(); 
        static void DrawMenu()
        {
            Console.WriteLine(
                "------------------------------------\n" +
                "| 1 - Додати об'єкт                |\n" +
                "| 2 - Переглянути всі об'єкти      |\n" +
                "| 3 - Знайти об'єкт                |\n" +
                "| 4 - Продемонструвати поведінку   |\n" +
                "| 5 - Видадити об'єкт              |\n" +
                "| 0 - Вийти з застосунку           |\n" +
                "------------------------------------"
            );
        }

        static string FormatAnswer(string regex, string message)
        {
            string text = "";
            
            try
            {
                text = Console.ReadLine();
                while (!Regex.IsMatch(text, regex))
                {
                    Console.WriteLine(message);
                    text = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return text;
        }

        static void ChooseIngredients(ref PizzaClass pizza)
        {
            int command;
            while (true)
            {
                try
                {
                    //Console.Clear();
                    Console.WriteLine("\nTo change ingredients write their number\n\n" +
                        $"{pizza.ShowIngredientsOnly()}" +
                        "0 - Add and close\n");
                    
                    command = int.Parse(Console.ReadLine());
                    if (command == 0)
                    {
                        //Console.Clear();
                        break;
                    }
                    else if (command < Enum.GetValues(typeof(Ingredients)).Length)
                    {
                        pizza.ChangeIngredients(command);
                    }
                    else
                    {
                        throw new Exception($"Write a number between 0 and {Enum.GetValues(typeof(Ingredients)).Length - 1}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        static void Add()
        {
            string name;
            float price;
            double weight;

            try
            {
                Console.WriteLine("Enter  name:");
                name = FormatAnswer("^[A-Za-z ]*$", "Write from 3 to 12 characters. Only Latin letters are allowed");
                while (name.Length < 3 || name.Length > 12)
                {
                    Console.WriteLine("Write from 3 to 12 characters. Only Latin letters are allowed");
                    name = FormatAnswer("^[A-Za-z ]*$", "Write from 3 to 12 characters. Only Latin letters are allowed");
                }

                Console.WriteLine("Enter  price:");
                price = float.Parse(FormatAnswer(@"^(0|[1-9]\d*)(\.\d{0,2})?$", "Only numbers are allowed. The price must be greater than 0"));
                while (price <= 0)
                {
                    Console.WriteLine("Only numbers are allowed. The price must be greater than 0");
                    price = float.Parse(FormatAnswer(@"^(0|[1-9]\d*)(\.\d{0,2})?$", "Only numbers are allowed. The price must be greater than 0"));
                }

                Console.WriteLine("Enter  weight:");
                weight = float.Parse(FormatAnswer(@"^(0|[1-9]\d*)(\.\d{0,3})?$",  "Only numbers are allowed. The price must be greater than 0"));
                while (weight <= 0)
                {
                    Console.WriteLine("Only numbers are allowed. The weight must be greater than 0");
                    weight = float.Parse(FormatAnswer(@"^(0|[1-9]\d*)(\.\d{0,3})?$", "Only numbers are allowed. The weight must be greater than 0"));
                }
                
                PizzaClass pizza = new PizzaClass(name, price, weight);
                ChooseIngredients(ref pizza);
                pizzas.Add(pizza);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

        static void ShowAll()
        {
            Console.WriteLine($"You have {pizzas.Count} pizzas");
            for(int i = 0; i < pizzas.Count; i++)
            {
                Console.WriteLine($"{i}: {pizzas[i].Name}");
                //Console.WriteLine($"{pizzas[i].Show}")
            }
        }

        static void ShowAllDetailed()
        {
            Console.WriteLine($"You have {pizzas.Count} pizzas");
            foreach(PizzaClass pizza in pizzas)
            {
                //Console.WriteLine($"{i}: {pizzas[i].Name}");
                Console.WriteLine($"{pizza.Show()}");
            }
        }
        static void Find()
        {
            Console.WriteLine("Choose type of sratching\n0 - Search by name\n1 - Search by maximum price");
            int command = 2;
            string name;
            float price;
            bool was = false;

            while (true)
            {
                try
                {
                    command = int.Parse(Console.ReadLine());
                    if (command < 0 || command > 1)
                    {
                        throw new Exception("Write a number netween 0 and 1");
                    }
                    else break;
                }

                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            //while (true)
            //{
                try
                {
                    if (command == 0)
                    {
                        Console.WriteLine("Enter  name:");
                        name = FormatAnswer("^[A-Za-z ]*$", "Write from 3 to 12 characters. Only Latin letters are allowed");
                        while (name.Length < 3 || name.Length > 12)
                        {
                            Console.WriteLine("Write from 3 to 12 characters. Only Latin letters are allowed");
                            name = FormatAnswer("^[A-Za-z ]*$", "Write from 3 to 12 characters. Only Latin letters are allowed");
                        }

                        foreach (PizzaClass pizza in pizzas)
                        {
                            if (pizza.Name == name)
                            {
                                Console.WriteLine($"\n{pizza.Show()}");
                                was = true;
                            }
                        }
                    }
                    else if (command == 1)
                    {
                        Console.WriteLine("Enter  maximum price:");
                        price = float.Parse(FormatAnswer(@"^(0|[1-9]\d*)(\.\d{0,2})?$", "Only numbers are allowed. The price must be greater than 0"));
                        while (price <= 0)
                        {
                            Console.WriteLine("Only numbers are allowed. The price must be greater than 0");
                            price = float.Parse(FormatAnswer(@"^(0|[1-9]\d*)(\.\d{0,2})?$", "Only numbers are allowed. The price must be greater than 0"));
                        }

                        foreach (PizzaClass pizza in pizzas)
                        {
                            if (pizza.Price <= price)
                            {
                                Console.WriteLine($"\n{pizza.Show()}");
                                was = true;
                            }
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

                if (!was)
                {
                    Console.WriteLine("\nNo items found");
                }
                //break;
            //}
        }

        static void Delete()
        {
            int command;
            if (pizzas.Count == 0)
            {
                Console.WriteLine("Nothing to delete");
            }
            else
            {
                Console.WriteLine("Choose type of sratching\n0 - Delete by name\n1 - Delete by index");

                while (true)
                {
                    try
                    {
                        command = int.Parse(Console.ReadLine());
                        if (command < 0 || command > 1)
                        {
                            throw new Exception("Write a number netween 0 and 1");
                        }
                        else
                        {
                            if (command == 0) DeleteByName();
                            if (command == 1) DeleteByIndex();
                            break;
                        }
                    }

                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
            }
        }

        static int GetIndex()
        {
            int command;
            Console.WriteLine($"Write a number netween {0} and {pizzas.Count-1}");
            while (true)
            {
                try
                {
                    command = int.Parse(Console.ReadLine());
                    if (command < 0 || command >= pizzas.Count)
                    {
                        throw new Exception($"Write a number netween {0} and {pizzas.Count}");
                    }
                    else
                    {
                        return command;
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }
        static void DeleteByIndex()
        {
            pizzas.RemoveAt(GetIndex());
            Console.WriteLine("The item was successfully deleted");
        }

        static void DeleteByName()
        {
            string name = "";
            Console.WriteLine("Enter  name:");
            try
            {
                name = FormatAnswer("^[A-Za-z ]*$", "Write from 3 to 12 characters. Only Latin letters are allowed");
                while (name.Length < 3 || name.Length > 12)
                {
                    Console.WriteLine("Write from 3 to 12 characters. Only Latin letters are allowed");
                    name = FormatAnswer("^[A-Za-z ]*$", "Write from 3 to 12 characters. Only Latin letters are allowed");
                }
            }catch (Exception ex) { Console.WriteLine(ex.Message); }

            bool was = false;
            for(int i = 0; i < pizzas.Count; i++)
            {
                if (name == pizzas[i].Name)
                {
                    pizzas.RemoveAt(i);
                    i--;
                    was = true;
                }
            }
            if (was) Console.WriteLine("The itens were successfully deleted");
            else Console.WriteLine("Nothing to delete");
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            //DrawMenu();
            int command;
            int N = 0;
            float money = 0;

            Console.WriteLine("Write maximum number of elements");
            while (true)
            {
                try
                {
                    N = int.Parse(Console.ReadLine());
                    if (N <= 0) throw new Exception("Write a number greater than 0\n");
                    else break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            //while (true)
            //{
            try
            {
                Console.WriteLine("Enter  money:");
                money = float.Parse(FormatAnswer(@"^(0|[1-9]\d*)(\.\d{0,2})?$", "Only numbers are allowed. The price must be greater than 0"));
                while (money <= 0)
                {
                    Console.WriteLine("Only numbers are allowed. The price must be greater than 0");
                    money = float.Parse(FormatAnswer(@"^(0|[1-9]\d*)(\.\d{0,2})?$", "Only numbers are allowed. The price must be greater than 0"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //}

            while (true)
            {
                try
                {
                    //Console.WriteLine("\nMenu was omitted");
                    DrawMenu();
                    Console.WriteLine("\nEnter a number between 0-5");

                    command = int.Parse(Console.ReadLine());
                    switch (command)
                    {
                        case 0:
                            return;
                        case 1:
                            if (pizzas.Count < N) Add();
                            else throw new Exception("You have already reach the limit");
                            break;
                        case 2:
                            ShowAll();
                            //ShowAllDetailed();
                            break;
                        case 3:
                            Find();
                            break;
                        case 4:
                            if (pizzas.Count > 0)
                            {
                                if(pizzas[GetIndex()].Sell(ref money))
                                {
                                    Console.WriteLine($"The pizza was bought. Money left {money}");
                                }
                                else
                                {
                                    Console.WriteLine($"Not enough money. Money left {money}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Nothing to buy");
                            }
                            //ShowAllDetailed();
                            break;
                        case 5:
                            Delete();
                            break;
                        default:
                            Console.WriteLine("Unknown command");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        
        }
    }
}
