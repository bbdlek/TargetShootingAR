using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";

    public Text connectionInfoTxt;
    public Button joinBtn;
    public GameObject nickNamePanel;
    public TMP_InputField nickInput;
    public Text nickInfo;

    public RectTransform title;
    private Sequence _sequence;
    private Sequence _sequence2;

    private void Start()
    {
        TitleMove();
        //퍼미션 요청
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            //퍼미션 요청.
            //RequestUserPermission(Permission.권한)
            Permission.RequestUserPermission(Permission.Camera);

        }
        
        
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinBtn.interactable = false;
        connectionInfoTxt.text = "서버 연결중...";
    }

    public override void OnConnectedToMaster()
    {
        joinBtn.interactable = true;
        connectionInfoTxt.text = "온라인";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        joinBtn.interactable = false;
        connectionInfoTxt.text = "재접속중...";

        PhotonNetwork.ConnectUsingSettings();
    }

    public void SetNickName()
    {
        joinBtn.interactable = false;
        nickNamePanel.SetActive(true);
    }

    public void Connect()
    {
        if (nickInput.text.Length == 0)
        {
            nickInfo.text = "닉네임은 한 글자 이상입니다!";
            return;   
        }

        joinBtn.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.NickName = nickInput.text;
            connectionInfoTxt.text = "룸에 접속중...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoTxt.text = "재접속중...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returncode, string message)
    {
        connectionInfoTxt.text = "새로운 방 생성중...";
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 4});
    }

    public override void OnJoinedRoom()
    {
        connectionInfoTxt.text = PhotonNetwork.NickName + "님 방 참가 성공";
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
