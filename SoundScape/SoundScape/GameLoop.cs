/*
 * 
 * Final Project: Sound Scape 
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoundScape.Levels;
using XNALib.Menus;
using XNALib.Scenes;
using Microsoft.Xna.Framework.Audio;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using GameOptions = SoundScape.GameplayScene.GameOptions;

namespace SoundScape
{


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameLoop : Game
    {
        private SoundEffect[] _menuEffects; 
        private SpriteBatch _spriteBatch;
        private SpeechSynthesizer _speechSynthesizer;

        private StartScene _menu;
        private GameScene _howToPlay;
        private GameScene _help;
        private GameScene _highScore;
        private GameScene _newHighScore;
        private GameScene _credit;

        private MenuComponent<GameOptions> _gametypeMenu;  

        public SpriteFont DefaultGameFont { get; private set; }
        public SpriteFont BigFont { get; private set; }

        public readonly VirtualController PlayerOne;
        public readonly VirtualController PlayerTwo;

        public GameLoop(string monitor = null)
        {
            PlayerOne = new VirtualController(this, PlayerIndex.One);

            PlayerTwo = new VirtualController(this, PlayerIndex.Two)
            {
                MovementUpKeys = new[] { Keys.Up },
                MovementDownKeys = new[] { Keys.Down },
                MovementLeftKeys = new[] { Keys.Left },
                MovementRightKeys = new[] { Keys.Right },

                AimUpKeys = new[] { Keys.NumPad8 },
                AimDownKeys = new[] { Keys.NumPad2 },
                AimLeftKeys = new[] { Keys.NumPad4 },
                AimRightKeys = new[] { Keys.NumPad6 },

                GameFireKeys = new[] { Keys.Add, Keys.Subtract, Keys.Delete, Keys.Insert, }
            };

            GraphicsDeviceManager graphics;
            if (monitor == null)
                graphics = new GraphicsDeviceManager(this);
            else
                graphics = new TargetedGraphicsDeviceManager(this, monitor);
            
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GraphicsAdapter.Adapters.Last().CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.Adapters.Last().CurrentDisplayMode.Height;
            graphics.ApplyChanges();

            // The next 4 lines are apparently the only way to get borderless in xna. 
            IntPtr hWnd = Window.Handle;
            var control = Control.FromHandle(hWnd);
            var form = control.FindForm();
            form.FormBorderStyle = FormBorderStyle.None;
            form.Left = 200;
            // End of xna borderless hack ( http://gamedev.stackexchange.com/questions/37109/ )

            _speechSynthesizer = new SpeechSynthesizer();
        }

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public HighScore HighScore
        {
            get { return _highScore as HighScore; }
        }

        public GameScene Gameplay { get; set; }

        public GameScene NewHighScore
        {
            get { return _newHighScore; }
        }

        public void Speak(string textToSpeak)
        {
            _speechSynthesizer.SpeakAsyncCancelAll();
            _speechSynthesizer.SpeakAsync(textToSpeak);
        }

        private void HideAllScene()
        {
            foreach (IGameComponent gc in Components)
            {
                var gs = gc as GameScene;
                if (gs != null) gs.Hide();
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

            _menuEffects = new[]
            {
                Content.Load<SoundEffect>("sounds/Beep2"),
                Content.Load<SoundEffect>("sounds/Beep4"),
            }; 

            Texture2D dimensions = Content.Load<Texture2D>("images/Help");
            //All images for menus should have same size 
            Vector2 centerScreen = new Vector2(GraphicsDevice.Viewport.Width / 2 - dimensions.Width / 2,
                GraphicsDevice.Viewport.Height / 2 - dimensions.Height / 2);

            DefaultGameFont = Content.Load<SpriteFont>("fonts/regularFont");
            BigFont = Content.Load<SpriteFont>("fonts/bigFont");
            Texture2D backGround = Content.Load<Texture2D>("images/back/earth");
            Random r = new Random();
            Components.Add(_menu = new StartScene(this, _spriteBatch, new[] { "One Player Game", "Two Player Game", "How To Play", "Help", "High Score", "Credits", "Quit" })
            {
                Background = backGround
            });

            Components.Add(_gametypeMenu = new MenuComponent<GameOptions>(this, _spriteBatch, Color.LightYellow, Color.Yellow, DefaultGameFont, DefaultGameFont, Vector2.One * 100 + Vector2.UnitX * 400)
            {
                {"Normal Mode", GameOptions.None},
                {"Spectator Mode (Score not saved)", GameOptions.SpectatorMode}
            });

            _gametypeMenu.Hide();

            Components.Add(_help = new InfoScene(this, Content.Load<Texture2D>("images/Help"), 
                backGround, centerScreen));
            Components.Add(_howToPlay = new InfoScene(this, Content.Load<Texture2D>("images/HowToPlay"), 
                backGround, centerScreen));
            Components.Add(_credit = new InfoScene(this, Content.Load<Texture2D>("images/Credits"),
                backGround, centerScreen));
            Components.Add(_highScore = new HighScore(this, Content.Load<Texture2D>("images/HighScore"),
                backGround, centerScreen, Toolbox.LoadObjectFromFile<List<HighScoreSaved>>("content/highscores.json")));

            Components.Add(_newHighScore = new NewHighscore(this, SpriteBatch) { Background = backGround });
            _newHighScore.Initialize();
            Campaign.New(this);
            _menu.Show();

            Components.Add(PlayerOne);
            Components.Add(PlayerTwo);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            AllowExit = !_newHighScore.Enabled;
            MainMenuInput();
            base.Update(gameTime);
        }

        /// <summary>
        /// Handles the user input for the menu
        /// </summary>
        void MainMenuInput()
        {
            var inputs = new[] {PlayerOne, PlayerTwo};

            if (_gametypeMenu.Enabled)
            {
                GameTypeMenuInput(inputs);
                return;
            }

            // Allows the game to exit
            if (AllowExit && inputs.Any(p => p.ActionBack))
            {
                if (_menu.Enabled)
                {
                    PlayMenuSound(0);
                    Exit();
                }
                else
                {
                    PlayMenuSound(0);
                    HideAllScene();
                    SetTitle();
                    _menu.Show();
                    inputs.ForEach(p => GamePad.SetVibration(p.PlayerIndex, 0, 0));
                }
            }

            if (_menu.Enabled)
            {
                if (inputs.Any(p=>p.ActionSelect))
                {
                    switch (_menu.SelectedIndex)
                    {
                        case 0:
                            _gametypeMenu.Show();
                            break;
                        case 1:
                            _gametypeMenu.Show();
                            break;
                        case 2:
                            HideAllScene();
                            SetTitle("How To Play");
                            _howToPlay.Show();
                            break;
                        case 3:
                            HideAllScene();
                            SetTitle("Help");
                            _help.Show();
                            break;
                        case 4:
                            HideAllScene();
                            SetTitle("High Score");
                            HighScore.Show();
                            break;
                        case 5:
                            HideAllScene();
                            SetTitle("Credit");
                            _credit.Show();
                            break;
                        case 6:
                            Exit();
                            break;
                        default:
                            SetTitle(_menu.SelectedItem.Component == null
                                ? string.Format("\"{0}\" cannot be opened.", _menu.SelectedItem.Name)
                                : _menu.SelectedItem.Name);
                            break;
                    }
                    PlayMenuSound(1);
                }

                if (inputs.Any(p=>p.ActionMenuDown))
                {
                    _menu.SelectedIndex = Math.Min(_menu.SelectedIndex + 1, _menu.Count - 1);
                    PlayMenuSound(0);
                }
                else if (inputs.Any(p=>p.ActionMenuUp))
                {
                    _menu.SelectedIndex = Math.Max(_menu.SelectedIndex - 1, 0);
                    PlayMenuSound(0);
                }
            }
        }

        private void GameTypeMenuInput(VirtualController[] inputs)
        {
            var menu = _gametypeMenu;
            GameOptions options = _menu.SelectedIndex == 0 ? GameOptions.None : GameOptions.Multiplayer;

            if (inputs.Any(c => c.ActionMenuDown))
            {
                menu.MenuIndex++;
                PlayMenuSound(0);
            }

            if (inputs.Any(c => c.ActionMenuUp))
            {
                menu.MenuIndex += 1 + menu.MenuItems.Count;
                PlayMenuSound(0);
            }

            if (inputs.Any(c => c.ActionBack))
            {
                menu.Hide();
                PlayMenuSound(0);
            }

            if (inputs.Any(c => c.ActionSelect))
            {
                menu.Hide();
                PlayMenuSound(1);
                HideAllScene();
                StartGame(options | menu.ActiveMenuItem.Component);
            }

            menu.MenuIndex %= menu.MenuItems.Count;
        }

        private void StartGame(GameOptions options)
        {
            SetTitle("Game thing");
            if (Gameplay != null)
            {
                Components.Remove(Gameplay);
                Gameplay.Dispose();
            }
            Gameplay = Campaign.New(options: options).NextLevel();
            Components.Add(Gameplay);
            Gameplay.Show();
        }

        public void PlayMenuSound(int i)
        {
            var menuSound = _menuEffects[i.Mid(_menuEffects.Length - 1)];
            menuSound.Play();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

        public bool AllowExit { get; set; }
    }
}