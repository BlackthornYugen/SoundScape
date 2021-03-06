﻿using System;
using System.Collections.Generic;
using System.Linq;
using SoundScape.GameplaySceneComponents;
using XNALib.Scenes;
using GameOptions = SoundScape.GameplayScene.GameOptions;

namespace SoundScape.Levels
{
    /// <summary>
    /// Campaign is a singleton pattern that makes sure 
    /// only one level is active at a time and handles 
    /// providing the levels to the GameLoop.
    /// </summary>
    class Campaign
    {
        private static GameLoop _game;
        private static List<GameplayScene> _gameplayScenes;
        private static Campaign _instance;
        private static int _currentLevel;
        private static GameOptions _options;

        private Campaign(GameLoop game)
        {
            _game = game;
#if DEBUG
            _gameplayScenes = new List<GameplayScene>()
            {   
                new LevelDebug(game, game.SpriteBatch, _options)
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
