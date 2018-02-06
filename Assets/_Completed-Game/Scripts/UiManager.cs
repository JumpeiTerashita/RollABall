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

    public Text countText;

    // Use this for initialization
    void Start()
    {
        win.enabled = false;
        announce.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
    public void SetCountText(int _score)
    {
        count.transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f, 1);
        count.text = "Count: " + _score.ToString();

        if (_score >= 12)
        {
            win.enabled = true;
            win.text = "You Win!";
            Sequence sequence = DOTween.Sequence();

            sequence.Append(
                win.transform.DOScale(new Vector3(3f, 3f, 3f), 0.1f)
                .SetEase(Ease.InQuint)
                );

            sequence.Append(
                  win.transform.DOScale(new Vector3(1, 1, 1), 0.5f)
                    .SetEase(Ease.InSine)
                );

            GameManager.Instance.gameClear();
        }
    }

    public void setAnnounce()
    {
        announce.enabled = true;
        announce.DOFade(0.0f, announceDurationSeconds).SetEase(announceEaseType).SetLoops(-1, LoopType.Yoyo);
    }
}
