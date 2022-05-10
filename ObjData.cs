using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjData : MonoBehaviour
{
    GameManager manager;
    GameObject scanObject;
    bool state;
    public GameObject[] tutorial;
    bool[] tutoState;

    public int id;
    public bool isNPC;

    void Awake()
    {
        tutoState = new bool[tutorial.Length];
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scanObject = gameObject; // 닿은 물체 정보 가져오기
    }

    void Update()
    {
        if (state && Input.GetKeyDown(KeyCode.Z))
        {
            manager.Action(scanObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            state = true;
/*            if (collision.gameObject.tutorial)
            {
                manager.Action(scanObject);
            }*/
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            state = false;
        }
    }
}
