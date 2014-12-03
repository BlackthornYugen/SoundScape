using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SoundScape.Levels
{


    public class LevelDataTransfer
    {
        public struct LevelEntity
        {
            public Type Type;
            public Vector2 Speed;
        }

        public struct LevelStartPosition
        {
            public Anchor Anchor;
            public Vector2 OffsetPosition;
        }

        private List<LevelEntity> _entities;
        private List<LevelStartPosition> _startPositions;

        public LevelDataTransfer()
        {
            Entities = new List<LevelEntity>();
            StartPositions = new List<LevelStartPosition>();
        }

        public List<LevelEntity> Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public List<LevelStartPosition> StartPositions
        {
            get { return _startPositions; }
            set { _startPositions = value; }
        }

        [Flags]
        public enum Anchor : byte
        {
            North = 1,
            East = 2,
            South = 4,
            West = 8,
            None = 16
        }
    }
}
