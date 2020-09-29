using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movescript : MonoBehaviour
{
    //reference variables
    public GameObject controller;

    GameObject reference = null;

    //Board positions
    int arrayX;
    int arrayY;

    //false = movement, true = capture
    public bool capture = false;

    public void Start()
    {
        if (capture)
        {
            //change moveplate sprite to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    //removes checker and moves it to new space when moevplate is clicked, and destroys checkers that are captured
    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (capture)
        {
            GameObject ch = controller.GetComponent<GameManager>().get_position(arrayX, arrayY);
            Destroy(ch);
        }

        controller.GetComponent<GameManager>().move_space(reference.GetComponent<Checker>().get_board_x(),
            reference.GetComponent<Checker>().get_board_y());

        //set new space for piece
        reference.GetComponent<Checker>().set_board_x(arrayX);
        reference.GetComponent<Checker>().set_board_y(arrayY);
        //keep track of new location
        reference.GetComponent<Checker>().set_coords();
        //controller tracks new position as well
        controller.GetComponent<GameManager>().set_position(reference);

        reference.GetComponent<Checker>().remove_plates();
    }

    //sets coordinates for moveplates
    public void set_coords(int x, int y)
    {
        arrayX = x;
        arrayY = y;
    }

    //sets reference
    public void set_reference(GameObject obj)
    {
        reference = obj;
    }

    //gives reference
    public GameObject get_reference()
    {
        return reference;
    }
}
