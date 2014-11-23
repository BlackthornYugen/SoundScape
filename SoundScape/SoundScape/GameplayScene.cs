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
        private int _wallThickness = 100;
        enum Entity
        {
            Wall,
            Item,
            Player,
            Enemy
        }

        public GameplayScene(Game game, SpriteBatch sb)
            : base(game, sb)
        {
            LoadContent();
        }

        protected override void LoadContent()
        {
            var cb = Game.Window.ClientBounds;
            var contentMgr = Game.Content;

            _sfx = new Dictionary<Entity, SoundEffect>
            {
                {Entity.Wall, contentMgr.Load<SoundEffect>("sounds/click")},
                {Entity.Item, contentMgr.Load<SoundEffect>("sounds/ding")},
                {Entity.Player, contentMgr.Load<SoundEffect>("sounds/777__vitriolix__808-kick")},
                {Entity.Enemy, contentMgr.Load<SoundEffect>("sounds/click")}
            };

            _textures = new Dictionary<Entity, Texture2D>
            {
                {Entity.Wall, contentMgr.Load<Texture2D>("images/gsc/wall")},
                {Entity.Item, contentMgr.Load<Texture2D>("images/gsc/item")},
                {Entity.Player, contentMgr.Load<Texture2D>("images/gsc/player")},
                {Entity.Enemy, contentMgr.Load<Texture2D>("images/gsc/enemy")}
            };
            Player player;
            Color[] colours = new Color[]
            {
                Color.Red
                //,Color.Blue
                //,Color.Green
                //,Color.Yellow
            };

            var gamepads = new PlayerIndex[]
            {
                PlayerIndex.One,
                PlayerIndex.Two,
                PlayerIndex.Three,
                PlayerIndex.Four
            };

            var r = new Random();
            var xLimit = cb.Width - _textures[Entity.Player].Width - _wallThickness;
            var yLimit = cb.Height - _textures[Entity.Player].Height - _wallThickness;
            for (int i = 0; i < colours.Length; i++)
            {

                int x = r.Next(xLimit) + _wallThickness;
                int y = r.Next(yLimit) + _wallThickness;
                player = new Player(this, spritebatch, new Vector2(x, y), _textures[Entity.Player], _sfx[Entity.Player],
                    i % 2 == 0 ? 1f : -1f, colours[i%colours.Length]);
                Components.Add(player);

                player.ControllerIndex = (PlayerIndex)i;
            }
            Wall wall;

            // North Wall
            wall = new Wall(
                scene: this,
                spriteBatch: spritebatch,
                texture: _textures[Entity.Wall],
                soundEffect: _sfx[Entity.Wall],
                hitbox: new Rectangle(0, 0, cb.Width, 100));
            Components.Add(wall);

            // West Wall
            wall = new Wall(
                scene: this,
                spriteBatch: spritebatch,
                texture: _textures[Entity.Wall],
                soundEffect: _sfx[Entity.Wall],
                hitbox: new Rectangle(0, 0, _wallThickness, cb.Height));
            Components.Add(wall);

            // East Wall
            wall = new Wall(
                 scene: this,
                 spriteBatch: spritebatch,
                 texture: _textures[Entity.Wall],
                 soundEffect: _sfx[Entity.Wall],
                 hitbox: new Rectangle(cb.Width - _wallThickness, 0, _wallThickness, cb.Height));
            Components.Add(wall);

            // South Wall
            wall = new Wall(
                 scene: this,
                 spriteBatch: spritebatch,
                 texture: _textures[Entity.Wall],
                 soundEffect: _sfx[Entity.Wall],
                 hitbox: new Rectangle(0, cb.Height - _wallThickness, cb.Width, _wallThickness));
            Components.Add(wall);

            Enemy enemy = new Bouncer(
                scene: this, 
                spriteBatch: spritebatch, 
                position: Vector2.One * 250, 
                texture: _textures[Entity.Enemy], 
                soundEffect: _sfx[Entity.Enemy], 
                colour: Color.Green);
            enemy.Speed = Vector2.One;
            Components.Add(enemy);
            base.LoadContent();
        }
    }
}
