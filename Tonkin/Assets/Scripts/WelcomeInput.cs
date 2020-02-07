using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WelcomeInput : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject gc;

    void Start()
    {
        gc = GameObject.Find("GameController");

        GameObject sButton = GameObject.Find("Single");
        GameObject mButton = GameObject.Find("Multi");

        Button sb = (Button)sButton.GetComponent<Button>();
        Button mb = (Button)mButton.GetComponent<Button>();

        sb.onClick.AddListener(ClickSingleButton);
        mb.onClick.AddListener(ClickMultiButton);
    }

    private void ClickMultiButton()
    {
        gc.GetComponent<GameControl>().GameStart(true);
    }

    private void ClickSingleButton()
    {
        gc.GetComponent<GameControl>().GameStart(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
