using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowWinner(GameControl.Players player) {
        Color color = player == GameControl.Players.Player1 ? Color.red : Color.blue;
        string c = player == GameControl.Players.Player1 ? "Red side" : "Blue side";
        string text = "- - - -" + c + " is Winner" + "- - - -";
        this.gameObject.GetComponent<Text>().text = text;
        this.gameObject.GetComponent<Text>().color = color;
    }

 
    public void Prompt(string text, GameControl.Players player)
    {
        Color color = player == GameControl.Players.Player1 ? Color.red : Color.blue;

        this.gameObject.GetComponent<Text>().text = text;
        this.gameObject.GetComponent<Text>().color = color;
    }


}
