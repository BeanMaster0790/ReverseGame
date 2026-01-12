using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] private string _text;
    [SerializeField] private float _hintTime = 2;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && GameManager.Instance.GameStarted)
        {
            GameObject.FindGameObjectWithTag("HintsManager").GetComponent<HintsManager>().DisplayHint(this._text, this._hintTime);
            GameObject.Destroy(this.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && GameManager.Instance.GameStarted)
        {
            GameObject.FindGameObjectWithTag("HintsManager").GetComponent<HintsManager>().DisplayHint(this._text, this._hintTime);
            GameObject.Destroy(this.gameObject);
        }
    }
}
