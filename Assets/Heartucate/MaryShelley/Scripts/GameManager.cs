using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isGameActive = false;
    [SerializeField] private GameTimer gameTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        if (!isGameActive)
        {
            gameTimer.StartGame();
        }
        
        isGameActive = true;

    }

    public void EndGame()
    {
        isGameActive = false;
    }
}
