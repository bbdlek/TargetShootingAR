using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class InGameUI : MonoBehaviourPunCallbacks
{
    private ShooterManager _shooterManager;
    private AudioManager _audioManager;

    public TMP_Text myNick;
    public TMP_Text otherNick;
    public GameObject CheckReady1;
    public GameObject CheckReady2;

    public GameObject versusObject;
    public TMP_Text myNick2;
    public TMP_Text otherNick2;
    public GameObject unreadyBtn;
    private Sequence mySequence;
    private Sequence endSequence;

    public TMP_Text logTxt;

    public GameObject crossHair;
    public GameObject ShootBtn;
    public GameObject targetBG;

    public TMP_Text P1Score;
    public TMP_Text P2Score;

    public GameObject timer;
    public GameObject pausePanel;

    private void Start()
    {
        _shooterManager = FindObjectOfType<ShooterManager>();
        _audioManager = FindObjectOfType<AudioManager>();
        myNick.text = PhotonNetwork.NickName;
        otherNick.text = PhotonNetwork.PlayerListOthers[0].NickName;
        logTxt.text = "";
        P1Score.text = "점수 : 0";
        P2Score.text = "점수 : 0";
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        otherNick.text = "";
        endSequence = DOTween.Sequence()
            .Append(logTxt.DOText(other.NickName + "님이\n퇴장하셨습니다!", 1f, false))
            .Insert(3f,logTxt.DOText("승리당하셨습니다!", 1f, false))
            .OnComplete(() =>
            {
                PauseGame();
            });

    }

    public void HomeBtnOnClick()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        GetComponent<PhotonView>().RPC("ControlTime", RpcTarget.All, 0f);
    }
    public void ResumeGame()
    {
        GetComponent<PhotonView>().RPC("ControlTime", RpcTarget.All, 1f);
        pausePanel.SetActive(false);
    }

    [PunRPC]
    public void ControlTime(float setTime)
    {
        Time.timeScale = setTime;
    }

    public void CheckReady()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Ready") &&
            (bool) PhotonNetwork.LocalPlayer.CustomProperties["Ready"])
        {
            Debug.Log("ImReady");
            CheckReady1.SetActive(true);
        }
        else if(PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Ready") &&
                !(bool) PhotonNetwork.LocalPlayer.CustomProperties["Ready"])
        {
            CheckReady1.SetActive(false);
        }
        
        if (PhotonNetwork.PlayerListOthers[0].CustomProperties.ContainsKey("Ready") &&
            (bool) PhotonNetwork.PlayerListOthers[0].CustomProperties["Ready"])
        {
            Debug.Log("UrReady");
            CheckReady2.SetActive(true);
        }
        else if(PhotonNetwork.PlayerListOthers[0].CustomProperties.ContainsKey("Ready") &&
                !(bool) PhotonNetwork.PlayerListOthers[0].CustomProperties["Ready"])
        {
            CheckReady2.SetActive(false);
        }
    }

    public void ShakeVersus()
    {
        versusObject.SetActive(true);
        myNick2.text = myNick.text;
        otherNick2.text = otherNick.text;
        mySequence = DOTween.Sequence()
            .OnStart(() =>
            {
                versusObject.transform.localScale = Vector3.zero;
                versusObject.GetComponent<CanvasGroup>().alpha = 0;
            })
            .Append(versusObject.transform.DOScale(1, 1).SetEase(Ease.OutBounce))
            .Join(versusObject.GetComponent<CanvasGroup>().DOFade(1, 1))
            .Insert(4.5f, versusObject.GetComponent<CanvasGroup>().DOFade(0, 1))
            .SetDelay(0.5f)
            .OnComplete(() =>
            {
                _shooterManager.StartCreate();
                crossHair.SetActive(true);
                ShootBtn.SetActive(true);
                targetBG.SetActive(true);
                timer.GetComponent<Timer>().TimerStarter();
            });
        unreadyBtn.SetActive(false);
    }

    public void TargetError()
    {
        logTxt.text = "다른 색을 맞추셨군요ㅠㅜ";
        Invoke(nameof(TargetDebug), 1f);
    }
    public void TargetDebug()
    {
        logTxt.text = "";
    }

    public void SetScore()
    {
        P1Score.text = "점수 : " + (int)PhotonNetwork.LocalPlayer.CustomProperties["Score"];
        P2Score.text = "점수 : " + (int)PhotonNetwork.PlayerListOthers[0].CustomProperties["Score"];
    }

    public void EndText(Player winner,Player loser, bool draw)
    {
        if (draw)
        {
            logTxt.text = "비겼습니다!!!\n두분다 정말 잘하시는 군요!!!";
        }
        else
        {
            if (Equals(PhotonNetwork.LocalPlayer, winner))
                logTxt.text = "이기셨군요!!\n다음 판도 이겨주실거죠?";
            else if(Equals(PhotonNetwork.LocalPlayer, loser))
                logTxt.text = "아쉽게 지셨네요...\n다음 판은 이겨주실거죠?";
        }
    }
}
