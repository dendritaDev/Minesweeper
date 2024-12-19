// BotManager.cs
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class BotManager : MonoBehaviour
{
    public BoardManager board;
    UserProfile currentProfile;
    float timeBetweenMoves = 0.5f;
    bool gameEnded = false;
    int clickCount = 0; // contador de clics

    void Start()
    {
        GenerateProfile();
        StartCoroutine(PlayGameRoutine());
    }

    void GenerateProfile()
    {
        currentProfile = new UserProfile();
        currentProfile.playerID = System.Guid.NewGuid().ToString();
        currentProfile.age = Random.Range(18, 51);
        currentProfile.gender = (Random.value < 0.5f) ? "M" : "F";

        string[] countries = { "Spain", "USA", "Germany", "France", "UK" };
        currentProfile.country = countries[Random.Range(0, countries.Length)];

        Debug.Log(currentProfile.playerID);
        //StartCoroutine(SendProfileToServer());
    }

    IEnumerator SendProfileToServer()
    {
        WWWForm form = new WWWForm();
        form.AddField("playerID", currentProfile.playerID);
        form.AddField("age", currentProfile.age);
        form.AddField("gender", currentProfile.gender);
        form.AddField("country", currentProfile.country);

        using (UnityWebRequest www = UnityWebRequest.Post("http://tuservidor.com/save_profile.php", form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log("Error uploading profile: " + www.error);
            else
                Debug.Log("Profile uploaded");
        }
    }

    IEnumerator PlayGameRoutine()
    {
        while (!gameEnded)
        {
            yield return new WaitForSeconds(timeBetweenMoves);

            Cell cell = GetRandomCoveredCell();
            if (cell == null)
            {
                // No hay más celdas cubiertas, finaliza
                gameEnded = true;
                break;
            }

            // Simular click
            clickCount++;
            cell.SendMessage("OnMouseUpAsButton");

            // Enviar el evento de click al servidor con el contador actual
            //StartCoroutine(SendClickToServer(cell, clickCount));
        }
    }

    Cell GetRandomCoveredCell()
    {
        List<Cell> coveredCells = new List<Cell>();
        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                if (board.cells[x, y].IsCovered())
                {
                    coveredCells.Add(board.cells[x, y]);
                }
            }
        }

        if (coveredCells.Count > 0)
        {
            return coveredCells[Random.Range(0, coveredCells.Count)];
        }

        return null;
    }


    IEnumerator SendClickToServer(Cell cell, int clickNumber)
    {
        Vector2 pos = cell.transform.position;

        WWWForm form = new WWWForm();
        form.AddField("playerID", currentProfile.playerID);
        form.AddField("x", (int)pos.x);
        form.AddField("y", (int)pos.y);
        form.AddField("clickNumber", clickNumber); // Enviamos el número de click

        using (UnityWebRequest www = UnityWebRequest.Post("http://tuservidor.com/save_click.php", form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log("Error uploading click: " + www.error);
        }
    }

    public void OnGameEnd(bool hasWon)
    {
        gameEnded = true;
        //StartCoroutine(SendGameEndToServer(hasWon));
    }

    IEnumerator SendGameEndToServer(bool hasWon)
    {
        WWWForm form = new WWWForm();
        form.AddField("playerID", currentProfile.playerID);
        form.AddField("hasWon", hasWon ? 1 : 0);
        form.AddField("totalTime", Time.timeSinceLevelLoad.ToString());
        //level, points??

        using (UnityWebRequest www = UnityWebRequest.Post("http://tuservidor.com/save_game_end.php", form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log("Error uploading game end: " + www.error);
            else
                Debug.Log("Game end uploaded");

            // Reinicia la escena para crear un nuevo perfil y generar más datos
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
    }
}
