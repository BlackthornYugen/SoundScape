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
            var scoreboard = _game.HighScore as HighScore;
            _game = game;
            _gameplayScenes = new List<GameplayScene>()
            {   // TODO: Find a better way to ref scoreboard or update constructor
                new MultiplayerLevel1(game, game.SpriteBatch) {Scoreboard = scoreboard},
                new MultiplayerLevel2(game, game.SpriteBatch) {Scoreboard = scoreboard},
                new MultiplayerLevel3(game, game.SpriteBatch) {Scoreboard = scoreboard},
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
            return _instance ?? (_instance = new MultiplayerCampaign(game));
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
