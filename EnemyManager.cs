using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<SodaCanEnemy> sodaCanEnemies;
    public float callDistance;

    public static EnemyManager instance;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        sodaCanEnemies = new List<SodaCanEnemy>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enemies.Length; i++)
        {
            sodaCanEnemies.Add(enemies[i].GetComponent<SodaCanEnemy>()); 
        }
    }

    public void CallOtherEnemy(SodaCanEnemy enemy)
    {
        for(int i =0; i < sodaCanEnemies.Count; i++)
        {
            // 입력한 변수의 일정 거리 이하에 존재하는 적을 Chase 상태로 만드는 함수
            if (Vector2.Distance(enemy.transform.position, sodaCanEnemies[i].transform.position) <= callDistance)
            {
                if(enemy != sodaCanEnemies[i] && sodaCanEnemies[i].enemyState != SodaCanEnemy.ENEMYSTATE.STUN) sodaCanEnemies[i].Chase();
            }
        }
    }


}
