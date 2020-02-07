using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InGameInput : MonoBehaviour
{
    private GameObject gc;

    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.Find("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableBackKey() {
        GameObject back = this.transform.Find("Back").gameObject;
        back.SetActive(true);
        back.GetComponent<Button>().onClick.AddListener(BackToWelcome);

    }

    public void DisableBackKey()
    {
        GameObject back = this.transform.Find("Back").gameObject;
        back.SetActive(false);
        back.GetComponent<Button>().onClick.RemoveAllListeners();

    }

    private void BackToWelcome()
    {
        gc.GetComponent<GameControl>().BackToWelcome();
    }
}
