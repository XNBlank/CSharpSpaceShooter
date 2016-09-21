using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FirstAttempt
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StarField stars;
        List<StarField> starList;
        SpriteFont font;
        public List<Enemy> enemyList;
        Random ran;
        int starDelay;
        Player player;
        public Enemy enemy;
        int nextWave;
        int waveTimer;
        int enemyDelay;
        bool setWave;
        int waveType;
        int waveNum;
        int waveSpeed;
        public int score;
        Vector2 startPosition;

        public Rectangle screenSpace;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            screenSpace = new Rectangle(0, 0, 720, 480);
        }


        protected override void Initialize()
        {
            ran = new Random();
            startPosition = new Vector2(0, screenSpace.Height / 2);
            player = new Player(this, 5.0f, startPosition, 32, 64);
            stars = new StarField(this, Color.White, 5.0f, new Vector2(0, 0));
            starList = new List<StarField>();
            enemyList = new List<Enemy>();
            starDelay = 30;
            enemyDelay = 120;
            setWave = false;
            waveType = 0;
            waveSpeed = 1;
            waveNum = 0;
            nextWave = ran.Next(0, screenSpace.Height - 64);
            waveTimer = 60;
            enemy = new Enemy(this, 50, 64, 64, new Vector2(screenSpace.Width + 80, nextWave), 1f, Content.Load<Texture2D>(@"Sprites\enemyship"), 1);
            enemy.isActive = true;
            base.Initialize();


        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>(@"Fonts\controlfont");
            player.LoadContent();
            stars.LoadContent();
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            enemyDelay--;
            waveTimer--;
            starDelay--;

            if(starDelay <= 0)
            {
                stars = new StarField(this, (new Color(ran.Next(0, 255), ran.Next(0, 255), ran.Next(0, 255))), ran.Next(2, 8), new Vector2(screenSpace.Width + 80, ran.Next(0, screenSpace.Height)));
                stars.LoadContent();
                stars.isActive = true;
                if (starList.Count <= 100)
                {
                    starList.Add(stars);
                }

                starDelay = ran.Next(5,25);
            }

            for (int i = 0; i < starList.Count; i++)
            {
                starList[i].Update(gameTime);
                if (starList[i].isActive == false)
                {
                    starList.RemoveAt(i);
                    i--;
                }
            }

            if(waveTimer <= 0)
            {
                if(player.health > 0)
                {
                    if (setWave == false)
                    {
                        nextWave = ran.Next(0, screenSpace.Height - 64);
                        waveSpeed = ran.Next(2, 6);
                        waveType = ran.Next(0, 2);
                        waveNum += 1;
                        setWave = true;
                    }
                }


                if (setWave)
                {
                    if (enemyDelay <= 0)
                    {

                        enemy = new Enemy(this, 50, 64, 64, new Vector2(screenSpace.Width + 80, nextWave), waveSpeed, Content.Load<Texture2D>(@"Sprites\enemyship"), waveType);
                        enemy.isActive = true;
                        if (enemyList.Count < 10)
                        {
                            enemyList.Add(enemy);
                        }
                        enemyDelay = 60 / waveSpeed;

                        if (enemyList.Count >= 10)
                        {
                            enemyDelay = 10;
                            setWave = false;
                            waveTimer = 200 * (waveSpeed / 2);
                        }

                    }
                }
                

            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].Update(gameTime);

                if (enemyList[i].isActive == false)
                {
                    if(setWave == false)
                    {
                        enemyList.RemoveAt(i);
                        i--;
                    }

                }
            }

            if(player.health <= 0)
            {
                if (InputHandler.KeyReleased(Keys.Enter))
                {
                    enemyList.Clear();
                    score = 0;
                    waveTimer = 120;
                    waveNum = 0;
                    player.health = 100;
                    player.position = startPosition;
                    player.isActive = true;
                    player.color = Color.White;
                }
            }

            // TODO: Add your update logic here
            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            foreach (StarField s in starList)
            {
                s.Draw(gameTime, spriteBatch);
            }

            player.Draw(gameTime, spriteBatch);

            foreach (Enemy e in enemyList)
            {
                e.Draw(gameTime, spriteBatch);
            }
            

            foreach (StarField s in starList)
            {
                s.Draw(gameTime, spriteBatch);
            }

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Score " + score, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, "Wave " + waveNum, new Vector2(screenSpace.Width/2, 0), Color.White);
            spriteBatch.DrawString(font, "HP " + player.health + "/" + player.maxhealth, new Vector2(screenSpace.Width - 50, 0), Color.White);

            if(player.health <= 0)
            {
                spriteBatch.DrawString(font, "Game Over", new Vector2(screenSpace.Width/2, screenSpace.Height/2), Color.Red);
                spriteBatch.DrawString(font, "Press Enter to Restart", new Vector2(screenSpace.Width / 2, screenSpace.Height / 2 + 50), Color.Red);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
