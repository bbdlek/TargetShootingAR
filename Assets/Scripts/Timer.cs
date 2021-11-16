using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public int setTime;
    private ShooterManager _shooterManager;

    private void Start()
    {
        _shooterManager = FindObjectOfType<ShooterManager>();
        
        setTime = 60;
        GetComponent<TMP_Text>().text = setTime.ToString();
    }

    public void TimerStarter()
    {
        StartCoroutine(StartTimer());
    }
    
    IEnumerator StartTimer()
    {
        while (setTime > 0)
        {
            yield return new WaitForSeconds(1);
            setTime -= 1;
            GetComponent<TMP_Text>().text = setTime.ToString();
        }
        _shooterManager.CheckScore();
    }
}
