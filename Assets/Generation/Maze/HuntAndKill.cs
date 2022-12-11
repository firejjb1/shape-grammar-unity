using System.Collections.Generic;
using System;

namespace Generation
{
    public class HuntAndKill
    {
        public static void Generate(MazeGrid grid)
        {
            var current = grid.RandomCell();

            var rnd = new Random();

            while (current != null)
            {
                var unvisitedNeighbours = current.Neighbours().FindAll(c => c.links.Count == 0);
                if (unvisitedNeighbours.Count > 0)
                {
                    var neighbour = unvisitedNeighbours[rnd.Next(0, unvisitedNeighbours.Count)];
                    current.Link(neighbour);
                    current = neighbour;
                } else
                {
                    current = null;
                    foreach(var obj in grid)
                    {
                        var cell = (Cell)obj;
                        var visitedNeighbours = cell.Neighbours().FindAll(c => c.links.Count > 0);
                        if (cell.links.Count == 0 && visitedNeighbours.Count > 0)
                        {
                            current = cell;
                            current.Link(visitedNeighbours[rnd.Next(0, visitedNeighbours.Count)]);
                            break;
                        }
                    }
                }
            }
        }
    }
}