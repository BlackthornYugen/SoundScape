using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNALib.Scenes;

namespace SoundScape.Levels
{
    class Campaign
    {
        private static GameLoop _game;
        private static List<GameplayScene> _gameplayScenes;
        private static Campaign _instance;
        private static int _currentLevel = -1;
        private HighScore _scoreboard;

        protected Campaign(GameLoop game)
        {
            _game = game;
            _scoreboard = _game.HighScore as HighScore;
            _gameplayScenes = new List<GameplayScene>()
            {   // TODO: Find a better way to ref scoreboard or update constructor
                new Level1(game, game.SpriteBatch) {Scoreboard = _scoreboard},
                new Level2(game, game.SpriteBatch) {Scoreboard = _scoreboard},
                new Level3(game, game.SpriteBatch) {Scoreboard = _scoreboard},
            };
        }

        public static int CurrentScore
        {
            get
            {
                return _gameplayScenes.Sum(gameplayScene => gameplayScene.Score);
            }
        }

        public static Campaign New(GameLoop game = null)
        {
            return _instance ?? (_instance = new Campaign(game));
        }

        public static Campaign Instance()
        {
            return _instance;
        }

        public GameScene NextLevel()
        {
            if (_instance == null)
                throw new Exception("No campaign started!");
            if (++_currentLevel >= _gameplayScenes.Count)
            {   // There are no more levels. Restart campaign.
                _instance = new Campaign(_game);
                _currentLevel = 0;
            }
            return _gameplayScenes[_currentLevel];
        }
    }
}
