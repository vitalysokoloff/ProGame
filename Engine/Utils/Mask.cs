using System.Text.RegularExpressions;

namespace Engine.Utils
{
    public static class Mask
    {
        // s - пробельный символ
        // w - Любой текстовый символ, не являющийся пробелом, символом табуляции и т.п.
        // * - Соответствует нулю или более экземплярам предшествующего шаблона
        // [abcd] - Только те символы которые записаны в квадратные скобки

        public static Regex GoTo = new Regex("^(перейти\\sна\\s\\w+;)"); 
        public static Regex GoToZero = new Regex("^(перейти\\sна\\s\\w+\\sесли\\sноль;)"); 
        public static Regex GoToNotZero = new Regex("^(перейти\\sна\\s\\w+\\sесли\\sне\\s(ноль|нуль);)");
        public static Regex GoToAbove = new Regex("^(перейти\\sна\\s\\w+\\sесли\\sбольше\\s(ноля|нуля);)");
        public static Regex GoToBelow = new Regex("^(перейти\\sна\\s\\w+\\sесли\\sменьше\\s(ноля|нуля);)"); 
        public static Regex Mark = new Regex("^((метка:|метка)\\s\\w+);");
        public static Regex Write = new Regex("^(записать\\sв\\s[abcd];)");
        public static Regex WriteValue = new Regex("^(записать\\sв\\s[abcd]\\s(значение|значение:)\\s\\d+;)");
        public static Regex Log = new Regex("^(записать\\sв\\s(лог|log);)");
        public static Regex ValueToLog = new Regex("^(записать\\sв\\s(лог|log)\\s(значение|значение:)\\s\\w+;)");
        public static Regex Read = new Regex("^(прочитать\\sиз\\s[abcd];)");
        public static Regex Add = new Regex("^(прибавить\\s\\d+\\sк\\s[abcd];)");
        public static Regex TakeAway = new Regex("^(отнять\\s\\d+\\sот\\s[abcd];)");
        public static Regex Step = new Regex("^(шагать;)");
        public static Regex Check = new Regex("^(проверить;)");
        public static Regex Eat = new Regex("^(кушать|есть;)");
        public static Regex Turn = new Regex("^(повернуть\\s(влево|вправо);)");
        public static Regex Comment = new Regex("^((комент|коммент|коментарий|комментарий|заметка|комент:|коммент:|коментарий:|комментарий:|заметка:)\\s\\w+;)");
        
        public static class Level
        {
            public static Regex Enemy = new Regex("^(enemy:\\d*:\\d*:[xy]:\\d*;)");
            public static Regex Map = new Regex("^(map:\\d*:\\d*;)");
        }

        public static class Config
        {
            public static Regex Map = new Regex("^((map|Map|MAP):\\w+.txt)");
            public static Regex Script = new Regex("^((script|Script|SCRIPT):\\w+.txt)");
            public static Regex Sound = new Regex("^((sound|Sound|SOUND):(on|On|ON|off|Off|OFF))");
            public static Regex Speed = new Regex("^((speed|Speed|SPEED):\\d+)");
        }
    }
}