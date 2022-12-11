using System.Collections;
using UnityEngine;
using Generation;

namespace Assets.Scripts.Generation
{
    public class MazeManager : MonoBehaviour
    {

        [SerializeField]
        int numRows = 10;

        [SerializeField]
        int numCols = 10;

        [SerializeField]
        float spacing = 50;

        [SerializeField]
        float cellRadius = 25;

        [SerializeField]
        GameObject wallObject;

        [SerializeField]
        int numWalls = 100;
        
        [SerializeField]
        float wallRadiusScaling = 2f;

        [SerializeField]
        GenerationManager generationManager;

        [SerializeField]
        Vector3 initialPosition;



        MazeGrid grid;

        private void Start()
        {
            grid = new MazeGrid(numRows, numCols);
            HuntAndKill.Generate(grid);
            generationManager = GetComponent<GenerationManager>();
            GenerateMaze();
            GenerateWalls();
        }

        void GenerateWalls()
        {
            Vector3 radius = new Vector3(numCols / 2 * spacing, 0, -numRows / 2 * spacing);
            Vector3 center = initialPosition + radius;
         

            for (int i = 0; i < numWalls; i++)
            {
                float angle = 360f / numWalls * i;
                var wallPosition = center + wallRadiusScaling * new Vector3(Mathf.Cos(angle) * radius.x, radius.y, Mathf.Sin(angle) * radius.z);
                var wallRotation = Quaternion.LookRotation(center - wallPosition, Vector3.up);
                Instantiate(wallObject, wallPosition, wallRotation);
            }
            
        }

        void GenerateMaze()
        {
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    var cell = grid[i][j];
                    var links = cell.Links();
                    if (cell.north == null || links.Find(c => c == cell.north) == null)
                        generationManager.StartGeneration(initialPosition + new Vector3(i * spacing, 0, -(j * spacing - cellRadius)), Quaternion.identity);
                  //  if (cell.south != null && links.Find(c => c == cell.south) != null)
                  //      generationManager.StartGeneration(initialPosition + new Vector3(i * spacing, 0, -(j * spacing + cellRadius)), Quaternion.identity);
                  //  if (cell.west != null && links.Find(c => c == cell.west) != null)
                  //      generationManager.StartGeneration(initialPosition + new Vector3(i * spacing - cellRadius, 0, -(j * spacing)), Quaternion.Euler(new Vector3(0, 90, 0)));
                    if (cell.east == null || links.Find(c => c == cell.east) == null)
                        generationManager.StartGeneration(initialPosition + new Vector3(i * spacing + cellRadius, 0, -(j * spacing)), Quaternion.Euler(new Vector3(0, 90, 0)));
                }
            }
        }
    }
}