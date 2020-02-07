using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{

    private GameObject board;
    private GameObject ui;
    // Start is called before the first frame update
    void Start()
    {
        board = GameObject.Find("Board");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeChess() {
        RemoveAllChess();
        GameObject chess;
        Vector3 pos;
        for (int i = 0; i < 10; i++)
        {
            chess = Instantiate(Resources.Load("Prefabs/Chess")) as GameObject;
            chess.name = "Chess_Red_" + i.ToString();
            pos = new Vector3(1.25f + i % 5 * 0.2f, 0, 0.3f - (int)i / 5 * 0.2f);
            chess.transform.Translate(pos);
            chess.GetComponent<Chess>().belongTo = GameControl.Players.Player1;
            chess.GetComponent<Chess>().id = i;
            chess.GetComponent<Chess>().transform.position = pos;
            chess.GetComponent<Chess>().node_index = -1;
        }
        for (int i = 0; i < 10; i++)
        {
            chess = Instantiate(Resources.Load("Prefabs/Chess")) as GameObject;
            chess.name = "Chess_Blue_" + i.ToString();
            chess.GetComponent<MeshRenderer>().material.color = Color.blue;
            pos = new Vector3(1.25f + i % 5 * 0.2f, 0, 0.9f - (int)i / 5 * 0.2f);
            chess.transform.Translate(pos);
            chess.GetComponent<Chess>().belongTo = GameControl.Players.Player2;
            chess.GetComponent<Chess>().id = i;
            chess.GetComponent<Chess>().transform.position = pos;
            chess.GetComponent<Chess>().node_index = -1;
        }
    }

    public void RemoveAllChess()
    {
        GameObject[] chesses = GameObject.FindGameObjectsWithTag("Chess");
        for (int i = 0; i < chesses.Length; i++) {
            GameObject.Destroy(chesses[i]);
        }
    }

    public void RemoveUnboardedChess()
    {
        GameObject[] chesses = GameObject.FindGameObjectsWithTag("Chess");
        for (int i = 0; i < chesses.Length; i++)
        {
            if(chesses[i].GetComponent<Chess>().node_index == -1)
                GameObject.Destroy(chesses[i]);
        }
    }

    public void InitializeWelcomeScreen() {
        GameObject ui = GameObject.Find("UiScreens");
        GameObject welcome = ui.transform.Find("Welcome").gameObject;
        welcome.SetActive(true);
        
        GameObject ingame = ui.transform.Find("InGame").gameObject;
        ingame.SetActive(false);
    }

    public void InitializeInGameScreen(){
        GameObject ui = GameObject.Find("UiScreens");
        GameObject welcome = ui.transform.Find("Welcome").gameObject;
        welcome.SetActive(false);

        GameObject ingame = ui.transform.Find("InGame").gameObject;
        ingame.SetActive(true);
        ingame.GetComponent<InGameInput>().DisableBackKey();
    }

    public void InitializeBoard() {
        Board.Node[] n = new Board.Node[45];
        for (int i = 0; i < 45; i++) {
            n[i].index = i;
            n[i].chess = null;
        }

        // hardcode of position and adjacent
        n[0].pos = new Vector3(0.5f, 0f, 0.5f);
        n[0].adjacent = new int[] { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };

        n[1].pos = new Vector3(0f, 0f, 0f);
        n[1].adjacent = new int[] { 2, 16, 33 };

        n[2].pos = new Vector3(0.25f, 0f, 0f);
        n[2].adjacent = new int[] { 1, 3, 33, 38 };

        n[3].pos = new Vector3(0.5f, 0f, 0f);
        n[3].adjacent = new int[] { 2, 4, 19, 38, 39 };

        n[4].pos = new Vector3(0.75f, 0f, 0f);
        n[4].adjacent = new int[] { 3, 5, 34, 39 };

        n[5].pos = new Vector3(1.0f, 0f, 0f);
        n[5].adjacent = new int[] { 4, 6, 34 };

        n[6].pos = new Vector3(1.0f, 0f, 0.25f);
        n[6].adjacent = new int[] { 5, 7, 34, 40 };

        n[7].pos = new Vector3(1.0f, 0f, 0.5f);
        n[7].adjacent = new int[] { 6, 8, 23, 40, 41 };

        n[8].pos = new Vector3(1.0f, 0f, 0.75f);
        n[8].adjacent = new int[] { 7, 9, 35, 41 };

        n[9].pos = new Vector3(1.0f, 0f, 1.0f);
        n[9].adjacent = new int[] { 8, 10, 35 };

        n[10].pos = new Vector3(0.75f, 0f, 1.0f);
        n[10].adjacent = new int[] { 9, 11, 35, 42 };

        n[11].pos = new Vector3(0.5f, 0f, 1.0f);
        n[11].adjacent = new int[] { 10, 12, 27, 42, 43 };

        n[12].pos = new Vector3(0.25f, 0f, 1.0f);
        n[12].adjacent = new int[] { 11, 13, 36, 43 };

        n[13].pos = new Vector3(0f, 0f, 1.0f);
        n[13].adjacent = new int[] { 12, 14, 36 };

        n[14].pos = new Vector3(0f, 0f, 0.75f);
        n[14].adjacent = new int[] { 13, 15, 36, 44 };

        n[15].pos = new Vector3(0f, 0f, 0.5f);
        n[15].adjacent = new int[] { 14, 16, 31, 37, 44 };

        n[16].pos = new Vector3(0f, 0f, 0.25f);
        n[16].adjacent = new int[] { 1, 15, 33, 37 };

        n[17].pos = new Vector3(0.25f, 0f, 0.25f);
        n[17].adjacent = new int[] { 0,18, 32, 33, 37, 38 };

        n[18].pos = new Vector3(0.375f, 0f, 0.25f);
        n[18].adjacent = new int[] { 0, 17, 19, 38 };

        n[19].pos = new Vector3(0.5f, 0f, 0.25f);
        n[19].adjacent = new int[] { 0, 3, 18, 20 };

        n[20].pos = new Vector3(0.625f, 0f, 0.25f);
        n[20].adjacent = new int[] { 0, 19, 21, 39 };

        n[21].pos = new Vector3(0.75f,0f,0.25f);
        n[21].adjacent = new int[] { 0, 20, 22, 34, 39, 40 };

        n[22].pos = new Vector3(0.75f,0f,0.375f);
        n[22].adjacent = new int[] { 0, 21, 23, 40 };

        n[23].pos = new Vector3(0.75f,0f,0.5f);
        n[23].adjacent = new int[] { 0, 7, 22, 24 };

        n[24].pos = new Vector3(0.75f,0f,0.625f);
        n[24].adjacent = new int[] { 0, 23, 25, 41 };

        n[25].pos = new Vector3(0.75f,0f,0.75f);
        n[25].adjacent = new int[] { 0, 24, 26, 35, 41, 42 };

        n[26].pos = new Vector3(0.625f,0f,0.75f);
        n[26].adjacent = new int[] { 0, 25, 27, 42 };

        n[27].pos = new Vector3(0.5f,0f,0.75f);
        n[27].adjacent = new int[] { 0, 11, 26, 28 };

        n[28].pos = new Vector3(0.375f,0f,0.75f);
        n[28].adjacent = new int[] { 0, 27, 29, 43 };

        n[29].pos = new Vector3(0.25f,0f,0.75f);
        n[29].adjacent = new int[] { 0, 28, 30, 36, 43, 44 };

        n[30].pos = new Vector3(0.25f,0f,0.625f);
        n[30].adjacent = new int[] { 0, 29, 31, 44 };

        n[31].pos = new Vector3(0.25f,0f,0.5f);
        n[31].adjacent = new int[] { 0, 15, 30, 32 };

        n[32].pos = new Vector3(0.25f,0f,0.375f);
        n[32].adjacent = new int[] { 0, 17, 31, 37 };

        n[33].pos = new Vector3(0.125f,0f,0.125f);
        n[33].adjacent = new int[] { 1, 2, 16, 17 };

        n[34].pos = new Vector3(0.875f,0f,0.125f);
        n[34].adjacent = new int[] { 4, 5, 6, 21 };

        n[35].pos = new Vector3(0.875f,0f,0.875f);
        n[35].adjacent = new int[] { 8, 9, 10, 25 };

        n[36].pos = new Vector3(0.125f,0f,0.875f);
        n[36].adjacent = new int[] { 12, 13, 14, 29 };

        n[37].pos = new Vector3(0.16667f,0f,0.33333f);
        n[37].adjacent = new int[] { 15, 16, 17, 32 };

        n[38].pos = new Vector3(0.333333f,0f,0.16667f);
        n[38].adjacent = new int[] { 2, 3, 17, 18 };

        n[39].pos = new Vector3(0.66667f,0f,0.16667f);
        n[39].adjacent = new int[] { 3, 4, 20, 21 };

        n[40].pos = new Vector3(0.83333f,0f,0.33333f);
        n[40].adjacent = new int[] { 6,7,21,22};

        n[41].pos = new Vector3(0.83333f,0f,0.66667f);
        n[41].adjacent = new int[] { 7, 8, 24, 25 };

        n[42].pos = new Vector3(0.66667f,0f,0.83333f);
        n[42].adjacent = new int[] { 10, 11, 25, 26 };

        n[43].pos = new Vector3(0.33333f,0f,0.83333f);
        n[43].adjacent = new int[] { 11, 12, 28, 29 };

        n[44].pos = new Vector3(0.16667f,0f,0.66667f);
        n[44].adjacent = new int[] { 14, 15, 29, 30 };

        board.GetComponent<Board>().nodes = n;

        // lines

        List<int[]> lines = new List<int[]>();
        int[] l;

        l = new int[] { 1, 2, 3, 4, 5 };
        lines.Add(l);

        l = new int[] { 17, 18, 19, 20, 21 };
        lines.Add(l);

        l = new int[] { 15, 31, 0, 23, 7 };
        lines.Add(l);

        l = new int[] { 29, 28, 27, 26, 25 };
        lines.Add(l);

        l = new int[] { 13, 12, 11, 10, 9 };
        lines.Add(l);

        l = new int[] { 13, 14, 15, 16, 1 };
        lines.Add(l);

        l = new int[] { 29, 30, 31, 32, 17 };
        lines.Add(l);

        l = new int[] { 11, 27, 0, 19, 3 };
        lines.Add(l);

        l = new int[] { 25, 24, 23, 22, 21 };
        lines.Add(l);

        l = new int[] { 9, 8, 7, 6, 5 };
        lines.Add(l);

        l = new int[] { 14, 36, 12 };
        lines.Add(l);

        l = new int[] { 15, 44, 29, 43, 11 };
        lines.Add(l);

        l = new int[] { 1, 33, 17, 0, 25, 35, 9 };
        lines.Add(l);

        l = new int[] { 3, 39, 21, 40, 7 };
        lines.Add(l);

        l = new int[] { 4, 34, 6 };
        lines.Add(l);

        l = new int[] { 10, 35, 8 };
        lines.Add(l);

        l = new int[] { 11, 42, 25, 41, 7 };
        lines.Add(l);

        l = new int[] { 13, 36, 29, 0, 21, 34, 5 };
        lines.Add(l);

        l = new int[] { 15, 37, 17, 38, 3 };
        lines.Add(l);

        l = new int[] { 16, 33, 2 };
        lines.Add(l);

        l = new int[] { 12, 43, 28, 0, 20, 39, 4 };
        lines.Add(l);

        l = new int[] { 14, 44, 30, 0, 22, 40, 6 };
        lines.Add(l);

        l = new int[] { 16, 37, 32, 0, 24, 41, 8 };
        lines.Add(l);

        l = new int[] { 2, 38, 18, 0, 26, 42, 10 };
        lines.Add(l);

        board.GetComponent<Board>().lines = lines;
    }
}
