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

    public float waitSeconds = 2.0f;

    [System.NonSerialized]
    public bool isGameCleared;

    public ReactiveProperty<bool> canInput = new ReactiveProperty<bool>();

    public int targetNum;

    // Use this for initialization
    void Start()
    {
        // Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
        SoundManager.Instance.Init();
        SoundManager.Instance.PlayBgm("blueneon1", true,1.0f);
        isGameCleared = false;
        canInput.Subscribe(_canInput =>
        {
            playerObject.GetComponent<PlayerController>().canInput = _canInput;
            cameraObject.GetComponent<KTB.CameraController>().canInput = _canInput;
        }
       );

        //  UiManagerで行われる
        //  カウントダウン終了まで操作不可に
        canInput.Value = false;

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
        SoundManager.Instance.PlaySe("jingle06",false,Vector3.zero);
        yield return new WaitForSeconds(waitSeconds);
        isGameCleared = true;
        UiManager.Instance.setAnnounce();
        //StartCoroutine(WaitingInput());
        yield break;
    }


}
