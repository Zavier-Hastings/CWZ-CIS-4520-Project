using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public bool capture = false;
    public bool jumped { get; set; }

    ///references.
    public GameObject controller;
    public GameObject moveplate;

    ///positions.
    private int board_x = -1;
    private int board_y = -1;

    ///variable for different players.
    public string player { get; private set; }

    public Sprite red_checker;
    public Sprite black_checker;
    public Sprite red_king;
    public Sprite black_king;
    public Sprite red_checker_captured;
    public Sprite black_checker_captured;
    public Sprite red_king_captured;
    public Sprite black_king_captured;


    public void Activate() //!<Creates red or black checker and sets it's position when called
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        set_coords();

        if (this.name == "red_checker")
        {
            this.GetComponent<SpriteRenderer>().sprite = red_checker; player = "red"; jumped = false;
        }
        else if (this.name == "black_checker")
        {
            this.GetComponent<SpriteRenderer>().sprite = black_checker; player = "black"; jumped = false;
        }
        else if (this.name == "red_king")
        {
            this.GetComponent<SpriteRenderer>().sprite = red_king; player = "red"; jumped = false;
            controller.GetComponent<GameManager>().set_position(gameObject); //!<Kings aren't called at game start, so position must be set when they appear
        }
        else if (this.name == "black_king")
        {
            this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; jumped = false;
            controller.GetComponent<GameManager>().set_position(gameObject);
        }
    }

    public void set_coords() //!<Sets visual coordinate of checkers
    {
        float x = board_x;
        float y = board_y;


        x *= 3.75f;
        y *= 3.75f;

        x += -13.14f;
        y += -13.16f;


        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int get_board_x() //!<Returns x board position
    {
        return board_x;
    }

    public int get_board_y() //!<Returns y board position
    {
        return board_y;
    }

    public void set_board_x(int x) //!<Sets x board position
    {
        board_x = x;
    }

    public void set_board_y(int y) //!<Sets y board position
    {
        board_y = y;
    }

    public void create_r_king() //!<Creates red king and removes previous checker
    {
        GameObject ch = controller.GetComponent<GameManager>().get_position(board_x, board_y);
        Destroy(ch);
        controller.GetComponent<GameManager>().Create("red_king", board_x, board_y);
    }

    public void create_b_king() //!<Creates black king and removes previous checkeri
    {
        GameObject ch = controller.GetComponent<GameManager>().get_position(board_x, board_y);
        Destroy(ch);
        controller.GetComponent<GameManager>().Create("black_king", board_x, board_y);
    }

    private void OnMouseUp() //!<Removes and creates move plates for checkers after clicking a checker
    {
        if (!controller.GetComponent<GameManager>().is_game_over() && controller.GetComponent<GameManager>().get_current_player() == player)
        {
            if (!controller.GetComponent<GameManager>().has_checker_jumped)
            {
                remove_plates();
                checker_plates();
            }

        }
    }

    public void remove_plates() //!<Gets rid of any existing move plates
    {
        GameObject[] moveplates = GameObject.FindGameObjectsWithTag("Moveplate");
        for (int i = 0; i < moveplates.Length; i++)
        {
            Destroy(moveplates[i]);
        }
    }

    public void checker_plates() //!<Places move plates based on color of checker
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
        else if (this.name == "red_king" || this.name == "black_king")
        {
            place_plates(board_x - 1, board_y - 1);
            place_plates(board_x + 1, board_y - 1);
            place_plates(board_x - 1, board_y + 1);
            place_plates(board_x + 1, board_y + 1);
        }
    }

    public void place_plates(int x, int y) //!<When called, places move plates in correct position
    {
        GameManager sc = controller.GetComponent<GameManager>();
        if (sc.position_open(x, y))
        {
            GameObject ch = sc.get_position(x, y); //ch is an a

            if (ch == null && !this.jumped)
            {
                spawn_plates(x, y);
            }
            else if (ch != null && ch.GetComponent<Checker>().player != player) //Error points to here, when I attempt to check ch without checking if ch is null first
            {
                spawn_capture_plates(x, y);

            }
        }
    }

    public void spawn_plates(int arrayX, int arrayY, bool capture = false) //!<Spawns and places move plates visually on the board
    {
        float x = arrayX;
        float y = arrayY;

        x *= 3.75f;
        y *= 3.75f;

        x += -13.15f;
        y += -13.15f;

        GameObject mp = Instantiate(moveplate, new Vector3(x, y, -3.0f), Quaternion.identity); //!< Displays plate on screen
        if (capture)
        {
            mp.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f); //!<Changes moveplate sprite to red on capture
        }


        ///Keeps track of new position.
        movescript mpScript = mp.GetComponent<movescript>();
        mpScript.set_reference(gameObject);
        mpScript.set_coords(arrayX, arrayY);
    }


    public void spawn_capture_plates(int x, int y)
    {
        GameManager sc = controller.GetComponent<GameManager>();
        if (this.name == "red_checker") //!<Check if the piece being moved is red, so will y axis will inrease
        {
            if (x > 0 && y < 7)
            {
                if (board_x > x && (sc.get_position(x - 1, y + 1) == null) && y + 1 < 8 && x - 1 > -1)
                {
                    spawn_plates(x - 1, y + 1, true);
                }
            }
            if (x < 7 && y < 7)
            {
                if (board_x < x && (sc.get_position(x + 1, y + 1) == null) && x + 1 < 8 && y + 1 < 8)
                {
                    spawn_plates(x + 1, y + 1, true);
                }
            }
        }

        else if (this.name == "red_king" || this.name == "black_king")
        {
            if (x > 0 && y < 7)
            {
                if (board_x > x && board_y < y && (sc.get_position(x - 1, y + 1) == null) && y + 1 < 8 && x - 1 > -1)
                {
                    spawn_plates(x - 1, y + 1, true);
                }
            }
            if (x < 7 && y < 7)
            {
                if (board_x < x && board_y < y && (sc.get_position(x + 1, y + 1) == null) && x + 1 < 8 && y + 1 < 8)
                {
                    spawn_plates(x + 1, y + 1, true); //spawn capture plate northeast
                }
            }
            if (x < 7 && y > 0 && (x - 1 > -1))
            {
                if (board_x > x && board_y > y && (sc.get_position(x - 1, y - 1) == null) && x - 1 > -1 && y - 1 > -1)
                {
                    spawn_plates(x - 1, y - 1, true);
                }
            }
            if (x < 7 && y > 0)
            {
                if (board_x < x && board_y > y && (sc.get_position(x + 1, y - 1) == null) && y - 1 > -1 && x + 1 < 8)
                {
                    spawn_plates(x + 1, y - 1, true);
                }
            }
        }

        else if (this.name == "black_checker")
        {
            if (x > 0 && y > 0)
            {
                if (board_x > x && (sc.get_position(x - 1, y - 1) == null))
                {
                    spawn_plates(x - 1, y - 1, true);
                }
            }
            if (x < 7 && y > 0)
            {
                if (board_x < x && (sc.get_position(x + 1, y - 1) == null))
                {
                    spawn_plates(x + 1, y - 1, true);
                }
            }
        }


    }

}
