using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knight : chess_piece
{
    public override bool[,] is_move_possible()
    {
        bool[,] knight_board = new bool[8, 8];

        knight_move(current_x + 1, current_y + 2,ref knight_board);

        knight_move(current_x - 1, current_y + 2,ref knight_board);

        knight_move(current_x + 1, current_y - 2, ref knight_board);

        knight_move(current_x - 1, current_y - 2, ref knight_board);

        knight_move(current_x + 2, current_y + 1, ref knight_board);

        knight_move(current_x - 2, current_y + 1, ref knight_board);

        knight_move(current_x + 2, current_y - 1, ref knight_board);

        knight_move(current_x - 2, current_y - 1, ref knight_board);

        return knight_board;
    }

    private void knight_move(int x, int y, ref bool [,] board)
    {
        chess_piece help;
        if (is_on_board(x,y))
        {
            help = board_manager.instance.chess_pieces_on_board[x , y];
            if(help==null)board[x, y] = true;
            else if (help.is_white != this.is_white) board[x, y] = true;
        } 
    }

}
