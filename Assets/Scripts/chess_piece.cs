using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class chess_piece : MonoBehaviour
{
    public int current_x{get;set;}
    public int current_y { get; set; }
    public bool is_white;

    public void set_position(int x, int y)
    {
        current_x = x;
        current_y = y;
    }

    public virtual bool[,] is_move_possible()
    {
        return new bool [8,8];
    }

    public bool is_on_board(int x, int y)
    {
        if (x >= 0 && x <= 7 && y >= 0 && y <= 7) return true;
        return false;
    }


}
