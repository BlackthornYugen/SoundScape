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
            Player
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
                {Entity.Player, contentMgr.Load<SoundEffect>("sounds/777__vitriolix__808-kick")}
            };

            textures = new Dictionary<Entity, Texture2D>
            {
                {Entity.Wall, contentMgr.Load<Texture2D>("images/wall")},
                {Entity.Item, contentMgr.Load<Texture2D>("images/item")},
                {Entity.Player, contentMgr.Load<Texture2D>("images/player")}
            };
            Player player;
            Color[] colours = new Color[]
            {
                Color.Pink,
                Color.Red,
                Color.Green,
                Color.Blue
            };

            var gamepads = new PlayerIndex[]
            {
                PlayerIndex.One,
                PlayerIndex.Two,
                PlayerIndex.Three,
                PlayerIndex.Four
            };


            for (int i = 0; i < colours.Length; i++)
            {
                int x = 175 + i*25;
                int y = 175 + (i*50%150);
                player = new Player(Game, spritebatch, new Vector2(x, y), textures[Entity.Player], sfx[Entity.Player],
                    textures[Entity.Player].Bounds, colours[i%colours.Length]);
                Compontents.Add(player);

                player.ControllerIndex = gamepads[i];
            }

            base.LoadContent();
        }
    }
}
