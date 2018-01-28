using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Julian_Hundreds
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        Texture2D circle;
        Texture2D pixel;
        Texture2D strike;
        SpriteFont arial;
        SpriteFont big;
        List<Ball> balls;
        List<int> xlocs;
        List<int> ylocs;
        List<int> xspeeds;
        List<int> yspeeds;
        List<Ball> strikeballs;
        int nextxloc;
        int nextyloc;
        Random rand;
        bool lost;
        int score = 0;
        int strikes = 0;
        int speed = 2;
        int numofballs = 30;
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            Mouse.SetPosition(-100, -100);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            balls = new List<Ball>();
            xlocs = new List<int>();
            ylocs = new List<int>();
            xspeeds = new List<int>();
            yspeeds = new List<int>();
            strikeballs = new List<Ball>();
            rand = new Random();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            circle = Content.Load<Texture2D>("clearcircle");
            pixel = Content.Load<Texture2D>("pixel");
            strike = Content.Load<Texture2D>("strike");
            arial = Content.Load<SpriteFont>("File");
            big = Content.Load<SpriteFont>("big");
            for (int i = 0; i < numofballs; i++)
            {
                nextxloc = rand.Next(0, (graphics.GraphicsDevice.Viewport.Width - 50));
                nextyloc = rand.Next(0, (graphics.GraphicsDevice.Viewport.Height - 50));
                xlocs.Add(nextxloc);
                ylocs.Add(nextyloc);
                xspeeds.Add(rand.Next(-1, 1) == 0 ? speed : -speed);
                yspeeds.Add(rand.Next(-1, 1) == 0 ? speed : -speed);
                balls.Add(new Ball(xlocs[i], ylocs[i], xspeeds[i], yspeeds[i], circle, graphics.GraphicsDevice, balls.Count - 1));
            }
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (lost || score >= 100)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    xspeeds.Clear();
                    yspeeds.Clear();
                    xlocs.Clear();
                    ylocs.Clear();
                    balls.Clear();
                    strikes = 0;
                    for (int i = 0; i < numofballs; i++)
                    {
                        Mouse.SetPosition(-100, -100);
                        nextxloc = rand.Next(0, (graphics.GraphicsDevice.Viewport.Width - 50));
                        nextyloc = rand.Next(0, (graphics.GraphicsDevice.Viewport.Height - 50));
                        xlocs.Add(nextxloc);
                        ylocs.Add(nextyloc);
                        xspeeds.Add(rand.Next(-1, 1) == 0 ? speed : -speed);
                        yspeeds.Add(rand.Next(-1, 1) == 0 ? speed : -speed);
                        balls.Add(new Ball(xlocs[i], ylocs[i], xspeeds[i], yspeeds[i], circle, graphics.GraphicsDevice, balls.Count - 1));
                    }
                    lost = false;
                    foreach (Ball b in balls)
                    {
                        b.lost = false;
                    }
                }
                return;
            }
            // TODO: Add your update logic here
            foreach (Ball b in balls)
            {
                b.Move(balls);
                if (b.lost)
                {
                    strikeballs.Add(b);
                    strikes++;
                    if (strikes >= 6)
                    {
                        lost = true;
                    }
                }
            }
            for (int i = 0; i < strikeballs.Count; i++)
            {
                balls.Remove(strikeballs[i]);
            }
            strikeballs.Clear();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (lost)
            {
                GraphicsDevice.Clear(Color.Red);
            }
            else if (score >= 100)
            {
                GraphicsDevice.Clear(Color.Green);
            }
            else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }
            spriteBatch.Begin();
            score = 0;
            // TODO: Add your drawing code here
            foreach (Ball b in balls)
            {
                //spriteBatch.Draw(pixel, b.X, Color.White);
                //spriteBatch.Draw(pixel, b.Y, Color.White);
                b.Draw(spriteBatch, arial);
                spriteBatch.DrawString(arial, ((b.colsize - 50) / 10).ToString(), new Vector2(b.Y.X - arial.MeasureString(((b.colsize - 50) / 10).ToString()).X / 2, b.X.Y - arial.MeasureString(((b.colsize - 50) / 10).ToString()).Y / 2), Color.White);
                //spriteBatch.Draw(pixel, new Rectangle(Mouse.GetState().Position.X, Mouse.GetState().Position.Y, 1,1), Color.White);
                score += ((b.colsize - 50) / 10);
            }
            spriteBatch.DrawString(big, score.ToString(), new Vector2(0, 0), Color.White);
            if (strikes >= 2)
            {
                spriteBatch.Draw(strike, new Vector2(GraphicsDevice.Viewport.Width - 3 * strike.Width, 0), Color.White);
                if (strikes >= 4)
                {
                    spriteBatch.Draw(strike, new Vector2(GraphicsDevice.Viewport.Width - 2 * strike.Width, 0), Color.White);
                    if (strikes >= 6)
                    {
                        spriteBatch.Draw(strike, new Vector2(GraphicsDevice.Viewport.Width - strike.Width, 0), Color.White);
                    }
                }
            }
            if (lost)
            {
                spriteBatch.DrawString(big, "You Lost", new Vector2(GraphicsDevice.Viewport.Width / 2 - big.MeasureString("You Lost").X / 2, 0), Color.White);
            }
            if (score >= 100)
            {
                spriteBatch.DrawString(big, "You Win", new Vector2(GraphicsDevice.Viewport.Width / 2 - big.MeasureString("You Win").X / 2, 0), Color.White);
            }
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
