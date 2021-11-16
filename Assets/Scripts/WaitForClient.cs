using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class WaitForClient : MonoBehaviourPunCallbacks
{
    public TMP_Text countTxt;

    
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        countTxt.text = PhotonNetwork.PlayerList.Length + "/" + "2";
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }

        countTxt.text = PhotonNetwork.PlayerList.Length + "/" + "2";
        
        if(PhotonNetwork.PlayerList.Length == 2)
            Invoke(nameof(GoToInGame), 2f);
    }

    private void GoToInGame()
    {
        PhotonNetwork.LoadLevel("InGame");
    }
}
