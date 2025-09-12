using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaConsole
{
    internal class IngredientClass
    {
        public string Name;
        //public Ingredients Ingredient; 
        public bool IsChoosen;

        public IngredientClass(string name = "None", bool isChoosen = false)
        //public IngredientClass(Ingredients ingredient, bool isChoosen = false)
        {
            Name = name;
            //Ingredient = ingredient;
            IsChoosen = isChoosen;
        }
    }
}