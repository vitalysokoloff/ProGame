using System;
using System.IO;
using System.Collections.Generic;
using Engine.Utils;

namespace Engine.Game
{
    public class Level
    {
        public char[,] Map {get; private set;}
        public Character Character;
        public Vector2 Finish;

        private Enemy[] enemies;           

        public Level(char[,] map, Enemy[] enemies, Character character, Vector2 finish)
        {
            Map = map;
            Character = character;
            Finish = finish;

            this.enemies = enemies;            
        }

        public void Update(Command cmd)
        {
            foreach (Enemy e in enemies)
            {
                Map = e.Update(Map);
                if (e.Position == Character.Position)
                    Character.Kill();
            }
            Map = Character.Update(Map, cmd);            
        }

        public int CheckMap(Command cmd)
        {
            Vector2 v = Character.Position + Character.Direction;
            if (Map[v.X, v.Y] == '-')
                return 0;
            if (Map[v.X, v.Y] == 'f')
                return -1;
            return 1;
        }

        public void DrawMapToConsole()
        {
            Console.Clear();
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    Console.Write(Map[j,i]);
                }
                Console.Write("\n");
            }
            Console.WriteLine("Character direction: " + Character.Direction.ToString());
        }

        public static Level FromFile(string path)
        {
            List<Enemy> list = new List<Enemy>();
            Character character = new Character();
            Vector2 finish = new Vector2(0, 0);
            char[,] map = new char[1,1];
            string line;
            char block;
            string[] tmp;
            string str;

            using (StreamReader sr = new StreamReader(path))
            {
                while(sr.Peek() > -1)
                {
                    str = sr.ReadLine();
                    if (Mask.Level.Enemy.IsMatch(str + ";"))
                    {
                        tmp = str.Split(':');
                        Vector2 position = new Vector2(Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2]));
                        Vector2 target = new Vector2();

                        if (tmp[3] == "x")
                        {
                            target.X = Convert.ToInt32(tmp[4]);
                            target.Y = position.Y;
                        }

                        if (tmp[3] == "y")
                        {
                            target.Y = Convert.ToInt32(tmp[4]);
                            target.X = position.X;
                        }
                        list.Add(new Enemy(position, target));
                    }
                    if (Mask.Level.Map.IsMatch(str + ";"))
                    {
                        tmp = str.Split(':');
                        int maxX = Convert.ToInt32(tmp[1]);
                        int maxY = Convert.ToInt32(tmp[2]);
                        map = new char[maxX, maxY];

                        for (int i = 0; i < maxY; i++)
                        {
                            line = sr.ReadLine();
                            for (int j = 0; j < maxX; j++)
                            {
                                block = line[j];
                                if (block == 'w' || block == '-' || block == 'f' || block == 'c')
                                {                                  
                                    map[j,i] = block;
                                    if (block == 'c')
                                        character = new Character(new Vector2(j,i), Vector2.A0);
                                    if (block == 'f')
                                        finish = new Vector2(j, i);
                                }
                            }
                        }
                    }
                }
                Enemy[] enemies = new Enemy[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    enemies[i] = list[i];
                    map[enemies[i].Position.X,enemies[i].Position.Y] = 'e';
                }
                return new Level(map, enemies, character, finish);
            }
        }
    }
}