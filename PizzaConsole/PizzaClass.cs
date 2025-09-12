using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace PizzaConsole
{
    internal class PizzaClass
    {
        public List<IngredientClass> ingredients = new List<IngredientClass>();
        public string Name;
        public float Price;
        public double Weight;
        private DateTime BakingDate = DateTime.Now;
        private double BakingTime = (new Random().Next() % 10000) / 1000;//time in sec
        public PizzaClass(string name = "None", float price = 0, double weight = 0)
        {   
            Name = name;
            Price = price;
            Weight = weight;

            foreach (var ingredient in Enum.GetNames(typeof(Ingredients)))
            {
                ingredients.Add(new IngredientClass(ingredient, false));
            }
            ingredients[0].IsChoosen = true;
        }
        //public void ChangeIngredients(IngredientClass ingredient)
        public void ChangeIngredients(int x)
        {
            ingredients[x].IsChoosen ^= true;
            //ingredients[(int)Enum.Parse(typeof(Ingredients), ingredient.Name)].IsChoosen = ingredient.IsChoosen;
        }

        public bool Sell(ref float money)
        {
            if (money >= Price)
            {
                money -= Price;
                return true;
            }
            return false;
        }
        public string ShowIngredientsOnly()
        {
            string text = $"Ingredients:\n";
            for (int i=1;i<ingredients.Count; i++)
            {
                text += $"{i}: {ingredients[i].Name} - {ingredients[i].IsChoosen}\n";               
            }

            return text;
        }
        public string Show()
        {
            string text = 
                $"Name:       {Name}\n" +
                $"Price:      {Price.ToString("F2")}$\n" +
                $"Weight:     {Weight.ToString("F3")}kg\n" +
                $"BakingDate: {BakingDate}\n" +
                $"Ingredients:\n";
            foreach(IngredientClass ingredient in ingredients)
            {
                if (ingredient.IsChoosen)
                {
                    text += $"\t{ingredient.Name}\n";
                }
            }
                    
            return text;
        }
    }
}
