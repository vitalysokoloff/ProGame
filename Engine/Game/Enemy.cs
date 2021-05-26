using Engine.Utils;

namespace Engine.Game
{
    public class Enemy
    {
        public Vector2 Position {get; private set;}
        private Vector2 start;
        private Vector2 finish;
        private bool gotcha = false;
        private Vector2 direction;

        public Enemy(Vector2 position, Vector2 target)
        {
            Position = position;
            start = position;
            finish = target;
            Vector2 delta = finish - start;
            int x = 0, y = 0;
            if (delta.X > 0)
                x = 1;
            if (delta.X < 0)
                x = -1;
            if (delta.Y > 0)
                y = 1;
            if (delta.Y < 0)
                y = -1;
            direction = new Vector2(x, y);
        }

        // Вернёт позишн
        public char[,] Update(char[,] map)
        {
            if (Position.X < map.GetLength(0) - 1 && Position.Y < map.GetLength(1) - 1)
            {                
                map[Position.X, Position.Y] = '-';
                if (!gotcha)
                    Position += direction;
                else
                    Position -= direction;
                map[Position.X, Position.Y] = 'e';
                if (Position == finish)
                    gotcha = true;
                if (Position == start)
                    gotcha = false;
            }
            return map;
        }

        public override string ToString()
        {
            return "P:" + Position.ToString() + " / T:" + start.ToString();
        }
    }
}