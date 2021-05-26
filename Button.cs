using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProGame
{
    public class Button
    {
        private Texture2D btn_0;
        private Texture2D btn_1;
        private Rectangle rectangle;
        private Vector2 position;
        private bool state = false;
        private bool mouseBtnPressed = false;

        public Button(Texture2D btn_0, Texture2D btn_1, Vector2 position)
        {
            this.btn_0 = btn_0;
            this.btn_1 = btn_1;
            this.position = position;
            rectangle = new Rectangle((int)position.X, (int)position.Y, btn_0.Width, btn_0.Height);
        }

        public bool GetState(MouseState mouseState)
        {
            if (mouseState.Position.X > rectangle.X && mouseState.Position.X < rectangle.X + rectangle.Width && mouseState.Position.Y > rectangle.Y && mouseState.Position.Y < rectangle.Y + rectangle.Width)
            {
                if (!mouseBtnPressed)
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (!state)
                            state = true;
                        else
                            state = false;
                        mouseBtnPressed = true;
                    }
                if (mouseState.LeftButton == ButtonState.Released)
                    mouseBtnPressed = false;
            }
            return state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(!state)
                spriteBatch.Draw(btn_0, position, Color.White);
            else
                spriteBatch.Draw(btn_1, position, Color.White);
        }
    }
}
