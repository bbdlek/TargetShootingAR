using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    public InGameUI InGameUI;

    public bool ImReady;
    public bool UrReady;

    public Button _readyBtn;
    public Button _unreadyBtn;

    public bool inGame;
    
        
    private AudioManager _audioManager;
    
    
    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        inGame = false;
        OnClickIamNotReady();
        StartCoroutine(_audioManager.ChangeMusic2());
    }
    
    //Ready
    public void OnClickIamReady()
    {
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        hash["Ready"] = true;   
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        _readyBtn.gameObject.SetActive(false);
        _unreadyBtn.gameObject.SetActive(true);

        if(!PhotonNetwork.IsMasterClient) return;

        CheckAllPlayersReady ();
    }
    public void OnClickIamNotReady()
    {
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        hash["Ready"] = false;   
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        _readyBtn.gameObject.SetActive(true);
        _unreadyBtn.gameObject.SetActive(false);

        //if(!PhotonNetwork.IsMasterClient) return;

        CheckAllPlayersReady ();
    } 

    public override void OnPlayerPropertiesUpdate (Player targetPlayer, Hashtable changedProps)
    {
        //if(!PhotonNetwork.IsMasterClient) return;

        if(!changedProps.ContainsKey("Ready")) return;
        if(!changedProps.ContainsKey("Score")) return;

        InGameUI.CheckReady();
        CheckAllPlayersReady();
        
        if(inGame)
            InGameUI.SetScore();
    }

    public override void OnMasterClientSwitched(Player  newMasterClient)
    {
        if(!Equals(newMasterClient, PhotonNetwork.LocalPlayer)) return;

        CheckAllPlayersReady ();
    }   


    private void CheckAllPlayersReady ()
    {
        if(inGame) return;
        
        var players = PhotonNetwork.PlayerList;
        if (players.Length != 2)
            return;

        if(players.All(p => p.CustomProperties.ContainsKey("Ready") && (bool)p.CustomProperties["Ready"]))
        {
            inGame = true;
            InGameUI.ShakeVersus();
            Debug.Log("All players are ready!");
        }
    }
    
}
