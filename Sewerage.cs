using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sewerage : MonoBehaviour
{
    [HideInInspector] public bool isPlayer = false;
    GameObject player;
    GameObject takenEnemy;
    [HideInInspector] public int count = 0;
    int originalCount = 0;
    List<Sewer> sewer;

    float time;
    float maxTime = 0.5f;

    private void Awake()
    {
        
    }

    
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        time = maxTime;

        // 자식 개체 리스트 저장
        sewer = new List<Sewer>();
        for (int i = 0; i < transform.childCount; i++)
        {
            sewer.Add(transform.GetChild(i).GetComponent<Sewer>());
        }
    }

    public void Update()
    {
        if (isPlayer == true)
        {
            if (Input.GetKeyDown(KeyCode.Z) && PlayerController.instance.playerState == PlayerController.PLAYERSTATE.MOVE)
            {
                player.transform.position = sewer[count].transform.position;
                GameManager.instance.MoveFocus(sewer[count].transform);   // 현재 플레이어가 올라가 있는 자식 개체에 카메라 포커스 이동
                originalCount = count;                          // 플레이어가 올라가 있는 개체 index 번호 저장
                player.SendMessage("PlayerWait", SendMessageOptions.DontRequireReceiver);               // 플레이어 상태 WAIT으로 변경
                isPlayer = true;
            }
            else if (PlayerController.instance.playerState == PlayerController.PLAYERSTATE.WAIT)
            {
                // 방향키 오른쪽으로 sewer 선택
                if (Input.GetKeyDown(KeyCode.RightArrow)||Input.GetKeyDown(KeyCode.D))
                {
                    count++;
                    if (count > sewer.Count - 1)
                    {
                        count = 0;
                    }
                    GameManager.instance.MoveFocus(sewer[count].transform);   // 카메라 포커스 선택된 sewer 개체로 이동
                }
                // 방향키 왼쪽으로 sewer 선택
                else if (Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.A))
                {
                    count--;
                    if (count < 0)
                    {
                        count = sewer.Count - 1;
                    }
                    GameManager.instance.MoveFocus(sewer[count].transform);   // 카메라 포커스 선택된 sewer 개체로 이동
                }
                // X키로 취소
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    player.SendMessage("PlayerShow", SendMessageOptions.DontRequireReceiver);   // 플레이어 상태 MOVE로 변경
                    count = originalCount;              // 값 초기화
                }
                // 현재 포커스된 sewer 개체 위에 적이 올라가 있고 그 적이 현재 클래스에 저장된 enemy와 같을때 Z를 누를 경우 적의 몸을 빼앗음
                else if (sewer[count].isEnemy == true && Input.GetKeyDown(KeyCode.Z))
                {
                    Debug.Log("몸빼앗음");
                    player.SendMessage("PlayerTaking", SendMessageOptions.DontRequireReceiver); // 플레이어 상태 TAKING으로 변경
                    sewer[count].enemy.SendMessage("Taken", SendMessageOptions.DontRequireReceiver);         // 적 상태 TAKEN으로 변경
                    sewer[count].enemy.transform.position = sewer[count].transform.position;
                    takenEnemy = sewer[count].enemy;
                }
            }
            // X키로 취소
            else if (PlayerController.instance.playerState == PlayerController.PLAYERSTATE.TAKING)
            {
                if (Input.GetKeyDown(KeyCode.X) && takenEnemy.GetComponent<SodaCanEnemy>().isOnSewer)
                {
                    player.SendMessage("PlayerShow", SendMessageOptions.DontRequireReceiver);   // 플레이어 상태 MOVE로 변경
                    takenEnemy.SendMessage("Stun", SendMessageOptions.DontRequireReceiver);     // 적 상태 STUN으로 변경
                    count = originalCount;
                }
                else if(takenEnemy.GetComponent<SodaCanEnemy>().enemyState==SodaCanEnemy.ENEMYSTATE.STUN)  // 다른 여러가지 상황 때문에 몸을 빼앗은 적이 STUN 상태가 되었을 경우
                {
                    time -= Time.deltaTime;
                    if (time <= 0)
                    {
                        player.SendMessage("PlayerShow", SendMessageOptions.DontRequireReceiver);   // 플레이어 상태 MOVE로 변경
                        count = originalCount;
                        time = maxTime;
                    }
                    
                }
            }
        }
    }


}
