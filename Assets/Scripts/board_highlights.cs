using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board_highlights : MonoBehaviour
{
    public static board_highlights instance { get; set; }
    public GameObject highlight_prefab;
    private List<GameObject> highlights;
    public GameObject highlight_tail;
    public GameObject castling_arrows;
    private Vector3 scale;

    private void Start()
    {
        instance = this;
        highlights = new List<GameObject>();
        scale = new Vector3(1, 1, 1);
    }

    private GameObject get_highlight_object(GameObject instantiate_me)
    {
        GameObject help = highlights.Find(g => !g.activeSelf);
        if (help == null)
        {
            help = Instantiate(instantiate_me);
            highlights.Add(help);
        }
        return help;
    }

    public void highlight_allowed_moves(bool[,] moves)
    {
        int i, j;
        GameObject help;
        for (i = 0; i < 8; i++)
        {
            for (j = 0; j < 8; j++)
            {
                if (moves[i, j])
                {
                    help = get_highlight_object(highlight_prefab);
                    help.SetActive(true);
                    help.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f);
                    if (board_manager.instance.chess_pieces_on_board[i, j] != null) help.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
                    else if (help.transform.localScale != scale) help.transform.localScale = scale;
                }
            }
        }
    }

    public void highlight_selected_tail(Vector3 position)
    {
        GameObject help = get_highlight_object(highlight_tail);
        help.SetActive(true);
        help.transform.position = position;
    }

    public void hide_highlight()
    {
        foreach (GameObject it in highlights) it.SetActive(false);

    }
}
