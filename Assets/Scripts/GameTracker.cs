using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTracker : MonoBehaviour
{
    public Sprite Tracker;
    public Text TurnCount;
    public Text RoundCount;

    public int turn_count = 0;
    public int round_count = 0;

    public void Activate()
    {
        this.GetComponent<SpriteRenderer>().sprite = Tracker;
        this.TurnCount = Instantiate(TurnCount, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0), Quaternion.identity);
    }

    public void increment_turn()
    {
        turn_count++;
        this.TurnCount.text = this.turn_count.ToString();
    }

}
