using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class ShooterManager : MonoBehaviour
{
    public InGameUI InGameUI;
    
    public GameObject flyingObject;
    [SerializeField] private int target_num;

    private Camera _cam;
    [SerializeField] public float createTime;
    public int maxObject = 10;
    [SerializeField] private Vector3 spawnPosition;

    public Image _targetColorImg;
    public Color targetColor;

    private int hit_num;

    Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
    private int score;
    
    private void Start()
    {
        InGameUI = FindObjectOfType<InGameUI>();
        _cam = Camera.main;
        
        score = 0;
        hash["Score"] = score;   
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        
        ChangeTargetColor();
    }

    void RandomPosition()
    {
        spawnPosition = _cam.transform.position +
                        new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
    }

    public void StartCreate()
    {
        if(PhotonNetwork.IsMasterClient)
            StartCoroutine(CreateObject());
    }
    
    IEnumerator CreateObject()
    {
        while (true)
        {
            int objCount = GameObject.FindGameObjectsWithTag("FlyingObject").Length;

            if (objCount < maxObject)
            {
                createTime = Random.Range(1.0f, 3.0f);
                RandomPosition();
                yield return new WaitForSeconds(createTime);
                if(PhotonNetwork.IsConnected)
                    PhotonNetwork.Instantiate("Chicken", spawnPosition, Quaternion.identity);
                else
                    Instantiate(flyingObject, spawnPosition, Quaternion.identity);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void ChangeTargetColor()
    {
        int choice = Random.Range(0, 3);
        switch (choice)
        {
            case 0:
                targetColor = Color.blue;
                target_num = 2;
                break;
            case 1:
                targetColor = Color.green;
                target_num = 1;
                break;
            case 2:
                targetColor = Color.red;
                target_num = 0;
                break;
        }

        _targetColorImg.color = targetColor;
    }
    
    public void Shoot()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("FlyingObject"))
            {
                //Destroy(hit.transform.gameObject);
                if (hit.transform.gameObject.GetComponent<FlyingObject>().type_num == target_num || hit.transform.gameObject.GetComponent<FlyingObject>().type_num == 3)
                {
                    hit_num = hit.transform.gameObject.GetComponent<FlyingObject>().type_num;
                    hit.transform.gameObject.GetComponent<FlyingObject>().Die();
                    GetScore();
                }
                else
                {
                    InGameUI.TargetError();
                }
            }
        }
    }
   
    private void GetScore()
    {
        if (hit_num == 0 || hit_num == 1 || hit_num == 2)
        {
            score += 20;

        }
        else if (hit_num == 3)
        {
            score += 50;
        }

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Score"))
        {
            hash["Score"] = score;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        ChangeTargetColor();
    }

    public void CheckScore()
    {
        var players = PhotonNetwork.PlayerList;
        
        if ((int)players[0].CustomProperties["Score"] > (int)players[1].CustomProperties["Score"])
        {
            InGameUI.EndText(players[0], players[1], false);
        }
        else if ((int) players[0].CustomProperties["Score"] == (int) players[1].CustomProperties["Score"])
        {
            InGameUI.EndText(players[0], players[1], true);
        }
        else if ((int) players[0].CustomProperties["Score"] < (int) players[1].CustomProperties["Score"])
        {
            InGameUI.EndText(players[1], players[0], false);
        }
        

        
    }
}
