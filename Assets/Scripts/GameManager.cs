using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    ///allows prefab checker to be used as checker_piece in script.
    public GameObject checker_piece; 

    ///positions and number of checkers for each player.
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] player_red = new GameObject[12];
    private GameObject[] player_black = new GameObject[12];

    private string current_player = "red"; //!<Starting player

    private bool game_over = false; //!<Variable for game over

    void Start() //!<On play, creates checkers and puts them in their appropriate positions in the array
    {

        player_red = new GameObject[]
        {
            Create("red_checker",0,0),
            Create("red_checker",0,2),
            Create("red_checker",1,1),
            Create("red_checker",2,0),
            Create("red_checker",2,2),
            Create("red_checker",3,1),
            Create("red_checker",4,0),
            Create("red_checker",4,2),
            Create("red_checker",5,1),
            Create("red_checker",6,0),
            Create("red_checker",6,2),
            Create("red_checker",7,1),
        };

        player_black = new GameObject[]
        {
            Create("black_checker",0,6),
            Create("black_checker",1,7),
            Create("black_checker",1,5),
            Create("black_checker",2,6),
            Create("black_checker",3,7),
            Create("black_checker",3,5),
            Create("black_checker",4,6),
            Create("black_checker",5,7),
            Create("black_checker",5,5),
            Create("black_checker",6,6),
            Create("black_checker",7,7),
            Create("black_checker",7,5),
        };

        for (int i = 0; i < player_black.Length; i++)
        {
            set_position(player_red[i]);
            set_position(player_black[i]);
        }
    }

    public GameObject Create(string name, int x, int y) //!<Creates checker piece and sets name and board position
    {
        GameObject obj = Instantiate(checker_piece, new Vector3(0, 0, -1), Quaternion.identity);
        Checker ch = obj.GetComponent<Checker>();
        ch.name = name;
        ch.set_board_x(x);
        ch.set_board_y(y);
        ch.Activate();
        return obj;
    }

        public void set_position(GameObject obj) //!<Sets position of checkers
    {
        Checker ch = obj.GetComponent<Checker>();
        positions[ch.get_board_x(), ch.get_board_y()] = obj;
    }

    public void move_space(int x, int y) //!<Removes old checker when checkers are moved
    {
        positions[x, y] = null;
    }

    public GameObject get_position(int x, int y) //!<Returns positions
    {
        return positions[x, y];
    }

    public bool position_open(int x, int y) //!<Checks if a position is occupied or empty
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public string get_current_player() //!<Returns current player
    {
        return current_player;
    }

    public bool is_game_over() //!<Returns game_over
    {
        return game_over;
    }

    public void change_player() //!<When called, changes the current player
    {
        if (current_player == "red")
        {
            current_player = "black";
        } else
        {
            current_player = "red";
        }
    }

    public void Update() //!<On update, checks if the game is over and if the player is left clicking. Currently only restarts game. 
    {
        if (game_over == true && Input.GetMouseButtonDown(0) )
        {
            game_over = false;

            SceneManager.LoadScene("SampleScene");
        }
    }
}
