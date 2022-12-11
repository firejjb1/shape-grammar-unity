using UnityEngine;
using System.Collections.Generic;

namespace Generation
{
    public abstract class Operation : ScriptableObject
    {
        public Shape predecessor;

        public abstract bool Apply(Stack<Shape> stack, List<Shape> results);

        protected Shape InheritShape(Shape predecessor, Shape outputShape=null)
        {
            Shape toPlace = CreateInstance<Shape>();
            toPlace.Position = predecessor.Position;
            toPlace.Rotation = predecessor.Rotation;
           
            if (outputShape != null)
            {
                toPlace.Symbol = outputShape.Symbol;
                toPlace.IsAxiom = outputShape.IsAxiom;
                toPlace.IsTerminal = outputShape.IsTerminal;

                // apply shape local transformations
                toPlace.Position += outputShape.Position;
                toPlace.Rotation += outputShape.Rotation;
                if (outputShape.Size.sqrMagnitude > 0.0)
                    toPlace.Size = outputShape.Size;
            }
            if (predecessor.Size.sqrMagnitude > 0.0)
                toPlace.Size = predecessor.Size;
            // else
            //     toPlace.Symbol = predecessor.Symbol;
            return toPlace;
        }
    }
}
