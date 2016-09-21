using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FirstAttempt
{
    public class Player
    {
        float moveSpeed;
        public Vector2 position;
        Texture2D sprite;
        int playerHeight;
        int playerWidth;
        Game1 gameRef;
        InputHandler input;
        Bullet bullet;
        int bulletDelay;
        public List<Bullet> bulletList;
        Rectangle boundBox;
        public bool isActive;

        public Color color;

        bool invincible;
        int invincibleTimer;
        public int health;
        public int maxhealth;

        public Player(Game game, float movespeed, Vector2 position, int height, int width)
        {
            gameRef = (Game1)game;
            bullet = new Bullet(gameRef, position, 4, 1, 5, 4.0f);
            playerHeight = height;
            playerWidth = width;
            bulletList = new List<Bullet>();
            bulletDelay = 5;
            this.position = position;
            moveSpeed = movespeed;
            health = 100;
            maxhealth = health;
            invincible = false;
            invincibleTimer = 15;
            input = new InputHandler(gameRef);
            input.Initialize();
            isActive = true;
            color = Color.White;
        }

        public void LoadContent()
        {
            sprite = gameRef.Content.Load<Texture2D>(@"Sprites\ship");
            bullet.LoadContent();
        }

        public void Update(GameTime gameTime)
        {

            boundBox = new Rectangle((int)position.X, (int)position.Y, playerWidth, playerHeight);

            if (isActive)
            {

                if (InputHandler.KeyDown(Keys.W))
                {
                    position.Y -= moveSpeed;
                }
                if (InputHandler.KeyDown(Keys.S))
                {
                    position.Y += moveSpeed;
                }
                if (InputHandler.KeyDown(Keys.D))
                {
                    position.X += moveSpeed;
                }
                if (InputHandler.KeyDown(Keys.A))
                {
                    position.X -= moveSpeed;
                }

                if (InputHandler.KeyDown(Keys.Space))
                {
                    bulletDelay--;
                    if (bulletDelay <= 0)
                    {
                        bullet = new Bullet(gameRef, position, 4, 1, 5, 10.0f);
                        bullet.position = new Vector2(position.X + playerWidth, position.Y + playerHeight / 2);
                        bullet.LoadContent();
                        bullet.isActive = true;
                        if (bulletList.Count() < 20)
                        {
                            bulletList.Add(bullet);
                        }
                        bulletDelay = 25;
                    }

                }

                position.X = MathHelper.Clamp(position.X, 0, gameRef.screenSpace.Width - playerWidth / 2);
                position.Y = MathHelper.Clamp(position.Y, 0, gameRef.screenSpace.Height - playerHeight);

                for (int i = 0; i < bulletList.Count; i++)
                {
                    for (int j = 0; j < gameRef.enemyList.Count; j++)
                    {
                        if (bulletList[i].boundBox.Intersects(gameRef.enemyList[j].boundBox))
                        {
                            Debug.WriteLine("Connected!");
                            gameRef.enemyList[j].isActive = false;
                            gameRef.score += 100;
                            Debug.WriteLine("Score : " + gameRef.score);
                            bulletList[i].isActive = false;
                        }

                    }
                    bulletList[i].Update(gameTime);
                    if (bulletList[i].isActive == false)
                    {
                        bulletList.RemoveAt(i);
                        i--;
                    }
                }


                for (int j = 0; j < gameRef.enemyList.Count; j++)
                {
                    if (boundBox.Intersects(gameRef.enemyList[j].boundBox))
                    {
                        Debug.WriteLine("You've Been Hit!");
                        invincibleTimer = 15;
                        if (invincible == false)
                            health -= 10;

                        invincible = true;
                        gameRef.enemyList[j].isActive = false;
                    }
                }
            }


            if(invincible == true)
            {
                invincibleTimer--;
                if (invincibleTimer <= 0)
                {
                    invincible = false;
                }
            }

            if(health <= 0)
            {
                isActive = false;
                color = Color.Red;
            }

            input.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(Bullet b in bulletList)
            {
                b.Draw(gameTime, spriteBatch);
            }
            
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, position, color);
            spriteBatch.End();
        }
    }
}
