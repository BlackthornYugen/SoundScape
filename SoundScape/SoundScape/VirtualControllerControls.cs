using System;
using Microsoft.Xna.Framework.Input;

namespace SoundScape
{

    public partial class VirtualController
    {
        public Keys[] MenuUpKeys { get; set; }
        public Buttons[] MenuUpButtons { get; set; }
        public bool ActionMenuUp
        {
            get
            {
                return KeyPressed(MenuUpKeys) || ButtonPressed(MenuUpButtons);
            }
        }

        public Keys[] MenuDownKeys { get; set; }
        public Buttons[] MenuDownButtons { get; set; }
        public bool ActionMenuDown
        {
            get
            {
                return KeyPressed(MenuDownKeys) || ButtonPressed(MenuDownButtons);
            }
        }

        public Keys[] MenuSelectKeys { get; set; }
        public Buttons[] MenuSelectButtons { get; set; }
        public bool ActionSelect
        {
            get
            {
                return KeyPressed(MenuSelectKeys) || ButtonPressed(MenuSelectButtons);
            }
        }
        public bool ActionFire
        {
            get { return ActionSelect; }
        }

        public Keys[] MenuBackKeys { get; set; }
        public Buttons[] MenuBackButtons { get; set; }
        public bool ActionBack
        {
            get
            {
                return KeyPressed(MenuBackKeys) || ButtonPressed(MenuBackButtons);
            }
        }


        public Keys[] MovementRightKeys { get; set; }
        public Keys[] MovementLeftKeys { get; set; }

        public float AxisX
        {
            get
            {
                try
                {
                    if (KeyDown(MovementLeftKeys))
                        return -1;

                    if (KeyDown(MovementRightKeys))
                        return 1;

                    return _padState.ThumbSticks.Left.X;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return 0;
                }
            }
        }

        public Keys[] MovementUpKeys { get; set; }
        public Keys[] MovementDownKeys { get; set; }
        public float AxisY
        {
            get
            {
                try
                {
                    if (KeyDown(MovementUpKeys))
                        return 1;

                    if (KeyDown(MovementDownKeys))
                        return -1;

                    return _padState.ThumbSticks.Left.Y;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return 0;
                }
            }
        }
    }
}
