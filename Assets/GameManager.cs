using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CellDoorAnimation _doorAnimation;
    [SerializeField] private GameObject _menuUI;

    [SerializeField] private GameObject _settingUI;
    [SerializeField] private GameObject optionsUI;

    [SerializeField] private Slider _voloumeSlider;
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private Slider _sensitivitySlider;

    [SerializeField] private GameObject _gameEnd;

    public bool GameStarted;

    public bool GamePaused;

    public static GameManager Instance;

    void Start()
    {
        Instance = this;

        this._voloumeSlider.maxValue = 20;
        this._voloumeSlider.minValue = -80;

        this._sensitivitySlider.maxValue = 1;
        this._sensitivitySlider.minValue = 0f;

        this._voloumeSlider.value = PlayerPrefs.GetFloat("Volume", 0);
        this._audioMixer.SetFloat("Master", this._voloumeSlider.value);

        this._sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitvity", 0.15f);
        ControlCamera.ControlModeSens = this._sensitivitySlider.value;
    }

    void OnTriggerEnter(Collider other)
    {
        this._gameEnd.SetActive(true);

        if (GameObject.FindGameObjectWithTag("ControlCamera"))
            GameObject.FindGameObjectWithTag("ControlCamera").GetComponent<ControlCamera>().enabled = false;

        if (GameObject.FindGameObjectWithTag("FollowCamera"))
            GameObject.FindGameObjectWithTag("FollowCamera").GetComponent<CinemachineInputAxisController>().enabled = false;

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = false;

        Cursor.lockState = CursorLockMode.None;
    }

    public void OnVolumeChange()
    {
        this._audioMixer.SetFloat("Master", this._voloumeSlider.value);

        PlayerPrefs.SetFloat("Volume", this._voloumeSlider.value);
    }

    public void OnSettingsPressed()
    {
        this.optionsUI.SetActive(false);
        this._settingUI.SetActive(true);
    }

    public void OnBackPressed()
    {
        this.optionsUI.SetActive(true);
        this._settingUI.SetActive(false);
    }

    public void OnSensChange() 
    {
        ControlCamera.ControlModeSens = Mathf.Clamp(this._sensitivitySlider.value, 0.1f, 1);

        PlayerPrefs.SetFloat("Sensitvity", this._sensitivitySlider.value);
    }

    public void OnQuit() 
    {
        Application.Quit();
    }

    public void StartGame()
    {
        if (!this.GameStarted)
        {
            this._menuUI.SetActive(false);

            if (!Keyboard.current.shiftKey.isPressed)
            {
                this._doorAnimation.StartAnimation();
            }
            else 
            {
                this._doorAnimation.AnimatorSkip();
            }
        }
        else
        {
            this.GamePaused = false;

            this._menuUI.SetActive(this.GamePaused);

            Cursor.lockState = (this.GamePaused) ? CursorLockMode.None : CursorLockMode.Locked;

            if (GameObject.FindGameObjectWithTag("ControlCamera"))
                GameObject.FindGameObjectWithTag("ControlCamera").GetComponent<ControlCamera>().enabled = !this.GamePaused;

            if (GameObject.FindGameObjectWithTag("FollowCamera"))
                GameObject.FindGameObjectWithTag("FollowCamera").GetComponent<CinemachineInputAxisController>().enabled = !this.GamePaused;

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = !this.GamePaused;
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed && this.GameStarted)
        {
            this.GamePaused = !this.GamePaused;

            this._menuUI.SetActive(this.GamePaused);

            Cursor.lockState = (this.GamePaused) ? CursorLockMode.None : CursorLockMode.Locked;

            if(GameObject.FindGameObjectWithTag("ControlCamera"))
                GameObject.FindGameObjectWithTag("ControlCamera").GetComponent<ControlCamera>().enabled = !this.GamePaused;

            if(GameObject.FindGameObjectWithTag("FollowCamera"))
                GameObject.FindGameObjectWithTag("FollowCamera").GetComponent<CinemachineInputAxisController>().enabled = !this.GamePaused;

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = !this.GamePaused;

        }
    }
}
