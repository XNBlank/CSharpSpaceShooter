using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstAttempt
{
    public class Bullet
    {
        int width;
        int height;
        public Vector2 position;
        public Rectangle boundBox;
        int power;
        float speed;
        public bool isActive;

        Texture2D sprite;

        Game1 gameRef;

        public Bullet(Game game, Vector2 position, int width, int height, int power, float speed)
        {
            gameRef = (Game1)game;
            this.width = width;
            this.height = height;
            
            this.power = power;
            this.speed = speed;
            this.position = position;
            isActive = false;
        }

        public void LoadContent()
        {
            sprite = gameRef.Content.Load<Texture2D>(@"Sprites\pixel");
            
        }

        public void Update(GameTime gameTime)
        {
            

            if (isActive)
            {
                boundBox = new Rectangle((int)position.X, (int)position.Y, this.width, this.height);
                position.X += speed;
            }

            if(position.X > gameRef.screenSpace.Width + 64)
            {
                isActive = false;
                
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, position, Color.White);
            spriteBatch.End();
        }
    }
}
