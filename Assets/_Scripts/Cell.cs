using UnityEngine;
using UnityEngine.SceneManagement;

public class Cell : MonoBehaviour
{
    public bool hasMine;
    public Sprite[] emptyTextures;
    public Sprite mineTexture;

    BoardManager board;

    public void Init(BoardManager boardManager, bool mine)
    {
        this.board = boardManager;
        this.hasMine = mine;
        // asignas textura inicial si lo deseas
    }

    public void LoadTexture(int adjacentCount)
    {
        if (hasMine)
        {
            GetComponent<SpriteRenderer>().sprite = mineTexture;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];
        }
    }

    public bool IsCovered()
    {
        return GetComponent<SpriteRenderer>().sprite.name == "minesweeper_12";
    }

    private void OnMouseUpAsButton()
    {
        if (hasMine)
        {
            board.UncoverAllMines();
            Debug.Log("Has perdido. Fin de la partida");
            board.OnGameEnd(false, board.botManager);
        }
        else
        {
            int x = (int)this.transform.localPosition.x;
            int y = (int)this.transform.localPosition.y;

            LoadTexture(board.CountAdjacentMines(x, y));
            FloodFillUncover(x, y, new bool[board.width, board.height]);

            if (board.HasTheGameEnded())
            {
                Debug.Log("Has Ganado.Fin de la partida");
                board.OnGameEnd(true, board.botManager);
            }
        }
    }

    void FloodFillUncover(int x, int y, bool[,] visited)
    {
        if (x >= 0 && y >= 0 && x < board.width && y < board.height)
        {
            if (visited[x, y]) return;

            int adjacentMines = board.CountAdjacentMines(x, y);
            LoadTexture(adjacentMines);
            if (adjacentMines > 0) return;

            visited[x, y] = true;

            FloodFillUncover(x - 1, y, visited);
            FloodFillUncover(x + 1, y, visited);
            FloodFillUncover(x, y - 1, visited);
            FloodFillUncover(x, y + 1, visited);
        }
    }
}
