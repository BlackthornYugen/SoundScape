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

        public Keys[] MenuLeftKeys { get; set; }

        public Buttons[] MenuLeftButtons { get; set; }
        public bool ActionMenuLeft
        {
            get
            {
                return KeyPressed(MenuLeftKeys) || ButtonPressed(MenuLeftButtons);
            }
        }

        public Keys[] MenuRightKeys { get; set; }

        public Buttons[] MenuRightButtons { get; set; }
        public bool ActionMenuRight
        {
            get
            {
                return KeyPressed(MenuRightKeys) || ButtonPressed(MenuRightButtons);
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

        public Keys[] GameFireKeys { get; set; }
        public Buttons[] GameFireButtons { get; set; }
        public bool ActionFire
        {
            get
            {
                if (Math.Abs(Math.Abs(AimAxisX) + Math.Abs(AimAxisY)) < 0.1f) return false;
                return KeyPressed(GameFireKeys) || ButtonPressed(GameFireButtons);
            }
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

        public float MovementAxisX
        {
            get
            {
                try
                {
                    if (KeyDown(MovementLeftKeys))
                        return -1;

                    if (KeyDown(MovementRightKeys))
                        return 1;

                    return PadState.ThumbSticks.Left.X;
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
        public float MovementAxisY
        {
            get
            {
                try
                {
                    if (KeyDown(MovementUpKeys))
                        return 1;

                    if (KeyDown(MovementDownKeys))
                        return -1;

                    return PadState.ThumbSticks.Left.Y;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return 0;
                }
            }
        }

        public Keys[] AimUpKeys { get; set; }
        public Keys[] AimDownKeys { get; set; }
        public float AimAxisY
        {
            get
            {
                try
                {
                    float factor = KeyDown(AimLeftKeys) || KeyDown(AimRightKeys) ? 0.7f : 1f;
                    if (KeyDown(AimUpKeys))
                        return factor;

                    if (KeyDown(AimDownKeys))
                        return -factor;

                    return PadState.ThumbSticks.Right.Y;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return 0;
                }
            }
        }

        public Keys[] AimLeftKeys { get; set; }
        public Keys[] AimRightKeys { get; set; }
        public float AimAxisX
        {
            get
            {
                try
                {
                    float factor = KeyDown(AimUpKeys) || KeyDown(AimDownKeys) ? 0.55f : 1f;
                    if (KeyDown(AimLeftKeys))
                        return -factor;

                    if (KeyDown(AimRightKeys))
                        return factor;

                    return PadState.ThumbSticks.Right.X;
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
