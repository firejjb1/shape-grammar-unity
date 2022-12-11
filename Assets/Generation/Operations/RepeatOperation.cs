using UnityEngine;
using System.Collections.Generic;

namespace Generation
{
    [CreateAssetMenu(fileName = "Repeat Operation", menuName = "ScriptableObjects/LevelGenerator/RepeatOperation", order = 1)]
    public class RepeatOperation : Operation
    {
        [SerializeField]
        Shape ShapeToPlace;


        [SerializeField]
        Shape ShapeAtEnd;

        [SerializeField]
        int axis = 0;

        [SerializeField]
        bool hasRandom = false;

        [SerializeField]
        int minRepeat = 1;

        public override bool Apply(Stack<Shape> stack, List<Shape> results)
        {
            var projectedShape = predecessor;
            if (stack.Count > 0)
            {
                var stateShape = stack.Pop();
                projectedShape = InheritShape(stateShape, ShapeToPlace);
            }
            int repeat = (int)(projectedShape.Size[axis] / ShapeToPlace.Size[axis]);
            if (hasRandom)
                repeat = Random.Range(minRepeat, repeat);
           // Debug.Log(repeat);
            for (int i = 0; i < repeat; ++i)
            {
                var rightShape = InheritShape(predecessor, ShapeToPlace);
                rightShape.Position[axis] += i * ShapeToPlace.Size[axis];
                // Debug.Log(rightShape.Position);
                results.Add(rightShape);
            }

            var finalShape = InheritShape(predecessor, ShapeAtEnd);
            finalShape.Position[axis] += repeat * ShapeToPlace.Size[axis];
            // Debug.Log(rightShape.Position);
            results.Add(finalShape);

            return true;
        }
    }
}
