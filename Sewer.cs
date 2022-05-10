using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sewer : MonoBehaviour
{
    Sewerage sewerage;  // 부모 개체 클래스
    int childNum;       // 현재 자식 개체 index 번호
    [HideInInspector] public GameObject enemy;
    [HideInInspector] public bool isEnemy = false;

    private void Start()
    {
        sewerage = transform.GetComponentInParent<Sewerage>();  // 부모 개체 클래스 저장
        childNum = transform.GetSiblingIndex();                 // index 번호 저장
    }

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sewerage.isPlayer = true;
            sewerage.count = childNum;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            isEnemy = true;
            collision.GetComponent<SodaCanEnemy>().isOnSewer = true;
            enemy = collision.gameObject;  // 현재 충돌 중인 Enemy 개체 저장
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sewerage.isPlayer = false;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            isEnemy = false;
            collision.GetComponent<SodaCanEnemy>().isOnSewer = false;
        }
    }
}
