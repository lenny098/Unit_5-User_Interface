using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DifficultyButton : MonoBehaviour
{
    [SerializeField] Difficulty difficulty;

    void StartGame()
    {
        GameManager.Instance.StartGame(difficulty);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(StartGame);
    }
}
