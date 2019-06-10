using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{
    // Storing created objects in one place ('Folder')
    private GameObject wallsFolder;
    private GameObject floorFolder;

    // Wall and floor prefabs
    public GameObject wall;
    public GameObject floor;
    private float wallLength = 2f; // Default wall length. Shouldn't change.
    private float floorLength = 2f; // Default floor length. Shouldn't change.

    // Size of the maze
    public int xSize;
    public int zSize;
    private Vector3 targetPosition; // Wall/floor position

    private Cell[] cells; // Array for storing cells
    private int currentCell = 0; // Current cell index
    private int visitedCells = 0; // Number of visited cells
    private int chosenNeighbour = 0; // Next neighbour to be visited
    private List<int> lastCells; // List to hold current cell before jumping to the neighbour
    private int lastCellNumber = 0; // Previous cell before current cell (if current cell does not have any more neighbours)
    private int destroyThisWall = 0; // Holds index, which is used to know which cell's wall to destroy
    
    void Start ()
    {
        GenerateWalls();
        GenerateFloor();
        GenerateCells();
        BuildMaze();
    }

    // Cell class for storing walls of a cell
    public class Cell
    {
        public bool visited;
        public GameObject downWall;
        public GameObject upWall;
        public GameObject leftWall;
        public GameObject rightWall;
    }

    // Function for generating walls
    private void GenerateWalls()
    {
        // For storing created walls in a 'folder'
        wallsFolder = new GameObject();
        wallsFolder.name = "WallsFolder";
        GameObject createdWall;

        // Creating walls in X Axis
        for (int i = 0; i < zSize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                targetPosition = new Vector3((j * wallLength) - wallLength/2, 0.5f , + (i * wallLength) - wallLength/2);  // Calculating next position of the wall
                createdWall = Instantiate(wall, targetPosition, Quaternion.identity); // Creating a clone of the wall
                createdWall.transform.parent = wallsFolder.transform; // Putting a clone in a 'folder'
            }
        }

        // Creating walls in Z Axis
        for (int i = 0; i <= zSize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                targetPosition = new Vector3((j* wallLength), 0.5f, (i * wallLength) - wallLength); // Calculating next position of the wall
                createdWall = Instantiate(wall, targetPosition, Quaternion.Euler(0,90,0)); // Creating a clone of a wall and turning it on Y axis
                createdWall.transform.parent = wallsFolder.transform; // Putting a clone in a 'folder'
            }
        }
    }

    // This function generates floors
    private void GenerateFloor()
    {
        // For storing created floors in a 'folder'
        floorFolder = new GameObject();
        floorFolder.name = "FloorFolder";
        GameObject createdFloor;

        // For creating floors below walls
        for (int i = 0; i < zSize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                targetPosition = new Vector3((j * floorLength), -0.5f, (i * floorLength) - floorLength / 2);
                createdFloor = Instantiate(floor, targetPosition, Quaternion.identity);
                createdFloor.transform.parent = floorFolder.transform;
            }
        }
    }
    
    

    // Function to count all the cells
    private int GetTotalCells()
    {
        return xSize * zSize;
    }

    // Function to generate and assign walls to cells
    private void GenerateCells()
    {
        int wallCount = wallsFolder.transform.childCount; // Number of total walls
        int upperWall = ((xSize + 1) * zSize) + xSize; // Upper wall number of the first cell
        int lowerWall = (xSize + 1) * zSize; // Lower wall number of the first cell
        int rowJumps = 0; // Counter for row jumps
        GameObject[] allWalls = new GameObject[wallCount]; // Array for storing all walls
        cells = new Cell[GetTotalCells()]; // Array for storing cells
        lastCells = new List<int>(); // List for storing cells, in case a cell doesn't have any more neighbours and backtracking is needed

        // Loop for putting all walls into array 
        for (int i = 0; i < allWalls.Length; i++)
        {
            allWalls[i] = wallsFolder.transform.GetChild(i).gameObject;
        }

        // Loop for filling all the cells
        for (int i = 0; i < cells.Length; i++)
        {
            // rowJumps are incremented only when i>0 and remainder after division of i and xSize = 0
            if (i % xSize == 0 && i != 0) rowJumps++;
            
            cells[i] = new Cell(); // Creating new cell

            // Assigning walls to each cell
            cells[i].leftWall = allWalls[i + rowJumps]; 
            cells[i].rightWall = allWalls[i + 1 + rowJumps];
            cells[i].upWall = allWalls[i + upperWall];
            cells[i].downWall = allWalls[i + lowerWall];
        }
    }

    private void CheckNeighbours()
    {
        int neighboursCount = 0; // Number of neighbours around the cell
        int[] neighbours = new int[4]; // Array with maximum number of neighbours

        /* destroyDirection indicated which side of the cell can be destroyed
         * if 1 - bottom wall
         * 2 - upper wall
         * 3 - left wall
         * 4 - right wall */
        int[] destroyDirection = new int[4]; // Selecting wall to break

        // Checking if there is a neighbour below current cell
        if (currentCell >= xSize && cells[currentCell - xSize].visited == false)
        {
            neighbours[neighboursCount] = currentCell - xSize; // Index of neighbour below
            destroyDirection[neighboursCount] = 1; // If chosen, wall below the cell can be destroyed
            neighboursCount++; 
        }

        // Checking if there is a neighbour above current cell
        if (currentCell < GetTotalCells() - xSize && cells[currentCell+xSize].visited == false)
        {
            neighbours[neighboursCount] = currentCell + xSize; // Index of neighbour alove
            destroyDirection[neighboursCount] = 2; // If chosen, wall above the cell can be destroyed
            neighboursCount++;
        }

        // Checking if there is a neighbour on the left of current cell
        if (currentCell % xSize != 0 && currentCell != 0 && cells[currentCell - 1].visited == false)
        {
            neighbours[neighboursCount] = currentCell - 1; // Index of neighbour on the left
            destroyDirection[neighboursCount] = 3; // If chosen, wall on the left of the cell can be destroyed
            neighboursCount++;
        }

        // Checking if there is a neighbour on the right of current cell
        if ((currentCell + 1) % xSize != 0 && cells[currentCell + 1].visited == false)
        {
            neighbours[neighboursCount] = currentCell + 1; // Index of neighbour on the right
            destroyDirection[neighboursCount] = 4; // If chosen, wall on the right of the cell can be destroyed
            neighboursCount++;
        }

        // If there are neighbours for current cell
        if (neighboursCount != 0)
        {
            int randomDirection = Random.Range(0, neighboursCount); // Choose random neighbour
            chosenNeighbour = neighbours[randomDirection]; // Set chosen neighbour as next cell to move to 
            destroyThisWall = destroyDirection[randomDirection]; // Set direction to know which wall to destroy
        }

        // If there are no neighbours left for current cell, make previous cell as current cell
        else
        {
            if (lastCellNumber > 0)
            {
                currentCell = lastCells[lastCellNumber];
                lastCellNumber--;
            }
        }
    }

    private void BuildMaze()
    {
        bool startedBuilding = false;
        // While there are unvisited cells
        while (visitedCells < GetTotalCells())
        {
            // Check neighbours of current cell
            CheckNeighbours();
            if (startedBuilding)
            {
                    // If random neighbour is unvisited
                    if (cells[chosenNeighbour].visited == false && cells[currentCell].visited == true)
                    {
                        DestroyWall(); // Remove the wall between visited and unvisited cell
                        lastCells.Add(currentCell); // Add current cell to the list
                        cells[chosenNeighbour].visited = true; // Marking chosen random neighbour as visited
                        visitedCells++; // Increasing visited cells count
                        currentCell = chosenNeighbour; // Current cell is changed to the chosen neighbour

                        if (lastCells.Count > 0)
                        {
                            lastCellNumber = lastCells.Count - 1;
                        }
                    }
            }
            // Before. Selecting random cell to start building from, marking it visited and icrementing visited cells counter. Executed only once.
            else
            {
                currentCell = Random.Range(0, GetTotalCells());
                cells[currentCell].visited = true;
                visitedCells++;
                startedBuilding = true;
            }
        }
    }

    // Function for checking which wall to destroy
    private void DestroyWall()
    {
        switch (destroyThisWall)
        {
            case 1: Destroy(cells[currentCell].downWall); break;
            case 2: Destroy(cells[currentCell].upWall); break;
            case 3: Destroy(cells[currentCell].leftWall); break;
            case 4: Destroy(cells[currentCell].rightWall); break;
        }
    }
}
