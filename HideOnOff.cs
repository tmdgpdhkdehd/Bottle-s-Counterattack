using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnOff : MonoBehaviour
{
    private bool state = false;
    GameObject player;
    Transform showPosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        showPosition = transform.GetChild(0).transform;
    }

    void Update()
    {
        if (PlayerController.instance.playerState == PlayerController.PLAYERSTATE.MOVE && Input.GetKeyDown(KeyCode.Z) && state)
        {
            state = true;
            player.SendMessage("PlayerHide", SendMessageOptions.DontRequireReceiver);
            player.transform.position = showPosition.position;
            GameManager.instance.lastHidePos = showPosition.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            state = true;
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
