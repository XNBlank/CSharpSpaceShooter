using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstAttempt
{
    public class Enemy
    {
        public int health;
        public float speed;
        public int score;
        int width;
        int height;
        public Rectangle boundBox;
        public Vector2 position;
        Texture2D sprite;
        int type;
        public bool isActive;
        float yspeed;
        Game1 gameRef;

        public Enemy(Game game, int health, int width, int height, Vector2 position, float speed, Texture2D sprite, int type)
        {
            this.health = health;
            this.speed = speed;
            this.width = width;
            this.height = height;
            this.position = position;
            this.sprite = sprite;
            this.type = type;
            yspeed = speed * 2;
            score = 50;
            
            isActive = false;
            gameRef = (Game1)game;
        }

        public void LoadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            

            if (isActive)
            {
                boundBox = new Rectangle((int)position.X, (int)position.Y, width, height);

                if (type == 0)
                {
                    position.X -= speed;
                    position.Y = -((float)Math.Cos(position.X / speed / 20) * 80) + gameRef.screenSpace.Height / 2;
                }
                else if(type == 1)
                {
                    
                    position.X -= speed;
                    position.Y += -yspeed;
                    if (position.Y < 0)
                    {
                        yspeed = yspeed * -1;
                    }
                    else if (position.Y > gameRef.screenSpace.Height - height)
                    {
                        yspeed = yspeed * -1;
                    }

                }

                if(position.X < -120)
                {
                    isActive = false;
                }
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (isActive)
                spriteBatch.Draw(sprite, position, Color.White);

            spriteBatch.End();
        }

    }
}
