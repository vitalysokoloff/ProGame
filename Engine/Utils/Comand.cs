using System;
using System.Text.RegularExpressions;

namespace Engine.Utils
{
    public class Command
    {
        public string Name {get;}
        public string Value {get; set;}
        public string eValue {get; set;}

        public Command(string name)
        {
            Name = name;
            Value = "0";
            eValue = "0";
        }

        public Command(string name, string value)
        {
            Name = name;
            Value = value;
            eValue = "0";
        }

        public Command(string name, string value, string evalue)
        {
            Name = name;
            Value = value;
            eValue = evalue;
        }

        public static bool Check(string str, Regex mask)
        {            
            str += ";"; // Обозначаем конец строки
            return mask.IsMatch(str);
        }

        public static string Normalize(string str)
        {
            str = str.ToLower().Trim(); // Приводим к нижнему регистру и удаляем лишние пробелы с краёв
            str = System.Text.RegularExpressions.Regex.Replace(str, "\\s+", " "); // Удаляем лишние пробелы в середине
            return str;
        }

        public static Command FromString(string str)
        {
            str = Normalize(str);
            
            string[] tmp = str.Split();

            if (str == "" || str == "\r\n")
                return new Command("nil");

            if (Check(str, Utils.Mask.GoTo))
            {
                if (tmp[2].Length < 31)
                    return new Command("goto", tmp[2]);
            }

            if (Check(str, Utils.Mask.GoToZero))
            {
                if (tmp[2].Length < 31)
                    return new Command("gotozero", tmp[2]);
            }

            if (Check(str, Utils.Mask.GoToNotZero))
            {
                if (tmp[2].Length < 31)
                    return new Command("gotonotzero", tmp[2]);
            }

            if (Check(str, Utils.Mask.GoToAbove))
            {
                if (tmp[2].Length < 31)
                    return new Command("gotoabove", tmp[2]);
            }

            if (Check(str, Utils.Mask.GoToBelow))
            {
                if (tmp[2].Length < 31)
                    return new Command("gotobelow", tmp[2]);
            }
            
            if (Check(str, Utils.Mask.Mark))
            {
                if (tmp[1].Length < 31)
                    return new Command("mark", tmp[1]);
            }

            if (Check(str, Utils.Mask.Write))
            {
                return new Command("write", tmp[2]);
            }

            if (Check(str, Utils.Mask.WriteValue))
            {
                return new Command("writevalue", tmp[2], tmp[4]);
            }

            if (Check(str, Utils.Mask.Log))
            {
                return new Command("log");
            }

            if (Check(str, Utils.Mask.ValueToLog))
            {
                return new Command("valuetolog", tmp[4]);
            }

            if (Check(str, Utils.Mask.Read))
            {
                return new Command("read", tmp[2]);
            }

            if (Check(str, Utils.Mask.Add))
            {
                return new Command("add", tmp[3], tmp[1]);
            }

            if (Check(str, Utils.Mask.TakeAway))
            {
                return new Command("takeaway", tmp[3], tmp[1]);
            }

            if (Check(str, Utils.Mask.Step))
            {
                return new Command("step");
            }

            if (Check(str, Utils.Mask.Check))
            {
                return new Command("check");
            }

            if (Check(str, Utils.Mask.Eat))
            {
                return new Command("eat");
            }

            if (Check(str, Utils.Mask.Turn))
            {
                string s = "";
                if (tmp[1] == "влево")
                    s = "left";
                if (tmp[1] == "вправо")
                    s = "right";
                return new Command("turn", s);
            }

            if (Check(str, Utils.Mask.Comment))
            {
                return new Command("valuetolog", tmp[1]);
            }

            return new Command("error:", " \"" + str + "\"");
        }

        public override string ToString()
        {
            return Name + " Value: " + Value + " eValue: " + eValue;
        }
    }
}