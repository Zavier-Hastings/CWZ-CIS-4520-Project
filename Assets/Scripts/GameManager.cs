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

    public bool has_checker_jumped = false;

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

    public void set_pos_null(GameObject obj)                    //test code to set the reference to a position to null
    {
        Checker ch = obj.GetComponent<Checker>();
        positions[ch.get_board_x(), ch.get_board_y()] = null;
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

    public bool is_game_over() //!<Returns game_over. Also checks for game over conditions.
    {

        int upper_Bound_X = positions.GetUpperBound(0);
        int upper_Bound_Y = positions.GetUpperBound(1);
        for (int index_x = 0; index_x <= upper_Bound_X; index_x ++)
        {
            for (int index_y = 0; index_y <= upper_Bound_Y; index_y++)
            {
                GameObject ch = get_position(index_x, index_y);
                if (ch != null && current_player != ch.GetComponent<Checker>().player) //!<If the space being checked is a checker, and that checker is not the same color as the current player, the game is not over
                {
                    return game_over = false;
                }
            }
        }
        if (!can_red_move() && !can_black_move()) //!<If neither player can move, the game ends.
        {
            return game_over = true;
        }

        return game_over = true; //!<If all conditionals have been checked, it defaults to the game being over. As of currently, it only reaches this point if there is no checker that is the opposite color of the current player.
    }

    public void change_player() //!<When called, checks if the game is over, then changes the current player
    {
        is_game_over();
        if (current_player == "red" && can_black_move() ) //!<If black has no available moves, it skips back to red.  —>Deleted out && can_black_move()
        {
            current_player = "black";
        }
        else if (current_player == "black" && can_red_move() ) //!<If red has no available moves, it skips back to black—-> took out && can_red_move()
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

    public bool can_red_move()
    {
        int upper_Bound_X = positions.GetUpperBound(0);
        int upper_Bound_Y = positions.GetUpperBound(1);
        for (int index_x = 0; index_x <= upper_Bound_X; index_x++)
        {
            for (int index_y = 0; index_y <= upper_Bound_Y; index_y++)
            {
                GameObject ch1 = get_position(index_x, index_y);
                GameObject ch2;
                if (ch1 != null && current_player != ch1.GetComponent<Checker>().player && current_player == "black") //!<Checking possible moves for red player
                {
                    if (ch1.GetComponent<Checker>().name == "red_checker") //!<Checking possible moves for red checker
                    {
                        if ( position_open(index_x + 1,index_y + 1) ) //!<Check if there is a space to the right. If yes, continue on.
                        {
                            if ((ch2 = get_position(index_x + 1, index_y + 1)) == null) //!<Check for unblocked normal move to right
                            {
                                return true;
                            } 
                        }
                        
                        if ( position_open(index_x + 2, index_y + 2) && position_open(index_x + 1, index_y + 1) ) //!<Check if there is space to jump over a checker
                        {
                            if ((ch2 = get_position(index_x + 1, index_y + 1)).GetComponent<Checker>().player == current_player && (ch2 = get_position(index_x + 2, index_y + 2)) == null) //!<Check for unblocked jump to right
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x - 1, index_y + 1)) //!<Check if there is space to left
                        {
                            if ((ch2 = get_position(index_x - 1, index_y + 1)) == null) //!<Check for unblocked normal move to left
                            {
                                return true;
                            }
                        }
                        
                        if ( position_open(index_x - 2, index_y + 2) && position_open(index_x - 1, index_y + 1) ) //Check if there is space to jump over a checker
                        {
                            if ((ch2 = get_position(index_x - 1, index_y + 1)).GetComponent<Checker>().player == current_player && (ch2 = get_position(index_x - 2, index_y + 2)) == null) //!<Check for unblocked jump to left
                            {
                                return true;
                            }
                        }
                    } else if (ch1.GetComponent<Checker>().name == "red_king")
                    {
                        if (position_open(index_x + 1, index_y + 1)) //!<Check if there is a space to the right. If yes, continue on.
                        {
                            if ((ch2 = get_position(index_x + 1, index_y + 1)) == null) //!<Check for unblocked normal move to right
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x + 2, index_y + 2) && position_open(index_x + 1, index_y + 1)) //!<Check if there is space to jump over a checker
                        {
                            if ((ch2 = get_position(index_x + 1, index_y + 1)).GetComponent<Checker>().player == current_player && (ch2 = get_position(index_x + 2, index_y + 2)) == null) //!<Check for unblocked jump to right
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x - 1, index_y + 1)) //!<Check if there is space to left
                        {
                            if ((ch2 = get_position(index_x - 1, index_y + 1)) == null) //!<Check for unblocked normal move to left
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x - 2, index_y + 2) && position_open(index_x - 1, index_y + 1)) //Check if there is space to jump over a checker
                        {
                            if ((ch2 = get_position(index_x - 1, index_y + 1)).GetComponent<Checker>().player == current_player && (ch2 = get_position(index_x - 2, index_y + 2)) == null) //!<Check for unblocked jump to left
                            {
                                return true;
                            }
                        } 
                        
                        if (position_open(index_x + 1, index_y - 1)) //!<Check if there is a space to the right. If yes, continue on.
                        {
                            if ((ch2 = get_position(index_x + 1, index_y - 1)) == null) //!<Check for unblocked normal move to right
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x + 2, index_y - 2) && position_open(index_x + 1, index_y - 1)) //!<Check if there is space to jump over a checker
                        {
                            if ((ch2 = get_position(index_x + 1, index_y - 1)).GetComponent<Checker>().player == current_player && (ch2 = get_position(index_x + 2, index_y - 2)) == null) //!<Check for unblocked jump to right
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x - 1, index_y - 1)) //!<Check if there is space to left
                        {
                            if ((ch2 = get_position(index_x - 1, index_y - 1)) == null) //!<Check for unblocked normal move to left
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x - 2, index_y - 2) && position_open(index_x - 1, index_y - 1)) //Check if there is space to jump over a checker
                        {
                            if ((ch2 = get_position(index_x - 1, index_y - 1)).GetComponent<Checker>().player == current_player && (ch2 = get_position(index_x - 2, index_y - 2)) == null) //!<Check for unblocked jump to left
                            {
                                return true;
                            }
                        }
                    }
                }            
            }
        }
        return false; //!<If no condition was met, red cannot move and the turn should go to black.
    }

    public bool can_black_move()
    {
        int upper_Bound_X = positions.GetUpperBound(0);
        int upper_Bound_Y = positions.GetUpperBound(1);
        for (int index_x = 0; index_x <= upper_Bound_X; index_x++)
        {
            for (int index_y = 0; index_y <= upper_Bound_Y; index_y++)
            {
                GameObject ch1 = get_position(index_x, index_y);
                GameObject ch2;
                if (ch1 != null && current_player != ch1.GetComponent<Checker>().player && current_player == "red") //!<Checking moves for black player
                {
                    if (ch1.GetComponent<Checker>().name == "black_checker") //!<Checking possible moves for red checker
                    {
                        if (position_open(index_x + 1, index_y - 1)) //!<Check if there is a space to the right. If yes, continue on.
                        {
                            if ((ch2 = get_position(index_x + 1, index_y - 1)) == null) //!<Check for unblocked normal move to right
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x + 2, index_y - 2) && position_open(index_x + 1, index_y - 1)) //!<Check if there is space to jump over a checker
                        {
                            if ((ch2 = get_position(index_x + 1, index_y - 1)).GetComponent<Checker>().player == current_player && (ch2 = get_position(index_x + 2, index_y - 2)) == null) //!<Check for unblocked jump to right
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x - 1, index_y - 1)) //!<Check if there is space to left
                        {
                            if ((ch2 = get_position(index_x - 1, index_y - 1)) == null) //!<Check for unblocked normal move to left
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x - 2, index_y - 2) && position_open(index_x - 1, index_y - 1)) //Check if there is space to jump over a checker
                        {
                            if ((ch2 = get_position(index_x - 1, index_y - 1)).GetComponent<Checker>().player == current_player && (ch2 = get_position(index_x - 2, index_y - 2)) == null) //!<Check for unblocked jump to left
                            {
                                return true;
                            }
                        }
                    }
                    else if (ch1.GetComponent<Checker>().name == "black_king")
                    {
                        if (position_open(index_x + 1, index_y + 1)) //!<Check if there is a space to the right. If yes, continue on.
                        {
                            if ((ch2 = get_position(index_x + 1, index_y + 1)) == null) //!<Check for unblocked normal move to right
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x + 2, index_y + 2) && position_open(index_x + 1, index_y + 1)) //!<Check if there is space to jump over a checker
                        {
                            if ((ch2 = get_position(index_x + 1, index_y + 1)).GetComponent<Checker>().player == current_player && (ch2 = get_position(index_x + 2, index_y + 2)) == null) //!<Check for unblocked jump to right
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x - 1, index_y + 1)) //!<Check if there is space to left
                        {
                            if ((ch2 = get_position(index_x - 1, index_y + 1)) == null) //!<Check for unblocked normal move to left
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x - 2, index_y + 2) && position_open(index_x - 1, index_y + 1)) //Check if there is space to jump over a checker
                        {
                            if ((ch2 = get_position(index_x - 1, index_y + 1)).GetComponent<Checker>().player == current_player && (ch2 = get_position(index_x - 2, index_y + 2)) == null) //!<Check for unblocked jump to left
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x + 1, index_y - 1)) //!<Check if there is a space to the right. If yes, continue on.
                        {
                            if ((ch2 = get_position(index_x + 1, index_y - 1)) == null) //!<Check for unblocked normal move to right
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x + 2, index_y - 2) && position_open(index_x + 1, index_y - 1)) //!<Check if there is space to jump over a checker
                        {
                            if ((ch2 = get_position(index_x + 1, index_y - 1)).GetComponent<Checker>().player == current_player && (ch2 = get_position(index_x + 2, index_y - 2)) == null) //!<Check for unblocked jump to right
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x - 1, index_y - 1)) //!<Check if there is space to left
                        {
                            if ((ch2 = get_position(index_x - 1, index_y - 1)) == null) //!<Check for unblocked normal move to left
                            {
                                return true;
                            }
                        }
                        
                        if (position_open(index_x - 2, index_y - 2) && position_open(index_x - 1, index_y - 1)) //Check if there is space to jump over a checker
                        {
                            if ((ch2 = get_position(index_x - 1, index_y - 1)).GetComponent<Checker>().player == current_player && (ch2 = get_position(index_x - 2, index_y - 2)) == null) //!<Check for unblocked jump to left
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false; //!<If no condition was met, black player cannot move and turn should go to red
    }

}
