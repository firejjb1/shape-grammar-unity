using UnityEngine;
using System.Collections.Generic;

namespace Generation
{
    [CreateAssetMenu(fileName = "Shape", menuName = "ScriptableObjects/LevelGenerator/Shape", order = 1)]
    public class Shape : ScriptableObject
    {
        public enum SymbolEnum
        {
            AXIOM,
            CITY,
            MAIN_WATER,
            MAIN_SECTION,
            BRANCHING_WATER,
            BRANCHING_SECTION,
            BRIDGE,
            SIDEWALK,
            SIDE_SECTION,
            BUILDING,
            WING,
            STORIES,
            STORY,
            ROOF,
            WALL,
            PAVEMENT,
            NPC_CLIENT,
            CONNECTOR
        }

        public Vector3 Position;

        // only 90 deg steps
        public Vector3 Rotation;

        public Vector3 Size = new Vector3Int(1, 1, 1);

        public SymbolEnum Symbol;

        public bool IsAxiom;
        public bool IsTerminal;

        public Shape(Shape other)
        {
            Position = other.Position;
            Symbol = other.Symbol;
        }
    }
}