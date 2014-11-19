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

namespace SoundScape
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Particle : Microsoft.Xna.Framework.DrawableGameComponent
    {

        Random rand = new Random();

        Texture2D tex;
        Vector2 position;
        float scale;
        float scaleFactor;
        Color currentColor;
        SpriteBatch spriteBatch;
        Vector2 speed;

        List<Texture2D> texBatch = new List<Texture2D>();
        List<Color> ourColor = new List<Color>();

        Game game;
        Rectangle sourceRect = new Rectangle();
        private bool destroyMe;

        public bool DestroyMe
        {
            get { return destroyMe; }
            set { destroyMe = value; }
        }

        public Particle(Game game, SpriteBatch spriteBatch, Vector2 originEmision, float iniScale = 0, float scaleFactor = 0, Texture2D tex = null, 
            Vector2 speed = new Vector2())
            : base(game)
        {
            this.game = game;
            this.tex = tex;
            this.position = originEmision;
            scale = iniScale;
            this.scaleFactor = scaleFactor;

            if (scale == 0) { scale = floatRandomizer(10, 100); }
            if(scaleFactor == 0) {scaleFactor = floatRandomizer(5, 30);}

            if (this.speed == new Vector2())
            {
                this.speed.X = floatRandomizer(10, 100);
                this.speed.Y = floatRandomizer(10, 100);
            }
            this.spriteBatch = spriteBatch;
        }

        //all is div by 100 so: 10 = 0.1
        float floatRandomizer(int min, int max)
        {
            int temp = rand.Next(min, max);
            return temp / 100;
        }

        int intRandomizer(int min, int max)
        {
            return rand.Next(min, max);
        }

        protected override void LoadContent()
        {
            texBatch.Add(game.Content.Load<Texture2D>("images/part/ParticleCircle97Percent"));
            texBatch.Add(game.Content.Load<Texture2D>("images/part/ParticleCircleBorder"));
            texBatch.Add(game.Content.Load<Texture2D>("images/part/ParticleCircleBorder2CircleIn"));
            texBatch.Add(game.Content.Load<Texture2D>("images/part/ParticleCircleFilled"));
            texBatch.Add(game.Content.Load<Texture2D>("images/part/ParticleCircleThickBorder"));

            ourColor.Add(Color.Beige);
            ourColor.Add(Color.Coral);
            ourColor.Add(Color.Crimson);

            if (tex == null)
            {
                LoadContent();
                tex = texBatch[intRandomizer(0, texBatch.Count)];
                sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);
            }
            currentColor = ourColor[intRandomizer(0, ourColor.Count)];

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);

            position += speed;

            scale += scaleFactor;

            DestroyIfNotOnScreen();
        }

        void DestroyIfNotOnScreen()
        {
            if(Game1.stage.Contains(sourceRect))
            {
                destroyMe = true;
            }
        }



        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.Draw(tex, position, sourceRect, currentColor, 0, new Vector2(),
                scale, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
