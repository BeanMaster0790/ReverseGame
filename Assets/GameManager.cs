using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CellDoorAnimation _doorAnimation;
    [SerializeField] private GameObject _menuUI;

    public bool GameStarted;

    public static GameManager Instance;

    void Start()
    {
        Instance = this;
    }

    public void StartGame()
    {
        this._menuUI.SetActive(false);
        this._doorAnimation.StartAnimation();
    }
}
