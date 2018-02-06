using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTB;
using UniRx;
using UnityEngine.UI;

public class GameManager : SingleTon<GameManager>
{

    public ReactiveProperty<int> score = new ReactiveProperty<int>();

    public Text countText;
    public Text winText;


    // Use this for initialization
    void Start()
    {
        // Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
        winText.text = "";

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

    }

    // Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
    void SetCountText()
    {
        countText.text = "Count: " + score.Value.ToString();

        if (score.Value >= 12)
        {
            winText.text = "You Win!";
        }
    }
}
