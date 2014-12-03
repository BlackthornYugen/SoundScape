using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SoundScape.Levels
{
    public class LevelDataTransfer
    {
        /// <summary>
        /// Represents a Player/Enemy/Wall or any 
        /// other object to add to a level
        /// </summary>
        public struct LevelEntity
        {
            // The type of entity.
            public Type Type;
            // The speed of the entity
            public Vector2? Speed;
            // The colour to draw the entity with
            public Color? Colour;
            // If set; level will allways spawn 
            // this entity using this spawn location.
            public int? SpawnIndex;
        }

        /// <summary>
        /// Entities will be randomly assigned to a 
        /// spawn position unless SpawnIndex is set.
        /// </summary>
        public struct LevelStartPosition
        {
            // Where to align to.
            public Anchor Anchor;
            // Distance from anchor point
            public Vector2 OffsetPosition;
        }

        /// <summary>
        /// The constructor for the level data object
        /// </summary>
        public LevelDataTransfer()
        {
            Entities = new List<LevelEntity>();
            StartPositions = new List<LevelStartPosition>();
        }

        /// <summary>
        /// A list of entities to spawn in the level
        /// </summary>
        public List<LevelEntity> Entities { get; set; }

        /// <summary>
        /// A list of positions to spawn entities
        /// </summary>
        public List<LevelStartPosition> StartPositions { get; set; }

        /// <summary>
        /// Directions used to anchor spawn points. For example: 
        ///     North = Top Middle
        ///     North | West = Top Left
        /// </summary>
        [Flags]
        public enum Anchor : byte
        {
            North = 1,
            East = 2,
            South = 4,
            West = 8
        }
    }
}
