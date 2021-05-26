using System;

namespace Engine.Utils
{
    public class Registry
    {
        public string Name {get;}
        private int value;

        public Registry(string name)
        {
            Name = name;
        }

        public void Put(string value)
        {
            this.value = Convert.ToInt32(value);
        }

        public void Put(int value)
        {
            this.value = value;
        }

        public int Take()
        {
            return value;
        }

        public override string ToString()
        {
            return "Регистр: " + Name + " Значение: " + value;
        }
    }
}