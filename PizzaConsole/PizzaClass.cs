using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace PizzaConsole
{
    internal class PizzaClass
    {
        public List<Ingredients> ingredients = new List<Ingredients>();
        private string name;
        private float price;
        private double weight;
        private DateTime bakingDate = DateTime.Now;
        private double bakingTime = (new Random().Next() % 10000) / 1000;//time in sec
        private double freshTime =  (new Random().Next() % 10000) / 100;
        private States state = States.Baking;

        public string Name {
            get
            {
                return name;
            }
            set
            {
                if (!Regex.IsMatch(value, "^[A-Za-z ]*$") || name.Length < 3 || name.Length > 12)
                {
                    throw new ArgumentNullException(null, "Write from 3 to 12 characters. Only Latin letters are allowed");
                }
                else
                {
                    name = value;
                }
            }
        }

        public float Price
        {
            get
            {
                return price;
            }
            set
            {
                if (!Regex.IsMatch(value.ToString(), @"^(0|[1-9]\d*)(\.\d{0,2})?$") || value <= 0 || value >= 10000)
                {
                    throw new ArgumentNullException(null, "Only numbers are allowed. The price must be greater than 0 and less than 10000");
                }
                else
                {
                    price = value;
                }
            }
        }

        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                if (!Regex.IsMatch(value.ToString(), @"^(0|[1-9]\d*)(\.\d{0,3})?$") || value <= 0 || value >= 10)
                {
                    throw new ArgumentNullException(null, "Only numbers are allowed. The weight must be greater than 0 and less than 10");
                }
                else
                {
                    weight = value;
                }
            }
        }

        public States State
        {
            get
            {
                return state;
            }
            private set
            {
                state = value;
            }
        }

        public void ChangeState(){
            double time = (DateTime.Now - bakingDate).TotalSeconds;
            switch (State)
            {
                case States.Baking:
                    if (time >= bakingTime)
                    {
                        State = States.Ready;
                    }
                    break;
                case States.Ready: 
                    if(time >= bakingTime + freshTime)
                    {
                        State = States.Spoiled;
                    }
                    break;
                default:
                    break;
            }
        }

        public bool ThrowAwayIfSpoiled()
        {
            if(State == States.Spoiled)
            {
                return true;
            }
            return false;
        }

        public PizzaClass(string name = "None", float price = 0, double weight = 0)
        {   
            this.name = name;
            this.price = price;
            this.weight = weight;

            ingredients.Add(Ingredients.Dough);
        }
        public void ChangeIngredients(Ingredients ingredient)
        {
            if (ingredients.Contains(ingredient))
            {
                ingredients.Remove(ingredient);
            } 
            else
            {
                ingredients.Add(ingredient);
            }
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
            string text = $" ---------------------------\n" +
                          $"| Ingredients:              |\n" +
                          $" ---------------------------\n";
            for (int i = 1; i < Enum.GetValues(typeof(Ingredients)).Length; i++)
            {
                text += $"| {i}";
                if (i < 10) text += " ";
                text += $" | {((Ingredients)(i)).ToString()} ";
                for (int j = 0; j < 12 - ((Ingredients)(i)).ToString().Length; j++) text += " ";
                text += $"| {ingredients.Contains((Ingredients)(i))} ";
                if (ingredients.Contains((Ingredients)(i))) text += " ";
                text += "|\n";
                //text += $"{i}: {((Ingredients)(i)).ToString()} - {ingredients.Contains((Ingredients)(i))}\n";
                //text += $"{i}: {((Ingredients)(i)).ToString()} - {ingredients.Contains((Ingredients)(i))}\n";
                //!text += $"{i}: {ingredients[i]} - {ingredients[i].IsChoosen}\n";
                text += $" ---------------------------\n";              
            }           

            return text;
        }
        public string Show()
        {
            ChangeState();

            string text = 
                $"Name:       {Name}\n" +
                $"Price:      {Price.ToString("F2")}$\n" +
                $"Weight:     {Weight.ToString("F3")}kg\n" +
                $"BakingDate: {bakingDate}\n" +
                $"BakingTime: {bakingTime}\n" +
                $"FreshTime:  {freshTime}\n" +
                $"Status:     {State}\n" +
                $"Ingredients:\n";

            for (int i = 1; i < Enum.GetValues(typeof(Ingredients)).Length; i++)
            {
                if (ingredients.Contains((Ingredients)(i))) {
                    text += $"\t{((Ingredients)(i)).ToString()}\n";
                }
                //!text += $"{i}: {ingredients[i]} - {ingredients[i].IsChoosen}\n";               
            }
            /*
            foreach(IngredientClass ingredient in ingredients)
            {
                if (ingredient.IsChoosen)
                {
                    text += $"\t{ingredient.Name}\n";
                }
            }
             */
            return text;
        }
    }
}
