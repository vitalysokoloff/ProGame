using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Engine.Utils;
using MyVector2 = Engine.Utils.Vector2;
using Vector2 = Microsoft.Xna.Framework.Vector2;


namespace ProGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D back;
        Texture2D btn_0;
        Texture2D btn_1;
        Texture2D spritePack;
        SpriteFont font;

        Button button;
        Player player;

        SoundEffect[] effects = new SoundEffect[3];
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 682;
            graphics.PreferredBackBufferWidth = 520;
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            back = Content.Load<Texture2D>("back");
            btn_0 = Content.Load<Texture2D>("btn_0");
            btn_1 = Content.Load<Texture2D>("btn_1");
            spritePack = Content.Load<Texture2D>("spritePack");
            font = Content.Load<SpriteFont>("font");
            effects[0] = Content.Load<SoundEffect>("step");
            effects[1] = Content.Load<SoundEffect>("finish");
            effects[2] = Content.Load<SoundEffect>("fail");

            button = new Button(btn_0, btn_1, new Vector2(20, 601));
            player = new Player(new InOut.Config("cfg.txt"), spritePack, font, effects);             
        }

        protected override void UnloadContent()
        {
            
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (button.GetState(Mouse.GetState()))
                player.Start();
            else
                player.Reset();

            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
                spriteBatch.Draw(back, new Vector2(0,0), Color.White);
                button.Draw(spriteBatch);
                player.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
