using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTracker : MonoBehaviour
{
    public Sprite Tracker;
    public TMP_Text TurnCount;
    public TMP_Text RoundCount;

    public Canvas canvas;

    public int turn_count = 0;
    public int round_count = 1;

    public void Activate() //!<Sets the sprite when called, and both counters down to their default value
    {
        this.GetComponent<SpriteRenderer>().sprite = Tracker;

        canvas = Instantiate(canvas, new Vector3(this.transform.position.x, this.transform.position.y, -1), Quaternion.identity);
        canvas.GetComponent<Canvas>();
        canvas.transform.SetParent(this.transform);

        TurnCount.GetComponent<TMP_Text>();
        TurnCount = Instantiate(TurnCount, new Vector3(-3.45f, -1.9f, -1), Quaternion.identity);
        TurnCount.transform.SetParent(this.canvas.transform, false);

        RoundCount.GetComponent<TMP_Text>();
        RoundCount = Instantiate(RoundCount, new Vector3(6.9f, -1.9f, -1), Quaternion.identity);
        RoundCount.transform.SetParent(this.canvas.transform, false);

        TurnCount.text = turn_count.ToString();
        RoundCount.text = round_count.ToString();
    }


    public void increment_turn()
    {
        turn_count++;
        TurnCount.text = this.turn_count.ToString();
    }

    public void increment_round()
    {
        round_count++;
        RoundCount.text = this.round_count.ToString();
    }

}
