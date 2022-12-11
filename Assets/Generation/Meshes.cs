using UnityEngine;
using System.Collections.Generic;

namespace Generation
{
    [CreateAssetMenu(fileName = "Meshes", menuName = "ScriptableObjects/LevelGenerator/Meshes", order = 1)]
    public class Meshes : ScriptableObject
    {

        public Shape.SymbolEnum Symbol;

        public List<GameObject> meshObjects;
    }
}