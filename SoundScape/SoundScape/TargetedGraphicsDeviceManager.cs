using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SoundScape
{
    public class TargetedGraphicsDeviceManager : GraphicsDeviceManager
    {   // http://www.carbonatethis.com/xna-multi-monitor-support/
        private string _target;

        public TargetedGraphicsDeviceManager(Game game, string displayTarget) : base(game)
        {
            _target = displayTarget;
        }

        protected override void OnPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs args)
        {
            args.GraphicsDeviceInformation.PresentationParameters.IsFullScreen = false;

            base.OnPreparingDeviceSettings(sender, args);
        }

        protected override void RankDevices(List<GraphicsDeviceInformation> foundDevices)
        {
            List<GraphicsDeviceInformation> removals = new List<GraphicsDeviceInformation>();
            for (int i = 0; i < foundDevices.Count; i++)
            {
                if (!foundDevices[i].Adapter.DeviceName.Contains("DISPLAY" + _target) 
                    && removals.Count + 1 < foundDevices.Count)
                    removals.Add(foundDevices[i]);
            }
            foreach (GraphicsDeviceInformation info in removals)
            {
                foundDevices.Remove(info);
            }

            base.RankDevices(foundDevices);
        }
    }
}
