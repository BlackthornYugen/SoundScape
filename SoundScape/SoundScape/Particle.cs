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
using XNALib.Scenes;

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

        List<Texture2D> texBatch;
        List<Color> ourColor = new List<Color>();

        Rectangle sourceRect = new Rectangle();
        private bool destroyMe;
        private bool isOnScreen;

        private Rectangle bounds =new Rectangle();

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        public bool DestroyMe
        {
            get { return destroyMe; }
            set { destroyMe = value; }
        }

        public Particle(GameScene gameScene, SpriteBatch spriteBatch, Vector2 originEmision, List<Texture2D> texBatch, float iniScale = 0, float scaleFactor = 0,  
            Vector2 speed = new Vector2())
            : base(gameScene.Game)
        {
            
            this.texBatch = texBatch;
            this.position = originEmision;
            scale = iniScale;
            this.scaleFactor = scaleFactor;

            if (scale == 0) { scale = floatRandomizer(5, 50); }
            if(scaleFactor == 0) {scaleFactor = floatRandomizer(1, 5);}

            if (this.speed == new Vector2())
            {
                this.speed.X = floatRandomizer(10, 100);
                this.speed.Y = floatRandomizer(10, 100);
            }
            this.spriteBatch = spriteBatch;

            tex = texBatch[intRandomizer(0, texBatch.Count)];
            sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);

            LoadContent();
        }

        //all is div by 100 so: 10 = 0.1
        float floatRandomizer(int min, int max)
        {
            float temp = rand.Next(min, max);
            return temp / 100;
        }

        int intRandomizer(int min, int max)
        {
            return rand.Next(min, max);
        }

        protected override void LoadContent()
        {


            ourColor.Add(Color.Beige);
            ourColor.Add(Color.Coral);
            ourColor.Add(Color.Crimson);
            ourColor.Add(Color.Red);
            ourColor.Add(Color.Blue);
            ourColor.Add(Color.Green);
            ourColor.Add(Color.Yellow);
            ourColor.Add(Color.Pink);
            ourColor.Add(Color.Wheat);



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

            UpdateBounds();
            UpdateStatus();
        }

        void UpdateBounds()
        {
            bounds.X = (int)position.X;
            bounds.Y = (int)position.Y;
        }

        void UpdateStatus()
        {
            if (Game1.stage.Contains(bounds))
            {
                isOnScreen = true;
            }
            else if (!Game1.stage.Contains(bounds) && isOnScreen || position.Y < Game1.stage.Height) 
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
