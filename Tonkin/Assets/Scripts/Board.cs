using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public struct Node {
        public int index;
        public Vector3 pos;
        public int[] adjacent;
        public GameObject chess;
    }

    public Node[] nodes;
    public List<int[]> lines;

    /*-----------------------------------------------------*/
    private GameObject gc;
    /*-----------------------------------------------------*/


    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.Find("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckWinner(GameControl.Players player)
    {
        bool flag;
        for(int i = 0 ; i < lines.Count; i++)
        {
            flag = true;
            for(int j = 0; j < lines[i].Length; j++)
            {
                if (!nodes[lines[i][j]].chess) {
                    flag = false;
                    break;
                } else if (nodes[lines[i][j]].chess.GetComponent<Chess>().belongTo != player) {
                    flag = false;
                    break;
                } 
            }
            if (flag) return true;
        }
        return false;

    }

    public bool CheckPlacement(GameObject chess) {
        int targetNode = FindNearestNode(chess.transform.position);
        if (targetNode == -1) return false;
        if (nodes[targetNode].chess) return false;
        if (chess.GetComponent<Chess>().node_index >= 0 && (Array.IndexOf(nodes[targetNode].adjacent, chess.GetComponent<Chess>().node_index)) == -1) return false;
        return true;
    }

    // Player move a chess
    public Vector3 PlayerMoveChess(GameObject chess) {
        int targetNode = FindNearestNode(chess.transform.position);
        int originalIndex = chess.GetComponent<Chess>().node_index;
        if (originalIndex >= 0)nodes[originalIndex].chess = null;
        nodes[targetNode].chess = chess;
        chess.GetComponent<Chess>().node_index = targetNode;
        return nodes[targetNode].pos;
    }

    // AI move a chess
    private Node[] AIMoveChess(GameObject chess, Node[] board, int target)
    {
        int originalIndex = chess.GetComponent<Chess>().node_index;
        if (originalIndex >= 0) nodes[originalIndex].chess = null;
        board[target].chess = chess;
        chess.GetComponent<Chess>().node_index = target;
        chess.transform.position = nodes[target].pos;
        return board;
    }

    private Node[] UndoMoveChess(GameObject chess, Node[] board, int target, Vector3 pos) {
        if (target == -1)
        {
            board[chess.GetComponent<Chess>().node_index].chess = null;
            chess.GetComponent<Chess>().node_index = target;
            chess.transform.position = pos;
            return board;
        }
        else {
            return AIMoveChess(chess, board, target);
        }
        
    }


    private int FindNearestNode(Vector3 pos) {
        float maxOffset = 0.05f;
        float minDis = 10000.0f;
        float distance;
        int nearestNode = -1;
        for (int i = 0; i < nodes.Length; i++) {
            distance = Vector3.Distance(pos, nodes[i].pos);
            if (distance <= maxOffset && distance <= minDis) {
                minDis = distance;
                nearestNode = i;
            }
        }
        return nearestNode;
    }


    public bool AllChesesOnBoard() {
        int count = 0;
        for (int i = 0; i < nodes.Length; i++) {
            if (nodes[i].chess) count++;
        }
        return count == 20 ? true : false;
    }

    public void AIOperation(GameControl.Players player)
    {
        GameObject[] chesses = GameObject.FindGameObjectsWithTag("Chess");
        GameObject selectChess;
        int[] availableNodes;

        // evaluation of assumption board
        float assumptionEva;

        // records of best choice
        GameObject bestChess = null;
        int bestTarget = -1;
        float bestEva = -10000;

        if (!AllChesesOnBoard())
        {
            selectChess = SelectUnboardedChess(player, chesses);
            availableNodes = FindAvailableNode(selectChess);
            if (availableNodes.Length == 0) return ;
            for (int i = 0; i < availableNodes.Length; i++)
            {
                // try to move a chess and evaluate the board
                int origin_index = selectChess.GetComponent<Chess>().node_index;
                Vector3 origin_pos = selectChess.GetComponent<Chess>().transform.position;
                nodes = AIMoveChess(selectChess, nodes, availableNodes[i]);
                assumptionEva = EvaluateBoard(nodes,player);

                // undo the movement
                nodes = UndoMoveChess(selectChess, nodes, origin_index,origin_pos);

                // record the choice if it is better than current record
                if (assumptionEva > bestEva)
                {
                    bestChess = selectChess;
                    bestTarget = availableNodes[i];
                    bestEva = assumptionEva;
                }
            }
        }
        else {
            for (int j = 0; j < chesses.Length; j++) {
                if (chesses[j].GetComponent<Chess>().belongTo != player)
                {
                    continue;
                }
                else {
                    selectChess = chesses[j];
                    availableNodes = FindAvailableNode(selectChess);
                    if (availableNodes.Length == 0) continue;
                    for (int i = 0; i < availableNodes.Length; i++)
                    {
                        // try to move a chess and evaluate the board
                        int origin_index = selectChess.GetComponent<Chess>().node_index;
                        Vector3 origin_pos = selectChess.GetComponent<Chess>().transform.position;
                        nodes = AIMoveChess(selectChess, nodes, availableNodes[i]);
                        assumptionEva = EvaluateBoard(nodes,player);

                        // undo the movement
                        nodes = UndoMoveChess(selectChess, nodes, origin_index, origin_pos);

                        // record the choice if it is better than current record
                        if (assumptionEva > bestEva)
                        {
                            bestChess = selectChess;
                            bestTarget = availableNodes[i];
                            bestEva = assumptionEva;
                        }
                    }
                }
            }
        }

        if (!bestChess || bestTarget == -1) {
            Debug.Log("Logic Error");
            return;
        }
        AIMoveChess(bestChess, nodes, bestTarget);
        gc.GetComponent<GameControl>().ChangeTurn();
    }

    private int EvaluateBoard(Node[] board, GameControl.Players player) {
        int LCM = 3 * 5 * 7; // common multiple of inserction numbers

        GameControl.Players compititor = player == GameControl.Players.Player1 ? GameControl.Players.Player2 : GameControl.Players.Player1;
        int playerCount = 0, compititorCount = 0;

        int p_num; // the number of chess on a line belonging to player
        int c_num; // the number of chess on a line belonging to compititor
        int line_length; // the total inserction of this line

        for (int i = 0; i < lines.Count; i++)
        {
            p_num = 0;
            c_num = 0;
            line_length = lines[i].Length;
            for(int j = 0; j < line_length; j++)
            {
                if (!nodes[lines[i][j]].chess) continue;
                if (nodes[lines[i][j]].chess.GetComponent<Chess>().belongTo == player) p_num++;
                else c_num++;

            }

            // compititor owns at least one chess on this line, so that the player can't got win via it
            if (c_num > 0)
            {
                playerCount += 0;

                // same as player count
                if (p_num > 0) compititorCount += 0;
                else compititorCount += (c_num * LCM / line_length) * (c_num * LCM / line_length);

            }

            // player may get win on this line, incremental marginal benefit
            else
            {
                if (p_num == line_length) playerCount += 100000000;
                playerCount += (p_num * LCM / line_length ) * (p_num * LCM / line_length );
            }

        }

        return playerCount-compititorCount;
    }

    private Node[] AssumptionOperation(Node[] originalBoard, GameObject chess, int target) {
        Node[] assumptionBoard = new Node[originalBoard.Length];
        originalBoard.CopyTo(assumptionBoard,0);

        return assumptionBoard;
    }

    private int[] FindAvailableNode(GameObject chess) {
        List<int> l = new List<int>();
        Chess c = chess.GetComponent<Chess>();
        if (c.node_index != -1)
        {
            for (int i = 0; i < nodes[c.node_index].adjacent.Length; i++)
            {
                if (!nodes[nodes[c.node_index].adjacent[i]].chess)
                    l.Add(nodes[c.node_index].adjacent[i]);
            }
        }
        else {
            for (int i = 0; i < nodes.Length; i++)
            {
                if (!nodes[i].chess) l.Add(i);
            }
        }

        return l.ToArray();
    }


    private GameObject SelectUnboardedChess(GameControl.Players player,GameObject[] chesses)
    {     
        for (int i = 0; i < chesses.Length; i++)
        {
            if(chesses[i].GetComponent<Chess>().node_index == -1 && chesses[i].GetComponent<Chess>().belongTo == player) return chesses[i];
        }
        return null;
    }
    /*
    private AITargets DetermineTarget()
    {
        return null;
    }*/



}
