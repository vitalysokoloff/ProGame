using Microsoft.Xna.Framework;

namespace ProGame
{
    public class Metronome
    {
        private int curTime;
        private int period;
        private bool isStart = false;
        public int Count { get; private set; }

        public Metronome(int period)
        {
            curTime = 0;
            Count = 0;
            this.period = period;
        }

        public bool Ticking(GameTime gameTime)
        {
            if (isStart)
            {
                curTime += gameTime.ElapsedGameTime.Milliseconds;
                if (curTime > period) // Анимация
                {
                    curTime = 0;
                    Count++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            curTime = 0;
            Count = 0;
        }

        public void Stop()
        {
            if (isStart)
                isStart = false;
        }

        public void Start()
        {
            if (!isStart)
                isStart = true;
        }
    }
}
