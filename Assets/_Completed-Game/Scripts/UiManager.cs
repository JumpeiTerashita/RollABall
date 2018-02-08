using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTB;
using UnityEngine.UI;
using DG.Tweening;

public class UiManager : SingleTon<UiManager>
{

    public Text count;
    public Text win;
    public Text announce;

    public float announceDurationSeconds;
    public Ease announceEaseType;

    public Text startCountDown;

    public int startCount = 3;

    private int targetNum;

    private bool isCountSeqRunning;

    private Sequence countTextSequence;
    private Sequence winTextSequence;

    // Use this for initialization
    void Start()
    {
        win.enabled = false;
        announce.enabled = false;
        targetNum = GameManager.Instance.targetNum;
        StartCoroutine(CountDown(startCount));
        isCountSeqRunning = false;
        sequenceSetup();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
    public void SetCountText(int _score)
    {
        count.text = "Count: " + _score.ToString() + " / " + targetNum.ToString();

        // countTextSequence

        if (isCountSeqRunning)
        {
            countTextSequence.Restart();
            countTextSequence.Pause();
            isCountSeqRunning = false;
        }
        
        isCountSeqRunning = true;
        countTextSequence = DOTween.Sequence();

        countTextSequence.Append(
            count.transform.DOPunchScale(
                new Vector3(
                    1.1f,
                    1.1f,
                    1.1f
                    )
                    , 0.5f, 1
                )
                .OnComplete(
                    () =>
                    {
                        //Debug.Log("countSeq Ended");
                        isCountSeqRunning = false;
                    }
                )
            );




        if (_score >= targetNum)
        {
            win.enabled = true;
            win.text = "You Win!";

            winTextSequence.Play();

            GameManager.Instance.gameClear();
        }
    }

    public void setAnnounce()
    {
        announce.enabled = true;
        announce.DOFade(0.0f, announceDurationSeconds).SetEase(announceEaseType).SetLoops(-1, LoopType.Yoyo);
    }

    void sequenceSetup()
    {
        // sequence
        // あらかじめセットアップしといて使いまわそうと思ったけど
        // 同じシークエンスは一回しか使えないっぽい

        // winText

        winTextSequence = DOTween.Sequence();

        winTextSequence.Append(
            win.transform.DOScale(new Vector3(3f, 3f, 3f), 0.1f)
            .SetEase(Ease.InQuint)
            );

        winTextSequence.Append(
              win.transform.DOScale(new Vector3(1, 1, 1), 0.5f)
                .SetEase(Ease.InSine)
            );

        winTextSequence.Pause();
    }

    IEnumerator CountDown(int _countDown)
    {
        //  文字変更
        if (_countDown == 0)
        {
            startCountDown.text = "Game Start !";
        }
        else startCountDown.text = _countDown.ToString();

        //  サイズ初期化
        startCountDown.transform.localScale = new Vector3(2, 2, 2);

        //  サイズ減少開始
        startCountDown.transform.DOScale(new Vector3(0, 0, 0), 1.0f)
                    .SetEase(Ease.InSine);

        //  カウントダウン待機（1秒）
        yield return new WaitForSeconds(1);

        //  カウント -1以下なら
        //  操作可能にしコルーチン終了
        if (_countDown - 1 < 0)
        {
            startCountDown.enabled = false;
            GameManager.Instance.canInput.Value = true;
            yield break;
        }

        //  カウント -1以下でないならコルーチンループ
        StartCoroutine(CountDown(_countDown - 1));

    }
}
