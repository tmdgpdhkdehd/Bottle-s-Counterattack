using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GAMESTATE
    {
        GAMESTART = 0,
        PLAY,
        CAUGHT,
        GAMEOVER,
        CLEAR
    }

    public GAMESTATE gameState = GAMESTATE.GAMESTART;

    [HideInInspector] public Transform focusObject;
    public static GameManager instance;

    GameObject Player;

    int life;
    public int maxLife = 3;

    public Vector3 lastHidePos;
    public Vector3 initialPos;

    public Text talkText;
    public GameObject scanObject;
    public GameObject talkPanel;
    public bool isAction; // 플레이어 행동 유무
    public TalkManager talkManager;
    public int talkIndex;
    public Image portraitImg;

    List<Magnet> magnets;

    public Canvas InventoryUI;
    public Canvas CaughtUI;
    public Canvas GameOverUI;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        focusObject = GameObject.FindGameObjectWithTag("Player").transform;
        Player = GameObject.FindGameObjectWithTag("Player");
        isAction = true;
        talkPanel.SetActive(false);
        GameOverUI.enabled = false;
        CaughtUI.enabled = false;
        initialPos = PlayerController.instance.transform.position;
        
        magnets = new List<Magnet>();
        GameObject[] tempMag = GameObject.FindGameObjectsWithTag("Magnet");
        for(int i = 0; i < tempMag.Length; i++)
        {
            magnets.Add(tempMag[i].GetComponent<Magnet>());
        }

        GameStart();
    }

    private void Update()
    {
        switch (gameState)
        {
            case GAMESTATE.GAMESTART: 
                {
                    Play();
                    break;
                }
            case GAMESTATE.PLAY:
                {
                    break;
                }
            case GAMESTATE.CAUGHT:
                {
                    break;
                }
            case GAMESTATE.GAMEOVER:
                {
                    break;
                }
            case GAMESTATE.CLEAR:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void GameStart()
    {
        lastHidePos = initialPos;
        life = maxLife;
        gameState = GAMESTATE.GAMESTART;
    }

    public void Play()
    {
        InventoryUI.enabled = true;
        CaughtUI.enabled = false;
        GameOverUI.enabled = false;

        gameState = GAMESTATE.PLAY;
    }

    public void Caught()
    {
        if(life > 0)
        {
            life--;
            Debug.Log("남은 목숨 : " + life);
            CaughtUI.enabled = true;
            gameState = GAMESTATE.CAUGHT;
        }
        else
        {
            GameOver();
        }
    }

    public void CaughtResume()
    {
        PlayerController.instance.transform.position = lastHidePos;
        PlayerController.instance.PlayerShow();
        if (lastHidePos != initialPos) PlayerController.instance.PlayerHide();
        for (int i = 0; i < EnemyManager.instance.sodaCanEnemies.Count; i++)
        {
            EnemyManager.instance.sodaCanEnemies[i].Initialize();
        }

        for(int i= 0; i < magnets.Count; i++)
        {
            magnets[i].isOn = false;
        }

        CaughtUI.enabled = false;

        Play();
    }

    public void GameOver()
    {
        Debug.Log("게임 오버");
        InventoryUI.enabled = false;
        CaughtUI.enabled = false;
        GameOverUI.enabled = true;
        

        gameState = GAMESTATE.GAMEOVER;
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(gameObject.scene.name);
    }

    public void Clear()
    {
        CaughtUI.enabled = false;
        InventoryUI.enabled = false;
        GameOverUI.enabled = false;

        gameState = GAMESTATE.CLEAR;
    }

    public void MoveFocus(Transform tr)
    {
        focusObject = tr;
    }

    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNPC);
        talkPanel.SetActive(isAction);
    }

    void Talk(int id, bool isNPC)
    {
        // talkManager에 생성한 대사를 반환하는 함수를 이용하여 대사 한 줄 입력받음
        string talkData = talkManager.GetTalk(id, talkIndex);
        Player.SendMessage("PlayerWait", SendMessageOptions.DontRequireReceiver);
        Time.timeScale = 0.0f;

        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            Player.SendMessage("PlayerShow", SendMessageOptions.DontRequireReceiver);
            Time.timeScale = 1.0f;
            return; // 아래 코드 실행 안 되게 강제 종료
        }

        if (isNPC)
        {
            talkText.text = talkData.Split(':')[0]; // 문장과 이미지 나누기 (문장)

            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1])); // 문장과 이미지 나누기 (이미지)
            portraitImg.color = new Color(1, 1, 1, 1); // 투명도
        }
        else
        {
            talkText.text = talkData;

            portraitImg.color = new Color(1, 1, 1, 0);
        }
        isAction = true;
        talkIndex++;
    }
}
