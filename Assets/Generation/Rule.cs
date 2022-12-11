using System;
using UnityEngine;
using System.Collections.Generic;

namespace Generation
{
    [CreateAssetMenu(fileName = "Rule", menuName = "ScriptableObjects/LevelGenerator/Rule", order = 1)]
    public class Rule : ScriptableObject
    {

        public Shape PredecessorShape;
        public List<Operation> operations;
        
        private Stack<Shape> stack = new Stack<Shape>();

        public Tuple<List<Shape>, bool> CalculateRule(Shape inputShape)
        {
            Debug.Log(this.name);
            var result = new List<Shape>();
            foreach (var operation in operations)
            {
                operation.predecessor = inputShape;
                if (!operation.Apply(stack, result)) return Tuple.Create(result, false);
            }

            return Tuple.Create(result, true);
        }
    }
}