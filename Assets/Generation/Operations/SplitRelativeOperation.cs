using UnityEngine;
using System;
using System.Collections.Generic;

namespace Generation
{
    [CreateAssetMenu(fileName = "Split Relative Operation", menuName = "ScriptableObjects/LevelGenerator/SplitRelativeOperation", order = 1)]
    public class SplitRelativeOperation : Operation
    {
        [Serializable]
        public struct SplitData
        {
            public Vector3 location;
            public bool isRelative;
        }
        public Vector3 SplitRange = new Vector3 (0, 0, 0);
        public List<SplitData> SplitLoc = new List<SplitData>();
        public Vector3 RandomRange = new Vector3();
        public override bool Apply(Stack<Shape> stack, List<Shape> results)
        {
            if (predecessor.Size.sqrMagnitude > 0)
                SplitRange = predecessor.Size;
            var shapeList = new List<Shape> { InheritShape(predecessor) };

            Vector3 nonRelativeSize = Vector3.zero;
            Vector3 relativeTotal = Vector3.zero;
            foreach (var split in SplitLoc)
            {
                if (!split.isRelative)
                {
                    nonRelativeSize += split.location;
                }
                else
                {
                    relativeTotal += split.location;
                }
            }
            Vector3 relativeSize = SplitRange - nonRelativeSize;
            //Debug.Log(relativeTotal);
            Vector3 unitRelativeSize = new Vector3(relativeSize.x / Mathf.Max(relativeTotal.x, 1), 
                relativeSize.y / Mathf.Max(relativeTotal.y, 1),
                relativeSize.z / Mathf.Max(relativeTotal.z, 1));
            // Debug.Log(unitRelativeSize);
            Vector3 currentOffset = Vector3.zero;
            int i = 1;
            foreach (var split in SplitLoc)
            {
                var rightShape = InheritShape(predecessor);

                var splitLoc = split.location;
                bool isRelative = split.isRelative;
                if (isRelative)
                {
                    splitLoc = new Vector3(splitLoc.x * unitRelativeSize.x, 
                        splitLoc.y * unitRelativeSize.y, 
                        splitLoc.z * unitRelativeSize.z);
                }
                currentOffset += splitLoc;
                rightShape.Position.x += RandomRange.x > 0 ? 
                    UnityEngine.Random.Range(currentOffset.x - RandomRange.x, currentOffset.x + RandomRange.x) : currentOffset.x;
                rightShape.Position.y += RandomRange.y > 0 ? 
                    UnityEngine.Random.Range(currentOffset.y - RandomRange.y, currentOffset.y + RandomRange.y) : currentOffset.y;
                rightShape.Position.z += RandomRange.z > 0 ? 
                    UnityEngine.Random.Range(currentOffset.z - RandomRange.z, currentOffset.z + RandomRange.z) : currentOffset.z;
                shapeList[i-1].Size = splitLoc;
                shapeList.Add(rightShape);
                ++i;
            }

            shapeList.Reverse();
            foreach (var s in shapeList)
                stack.Push(s);

            return true;
        }
    }
}
