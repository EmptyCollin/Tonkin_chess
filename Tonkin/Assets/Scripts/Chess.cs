using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chess : MonoBehaviour
{
    public int node_index = 0; // -1 - out of the board, 0 ~ 44 , the node index on board
    public bool isSelected = false;

    public GameControl.Players belongTo;
    public int id;

    public Vector3 OringinalPos;
    private GameObject gc;
    private GameObject board;
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.Find("GameController");
        board = GameObject.Find("Board");
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected) {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            transform.position = new Vector3(hit.point.x, 0, hit.point.z);

        }
    }

    void OnMouseDown()
    {
        //transform.Translate(new Vector3(0.1f, 0, 0));

        GameControl g = gc.GetComponent<GameControl>();
        Board b = board.GetComponent<Board>();

        if(g.gameState == GameControl.GameState.InGame)
        {
            if (!g.selectedChess && belongTo == g.currentPlayer)
            {
                if (!b.AllChesesOnBoard() && node_index != -1) return;

                g.selectedChess = this.gameObject;
                isSelected = true;
                OringinalPos = transform.position;

            }
            else if (g.selectedChess)
            {
                if (b.CheckPlacement(this.gameObject))
                {
                    transform.position = b.PlayerMoveChess(this.gameObject);
                    OringinalPos = transform.position;
                    g.ChangeTurn();
                }
                else
                {
                    transform.position = OringinalPos;
                }
                isSelected = false;
                g.selectedChess = null;
            }
        }
        
    }
}
