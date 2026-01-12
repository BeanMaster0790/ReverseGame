using System.Collections.Generic;
using UnityEngine;

public class CheckpointZone : MonoBehaviour
{
    [SerializeField] private GameObject _checkPointPrefab;
    public Transform RespawnPoint;

    private GameObject _player;

    public int Index;

    public static List<CheckpointZone> S_SheckpointZones;

    [SerializeField] private List<GameObject> _disableUntilTrigger;

    void Start()
    {
        this._player = GameObject.FindGameObjectWithTag("Player");

        this._checkPointPrefab = (GameObject)Resources.Load($"Prefabs/CheckpointZones/CheckpointZone{this.Index}", typeof(GameObject));

        if(CheckpointZone.S_SheckpointZones == null)
        {
            CheckpointZone.S_SheckpointZones = new List<CheckpointZone>();
            CheckpointZone.S_SheckpointZones.Add(this);
        }
        else
        {
            CheckpointZone.S_SheckpointZones.Add(this);
        }

        foreach (GameObject obj in this._disableUntilTrigger)
        {
            obj.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SetAsCheckpoint();

            foreach (GameObject obj in this._disableUntilTrigger)
            {
                obj.SetActive(true);
            }
        }
    }

    public void SetAsCheckpoint(bool checkForIndex = true)
    {
        PlayerHealth playerHealth = this._player.GetComponent<PlayerHealth>();

        if(checkForIndex && playerHealth.CurrentCheckPoint != null)
        {
            if(this.Index > playerHealth.CurrentCheckPoint.Index)
                playerHealth.CurrentCheckPoint = this;
        }
        else
            playerHealth.CurrentCheckPoint = this;
    }

    public void Respawn()
    {
        this._player.GetComponent<PlayerHealth>().CurrentCheckPoint = GameObject.Instantiate(_checkPointPrefab).GetComponent<CheckpointZone>();

        CheckpointZone.S_SheckpointZones.Remove(this);

        GameObject.Destroy(this.gameObject);
    }
}
