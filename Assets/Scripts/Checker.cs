using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    //references
    public GameObject controller;
    public GameObject moveplate;

    //positions
    private int board_x = -1;
    private int board_y = -1;

    //variable for different players
    private string player;

    public Sprite red_checker;
    public Sprite black_checker;

    //Creates red or black checker and sets it's position when called
    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        set_coords();

        if (this.name == "red_checker")
        {
            this.GetComponent<SpriteRenderer>().sprite = red_checker; player = "red";
        }
        else if (this.name == "black_checker")
        {
            this.GetComponent<SpriteRenderer>().sprite = black_checker; player = "black";
        }
    }

    //sets visual coordinates of checkers
    public void set_coords()
    {
        float x = board_x;
        float y = board_y;


        x *= 3.75f;
        y *= 3.75f;

        x += -13.14f;
        y += -13.16f;


        this.transform.position = new Vector3(x, y, -1.0f);
    }

    //returns board position x
    public int get_board_x()
    {
        return board_x;
    }

    //returns board position y
    public int get_board_y()
    {
        return board_y;
    }

    //sets board position x
    public void set_board_x(int x)
    {
        board_x = x;
    }

    //sets board position y
    public void set_board_y(int y)
    {
        board_y = y;
    }

    //removes and creates move plates for checkers on click
    private void OnMouseUp()
    {
        remove_plates();
        checker_plates();
    }

    //gets rid of existing move plates
    public void remove_plates()
    {
        GameObject[] moveplates = GameObject.FindGameObjectsWithTag("Moveplate");
        for (int i = 0; i < moveplates.Length; i++)
        {
            Destroy(moveplates[i]);
        }
    }

    //creates plates depending on checker clicked
    public void checker_plates()
    {
        if (this.name == "red_checker")
        {
            place_plates(board_x - 1, board_y + 1);
            place_plates(board_x + 1, board_y + 1);
        }
        else if (this.name == "black_checker")
        {
            place_plates(board_x - 1, board_y - 1);
            place_plates(board_x + 1, board_y - 1);
        }
    }

    //places the plates in the right spot when called
    public void place_plates(int x, int y)
    {
        GameManager sc = controller.GetComponent<GameManager>();
        if (sc.position_open(x, y))
        {
            GameObject ch = sc.get_position(x, y);

            if (ch == null)
            {
                spawn_plates(x, y);
            }
            else if (ch.GetComponent<Checker>().player != player)
            {
                spawn_capture_plates(x, y);
            }
        }
    }

    //spawns the plates into existence and sets their visual placement
    public void spawn_plates(int arrayX, int arrayY)
    {
        float x = arrayX;
        float y = arrayY;

        x *= 3.75f;
        y *= 3.75f;

        x += -13.15f;
        y += -13.15f;

        //displays plate on screen
        GameObject mp = Instantiate(moveplate, new Vector3(x, y, -3.0f), Quaternion.identity);

        //keep track of new position
        movescript mpScript = mp.GetComponent<movescript>();
        mpScript.set_reference(gameObject);
        mpScript.set_coords(arrayX, arrayY);
    }

    //to create capture plates properly, there will need to be two spawn plates
    //for a movement to the left, and a movement to the right, and this will
    //have to be done continously until there are no more jumps left. Will also
    //have to delete capture plates as they are created until we reach the end
    //of that jump. In addition, combine with move plate spawn
    public void spawn_capture_plates(int x, int y)
    {

    }
}