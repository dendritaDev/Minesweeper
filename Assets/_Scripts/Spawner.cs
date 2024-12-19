using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject minesweeperPrefab;
    public int numBots = 1;
    void Start()
    {
        for (int i = 0; i < numBots; i++)
        {
            Vector3 position = new Vector3((i % 10) * 20, (i / 10) * 20, 0);
            // esto coloca 10 tableros por fila, 5 filas, separados 20 unidades.
            Instantiate(minesweeperPrefab, position, Quaternion.identity);
        }
    }
}
