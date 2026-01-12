using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using System;
using JetBrains.Annotations;

public class CheatsManager : MonoBehaviour
{
    private bool _isActive;

    [SerializeField] private TMP_InputField _input;
    [SerializeField] private TMP_Text _errorText;

    private GameObject _player;


    void Start()
    {
        this._player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current != null && Keyboard.current.leftCtrlKey.isPressed && Keyboard.current.mKey.wasPressedThisFrame)
        {
            this._isActive = !this._isActive;

            if(this._isActive)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                this._input.gameObject.SetActive(true);
                this._input.Select();
                this._errorText.gameObject.SetActive(true);
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                this._input.gameObject.SetActive(false);
                this._errorText.gameObject.SetActive(false);
            }
        }

        if(!this._isActive)
            return;

        if(EventSystem.current != null && EventSystem.current.currentSelectedGameObject == this._input.gameObject && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            string fullCommand = this._input.text.ToLower();

            string[] split = fullCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string command = split[0];
            string[] args = (split.Length > 1) ? split[1..] : Array.Empty<string>();

            switch(command)
            {
                case "damage":

                    if(args.Length == 0)
                    {
                        this._errorText.text = "Command 'damage' missing an argument (amount)";
                    }
                    else
                    {
                        if(int.TryParse(args[0], out int damage))
                        {
                            this._player.GetComponent<PlayerHealth>().Damage(damage);
                            this._errorText.text = "Command 'damage' executed";
                        }
                    }

                    break;

                case "kill":

                    this._player.GetComponent<PlayerHealth>().Damage(int.MaxValue);
                    this._errorText.text = "Command 'kill' executed";
                    
                    break;

                case "checkpoint":

                    if(args.Length == 0)
                    {
                        this._errorText.text = "Command 'checkpoint' missing an argument (index, checkForIndex = true). Valid checkpoints = ";

                        foreach (CheckpointZone checkpoint in CheckpointZone.S_SheckpointZones)
                        {
                            this._errorText.text += (checkpoint.Index + ", ");
                        }
                    }

                    bool foundCheckpoint = false;

                    foreach (CheckpointZone checkpoint in CheckpointZone.S_SheckpointZones)
                    {
                        if(int.TryParse(args[0], out int index) && checkpoint.Index == index)
                        {
                            foundCheckpoint = true;

                            this._player.GetComponent<CharacterController>().enabled = false;
                            this._player.transform.position = checkpoint.RespawnPoint.position;
                            this._player.GetComponent<CharacterController>().enabled = true;

                            if(args.Length > 1 && args[1] == "false")
                            {
                                this._errorText.text = "Command 'chekpoint' executed and set as active checkpoint";
                                checkpoint.SetAsCheckpoint(false);
                            }
                            else
                            {
                                this._errorText.text = "Command 'chekpoint' executed and followed normal index rules";
                                checkpoint.SetAsCheckpoint();
                            }
                        }
                    }

                    if(!foundCheckpoint)
                    {
                        this._errorText.text = "Command 'checkpoint' had an invalid argument: Valid checkpoints = ";

                        foreach (CheckpointZone checkpoint in CheckpointZone.S_SheckpointZones)
                        {
                            this._errorText.text += (checkpoint.Index + ", ");
                        }
                    }

                    break;

                case "play":

                    if(args.Length == 0)
                    {
                        this._errorText.text = "Command 'play' missing an argument (soundName)";

                        break;
                    }

                    if(PlayerSoundsManager.Current.PlaySound(args[0]))
                        this._errorText.text = "Command 'play' executed.";
                    else
                        this._errorText.text = "Command 'play' had an invalid argument: The sound doesn't exist.";


                    break;

                case "deathless":

                    this._player.GetComponent<PlayerHealth>().Invincable = !this._player.GetComponent<PlayerHealth>().Invincable;

                    this._errorText.text = (this._player.GetComponent<PlayerHealth>().Invincable) ? "Command 'deathless' executed now invincable" : "Command 'deathless' executed now vunerable";

                    break;
            }


        }
    }
}
