using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SoundScape.GameplaySceneComponents;
using XNALib.Scenes;
using GameOptions = SoundScape.GameplayScene.GameOptions;

namespace SoundScape.Levels
{
    class Campaign
    {
        private static GameLoop _game;
        private static List<GameplayScene> _gameplayScenes;
        private static Campaign _instance;
        private static int _currentLevel;
        private HighScore _scoreboard;
        private static GameOptions _options;

        private Campaign(GameLoop game)
        {
            _game = game;
            _scoreboard = _game.HighScore as HighScore;
#if DEBUG
            _gameplayScenes = new List<GameplayScene>()
            {   
                new Level1(game, game.SpriteBatch, _spectatorMode)
            };
#else
            _gameplayScenes = new List<GameplayScene>()
            {   
                new Level1(game, game.SpriteBatch, _options),
                new Level2(game, game.SpriteBatch, _options),
                new Level3(game, game.SpriteBatch, _options),
            };
#endif
        }

        public static int CurrentScore
        {
            get
            {
                return _gameplayScenes.Sum(gameplayScene => gameplayScene.Score);
            }
        }

        public static Campaign New(GameLoop game = null, GameOptions options = GameOptions.None)
        {
            _currentLevel = -1;
            _options = options;
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
            var nextLevel = _gameplayScenes[_currentLevel];
            nextLevel.Components.Where(c => c is Player).ForEach(p => ((Player) p).Visible = false );
            return nextLevel;
        }

        public static bool OnLastLevel
        {
            get { return _currentLevel + 1 >= _gameplayScenes.Count(); }
        }
    }
}
