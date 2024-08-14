using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DifficultyButton : MonoBehaviour
{
    [SerializeField] Difficulty difficulty;

    GameManager gameManager;

    void StartGame()
    {
        gameManager.StartGame(difficulty);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        GetComponent<Button>().onClick.AddListener(StartGame);
    }
}
