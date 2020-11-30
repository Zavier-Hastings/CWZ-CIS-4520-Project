using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTracker : MonoBehaviour
{
    public Sprite Tracker;
    public TMP_Text TurnCount;

    public Canvas canvas;

    public int turn_count = 0;

    public void Activate() //!<Sets the sprite when called, and both counters down to their default value
    {
        this.GetComponent<SpriteRenderer>().sprite = Tracker;

        canvas = Instantiate(canvas, new Vector3(this.transform.position.x, this.transform.position.y, -1), Quaternion.identity); //!<Creates a canvas directly where the gametracker is placed
        canvas.GetComponent<Canvas>();
        canvas.transform.SetParent(this.transform); //!<Sets the parent to the gametracker object

        TurnCount.GetComponent<TMP_Text>();
        TurnCount = Instantiate(TurnCount, new Vector3(1.6f, -2f, -1), Quaternion.identity); //!<Creates a text object to hold the current turn number
        TurnCount.transform.SetParent(this.canvas.transform, false); //!<Sets the parent to the canvas so the text can be displayed in the right position
        
        TurnCount.text = turn_count.ToString(); //!<Reset turn count
    }


    public void increment_turn()
    {
        turn_count++;
        TurnCount.text = this.turn_count.ToString();
    }

}
