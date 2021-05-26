using System;
using System.IO;
using System.Collections.Generic;

namespace Engine.Utils
{
    public static class InOut
    {
        public static int GetStringQty(string path)
        {
            int qty = 0;
            using (StreamReader sr = new StreamReader(path))
            {
                while(sr.Peek() > -1)
                {
                    sr.ReadLine();
                    qty++;
                }
            }
            return qty;
        }

        public static void Write(string text, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(text);
            }
        }

        public static string Read(string path)
        {
            string str = "";
            using (StreamReader sr = new StreamReader(path))
            {
                while(sr.Peek() > -1)
                {
                    str += sr.ReadLine();
                    if (sr.Peek() > -1)
                        str += "\r\n";
                }
            }
            return str;
        }

        public static Dictionary<string, string> GetMarksPoits(string path)
        {
            int point = 0;
            Dictionary<string, string> marks = new Dictionary<string, string>();
            string str = "";
            using (StreamReader sr = new StreamReader(path))
            {
                while(sr.Peek() > -1)
                {
                    str = sr.ReadLine();
                    if (Command.Check(Command.Normalize(str), Utils.Mask.Mark))
                    {
                        string[] tmp = str.Split();
                        marks.Add(Command.Normalize(tmp[1]), "" + point);                        
                    }
                    point++;
                    if (str == "" || str == "\r\n")
                    point--;
                }
            }
            return marks;
        }

        public static Command[] GetCommands(string path)
        {
            Dictionary<string, string> marks = GetMarksPoits(path);
            int qty = GetStringQty(path);
            List<Command> cmds = new List<Command>();

            using (StreamReader sr = new StreamReader(path))
            {
                Command command = new Command("Def");

                while(sr.Peek() > -1)
                {
                    string str = sr.ReadLine();
                    if (str != null)
                    {
                        command = Command.FromString(str);
                        if (command.Name == "goto")
                            if (marks.ContainsKey(command.Value))
                            cmds.Add(new Command("goto", marks[command.Value]));
                            else
                            cmds.Add(new Command("error", "метка <<" + command.Value +">> не найдена"));
                        if (command.Name == "gotozero")                            
                            if (marks.ContainsKey(command.Value))
                            cmds.Add(new Command("gotozero", marks[command.Value]));
                            else
                            cmds.Add(new Command("error", "метка <<" + command.Value +">> не найдена"));
                        if (command.Name == "gotonotzero")
                            if (marks.ContainsKey(command.Value))
                            cmds.Add(new Command("gotonotzero", marks[command.Value]));
                            else
                            cmds.Add(new Command("error", "метка <<" + command.Value +">> не найдена"));
                        if (command.Name == "gotoabove")
                            if (marks.ContainsKey(command.Value))
                            cmds.Add(new Command("gotoabove", marks[command.Value]));
                            else
                            cmds.Add(new Command("error", "метка <<" + command.Value +">> не найдена"));
                        if (command.Name == "gotobelow")
                            if (marks.ContainsKey(command.Value))
                            cmds.Add(new Command("gotobelow", marks[command.Value]));
                            else
                            cmds.Add(new Command("error", "метка <<" + command.Value +">> не найдена"));
                        if (command.Name != "goto" && command.Name != "gotozero" && command.Name != "gotonotzero" && command.Name != "gotoabove" && command.Name != "gotobelow")
                            cmds.Add(command);
                    }
                    
                }
            }
            
            cmds.Add(new Command("valuetolog", "Конец"));
            for (int i = 0; i < cmds.Count; i++)
            {
                if (cmds[i].Name == "nil")
                {
                    cmds.RemoveAt(i);
                    i--;
                }
            }
            
            Command[] array = new Command[cmds.Count];
            for (int i = 0; i < cmds.Count; i++)
            {
                array[i] = cmds[i];
            }

            return array;
        }

        public class Config
        {
            public string Map { get; private set; }
            public string Script { get; private set; }
            public int Speed { get; private set; }
            public bool SoundState { get; private set; }

            public Config(string path)
            {
                Map = "Map0.txt";
                Script = "Script.txt";
                Speed = 500;
                SoundState = false;
                Read(path);
            }

            private void Read(string path)
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string str;
                    string[] tmp;
                    while (sr.Peek() > -1)
                    {
                        str = sr.ReadLine();
                        if (Mask.Config.Map.IsMatch(str))
                        {
                            tmp = str.Split(':');
                            Map = tmp[1];
                        }
                        if (Mask.Config.Script.IsMatch(str))
                        {
                            tmp = str.Split(':');
                            Script = tmp[1];
                        }
                        if (Mask.Config.Sound.IsMatch(str))
                        {
                            tmp = str.Split(':');
                            if (tmp[1].ToLower() == "on")
                                SoundState = true;
                            if (tmp[1].ToLower() == "off")
                                SoundState = false;
                        }
                        if (Mask.Config.Speed.IsMatch(str))
                        {
                            tmp = str.Split(':');
                            Speed = Convert.ToInt32(tmp[1]);
                        }
                    }
                }
            }
        }
    }

    
}