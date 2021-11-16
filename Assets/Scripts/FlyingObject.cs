using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlyingObject : MonoBehaviour
{
    public SkinnedMeshRenderer _mat;
    public Material[] mat_types;
    public int type_num;
    private int initColorPicker;
    [SerializeField] private Vector3 randomPoint;
    [SerializeField] private float randomMoveTime;

    private Vector3 camPosition;

    private AudioSource _audio;

    private void Start()
    {
        initColorPicker = Random.Range(0, 3);
        _mat = GetComponentInChildren<SkinnedMeshRenderer>();
        _audio = GetComponent<AudioSource>();
        camPosition = Camera.main.transform.position;
        
        InitColor();
        StartCoroutine(ChangeColor());
        StartCoroutine(Move());
    }

    private void InitColor()
    {
        switch (initColorPicker)
        {
            case 0:
                _mat.material = mat_types[0];
                type_num = 0;
                break;
            case 1:
                _mat.material = mat_types[1];
                type_num = 1;
                break;
            case 2:
                _mat.material = mat_types[2];
                type_num = 2;
                break;
        }
    }

    IEnumerator ChangeColor()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            int picker = Random.Range(0, 4);
            switch (picker)
            {
                case 0:
                    _mat.material = mat_types[0];
                    type_num = 0;
                    break;
                case 1:
                    _mat.material = mat_types[1];
                    type_num = 1;
                    break;
                case 2:
                    _mat.material = mat_types[2];
                    type_num = 2;
                    break;
                case 3:
                    _mat.material = mat_types[3];
                    type_num = 3;
                    break;
            }
            /*if (_mat.material == mat_types[0])
            {
                int picker = Random.Range(0, 2);
                switch (picker)
                {
                    case 0:
                        _mat.material = mat_types[1];
                        break;
                    case 1:
                        _mat.material = mat_types[2];
                        break;
                }
            }
            else if (_mat.material == mat_types[1])
            {
                int picker = Random.Range(0, 2);
                switch (picker)
                {
                    case 0:
                        _mat.material = mat_types[0];
                        break;
                    case 1:
                        _mat.material = mat_types[2];
                        break;
                }
            }
            else if (_mat.material == mat_types[2])
            {
                int picker = Random.Range(0, 2);
                switch (picker)
                {
                    case 0:
                        _mat.material = mat_types[0];
                        break;
                    case 1:
                        _mat.material = mat_types[1];
                        break;
                }
            }*/
        }
        
    }

    IEnumerator Move()
    {
        while (true)
        {
            randomPoint = camPosition + new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            randomMoveTime = Random.Range(10f, 15f);
            transform.DOMove(randomPoint, randomMoveTime, false);
            transform.LookAt(randomPoint);
            
            yield return new WaitForSeconds(randomMoveTime);            
        }
        
    }

    public void Die()
    {
        PhotonNetwork.Instantiate("DieParticle", transform.position, Quaternion.identity);
        _audio.Play();
        StopAllCoroutines();
        //DOTween.KillAll();
        Destroy(gameObject, 0.7f);
    }
}
