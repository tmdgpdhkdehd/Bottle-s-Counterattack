using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;

    public Sprite[] portraitArr;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenetateData();
    }

    void Update()
    {
        
    }

    void GenetateData()
    {
        // id가 1000인 오브젝트의 대화 추가
        talkData.Add(1000, new string[] { "우와:0", "잠입에 성공했구나!:1" }); // 우와, 잠입~ 총 문자열 2개
        talkData.Add(100, new string[] { "박스가 있다", "박스구나" });
        talkData.Add(10, new string[] { "저 나쁜 캔이 경비 중이야:0", "들키면 냉장고에 숨어야겠어:0" });

        portraitData.Add(10 + 0, portraitArr[0]); // 표정이 하나밖에 없으니까 1000 + 0
        portraitData.Add(1000 + 0, portraitArr[1]);
        portraitData.Add(1000 + 1, portraitArr[2]);
    }

    public string GetTalk(int id, int talkIndex) // talkIndex는 몇 번째 문자열을 가져올지 정해줌
    {
        // talkIndex와 대화의 문장 갯수를 비교하여 끝을 확인
        if(talkIndex == talkData[id].Length)
        {
            return null;
        }
        else
        {
            return talkData[id][talkIndex];
        }
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }
}
