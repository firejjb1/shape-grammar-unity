using UnityEngine;
using System.Collections.Generic;

namespace Generation
{
    [CreateAssetMenu(fileName = "Surround Operation", menuName = "ScriptableObjects/LevelGenerator/SurroundOperation", order = 1)]
    public class SurroundOperation : Operation
    {
        public bool horizontal = true;
        public bool vertical = false;

        public float horizontalRadius = 2;
        public float widthRadius = 1;
        public float verticalRadius = 1;

        public override bool Apply(Stack<Shape> stack, List<Shape> results)
        {
            var shapeList = new List<Shape> { InheritShape(predecessor) };
            if (predecessor.Size[0] != 0)
                horizontalRadius = predecessor.Size[0] / 2;
            if (predecessor.Size[1] != 0)
                verticalRadius = predecessor.Size[1] / 2;
            if (predecessor.Size[2] != 0)
                widthRadius = predecessor.Size[2] / 2;
            var offsets = new float[][] { new float[] { 0, -widthRadius, 90 },
                                        new float[] { -horizontalRadius, 0, 0 }, new float[] { -horizontalRadius-widthRadius, -horizontalRadius, 270 },};
            foreach (var offset in offsets)
            {
                var rightShape = InheritShape(predecessor);
                rightShape.Position.x += offset[0];
                rightShape.Position.z += offset[1];
                rightShape.Rotation.y += offset[2];
                shapeList.Add(rightShape);
            }

            shapeList.Reverse();
            foreach (var s in shapeList)
                stack.Push(s);

            return true;
        }
    }
}
