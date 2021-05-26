using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Engine.Utils;
using Engine.Game;
using MyVector2 = Engine.Utils.Vector2;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProGame
{
    public class Player
    {
        private InOut.Config cfg;
        private Texture2D spritePack;
        private SpriteFont font;

        private bool isPlaying = false;

        private Level level;
        private Command[] commands = new Command[] { new Command("valuetolog", "Что мне делать?"), new Command("valuetolog", "Конец") };

        private int pointer = 0;
        private Registry[] Regs = new Registry[] {
            new Registry("a"), // 0
            new Registry("b"), // 1
            new Registry("c"), // 2
            new Registry("d"), // 3
            new Registry("tmp") // 4
        };

        private Metronome metronome;
        private bool isWon = false;

        private int CharFrame = 0;
        private int MaxFrame = 1;
        private int DeadFrame = 3;
        private float CharRotation = 3.14f;
        private Vector2 CharVectorOrigin;

        private SoundEffectInstance step;
        private SoundEffectInstance finish;
        private SoundEffectInstance fail;

        private bool gotcha = false;

        public string Log { get; private set; } = "";

        public Player(InOut.Config cfg, Texture2D spritePack, SpriteFont font, SoundEffect[] effects)
        {
            this.cfg = cfg;
            this.spritePack = spritePack;
            this.font = font;

            CharVectorOrigin = new Vector2(24, 24);

            level = Level.FromFile(cfg.Map);
            step = effects[0].CreateInstance();
            step.IsLooped = false;
            step.Volume = 0.4f;
            finish = effects[1].CreateInstance();
            finish.IsLooped = false;
            fail = effects[2].CreateInstance();
            fail.IsLooped = false;

            if (!cfg.SoundState)
            {
                step.Volume = 0;
                finish.Volume = 0;
                fail.Volume = 0;
            }


            metronome = new Metronome(cfg.Speed);
            metronome.Start();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < level.Map.GetLength(1); i++)
            {
                for (int j = 0; j < level.Map.GetLength(0); j++)
                {
                    switch (level.Map[j, i])
                    {
                        case 'w':
                            spriteBatch.Draw(spritePack, new Vector2(j * 48 + 20, i * 48 + 20), new Rectangle(0, 0, 48, 48), Color.White);
                            break;
                        case '-':
                            spriteBatch.Draw(spritePack, new Vector2(j * 48 + 20, i * 48 + 20), new Rectangle(48, 0, 48, 48), Color.White);
                            break;
                        case 'f':
                            spriteBatch.Draw(spritePack, new Vector2(j * 48 + 20, i * 48 + 20), new Rectangle(96, 0, 48, 48), Color.White);
                            break;
                        case 'e':
                            spriteBatch.Draw(spritePack, new Vector2(j * 48 + 20, i * 48 + 20), new Rectangle(0, 0, 48, 48), Color.White);
                            break;
                        case 'c':
                            if (CharFrame < DeadFrame + 1)
                                spriteBatch.Draw(spritePack, new Vector2(j * 48 + 44, i * 48 + 44), new Rectangle(0 + 48 * CharFrame, 48, 48, 48), Color.White, CharRotation, CharVectorOrigin, 1, SpriteEffects.None, 0);
                            break;
                    }
                }
            }
            for (int i = 0; i < Regs.Length; i++)
                spriteBatch.DrawString(font, Regs[i].Name + ":" + Regs[i].Take(), new Vector2(30 + i * 35, 550), new Color(67, 67, 65));
            spriteBatch.DrawString(font, Log, new Vector2(250, 550), new Color(67, 67, 65));
            if (!level.Character.IsAlive)
                spriteBatch.Draw(spritePack, new Vector2(level.Character.Position.X * 48 + 44, level.Character.Position.Y * 48 + 44), new Rectangle(0 + 48 * CharFrame, 48, 48, 48), Color.White, CharRotation, CharVectorOrigin, 1, SpriteEffects.None, 0);
        }

        public void Start()
        {
            if (!isPlaying)
            {
                Compile();
                isPlaying = true;
            }
        }

        public void Reset()
        {
            if (isPlaying)
            {
                isPlaying = false;
                isWon = false;
                level = Level.FromFile(cfg.Map);
                pointer = 0;
                CharRotation = 3.14f;
                CharFrame = 0;
                gotcha = false;

                foreach (Registry r in Regs)
                    r.Put(0);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (isPlaying)
            {
                if (metronome.Ticking(gameTime))
                {
                    if (level.Character.IsAlive)
                    {
                        if (level.Character.Position == level.Finish)
                            isWon = true;
                        if (!isWon)
                        {
                            if (pointer < commands.Length)
                            {
                                step.Play();
                                level.Update(commands[pointer]);

                                if (CharFrame < MaxFrame + 1)
                                    CharFrame++;
                                if (CharFrame > MaxFrame)
                                    CharFrame = 0;

                                if (level.Character.Direction == MyVector2.A0)
                                    CharRotation = 3.14f;
                                if (level.Character.Direction == MyVector2.A90)
                                    CharRotation = 4.71f;
                                if (level.Character.Direction == MyVector2.A180)
                                    CharRotation = 0;
                                if (level.Character.Direction == MyVector2.A270)
                                    CharRotation = 1.57f;

                                Interpret();
                            }
                        }
                        else
                        {
                            if (!gotcha)
                                finish.Play();
                            Log = "Победа! Дошёл до финиша.";
                            isPlaying = false;
                            gotcha = true;
                        }
                    }
                    else
                    {
                        if (!gotcha)
                            fail.Play();
                        if (CharFrame < DeadFrame + 1)
                            CharFrame++;
                        if (CharFrame > DeadFrame)
                        {                            
                            Log = "Опаньки!";
                            isPlaying = false;
                        }
                        gotcha = true;
                    }
                }
            }
        }

        public void Compile()
        {
            commands = InOut.GetCommands(cfg.Script);
        }

        private void Interpret()
        {
            Log = "[" + pointer + "] ";
            switch (commands[pointer].Name)
            {
                case "goto":
                    pointer = Convert.ToInt32(commands[pointer].Value);
                    Log += "Иду на " + pointer; 
                    break;
                case "gotozero":
                    if (Regs[4].Take() == 0)
                    {
                        pointer = Convert.ToInt32(commands[pointer].Value);
                        Log += "Иду на " + pointer;
                    }
                    else
                        pointer++;
                    break;
                case "gotonotzero":
                    if (Regs[4].Take() != 0)
                    { 
                        pointer = Convert.ToInt32(commands[pointer].Value);
                        Log += "Иду на " + pointer;
                    }
                    else
                        pointer++;
                    break;
                case "gotoabove":
                    if (Regs[4].Take() > 0)
                    { 
                        pointer = Convert.ToInt32(commands[pointer].Value);
                        Log += "Иду на " + pointer;
                    }
                    else
                        pointer++;
                    break;
                case "gotobelow":
                    if (Regs[4].Take() < 0)
                    { 
                        pointer = Convert.ToInt32(commands[pointer].Value);
                        Log += "Иду на " + pointer;
                    }
                    else
                        pointer++;
                    break;
                case "write":
                    for (int i = 0; i < Regs.Length; i++)
                        if (commands[pointer].Value == Regs[i].Name)
                        {
                            Regs[i].Put(Regs[4].Take());
                            Log += "Пишу в " + Regs[i].Name + " " + Regs[4].Take();
                            break;
                        }                    
                    pointer++;
                    break;
                case "writevalue":
                    for (int i = 0; i < Regs.Length; i++)
                        if (commands[pointer].Value == Regs[i].Name)
                        {
                            Regs[i].Put(commands[pointer].eValue);
                            Log += "Пишу в " + Regs[i].Name + " " + commands[pointer].eValue;
                            break;
                        }
                    pointer++;
                    break;
                case "log":
                    Log += " " + Regs[4];
                    pointer++;
                    break;
                case "valuetolog":
                    Log += " " + commands[pointer].Value;
                    pointer++;
                    break;
                case "read":
                    for (int i = 0; i < Regs.Length; i++)
                        if (commands[pointer].Value == Regs[i].Name)
                        {
                            Regs[4].Put(Regs[i].Take());
                            Log += "Читаю из " + Regs[i].Name;
                            break;
                        }
                    pointer++;
                    break;
                case "add":
                    for (int i = 0; i < Regs.Length; i++)
                        if (commands[pointer].Value == Regs[i].Name)
                        {
                            Regs[4].Put(Regs[i].Take() + Convert.ToInt32(commands[pointer].eValue));
                            Log += Regs[i].Take() + "+" + Convert.ToInt32(commands[pointer].eValue) + "=" + (Regs[i].Take() + Convert.ToInt32(commands[pointer].eValue));
                            break;
                        }
                    pointer++;
                    break;
                case "takeaway":
                    for (int i = 0; i < Regs.Length; i++)
                        if (commands[pointer].Value == Regs[i].Name)
                        {
                            Regs[4].Put(Regs[i].Take() - Convert.ToInt32(commands[pointer].eValue));
                            Log += Regs[i].Take() + "-" + Convert.ToInt32(commands[pointer].eValue) + "=" + (Regs[i].Take() - Convert.ToInt32(commands[pointer].eValue));
                            break;
                        }
                    pointer++;
                    break;
                case "check":
                    Regs[4].Put(level.CheckMap(commands[pointer]));
                    Log += "Впереди меня " + level.CheckMap(commands[pointer]);
                    pointer++;
                    break;
                case "error:":
                    Log = "\nОшибка в строке: " + commands[pointer].Value;
                    pointer = commands.Length - 1;
                    break;
                default:
                    pointer++;
                    break;
            }
        }
    }
}
