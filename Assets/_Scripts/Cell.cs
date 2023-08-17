using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cell : MonoBehaviour
{
    public bool hasMine;
    public Sprite[] emptyTextures;
    public Sprite mineTexture;

    // Start is called before the first frame update
    void Start()
    {
        hasMine = (Random.value < 0.15); //Random.value te devuelve un float entre 0-1 con distribución uniforme
                                         //Por tanto con este 0.15, hacemos que aproximadamente del numero total de celdas que hay
                                         //el 15% sean minas

        int x = (int)this.transform.position.x;
        int y = (int)this.transform.parent.transform.position.y;//Como las he agruado por filas, es el padre de las celdas de cada fila quien tiene la posicion en Y

        GridHelper.cells[x, y] = this; //en vez de rellenar la matriz con el manager, llamo a todas las celdas para que rellenen su posicion

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadTexture(int adjacentCount)
    {
        if(hasMine)
        {
            GetComponent<SpriteRenderer>().sprite = mineTexture;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];
        }
    }

    /// <summary>
    /// Chequear que la celda esta tapada y no ha sido destapada
    /// </summary>
    /// <returns></returns>
    public bool IsCovered()
    {
        return GetComponent<SpriteRenderer>().sprite.name == "minesweeper_12";
    }

    //Es como un mouseUP+mouseDown
    private void OnMouseUpAsButton()
    {

        if (hasMine)
        {
            GridHelper.UncoverAllTheMines();
            Debug.Log("Has perdido. Fin de la partida");
            Invoke(nameof(ReturnToMainMenu), 1.5f);
        }
        else
        {
            int x = (int)this.transform.position.x;
            int y = (int)this.transform.position.y;

            LoadTexture(GridHelper.CountAdjacentMines(x, y));

            GridHelper.FloodFillUncover(x, y, new bool[GridHelper.width, GridHelper.height]);

            if(GridHelper.HasTheGameEnded())
            {
                Debug.Log("Has Ganado.Fin de la partida");
                Invoke(nameof(ReturnToMainMenu), 1.5f);
            }

        }
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
