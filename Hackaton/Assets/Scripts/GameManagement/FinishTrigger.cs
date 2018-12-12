using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour {
    GameManagement manager;
    GameObject nextLevelUI;
    // Use this for initialization
    void Start () {
        manager = GameObject.Find("GameManager").GetComponent<GameManagement>();
        nextLevelUI = GameObject.Find("LevelComplete");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Time.timeScale = 0;
            manager.SetVisiblePos(nextLevelUI);
        }
    }
}
