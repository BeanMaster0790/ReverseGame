using TMPro;
using UnityEngine;

public class InfoPannelManager : MonoBehaviour
{
    private SelectionObject _rewindObject;

    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _desc;
    [SerializeField] private TMP_Text _pauseTime;
    [SerializeField] private TMP_Text _rewindTime;


    void Start()
    {
        gameObject.SetActive(false);

        ObjectController.S_ObjectSelect += (object sender, ObjectSelectEvent e) =>
        {
            this._rewindObject = e.RewindObject;

            if (this._rewindObject == false)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        };
    }

    void Update()
    {
        this._name.text = this._rewindObject.Name;
        this._desc.text = this._rewindObject.Desc;
        this._pauseTime.text = this._rewindObject.PasueTime;
        this._rewindTime.text = this._rewindObject.RewindTime;

    }
}
