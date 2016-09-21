using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstAttempt
{
    public class StarField
    {
        public Color color;
        float speed;
        public Vector2 position;
        Texture2D sprite;
        Game1 gameRef;
        public bool isActive;

        public StarField(Game game, Color color, float speed, Vector2 position)
        {
            gameRef = (Game1)game;
            this.color = color;
            this.speed = speed;
            this.position = position;
            isActive = false;
        }

        public void LoadContent()
        {
            sprite = gameRef.Content.Load<Texture2D>(@"Sprites\star");
        }

        public void Update(GameTime gameTime)
        {
            if(position.X >= -5)
            {
                position.X -= speed;
            }
            else
            {
                isActive = false;
            }
            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, position, color);
            spriteBatch.End();
        }
    }
}
