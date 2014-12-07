using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNALib.Scenes;

namespace SoundScape.Levels
{
    class Campaign
    {
        private static GameLoop _game;
        private static List<GameplayScene> _gameplayScenes;
        private static Campaign _instance;
        private static int _currentLevel;
        private HighScore _scoreboard;

        private Campaign(GameLoop game)
        {
            _game = game;
            _scoreboard = _game.HighScore as HighScore;
            _gameplayScenes = new List<GameplayScene>()
            {   // TODO: Find a better way to ref scoreboard or update constructor
                new Level1(game, game.SpriteBatch),
#if !DEBUG
                new Level2(game, game.SpriteBatch),
                new Level3(game, game.SpriteBatch),
#endif
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
            _currentLevel = -1;
            return _instance = new Campaign(game ?? _game);
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

        public bool OnLastLevel
        {
            get { return _currentLevel + 1 >= _gameplayScenes.Count(); }
        }
    }
}
