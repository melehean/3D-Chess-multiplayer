using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class queen : chess_piece
{
    public override bool[,] is_move_possible()
    {
        int i;
        bool[,] queen_board = new bool[8, 8];

        //Horizontally right
        for (i = 1; i <= 7; i++)
            if (queen_move(current_x + i, current_y, ref queen_board) == false) break;
        //Horizontally left
        for (i = 1; i <= 7; i++)
            if (queen_move(current_x - i, current_y, ref queen_board) == false) break;
        //Vertically up
        for (i = 1; i <= 7; i++)
            if (queen_move(current_x, current_y + i, ref queen_board) == false) break;
        //Vertically down
        for (i = 1; i <= 7; i++)
            if (queen_move(current_x, current_y - i, ref queen_board) == false) break;

        //Right up
        for (i = 1; i <= 7; i++)
            if (queen_move(current_x + i, current_y + i, ref queen_board) == false) break;
        //Left up
        for (i = 1; i <= 7; i++)
            if (queen_move(current_x - i, current_y + i, ref queen_board) == false) break;
        //Right down
        for (i = 1; i <= 7; i++)
            if (queen_move(current_x + i, current_y - i, ref queen_board) == false) break;
        //Left down
        for (i = 1; i <= 7; i++)
            if (queen_move(current_x - i, current_y - i, ref queen_board) == false) break;

        return queen_board;
    }

    private bool queen_move(int x, int y, ref bool[,] board)
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
