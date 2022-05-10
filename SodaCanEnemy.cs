using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SodaCanEnemy : MonoBehaviour
{
    // 적 상태 구분용 ENUM
    public enum ENEMYSTATE
    {
        IDLE = 0,   // 대기
        CATCH,      // 이동
        ALERT,      // 경계
        CHASE,      // 추적
        DEAD,       // 죽음
        TAKEN,      // 몸 빼앗김
        STUN        // 기절
    }
    
    public ENEMYSTATE enemyState = ENEMYSTATE.IDLE;

    NavMeshAgent agent; // NavMesh 설정

    Rigidbody2D _rBody2D;

    public Material normal;
    public Material alert;
    public Material chase;

    MeshRenderer sightMeshRenderer;

    Transform target;   // 플레이어 설정
    EnemyRaycastSight enemyRaycastSight;    // 시야 설정


    public List<Vector3> wayPoints;

    Vector3 lastPosition;

    public float lookDirectionOnStart;

    public float walkSpeed = 2f;    // 기본 이동 속도
    public float chaseSpeed = 5f;    // 추적시 이동 속도
    float walkRotateSpeed = 20f;   // 기본상태 순찰 중 회전속도
    public float maxChaseDistance = 10f;    // 최대 추적거리
    public float alartRotateSpeed = 120f;   // 경계 중 시야 회전속도(임시)
    public float alartMaxWait = 10f;    // 최대 경계 시간
    float alartWait;    // 경계 시간 변수
    public float stunMaxWait = 5f;      // 최대 스턴 시간
    float stunWait;     // 스턴 시간 변수
    public float chaseMaxWait = 1f;
    float chaseWait;
    
    float lookDirection=0;

    int wayPointNum = 0;
    bool isWaypointReverse = false;

    [HideInInspector] public bool isOnSewer = false;


    private void Awake()
    {
        _rBody2D = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        
        // wayPoints 리스트에 자식 개체(sodacan 포함) 위치 vector3 저장
        wayPoints = new List<Vector3>();
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            wayPoints.Add(transform.parent.GetChild(i).position);
        }

        enemyRaycastSight = transform.GetChild(0).GetComponent<EnemyRaycastSight>();
        sightMeshRenderer = enemyRaycastSight.GetComponent<MeshRenderer>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = walkSpeed;

        lastPosition = transform.position;

        enemyRaycastSight.SetOrigin(transform.position);
        enemyRaycastSight.SetStartingAngle(lookDirectionOnStart);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && enemyState != ENEMYSTATE.TAKEN && enemyState != ENEMYSTATE.STUN)
        {
            Catch();
        }
    }

    private void FixedUpdate()
    {
        if(enemyState == ENEMYSTATE.TAKEN)
        {
            // 플레이어가 몸을 빼앗으면 조종 가능
            transform.Translate(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * chaseSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case ENEMYSTATE.IDLE:
                {

                    float distance = Vector2.Distance(wayPoints[wayPointNum], transform.position);
                    Vector2 dir = transform.position - lastPosition;    // 이전 프레임 위치와 현재 위치의 차이
                    dir.Normalize();    // 거리 평준화 함수
                    lastPosition = transform.position;  // 위치 저장

                    enemyRaycastSight.SetOrigin(transform.position);


                    // 현재 이동방향(이전 프레임과 위치 차이)에 따라 sight를 회전시킨다
                    if (dir != Vector2.zero)
                    {
                        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                        if (n < 0) n += 360;
                        if (wayPoints.Count == 1 && distance <= 0.05f)
                        {
                            n = lookDirectionOnStart;
                        }
                        lookDirection = Mathf.LerpAngle(lookDirection, n, walkRotateSpeed * Time.deltaTime);
                        enemyRaycastSight.SetStartingAngle(lookDirection);
                                                
                    }
                    
                    agent.SetDestination(wayPoints[wayPointNum]);  // 웨이포인트로 이동
                    


                    if (wayPoints.Count != 1)
                    {
                        if (distance <= 0.05f)
                        {
                            if (isWaypointReverse) wayPointNum--;    // 웨이포인트에 도착 시 다음 웨이포인트 설정
                            else wayPointNum++;
                        }
                        if (wayPointNum >= wayPoints.Count - 1) isWaypointReverse = true; // 리스트 끝에 도달 시 wayPointNum 감산으로 변경
                        else if (wayPointNum <= 0) isWaypointReverse = false;   // 리스트 처음에 도달 시 wayPointNum 가산으로 변경
                    }

                    break;
                }
            case ENEMYSTATE.CATCH:
                {
                    break;
                }
            case ENEMYSTATE.ALERT:
                {
                    // 경계 중 sight 트리거 회전(추후 기획사항에 따라 변경)
                    enemyRaycastSight.RotateStartingAngle(alartRotateSpeed);
                    alartWait -= Time.deltaTime;    // 일정 시간 동안 ALERT 상태 유지

                    enemyRaycastSight.SetOrigin(transform.position);


                    if (alartWait <= 0)
                    {
                        Idle();
                    }
                    break;
                }
            case ENEMYSTATE.CHASE:
                {

                    float distance = Vector2.Distance(target.position, transform.position);
                    Vector2 dir = target.position - transform.position;
                    dir.Normalize();    // 거리 평준화 함수
                    agent.SetDestination(target.position);  // 타겟 추적

                    enemyRaycastSight.SetOrigin(transform.position);

                    chaseWait -= Time.deltaTime;

                    // 타겟 방향에 따라서 sight를 회전시킨다
                    if (dir != Vector2.zero)
                    {
                        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                        if (n < 0) n += 360;
                        lookDirection = Mathf.LerpAngle(lookDirection, n, walkRotateSpeed * Time.deltaTime);
                        enemyRaycastSight.SetStartingAngle(lookDirection);
                    }

                    // 일정 거리 이상 벗어나거나 플레이어가 숨으면 추적을 중지하고 경계 상태에 들어감
                    if (chaseWait <= 0 || PlayerController.instance.playerState == PlayerController.PLAYERSTATE.HIDE)    
                    {
                        Alert();
                    }

                    break;
                }
            case ENEMYSTATE.DEAD:
                {
                    break;
                }
            case ENEMYSTATE.TAKEN:
                {
                    break;
                }
            case ENEMYSTATE.STUN:
                {
                    // 일정 시간 중지됨
                    stunWait -= Time.deltaTime;
                    if (stunWait <= 0)
                    {
                        Idle();
                    }
                    break;
                }
        }
    }

    public void Initialize()
    {
        enemyState = ENEMYSTATE.IDLE;

        enemyRaycastSight.gameObject.SetActive(true);
        sightMeshRenderer.material = normal;
        agent.enabled = true;
        agent.speed = walkSpeed;

        transform.position = wayPoints[0];
        lastPosition = transform.position;

        wayPointNum = 0;
        isWaypointReverse = false;

        enemyRaycastSight.SetOrigin(transform.position);
        enemyRaycastSight.SetStartingAngle(lookDirectionOnStart);
    }

    public void Taken()
    {
        enemyState = ENEMYSTATE.TAKEN;
        agent.enabled = false;
        GameManager.instance.MoveFocus(transform);
        enemyRaycastSight.gameObject.SetActive(false);
    }

    public void Chase()
    {
        if(enemyState != ENEMYSTATE.CATCH)
        {
            enemyRaycastSight.gameObject.SetActive(true);

            chaseWait = chaseMaxWait;

            sightMeshRenderer.material = chase;
            agent.speed = chaseSpeed;
            agent.enabled = true;

            enemyState = ENEMYSTATE.CHASE;
        }
    }

    public void Idle()
    {
        sightMeshRenderer.material = normal;
        enemyRaycastSight.gameObject.SetActive(true);
        agent.speed = walkSpeed;
        enemyState = ENEMYSTATE.IDLE;
        agent.enabled = true;

    }

    public void Stun()
    {
        if(PlayerController.instance.playerState == PlayerController.PLAYERSTATE.TAKING)
        {

        }

        enemyState = ENEMYSTATE.STUN;
        stunWait = stunMaxWait;
        agent.enabled = false;
        enemyRaycastSight.gameObject.SetActive(false);

    }

    public void Alert()
    {
        enemyRaycastSight.gameObject.SetActive(true);

        sightMeshRenderer.material = alert;
        alartWait = alartMaxWait;   // 경계 대기 시간 초기화
        enemyState = ENEMYSTATE.ALERT;
        agent.enabled = false;
    }

    public void Catch()
    {
        enemyState = ENEMYSTATE.CATCH;
        agent.enabled = false;
        enemyRaycastSight.gameObject.SetActive(false);
        PlayerController.instance.PlayerCaught();
        if ( GameManager.instance.gameState == GameManager.GAMESTATE.PLAY) GameManager.instance.Caught();
    }
}
