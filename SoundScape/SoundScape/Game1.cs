/*
 * 
 * Sound Scape 2nd Time
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XNALib.Scenes;

namespace SoundScape
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState oldState;

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }
        private StartScene menu;

        private GameScene howToPlay;
        private GameScene help;
        private GameScene highScore;
        private GameScene credit;

        private GameScene gameplay;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 800;
        }

        private void HideAllScene()
        {
            foreach (GameComponent gc in Components)
            {
                if (gc is GameScene)
                    (gc as GameScene).Hide();
            }
        }

        public void SetTitle(string title = null)
        {
            if (String.IsNullOrWhiteSpace(title))
                Window.Title = "SoundScape";
            else
                Window.Title = string.Format("SoundScape - {0}", title.Trim());
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            SetTitle();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            this.Components.Add(menu = new StartScene(this, spriteBatch, new string[] 
                { "Start Game", "How To Play", "Help", "High Score", "Credit", "Quit" }));
            this.Components.Add(gameplay = new GameplayScene(this, spriteBatch));

            this.Components.Add(help = new HelpScene(this, Content.Load<Texture2D>("helpImage")));
            this.Components.Add(howToPlay = new HelpScene(this, Content.Load<Texture2D>("howToPlay")));
            this.Components.Add(credit = new HelpScene(this, Content.Load<Texture2D>("credit")));

            menu.Show();
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            KeyboardState ks = Keyboard.GetState();
            // Allows the game to exit
            if (ks.IsKeyDown(Keys.Escape) && oldState.IsKeyUp(Keys.Escape))
            {
                if(menu.Enabled)
                    this.Exit();
                else
                {
                    HideAllScene();
                    SetTitle();
                    menu.Show();
                }
            }

            // { "Start Game", "How To Play", "Help", "High Score", "Credit", "Quit" }));
            if (menu.Enabled && ks.IsKeyDown(Keys.Enter))
            {
                switch (menu.SelectedItem.Name)
                {
                    case "Start Game":
                        HideAllScene();
                        SetTitle("Game thing");
                        gameplay.Show();
                        break;
                    case "How To Play":
                        HideAllScene();
                        SetTitle("How To Play");
                        howToPlay.Show();
                        break;
                    case "Help":
                        HideAllScene();
                        SetTitle("Help");
                        help.Show();
                        break;
                    case "Credit":
                        HideAllScene();
                        SetTitle("Credit");
                        credit.Show();
                        break;
                    case "Quit":
                        this.Exit();
                        break;
                    default:
                        if (menu.SelectedItem.Component == null)
                        {
                            SetTitle(string.Format("\"{0}\" cannot be opened.", menu.SelectedItem.Name));
                        }
                        else
                        {
                            SetTitle(menu.SelectedItem.Name);
                        }
                        break;
                }
            }

            oldState = ks;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
