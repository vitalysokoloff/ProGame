using Engine.Utils;

namespace Engine.Game
{
    public class Character
    {
        public Vector2 Position {get; private set;}
        public Vector2 Direction {get; private set;}
        public bool IsAlive {get; private set;} = true;

        public Character(Vector2 position, Vector2 direction)
        {
            Position = position;
            Direction = direction;
        }

        public Character()
        {
            Position = new Vector2(1,1);
            Direction = Vector2.A0;
        }

        public char[,] Update(char[,] map, Command cmd)
        {
            switch (cmd.Name)
            {
                case "step":
                    map = Step(map);
                break;
                case "turn":
                    Turn(cmd);
                break;
            }
            return map;
        }

        private char[,] Step(char[,] map)
        {
            if (Position.X < map.GetLength(0) - 1 && Position.Y < map.GetLength(1) - 1)
            {
                Vector2 v = Position + Direction;
                if(map[v.X, v.Y] == '-' || map[v.X, v.Y] == 'f')
                {
                    map[Position.X, Position.Y] = '-';
                    Position = v;
                    map[v.X, v.Y] = 'c';
                }  
            }
            return map;
        }

        private void Turn(Command cmd)
        {
            if (cmd.Value == "left")
            {
                if (Direction == Vector2.A0)
                {
                    Direction = Vector2.A270;
                    return;
                }
                if (Direction == Vector2.A270)
                {
                    Direction = Vector2.A180;
                    return;
                }
                if (Direction == Vector2.A180)
                {
                    Direction = Vector2.A90;
                    return;
                }
                if (Direction == Vector2.A90)
                {
                    Direction = Vector2.A0;
                    return;
                }

            }
            if (cmd.Value == "right")
            {
                if (Direction == Vector2.A0)
                {
                    Direction = Vector2.A90;
                    return;
                }
                if (Direction == Vector2.A90)
                {
                    Direction = Vector2.A180;
                    return;
                }
                if (Direction == Vector2.A180)
                {
                    Direction = Vector2.A270;
                    return;
                }
                if (Direction == Vector2.A270)
                {
                    Direction = Vector2.A0;
                    return;
                }
                
            }
        }

        public void Kill()
        {
            IsAlive = false;
        }

        public void WakeUp()
        {
            IsAlive = true;
        }

        public override string ToString()
        {
            return "P:" + Position.ToString() + " / D:" + Direction.ToString();
        }
    }
}