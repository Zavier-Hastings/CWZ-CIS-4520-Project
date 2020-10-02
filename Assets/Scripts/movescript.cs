using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movescript : MonoBehaviour
{
    ///Reference variables.
    public GameObject controller;

    GameObject reference = null;

    ///Board positions.
    int arrayX;
    int arrayY;

    public bool capture = false; //!<false = movement, true = capture
    
    public void Start()
    {
        if (capture)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f); //!<Changes moveplate sprite to red on capture
        }
    }

    public void OnMouseUp() //!<When move plate is clicked, moves checker to the move plate's spot, removes move plates, and destroys captured checkers
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (capture)
        {
            GameObject ch = controller.GetComponent<GameManager>().get_position(arrayX, arrayY);
            Destroy(ch);
        }

        controller.GetComponent<GameManager>().move_space(reference.GetComponent<Checker>().get_board_x(),
            reference.GetComponent<Checker>().get_board_y());

        ///Set new space for piece.
        reference.GetComponent<Checker>().set_board_x(arrayX);
        reference.GetComponent<Checker>().set_board_y(arrayY);
        ///Keep track of new location.
        reference.GetComponent<Checker>().set_coords();
        ///Controller tracks new position as well.
        controller.GetComponent<GameManager>().set_position(reference);

        reference.GetComponent<Checker>().remove_plates();
    }

    public void set_coords(int x, int y) //!<Sets move plate coordinates
    {
        arrayX = x;
        arrayY = y;
    }

    public void set_reference(GameObject obj) //!<Sets reference
    {
        reference = obj;
    }

    public GameObject get_reference() //!<Returns reference
    {
        return reference;
    }
}
