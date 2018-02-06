using System.Collections;
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

    public Text countText;
    public Text winText;
    public Text announceText;

    public float waitSeconds = 2.0f;
    public bool isGameCleared;

    public ReactiveProperty<bool> canInput = new ReactiveProperty<bool>();

    // Use this for initialization
    void Start()
    {

        // Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
        winText.text = "";
        announceText.enabled = false;
        isGameCleared = false;
        canInput.Subscribe(_canInput =>
        {
            playerObject.GetComponent<PlayerController>().canInput = _canInput;
            cameraObject.GetComponent<CameraController>().canInput = _canInput;
        }
       );
        canInput.Value = true;

        score.Subscribe(_ =>
            {
                Debug.Log(score);
                SetCountText();
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

    // Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
    void SetCountText()
    {
        countText.text = "Count: " + score.Value.ToString();

        if (score.Value >= 12)
        {
            winText.text = "You Win!";
            canInput.Value = false;
            StartCoroutine(WaitTimer());
        }
    }

    IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(waitSeconds);
        isGameCleared = true;
        announceText.enabled = true;
        //StartCoroutine(WaitingInput());
        yield break;
    }


}
