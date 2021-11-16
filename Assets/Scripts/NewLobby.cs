using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NewLobby : MonoBehaviourPunCallbacks
{
    public RectTransform title;
    private Sequence _sequence;
    private Sequence _sequence2;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        StartCoroutine(_audioManager.ChangeMusic1());
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returncode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 4});
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("WaitForClient");
    }

    void TitleMove()
    {
        _sequence = DOTween.Sequence()
            .Append(title.DOAnchorPos(new Vector3(0, 747), 1f))
            .Append(title.DOShakeAnchorPos(1f, 5f, 2, 10f, false, true))
            .OnComplete(() =>
            {
                _sequence2 = DOTween.Sequence()
                    .Append(title.DOSizeDelta(new Vector2(480, 480), 1.5f))
                    .Append(title.DOSizeDelta(new Vector2(530, 530), 1.5f))
                    .SetLoops(-1, LoopType.Yoyo);
            });

    }
}
