using System.Collections.Generic;
using System.Collections;
using System;

namespace Generation
{
    public class MazeGrid : IEnumerator, IEnumerable
    {
        List<List<Cell>> rows = new List<List<Cell>>();

        int nRows = 0;
        int nCols = 0;
        int curRows = 0;
        int curCols = 0;
        Random rnd = new Random();

        public MazeGrid(int rs, int cs)
        {
            nRows = rs;
            nCols = cs;
            PrepareGrid();
            ConfigureCells();

        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }

        public bool MoveNext()
        {
            curCols++;
            if (curCols >= nCols)
            {
                curRows++;
                curCols = 0;
            }
            return curRows < nRows;

        }


        public object Current
        {
            get { return this[curRows][curCols]; }
        }

        public void Reset()
        {
            curRows = 0;
            curCols = -1;
        }

        public List<Cell> this[int i]
        {
            get { return rows[i]; }
        }

        void PrepareGrid()
        {
            for (int i = 0; i < nRows; i++)
            {
                var row = new List<Cell>();
                for (int j = 0; j < nCols; j++)
                {
                    row.Add(new Cell(i, j));

                }
                rows.Add(row);
            }
        }

        void ConfigureCells()
        {
            foreach (var row in rows)
            {
                foreach(var cell in row)
                {
                    if (cell.row - 1 >= 0)
                        cell.north = rows[cell.row - 1][cell.col];
                    if (cell.row + 1 < nRows)
                        cell.south = rows[cell.row + 1][cell.col];
                    if (cell.col - 1 >= 0)
                        cell.west = rows[cell.row][cell.col - 1];
                    if (cell.col + 1 < nCols)
                        cell.east = rows[cell.row][cell.col + 1];
                }
            }
        }

        public Cell RandomCell()
        {
            int r = rnd.Next(0, nRows);
            
            int c = rnd.Next(0, nCols);
            return this[r][c];
        }

        public int Size()
        {
            return nRows * nCols;
        }

        
    }
}