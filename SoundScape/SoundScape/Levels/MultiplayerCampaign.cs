using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNALib.Scenes;

namespace SoundScape.Levels
{
    class MultiplayerCampaign
    {
        private static GameLoop _game;
        private static List<GameplayScene> _gameplayScenes;
        private static MultiplayerCampaign _instance;
        private static int _currentLevel = -1;

        protected MultiplayerCampaign(GameLoop game)
        {
            _game = game;
            _gameplayScenes = new List<GameplayScene>()
            {
                new MultiplayerLevel1(game, game.SpriteBatch),
                new MultiplayerLevel2(game, game.SpriteBatch),
                new MultiplayerLevel3(game, game.SpriteBatch)
            };
        }

        public static int CurrentScore
        {
            get
            {
                return _gameplayScenes.Sum(gameplayScene => gameplayScene.Score);
            }
        }

        public static MultiplayerCampaign NewCampaign(GameLoop game)
        {
            if (_instance == null)
            {
                _instance = new MultiplayerCampaign(game);
            }

            return _instance;
        }

        public static GameScene NextLevel()
        {
            if (_instance == null)
                throw new Exception("No campaign started!");
            if (++_currentLevel >= _gameplayScenes.Count)
            {   // There are no more levels. Restart campaign.
                _instance = new MultiplayerCampaign(_game);
                _currentLevel = 0;
            }

            return _gameplayScenes[_currentLevel];
        }
    }
}
