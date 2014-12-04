using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SoundScape.GameplaySceneComponents;
using XNALib.Scenes;


namespace SoundScape
{
    public class GameplayScene : GameScene
    {
        private Dictionary<Entity, SoundEffect> _sfx;
        private Dictionary<Entity, Texture2D> _textures;
        private HighScore _scoreboard;
        private const int WALL_THICKNESS = 100;
        private int _score = 0;
        private DateTime _startTime;
        private TimeSpan _runTime;
        private Texture2D _backgroundTexture;

        public override void Update(GameTime gameTime)
        {
            int playerCount = 0;
            int enemyCount = 0;

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
            Item,
            PlayerOne,
            PlayerTwo,
            EnemyBouncer,
            EnemyCircler,
        }

        public GameplayScene(GameLoop game, SpriteBatch sb)
            : base(game, sb)
        {
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
            get { return _backgroundTexture; }
            set { _backgroundTexture = value; }
        }

        public HighScore Scoreboard
        {
            get { return _scoreboard; }
            set { _scoreboard = value; }
        }

        protected override void LoadContent()
        {
            Console.WriteLine("{0} is being loaded.", this);
            var contentMgr = Game.Content;
            SFX = new Dictionary<Entity, SoundEffect>
            {
                {Entity.Wall, contentMgr.Load<SoundEffect>("sounds/click")},
                {Entity.Item, contentMgr.Load<SoundEffect>("sounds/ding")},
                {Entity.PlayerOne, contentMgr.Load<SoundEffect>("sounds/777__vitriolix__808-kick")},
                {Entity.PlayerTwo, contentMgr.Load<SoundEffect>("sounds/777__vitriolix__808-kick")},
                {Entity.EnemyBouncer, contentMgr.Load<SoundEffect>("sounds/406__tictacshutup__click-1-d")},
                {Entity.EnemyCircler, contentMgr.Load<SoundEffect>("sounds/406__tictacshutup__click-1-d")},
            };

            Textures = new Dictionary<Entity, Texture2D>
            {
                {Entity.Wall, contentMgr.Load<Texture2D>("images/gsc/wall")},
                {Entity.Item, contentMgr.Load<Texture2D>("images/gsc/item")},

                {Entity.PlayerOne, contentMgr.Load<Texture2D>("images/gsc/player")},
                {Entity.PlayerTwo, contentMgr.Load<Texture2D>("images/gsc/player2")},

                {Entity.EnemyBouncer, contentMgr.Load<Texture2D>("images/gsc/enemy")},
                {Entity.EnemyCircler, contentMgr.Load<Texture2D>("images/gsc/enemy2")},
            };
            Console.WriteLine("{0} finished loading.\n", this);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            if (_backgroundTexture != null)
            {   // TODO: Test this
                _spritebatch.Begin();
                _spritebatch.Draw(_backgroundTexture, Game.Window.ClientBounds, Color.White);
                _spritebatch.End();
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Play a sound to all players with a volume relative to a given position.
        /// </summary>
        /// <param name="sEffect">The soundeffect</param>
        /// <param name="sPosition">The position of the sound</param>
        /// <param name="sPitch">The pitch to play the sound at</param>
        public void PlayBounceSound(SoundEffect sEffect, Vector2 sPosition, float sPitch = 1f)
        {   return; // TODO: Temp disabled because I don't know if I still want it.
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
            Game.Speak("You have been defeated.");
            GameOver();           
        }

        private void Defeat()
        {
            Game.Speak("You are Victorious!");
            GameOver();
        }

        private void GameOver()
        {
            Enabled = false;
            Visible = true;
            // TODO: Call something on _scoreboard to let it know our score. 
        }
    }
}
