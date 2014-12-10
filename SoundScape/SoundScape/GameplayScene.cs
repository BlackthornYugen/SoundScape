using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SoundScape.GameplaySceneComponents;
using SoundScape.Levels;
using XNALib.Scenes;


namespace SoundScape
{
    /// <summary>
    /// Gameplay Scene abstract class inherits from 
    /// XNALIB.SCENES.GAMESCENE. It extends the 
    /// functionality of that class to handle things 
    /// common to all Levels. 
    /// </summary>
    public abstract class GameplayScene : GameScene
    {
        private Dictionary<Entity, SoundEffect> _sfx;
        private Dictionary<Entity, Texture2D> _textures;
        private const int WALL_THICKNESS = 100;
        private int _score;
        private DateTime _startTime;
        private DateTime _gameOverTime;
        private TimeSpan _runTime;
        private GameState _gameState;

        [Flags]
        public enum GameState
        {
            None = 0x0,
            Victory = 0x1,
            Defeat = 0x2,
            Gameover = 0x4,
            NewRecord = 0x8,
        }

        [Flags]
        public enum GameOptions
        {
            None = 0x0,
            SpectatorMode = 0x1,
            Multiplayer = 0x2,
        }

        public GameState State
        {
            get { return _gameState; }
        }

        public override void Update(GameTime gameTime)
        {
            int playerCount = 0;
            int enemyCount = 0;

            if (_gameState == GameState.None)
            {
                // Hide all non-walls
                if (!Options.HasFlag(GameOptions.SpectatorMode))
                    Components.Where(c => !(c is Wall)).ForEach(c =>
                    {
                        var gsc = c as GameplaySceneComponent;
                        if (gsc != null && gsc.Visible )
                        {
                            var colour = gsc.Colour;
                            if (colour.A != 0)
                            {
                                gsc.Colour = new Color(colour.R - 2, colour.G - 2, colour.B - 2, colour.A - 2);
                            }
                        }
                    });
            }
            else
            {
                if (DateTime.Now > _gameOverTime)
                {
                    GameOver();
                    return;
                }
                return;
            }

            foreach (var c in Components)
            {
                var sceneComponent = c as GameplaySceneComponent;
                if (sceneComponent == null || !sceneComponent.Enabled) continue;
                if (sceneComponent is Enemy)
                    enemyCount++;
                if (sceneComponent is Player)
                    playerCount++;
            }

            if (playerCount == 0)
                Defeat();
            else if (enemyCount == 0)
                Victory();

            base.Update(gameTime);
        }

        public new GameLoop Game
        {
            get { return base.Game as GameLoop; }
        }

        public enum Entity
        {
            Wall,
            Sonar,
            PlayerOne,
            PlayerTwo,
            EnemyBouncer,
            EnemyCircler,
        }

        public GameplayScene(GameLoop game, SpriteBatch sb, GameOptions options)
            : base(game, sb)
        {
            Options = options;
            _runTime = TimeSpan.Zero;
        }

        protected Dictionary<Entity, SoundEffect> SFX
        {
            get { return _sfx; }
            set { _sfx = value; }
        }

        protected Dictionary<Entity, Texture2D> Textures
        {
            get { return _textures; }
            set { _textures = value; }
        }

        protected int WallThickness
        {
            get { return WALL_THICKNESS; }
        }

        public int Score
        {
            get { return _score; }
            set
            {
                ((GameLoop)Game).SetTitle(
                    String.Format(
                        "Your {0} score is {1:n0} ({2} {3} Points!)",
                        this.ToString().Replace("SoundScape.Levels.", ""),
                        value,
                        value - _score < 0 ? "Lost" : "Earned",
                        Math.Abs(value - _score)));
                _score = value; 
            }
        }

        public int RunningSeconds
        {
            get { return (DateTime.Now - _startTime + _runTime).Seconds; }
        }

        public Texture2D BackgroundTexture
        {
            get { return _background; }
            set { _background = value; }
        }

        public GameOptions Options { get; set; }

        protected override void LoadContent()
        {
            Console.WriteLine("{0} is being loaded.", this);
            var contentMgr = Game.Content;
            SFX = new Dictionary<Entity, SoundEffect>
            {
                {Entity.Wall, contentMgr.Load<SoundEffect>("sounds/click")},
                {Entity.Sonar, contentMgr.Load<SoundEffect>("sounds/ding")},
                {Entity.PlayerOne, contentMgr.Load<SoundEffect>("sounds/777__vitriolix__808-kick")},
                {Entity.PlayerTwo, contentMgr.Load<SoundEffect>("sounds/777__vitriolix__808-kick")},
                {Entity.EnemyBouncer, contentMgr.Load<SoundEffect>("sounds/406__tictacshutup__click-1-d")},
                {Entity.EnemyCircler, contentMgr.Load<SoundEffect>("sounds/406__tictacshutup__click-1-d")},
            };

            Textures = new Dictionary<Entity, Texture2D>
            {
                {Entity.Wall, contentMgr.Load<Texture2D>("images/gsc/wall")},
                {Entity.Sonar, contentMgr.Load<Texture2D>("images/gsc/sonar")},

                {Entity.PlayerOne, contentMgr.Load<Texture2D>("images/gsc/player")},
                {Entity.PlayerTwo, contentMgr.Load<Texture2D>("images/gsc/player2")},

                {Entity.EnemyBouncer, contentMgr.Load<Texture2D>("images/gsc/enemy")},
                {Entity.EnemyCircler, contentMgr.Load<Texture2D>("images/gsc/enemy2")},
            };

            // Default background texture
            _background = _background ?? contentMgr.Load<Texture2D>("images/back/deep");
            Console.WriteLine("{0} finished loading.\n", this);
            base.LoadContent();
        }

        /// <summary>
        /// Play a sound to all players with a volume relative to a given position.
        /// </summary>
        /// <param name="sEffect">The soundeffect</param>
        /// <param name="sPosition">The position of the sound</param>
        /// <param name="sPitch">The pitch to play the sound at</param>
        public void PlayBounceSound(SoundEffect sEffect, Vector2 sPosition, float sPitch = 1f)
        {   //return; // TODO: Temp disabled because I don't know if I still want it.
            foreach (IGameComponent component in Components)
            {
                if (component is Player)
                {
                    var cb = Game.Window.ClientBounds;
                    Player player = component as Player;
                    var distance = sPosition - player.Position;
                    sEffect.Play(
                        volume: Math.Max(1 - distance.Length() / new Vector2(cb.Width, cb.Height).Length(), 1), 
                        pitch: sPitch, 
                        pan: player.Pan);
                }
            }
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (Enabled)
            {
                _startTime = DateTime.Now;
            }
            else
            {
                _runTime = DateTime.Now - _startTime;
            }
            base.OnEnabledChanged(sender, args);
        }

        private void Victory()
        {
            Components.Where(c => c is Player).ForEach(c =>
            {
                var gsc = c as GameplaySceneComponent;
                if (gsc != null)
                {
                    gsc.ResetColour();
                }
            });
            Game.Speak("You are Victorious!");
            _gameOverTime = DateTime.Now + TimeSpan.FromSeconds(5);
            _gameState |= GameState.Victory;

            if (Game.HighScore.IsHighScore(Campaign.CurrentScore) && !Options.HasFlag(GameOptions.SpectatorMode))
                _gameState |= GameState.NewRecord;
        }

        private void Defeat()
        {
            Components.Where(c => c is Player).ForEach(c =>
            {
                var gsc = c as GameplaySceneComponent;
                if (gsc != null)
                {
                    gsc.ResetColour();
                }
            });
            Game.Speak("You have been defeated.");
            _gameOverTime = DateTime.Now + TimeSpan.FromSeconds(5);
            _gameState |= GameState.Defeat;

            if (Game.HighScore.IsHighScore(Campaign.CurrentScore) && !Options.HasFlag(GameOptions.SpectatorMode))
                _gameState |= GameState.NewRecord;
        }

        private void GameOver()
        {
            _gameState |= GameState.Gameover;

            Enabled = false; // Pause game since it's over.

            if (_gameState.HasFlag(GameState.Defeat))
            {
                if (_gameState.HasFlag(GameState.NewRecord))
                {   // We died before finishing but we have a high score
                    NewHighscore();
                }
                return;
            }
            
            if (_gameState.HasFlag(GameState.Victory))
            {
                if (Campaign.OnLastLevel)
                {
                    // We are on the last level
                    if (_gameState.HasFlag(GameState.NewRecord))
                    {
                        // And we got a highscore! Go us!
                        NewHighscore();
                    }
                }
                else
                {   // Onwards to the Next level
                    GameScene nextLevelScene = Campaign.Instance().NextLevel();
                    Hide();
                    Components.Remove(this);
                    Game.Components.Add(nextLevelScene);
                    nextLevelScene.Show();
                }
                return;
            }
            
            // We didn't get a highscore. Lets see who did...
            Hide();
            Game.HighScore.Show();
        }

        private void NewHighscore()
        {
            Hide();
            Game.NewHighScore.Show();
        }

        /// <summary>
        /// This draw is only used in the last moments of a game
        /// after victory or defeat.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (_gameState != GameState.None)
            {
                const int scale = 1;
                const int margin = 8;
                bool lastLevel = Campaign.OnLastLevel;
                var score = (string) Campaign.CurrentScore.ToString("n0");
                var cb = (Rectangle) Game.Window.ClientBounds;
                var font = (SpriteFont) Game.BigFont;
                var position = new Vector2(cb.Width / 2f, cb.Height / 2f);
                string[] messageStrings = new[] { null, score };

                if (_gameState.HasFlag(GameState.Defeat))
                    messageStrings[0] = _gameState.HasFlag(GameState.NewRecord) ? "New Record!" : "Maybe next time...";
                else if (lastLevel && _gameState.HasFlag(GameState.Victory))
                    messageStrings[0] = _gameState.HasFlag(GameState.NewRecord) ? "New Record!" : "You win!";
                else
                    messageStrings = new[]
                    {
                        "Get ready!", "Next level in...",
                        String.Format("{0}...", (_gameOverTime - DateTime.Now).Seconds + 1)
                    };

                position -= (Vector2.UnitY * (font.LineSpacing * scale + margin)) / 2;
                _spritebatch.Begin();
                foreach (string s in messageStrings)
                {
                    _spritebatch.DrawString(font, s, position - Vector2.One * 10, Color.Black, 0, font.MeasureString(s) / 2, scale, SpriteEffects.None, 0);
                    _spritebatch.DrawString(font, s, position - Vector2.One * 0, Color.White, 0, font.MeasureString(s) / 2, scale, SpriteEffects.None, 0);
                    position += Vector2.UnitY * (font.LineSpacing * scale + margin);
                }
                _spritebatch.End();
            }
        }
    }
}