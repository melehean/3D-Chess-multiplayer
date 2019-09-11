using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bishop : chess_piece
{
    public override bool[,] is_move_possible()
    {
        int i;
        bool[,] bishop_board = new bool [8,8];
        //Right up
        for (i = 1; i <= 7; i++)
            if (bishop_move(current_x + i, current_y + i, ref bishop_board) == false) break;
        //Left up
        for (i = 1; i <= 7; i++)
            if (bishop_move(current_x - i, current_y + i, ref bishop_board) == false) break;
        //Right down
        for (i = 1; i <= 7; i++)
            if (bishop_move(current_x + i, current_y - i, ref bishop_board) == false) break;
        //Left down
        for (i = 1; i <= 7; i++)
            if (bishop_move(current_x - i, current_y - i, ref bishop_board) == false) break;

        return bishop_board;
    }

    private bool bishop_move(int x, int y, ref bool[,] board)
    {
        chess_piece help;
        if (is_on_board(x, y))
        {
            help = board_manager.instance.chess_pieces_on_board[x, y];
            if (help == null)
            {
                board[x, y] = true;
                return true;
            }
            else if (help.is_white != this.is_white) board[x, y] = true;
        }
        return false;
    }
}
