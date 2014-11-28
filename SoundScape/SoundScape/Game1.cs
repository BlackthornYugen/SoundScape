/*
 * 
 * Final Project: Sound Scape 
 * 
 * 
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoundScape.Levels;
using XNALib.Scenes;

namespace SoundScape
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        SpriteBatch _spriteBatch;
        KeyboardState _oldKeyboardState;
        GamePadState _oldPadState;

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }
        private StartScene menu;

        private GameScene howToPlay;
        private GameScene help;
        private HighScore highScore;
        private GameScene credit;

        private GameScene gameplay;

        public Game1()
        {
            GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            this.Components.Add(menu = new StartScene(this, _spriteBatch, new string[] 
                { "Start Game", "How To Play", "Help", "High Score", "Credits", "Quit" }));

            this.Components.Add(help = new HelpScene(this, Content.Load<Texture2D>("images/Help")));
            this.Components.Add(howToPlay = new HelpScene(this, Content.Load<Texture2D>("images/HowToPlay")));

            int tempHigh = 3;
            this.Components.Add(highScore = new HighScore(this, Content.Load<Texture2D>("images/HighScore"), tempHigh));
                
            this.Components.Add(credit = new HelpScene(this, Content.Load<Texture2D>("images/Credits")));

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
            ControlInput();

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// Handles the user input for the menu
        /// </summary>
        void ControlInput()
        {
            KeyboardState ks = Keyboard.GetState();
            GamePadState ps = GamePad.GetState(0);

            // Allows the game to exit
            if (ks.IsKeyDown(Keys.Escape) && _oldKeyboardState.IsKeyUp(Keys.Escape) ||
                ps.IsButtonDown(Buttons.Back) && _oldPadState.IsButtonUp(Buttons.Back))
            {
                if (menu.Enabled)
                    this.Exit();
                else
                {
                    HideAllScene();
                    SetTitle();
                    menu.Show();
                }
            }

            // { "Start Game", "How To Play", "Help", "High Score", "Credit", "Quit" }));
            if (menu.Enabled && ks.IsKeyDown(Keys.Enter) ||
                ps.IsButtonDown(Buttons.Start) && _oldPadState.IsButtonUp(Buttons.Start))
            {
                switch (menu.SelectedItem.Name)
                {
                    case "Start Game":
                        HideAllScene();
                        SetTitle("Game thing");
                        if(gameplay!= null)
                        {
                            Components.Remove(gameplay);
                            gameplay.Dispose();
                        }
                        Components.Add(gameplay = new MultiplayerLevel1(this, _spriteBatch));
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
                    case "High Score":
                        HideAllScene();
                        SetTitle("High Score");
                        highScore.Show();
                        break;
                    case "Credits":
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


            if (ks.IsKeyUp(Keys.Down) && _oldKeyboardState.IsKeyDown(Keys.Down) ||
                ps.IsButtonDown(Buttons.DPadDown) && _oldPadState.IsButtonUp(Buttons.DPadDown))
                menu.SelectedIndex = Math.Min(menu.SelectedIndex + 1, menu.Count - 1);
            else if (ks.IsKeyUp(Keys.Up) && _oldKeyboardState.IsKeyDown(Keys.Up) ||
                ps.IsButtonDown(Buttons.DPadUp) && _oldPadState.IsButtonUp(Buttons.DPadUp))
                menu.SelectedIndex = Math.Max(menu.SelectedIndex - 1, 0);

            _oldKeyboardState = ks;
            _oldPadState = ps;
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
