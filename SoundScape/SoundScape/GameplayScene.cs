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
        private const int WALL_THICKNESS = 100;
        private int _score = 0;
        private DateTime _startTime;
        private int _livingPlayers;
        private int _livingEnemies;

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

            _livingPlayers = playerCount;
            _livingEnemies = enemyCount;
            // TODO: Make victory/defeat code better (maybe raise an event?)
            if (playerCount == 0)
            {
                Game.Speak("You have been defeated.");
                Enabled = false;
                Visible = true;
            }
            else if (enemyCount == 0)
            {
                Game.Speak("You are Victorious!");
                Enabled = false;
                Visible = true;
            }
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
            Player,
            Enemy
        }

        public GameplayScene(GameLoop game, SpriteBatch sb)
            : base(game, sb)
        {
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
            get { return (DateTime.Now - _startTime).Seconds; }
        }

        protected override void LoadContent()
        {
            Console.WriteLine("{0} is being loaded.", this);
            var contentMgr = Game.Content;
            SFX = new Dictionary<Entity, SoundEffect>
            {
                {Entity.Wall, contentMgr.Load<SoundEffect>("sounds/click")},
                {Entity.Item, contentMgr.Load<SoundEffect>("sounds/ding")},
                {Entity.Player, contentMgr.Load<SoundEffect>("sounds/777__vitriolix__808-kick")},
                {Entity.Enemy, contentMgr.Load<SoundEffect>("sounds/406__tictacshutup__click-1-d")}
            };

            Textures = new Dictionary<Entity, Texture2D>
            {
                {Entity.Wall, contentMgr.Load<Texture2D>("images/gsc/wall")},
                {Entity.Item, contentMgr.Load<Texture2D>("images/gsc/item")},
                {Entity.Player, contentMgr.Load<Texture2D>("images/gsc/player")},
                {Entity.Enemy, contentMgr.Load<Texture2D>("images/gsc/enemy")}
            };
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
        {
            foreach (IGameComponent component in Components)
            {
                if (component is Player)
                {
                    var cb = Game.Window.ClientBounds;
                    Player player = component as Player;
                    var distance = sPosition - player.Position;
                    sEffect.Play(
                        volume: 1 - distance.Length() / new Vector2(cb.Width, cb.Height).Length(), 
                        pitch: sPitch, 
                        pan: player.Pan);
                }
            }
        }

        protected override void OnVisibleChanged(object sender, EventArgs args)
        {
            if (Visible && DateTime.MinValue == _startTime)
            {
                _startTime = DateTime.Now;
            }
            base.OnVisibleChanged(sender, args);
        }
    }
}
