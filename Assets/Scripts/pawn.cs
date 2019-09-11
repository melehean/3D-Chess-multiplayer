using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pawn : chess_piece
{
    public override bool[,] is_move_possible()
    {
        bool[,] pawn_board = new bool[8, 8];
        chess_piece help_1, help_2;
        int[] en_passant_help = board_manager.instance.en_passant_move;
        //white pawn moves
        if(is_white)
        {
            //Diagonal left
            if(current_x!=0&&current_y!=7)
            {
                if(en_passant_help[0]==current_x-1&&en_passant_help[1]==current_y+1)
                    pawn_board[current_x - 1, current_y + 1] = true;
                
                help_1 = board_manager.instance.chess_pieces_on_board[current_x - 1, current_y + 1];
                if (help_1 != null && !help_1.is_white) pawn_board[current_x - 1, current_y + 1] = true;
            }

            //Diagonal right
            if (current_x != 7 && current_y != 7)
            {
                if (en_passant_help[0] == current_x + 1 && en_passant_help[1] == current_y + 1)
                    pawn_board[current_x + 1, current_y + 1] = true;
                
                help_1 = board_manager.instance.chess_pieces_on_board[current_x + 1, current_y + 1];
                if (help_1 != null && !help_1.is_white) pawn_board[current_x + 1, current_y + 1] = true;
            }

            //Middle
            if (current_y != 7)
            {
                help_1 = board_manager.instance.chess_pieces_on_board[current_x, current_y + 1];
                if (help_1 == null) pawn_board[current_x, current_y + 1] = true;
            }

            //Middle on first move
            if(current_y==1)
            {
                help_1 = board_manager.instance.chess_pieces_on_board[current_x, current_y + 2];
                help_2 = board_manager.instance.chess_pieces_on_board[current_x, current_y + 1];
                if (help_1 == null && help_2 == null) pawn_board[current_x, current_y + 2]=true;
            }
        }
        //black pawn moves
        else
        {
            //Diagonal left
            if (current_x != 0 && current_y != 0)
            {
                if (en_passant_help[0] == current_x - 1 && en_passant_help[1] == current_y - 1)
                    pawn_board[current_x - 1, current_y - 1] = true;
                
                help_1 = board_manager.instance.chess_pieces_on_board[current_x - 1, current_y - 1];
                if (help_1 != null && help_1.is_white) pawn_board[current_x - 1, current_y - 1] = true;
            }
            //Diagonal right
            if (current_x != 7 && current_y != 0)
            {
                if (en_passant_help[0] == current_x + 1 && en_passant_help[1] == current_y - 1)
                    pawn_board[current_x + 1, current_y - 1] = true;
                
                help_1 = board_manager.instance.chess_pieces_on_board[current_x + 1, current_y - 1];
                if (help_1 != null && help_1.is_white) pawn_board[current_x + 1, current_y - 1] = true;
            }

            //Middle
            if (current_y != 0)
            {
                help_1 = board_manager.instance.chess_pieces_on_board[current_x, current_y - 1];
                if (help_1 == null) pawn_board[current_x, current_y - 1] = true;
            }

            //Middle on first move
            if (current_y == 6)
            {
                help_1 = board_manager.instance.chess_pieces_on_board[current_x, current_y - 2];
                help_2 = board_manager.instance.chess_pieces_on_board[current_x, current_y - 1];
                if (help_1 == null && help_2 == null) pawn_board[current_x, current_y - 2] = true;
            }
        }

        
        return pawn_board;
    }
}
