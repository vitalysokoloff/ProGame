namespace Engine.Utils
{
    public class Vector2
    {
        public int X {get; set;}
        public int Y {get; set;}

        public Vector2()
        {
            X = 0;
            Y = 0;
        }

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        // сложение векторов
        public static Vector2 operator + (Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        // вычитание векторов
        public static Vector2 operator - (Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        // равенство векторов
        public static bool operator == (Vector2 a, Vector2 b)
        {
            if (a.X == b.X && a.Y == b.Y)
            {
                return true;
            }
            else
                return false;
        }
        
         public static bool operator != (Vector2 a, Vector2 b)
        {
            if (a.X != b.X || a.Y != b.Y)
            {
                return true;
            }
            else
                return false;
        }

        public override string ToString()
        {
            return "X:" + X + " Y:" + Y;
        }

        public static Vector2 Zero = new Vector2(0, 0);
        public static Vector2 A0 = new Vector2(0, 1);
        public static Vector2 A45 = new Vector2(-1, 1);
        public static Vector2 A90 = new Vector2(-1, 0);
        public static Vector2 A135 = new Vector2(-1, -1);
        public static Vector2 A180 = new Vector2(0, -1);
        public static Vector2 A225 = new Vector2(1, -1);
        public static Vector2 A270 = new Vector2(1, 0);
        public static Vector2 A315 = new Vector2(1, 1);
    } 
}
