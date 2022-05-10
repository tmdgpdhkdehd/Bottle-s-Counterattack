using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public enum PLAYERSTATE
    {
        MOVE = 0,   // 이동 가능 기본 상태
        HIDE,       // 숨어있는 상태
        WAIT,       // 하수도에 들어가 있는 상태
        TAKING,     // 적의 몸을 빼앗은 상태
        CAUGHT      // 적에게 붙잡힌 상태
    }
    [HideInInspector] public PLAYERSTATE playerState = PLAYERSTATE.MOVE;

    public float speed;

    SpriteRenderer spriteRenderer;
    Collider2D _collider2D;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
    }



    private void FixedUpdate()
    {
        if(playerState == PLAYERSTATE.MOVE)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            Vector2 dir = new Vector2(x, y);
            transform.Translate(dir * Time.deltaTime * speed);
        }
    }


    private void Update()
    {
        switch (playerState)
        {
            case PLAYERSTATE.MOVE:
                {
                    break;
                }
            case PLAYERSTATE.HIDE:
                {
                    if(Input.GetKeyDown(KeyCode.Z))
                    {
                        PlayerShow();
                    }

                    break;
                }
            default:
                {
                    break;
                }
        }
    }


    public void PlayerHide()
    {
        playerState = PLAYERSTATE.HIDE;

        spriteRenderer.enabled = false;
        _collider2D.enabled = false;
    }

    public void PlayerShow()
    {
        playerState = PLAYERSTATE.MOVE;

        spriteRenderer.enabled = true;
        _collider2D.enabled = true;
    }

    public void PlayerTaking()
    {
        playerState = PLAYERSTATE.TAKING;
    }

    public void PlayerWait()
    {
        playerState = PLAYERSTATE.WAIT;
    }

    public void PlayerCaught()
    {
        playerState = PLAYERSTATE.CAUGHT;
    }
}
