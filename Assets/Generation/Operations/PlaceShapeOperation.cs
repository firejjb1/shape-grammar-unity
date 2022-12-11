using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Generation
{
    [CreateAssetMenu(fileName = "Place Shape Operation", menuName = "ScriptableObjects/LevelGenerator/PlaceShapeOperation", order = 1)]
    public class PlaceShapeOperation : Operation
    {
        [SerializeField]
        Shape ShapeToPlace;

        public Vector3 Position;

        // only 90 deg steps
        public Vector3 Rotation;

        public Vector3Int Scale = new Vector3Int(1, 1, 1);

        public override bool Apply(Stack<Shape> stack, List<Shape> results)
        {
            var ShapeToPlaceCopy = InheritShape(predecessor, ShapeToPlace);
         
            if (stack.Count > 0)
            {
                var stateShape = stack.Pop();
                ShapeToPlaceCopy = InheritShape(stateShape, ShapeToPlace);
            }
          
            results.Add(ShapeToPlaceCopy);

            return true;
        }
    }
}
