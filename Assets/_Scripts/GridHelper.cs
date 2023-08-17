using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHelper : MonoBehaviour
{

    public static int width = 15, height = 15;

    public static Cell[,] cells = new Cell[width, height];

    public static void UncoverAllTheMines()
    {
        foreach(Cell cell in cells)
        {
            if(cell.hasMine)
            {
                cell.LoadTexture(0);
            }
        }
    }

    public static bool HasMineAt(int x, int y)
    {
        if(x>=0 && y >= 0 && x<width && y<height)
        {
            Cell cell = cells[x, y];
            return cell.hasMine;
        }
        else
        {
            return false;
        }
    }

    public static int CountAdjacentMines(int x, int y)
    {
        int count = 0;

        //Columna de la izquierda
        if (HasMineAt(x - 1, y - 1)) count++;
        if (HasMineAt(x - 1, y)) count++;
        if (HasMineAt(x - 1, y + 1)) count++;

        //columna de la derecha
        if (HasMineAt(x + 1, y - 1)) count++;
        if (HasMineAt(x + 1, y)) count++;
        if (HasMineAt(x + 1, y + 1)) count++;
        
        //Misma columna
        if (HasMineAt(x, y + 1)) count++;
        if (HasMineAt(x, y - 1)) count++;


        return count;
    }

    /// <summary>
    /// Algoritmo floodfill
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="visited"></param>
    public static void FloodFillUncover(int x, int y, bool[,] visited)
    {

        if(x>= 0 && y >= 0 && x < width && y < height) //Si estamos dentro del mapa:
        {
            //Miramos si hemos ya visitado esa celda
            if (visited[x, y])
            {
                return;
            }

            int adjacentMines = CountAdjacentMines(x, y); //Contamos las minas al rededor de la celda

            cells[x, y].LoadTexture(adjacentMines); //Printamos el numero de minas

            if(adjacentMines > 0) //Si tenemos una mina alrededor, dejamos de hacer el algoritmo
            {
                return;
            }

            visited[x, y] = true;

            //Algoritmo Floodfill en 4 direcciones en caso de que no haya ninguna mina adyacente a esta celda
            FloodFillUncover(x - 1, y, visited);
            FloodFillUncover(x + 1, y, visited);
            FloodFillUncover(x, y - 1, visited);
            FloodFillUncover(x, y + 1, visited);
        }

   
    }

    public static bool HasTheGameEnded()
    {
        //Si aun quedan celdas sin destapar que no tengan minas, aun no ha acabado el juego.
        foreach(Cell cell in cells)
        {
            
            if(cell.IsCovered() && !cell.hasMine)
            {
                
                return false;
            }

        }

        return true;
    }

}
