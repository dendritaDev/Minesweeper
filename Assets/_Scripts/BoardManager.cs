using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int width = 15, height = 15;
    public Cell cellPrefab;
    public Cell[,] cells;
    private System.Random rand;
    public BotManager botManager;

    private void Awake()
    {
        botManager = GetComponentInChildren<BotManager>();
    }

    void Start()
    {
        rand = new System.Random(12345); // o variar la semilla si quieres distintas configuraciones
        cells = new Cell[width, height];
        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell newCell = Instantiate(cellPrefab, new Vector3(x, y, 0) + this.gameObject.transform.localPosition, Quaternion.identity, this.transform);
                newCell.Init(this, HasMineAtInit(x, y));
                cells[x, y] = newCell;
            }
        }
    }

    bool HasMineAtInit(int x, int y)
    {
        // Usa rand.NextDouble() en lugar de Random.value si quieres reproducibilidad
        return rand.NextDouble() < 0.15;
    }

    public int CountAdjacentMines(int x, int y)
    {
        int count = 0;
        // Similar a antes, pero usando this.cells
        if (HasMineAt(x - 1, y - 1)) count++;
        // ... repite para todas las posiciones adyacentes ...
        return count;
    }

    bool HasMineAt(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
            return cells[x, y].hasMine;
        return false;
    }

    public bool HasTheGameEnded()
    {
        foreach (var cell in cells)
        {
            if (cell.IsCovered() && !cell.hasMine)
                return false;
        }
        return true;
    }

    public void UncoverAllMines()
    {
        foreach (var c in cells)
        {
            if (c.hasMine) c.LoadTexture(0);
        }
    }

    public void OnGameEnd(bool won, BotManager botManager)
    {
        botManager.OnGameEnd(won);
    }
}
