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
        private Dictionary<Entity, SoundEffect> sfx;
        private Dictionary<Entity, Texture2D> textures;

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
            var contentMgr = Game.Content;

            sfx = new Dictionary<Entity, SoundEffect>
            {
                {Entity.Wall, contentMgr.Load<SoundEffect>("sounds/click")},
                {Entity.Item, contentMgr.Load<SoundEffect>("sounds/ding")},
                {Entity.Player, contentMgr.Load<SoundEffect>("sounds/777__vitriolix__808-kick")},
                {Entity.Enemy, contentMgr.Load<SoundEffect>("sounds/click")}
            };

            textures = new Dictionary<Entity, Texture2D>
            {
                {Entity.Wall, contentMgr.Load<Texture2D>("images/wall")},
                {Entity.Item, contentMgr.Load<Texture2D>("images/item")},
                {Entity.Player, contentMgr.Load<Texture2D>("images/player")},
                {Entity.Enemy, contentMgr.Load<Texture2D>("images/enemy")}
            };
            Player player;
            Color[] colours = new Color[]
            {
                Color.Red,
                Color.Blue
            };

            var gamepads = new PlayerIndex[]
            {
                PlayerIndex.One,
                PlayerIndex.Two,
                PlayerIndex.Three,
                PlayerIndex.Four
            };

            var r = new Random();
            var xLimit = Game.Window.ClientBounds.Width - textures[Entity.Player].Width;
            var yLimit = Game.Window.ClientBounds.Height - textures[Entity.Player].Height;
            for (int i = 0; i < colours.Length; i++)
            {

                int x = r.Next(xLimit);
                int y = r.Next(yLimit);
                player = new Player(this, spritebatch, new Vector2(x, y), textures[Entity.Player], sfx[Entity.Player],
                    i % 2 == 0 ? 1f : -1f, colours[i%colours.Length]);
                Components.Add(player);

                player.ControllerIndex = (PlayerIndex)i;
            }

            Enemy enemy = new Enemy(this, spritebatch, Vector2.Zero, textures[Entity.Enemy], sfx[Entity.Enemy], Color.Green);
            enemy.Speed = Vector2.One * 3;
            Components.Add(enemy);
            base.LoadContent();
        }
    }
}
