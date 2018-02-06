﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTB;
using UniRx;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public GameObject playerObject;
    public GameObject cameraObject;

    public ReactiveProperty<int> score = new ReactiveProperty<int>();

    public float waitSeconds = 2.0f;
    public bool isGameCleared;

    public ReactiveProperty<bool> canInput = new ReactiveProperty<bool>();

    // Use this for initialization
    void Start()
    {
        // Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
       
        isGameCleared = false;
        canInput.Subscribe(_canInput =>
        {
            playerObject.GetComponent<PlayerController>().canInput = _canInput;
            cameraObject.GetComponent<CameraController>().canInput = _canInput;
        }
       );
        canInput.Value = true;

        score.Subscribe(_score =>
            {
                Debug.Log(_score);
                UiManager.Instance.SetCountText(_score);
            }
        );

    }

    // Update is called once per frame
    void Update()
    {
        if (isGameCleared && Input.anyKeyDown)
        {
            SceneManager.LoadScene("Roll-a-ball");
        }
    }

    public void gameClear()
    {
        canInput.Value = false;
        StartCoroutine(WaitEndTimer());
    }

    IEnumerator WaitEndTimer()
    {
        yield return new WaitForSeconds(waitSeconds);
        isGameCleared = true;
        UiManager.Instance.setAnnounce();
        //StartCoroutine(WaitingInput());
        yield break;
    }


}
