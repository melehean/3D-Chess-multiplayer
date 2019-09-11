using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board_manager : MonoBehaviour
{
    public static board_manager instance { get; set; }
    private bool[,] allowed_moves { get; set; }
    public chess_piece[,] chess_pieces_on_board { get; set; }
    private chess_piece selected_chess_piece{get; set;}

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    private int selected_x = -1;
    private int selected_y = -1;

    public int[] en_passant_move{get;set;}

    public List<GameObject> chess_pieces_help;
    private List<GameObject> chess_pieces;




    public bool is_white_turn = true;

    private void Start()
    {
        instance = this;
        spawn_all_chess_pieces();
    }

    private void Update()
    {
        update_selection();
        draw_chess_board();
        if(Input.GetMouseButtonDown(0))
        {
            if(selected_x>=0&&selected_y>=0)
            {
                if(selected_chess_piece == null)
                    select_chess_piece(selected_x, selected_y);
                else
                    move_chess_piece(selected_x, selected_y);
            }
        }
    }

    private void select_chess_piece(int x, int y)
    {
        if (chess_pieces_on_board[x, y] == null) return;
        if (chess_pieces_on_board[x, y].is_white != is_white_turn) return;

        bool has_at_least_one_move = false;
        int i, j;
        allowed_moves = chess_pieces_on_board[x, y].is_move_possible();
        for (i = 0; i < 8; i++)
            for (j = 0; j < 8; j++)
                if (allowed_moves[i,j] == true) has_at_least_one_move = true;
        if(has_at_least_one_move)
        {
            selected_chess_piece = chess_pieces_on_board[x, y];
            board_highlights.instance.highlight_selected_tail(new Vector3(x + 0.5f, -0.029f, y + 0.5f));
            board_highlights.instance.highlight_allowed_moves(allowed_moves);
        }

    }

    private void move_chess_piece(int x, int y)
    {
        if(allowed_moves[x,y])
        {
            chess_piece help = chess_pieces_on_board[x, y];
            if(help!=null && help.is_white!=is_white_turn)
            {
                //Capture a piece

                //If this is a king
                if(help.GetType()==typeof(king))
                {
                    endgame();
                    return;
                }

                chess_pieces.Remove(help.gameObject);
                Destroy(help.gameObject);
            }

            if(x==en_passant_move[0]&&y==en_passant_move[1])
            {
                if(is_white_turn) help = chess_pieces_on_board[x, y-1];
                else help = chess_pieces_on_board[x, y + 1];
                chess_pieces.Remove(help.gameObject);
                Destroy(help.gameObject);
            }
            en_passant_move[0] = en_passant_move[1] = -1;
            if(selected_chess_piece.GetType()==typeof(pawn))
            {
                if(y==7)
                {
                    chess_pieces.Remove(selected_chess_piece.gameObject);
                    Destroy(selected_chess_piece.gameObject);
                    spawn_chess_piece(1, x, y, true);
                    selected_chess_piece = chess_pieces_on_board[x, y];
                }
                if(y==0)
                {
                    chess_pieces.Remove(selected_chess_piece.gameObject);
                    Destroy(selected_chess_piece.gameObject);
                    spawn_chess_piece(7, x, y, false);
                    selected_chess_piece = chess_pieces_on_board[x, y];
                }
                if(selected_chess_piece.current_y == 1 && y == 3)
                {
                    en_passant_move[0] = x;
                    en_passant_move[1] = y-1;
                }
                else if(selected_chess_piece.current_y == 6 && y==4)
                {
                    en_passant_move[0] = x;
                    en_passant_move[1] = y+1;
                }
            }

            if(selected_chess_piece.GetType()==typeof(king))
            {
                if(is_white_turn&&x==2&&y==0)
                {
                    help = chess_pieces_on_board[0, 0];
                    chess_pieces_on_board[0, 0] = null;
                    help.transform.position = get_tile_center(3, 0);
                    help.set_position(3, 0);
                    chess_pieces_on_board[3, 0] = help;
                }
                else if(is_white_turn&&x==6&&y==0)
                {
                    help = chess_pieces_on_board[7, 0];
                    chess_pieces_on_board[7, 0] = null;
                    help.transform.position = get_tile_center(5, 0);
                    help.set_position(5, 0);
                    chess_pieces_on_board[5, 0] = help;
                }
                else if (!is_white_turn && x == 2 && y == 7)
                {
                    help = chess_pieces_on_board[0, 7];
                    chess_pieces_on_board[0, 7] = null;
                    help.transform.position = get_tile_center(3, 7);
                    help.set_position(3, 7);
                    chess_pieces_on_board[3, 7] = help;
                }
                else if (!is_white_turn && x == 6 && y == 7)
                {
                    help = chess_pieces_on_board[7, 7];
                    chess_pieces_on_board[7, 7] = null;
                    help.transform.position = get_tile_center(5, 7);
                    help.set_position(5, 7);
                    chess_pieces_on_board[5, 7] = help;
                }
            }

            chess_pieces_on_board[selected_chess_piece.current_x, selected_chess_piece.current_y] = null;
            selected_chess_piece.transform.position = get_tile_center(x, y);
            selected_chess_piece.set_position(x, y);
            chess_pieces_on_board[x, y] = selected_chess_piece;

            if (is_white_turn) Camera.main.transform.SetPositionAndRotation(new Vector3(4, 5, 9.2f), Quaternion.Euler(50, 180, 0));
            else Camera.main.transform.SetPositionAndRotation(new Vector3(4, 5, -1.2f), Quaternion.Euler(50, 0, 0));

            is_white_turn = !is_white_turn;
            
            
        }
        board_highlights.instance.hide_highlight();
        selected_chess_piece = null;
    }

    private void update_selection()
    {
        if (!Camera.main) return;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("chess_plane")))
        {
            selected_x = (int)hit.point.x;
            selected_y = (int)hit.point.z;
        }
        else selected_x = selected_y = -1;

    }

    private void draw_chess_board()
    {
        int i,j;
        Vector3 width_line = Vector3.right * 8;
        Vector3 height_line = Vector3.forward * 8;
        Vector3 start;
        for(i=0;i<=8;i++)
        {
            start=Vector3.forward * i;
            Debug.DrawLine(start, start+width_line);
            for(j=0;j<=8;j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + height_line);
            }
        }

        //Draw the selection
        if(selected_x>=0&&selected_y>=0)
        {
            Debug.DrawLine(Vector3.forward * selected_y + Vector3.right * selected_x,
                Vector3.forward * (selected_y + 1) + Vector3.right * (selected_x + 1));
            Debug.DrawLine(Vector3.forward * selected_y + Vector3.right * (selected_x+1),
                Vector3.forward * (selected_y + 1) + Vector3.right * selected_x);
        }

    }

    private void spawn_chess_piece(int index, int x, int y, bool color)
    {
        Quaternion orientation;
        if(color == true)orientation = Quaternion.Euler(-90, 0, 0);
        else orientation = Quaternion.Euler(-90, 180, 0);
        GameObject help = Instantiate(chess_pieces_help[index], get_tile_center(x,y), orientation) as GameObject;
        help.transform.SetParent(transform);
        chess_pieces_on_board[x, y] = help.GetComponent<chess_piece>();
        chess_pieces_on_board[x, y].set_position(x, y);
        chess_pieces.Add(help);
    }

    private void spawn_all_chess_pieces()
    {
        int i;
        chess_pieces = new List<GameObject>();
        chess_pieces_on_board = new chess_piece[8, 8];
        en_passant_move = new int[2]{-1,-1};

        // Spawn white chess pieces

        //King
        spawn_chess_piece(0, 4, 0,true);
        //Queen
        spawn_chess_piece(1, 3, 0,true);
        //Rooks
        spawn_chess_piece(2, 0, 0,true);
        spawn_chess_piece(2, 7, 0,true);
        //Bishops
        spawn_chess_piece(3, 2, 0,true);
        spawn_chess_piece(3, 5, 0,true);
        //Knights
        spawn_chess_piece(4, 1, 0,true);
        spawn_chess_piece(4, 6, 0,true);
        //Pawns
        for(i=0;i<8;i++)spawn_chess_piece(5, i, 1,true);

        // Spawn blackchess pieces

        //King
        spawn_chess_piece(6, 4, 7,false);
        //Queen
        spawn_chess_piece(7, 3, 7, false);
        //Rooks
        spawn_chess_piece(8, 0, 7, false);
        spawn_chess_piece(8, 7, 7, false);
        //Bishops
        spawn_chess_piece(9, 2, 7, false);
        spawn_chess_piece(9, 5, 7, false);
        //Knights
        spawn_chess_piece(10, 1, 7, false);
        spawn_chess_piece(10, 6, 7, false);
        //Pawns
        for (i = 0; i < 8; i++) spawn_chess_piece(11, i, 6, false);
    }

    private Vector3 get_tile_center(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        //origin.y += 0.1f;
        return origin;
    }

    private void endgame()
    {
        if (is_white_turn) Debug.Log("White team wins");
        else Debug.Log("Black team wins");
        foreach(GameObject it in chess_pieces)Destroy(it);
        is_white_turn = true;
        board_highlights.instance.hide_highlight();
        spawn_all_chess_pieces();
    }
}
