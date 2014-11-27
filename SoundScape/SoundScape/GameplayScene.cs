using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SoundScape.GameplaySceneComponents;
using XNALib.Scenes;
using XNALib.Menus;


namespace SoundScape
{
    public class GameplayScene : GameScene
    {
        private Dictionary<Entity, SoundEffect> _sfx;
        private Dictionary<Entity, Texture2D> _textures;
        private const int _wallThickness = 100;
        private int _score = 0;
        private DateTime _startTime;

        public enum Entity
        {
            Wall,
            Item,
            Player,
            Enemy
        }

        public GameplayScene(Game game, SpriteBatch sb)
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
            get { return _wallThickness; }
        }

        public int Score
        {
            get { return _score; }
            set { _score = value; }
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
