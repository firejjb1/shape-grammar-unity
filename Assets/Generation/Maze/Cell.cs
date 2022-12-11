using System;
using System.Collections.Generic;

namespace Generation
{
    public class Cell
    {
        public int row;
        public int col;
        public Cell north;
        public Cell south;
        public Cell west;
        public Cell east;

        public Cell(int r, int c)
        {
            row = r;
            col = c;
        }

        public Dictionary<Cell, bool> links = new Dictionary<Cell, bool>();

        public void Link(Cell toLink)
        {
            links[toLink] = true;
            toLink.links[this] = true;
        }

        public void Unlink(Cell toUnlink)
        {
            links[toUnlink] = false;
            toUnlink.links[this] = false;
        }

        public List<Cell> Neighbours()
        {
            var neighbours = new List<Cell>();
            if (north != null)
                neighbours.Add(north);
            if (south != null) 
                neighbours.Add(south);
            if (west != null)
                neighbours.Add(west);
            if (east != null)
                neighbours.Add(east);
            return neighbours;
        }

        public List<Cell> Links()
        {
            return new List<Cell>(links.Keys);
        }

        public bool IsLink(Cell cell)
        {
            return links[cell];
        }

    }
}