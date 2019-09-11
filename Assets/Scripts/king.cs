using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class king : chess_piece
{
    public override bool[,] is_move_possible()
    {
        int i, j;
        bool[,] king_board = new bool[8, 8];


        for (i = -1; i <= 1;i++)
            for (j = -1; j <= 1; j++) 
                king_move(current_x + i,current_y + j, ref king_board);

        //Castling

        if (is_white == true) castling(0, ref king_board);
        else castling(7,ref king_board);

        return king_board;
    }

    private void king_move(int x, int y, ref bool[,] board)
    {
        chess_piece help;
        if (is_on_board(x, y))
        {
            help = board_manager.instance.chess_pieces_on_board[x, y];
            if (help == null) board[x, y] = true;
            else if (help.is_white != this.is_white) board[x, y] = true;
        }
    }

    private void castling(int y, ref bool[,] board)
    {
        chess_piece help;
        bool is_free_space;
        int i;
        if (current_x == 4 && current_y == y)
        {
            //Left castling
            help = board_manager.instance.chess_pieces_on_board[0, y];
            if (help != null && help.GetType() == typeof(rook))
            {
                is_free_space = true;
                for (i = 1; i <= 3; i++)
                {
                    help = board_manager.instance.chess_pieces_on_board[i, y];
                    if (help != null) is_free_space = false;
                }
                if (is_free_space == true) board[2, y] = true;
            }
            //Right castling
            help = board_manager.instance.chess_pieces_on_board[7, y];
            if (help != null && help.GetType() == typeof(rook))
            {
                is_free_space = true;
                for (i = 5; i <= 6; i++)
                {
                    help = board_manager.instance.chess_pieces_on_board[i, y];
                    if (help != null) is_free_space = false;
                }
                if (is_free_space == true) board[6, y] = true;
            }
        }
    }
}
