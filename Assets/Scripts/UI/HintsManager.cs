using UnityEngine;
using TMPro;

public class HintsManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _hintText;

    private Timer _hintsTimer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this._hintText.text = "";

        this._hintsTimer = new Timer();

        this._hintsTimer.StartTimer(0);

    }

    // Update is called once per frame
    void Update()
    {
        this._hintsTimer.Update();

        if(this._hintsTimer.IsFinished)
        {
            this._hintText.text = "";
        }
    }

    public void DisplayHint(string hint, float time)
    {
        this._hintText.text = hint;

        this._hintsTimer.StartTimer(time);
    }
}
