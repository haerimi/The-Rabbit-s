using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 오픈소스 사용
// 23번 ~ 32번째줄 응용하여 사용

public class QuestManager : MonoBehaviour
{
    public int questId; //진행중인 퀘스트 id
    public int questActionIndex;
    Dictionary<int, QuestData> questList;

    // Start is called before the first frame update
    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    // Update is called once per frame
    void GenerateData()
    {
        // int에는 연관된 NPC ID를 입력
        questList.Add(0, new QuestData("오브레마을 찾아가기", new int[] { 100 }));
        questList.Add(10, new QuestData("만두 장인 찾기", new int[] { 300, 400, 700, 500}));
        questList.Add(20, new QuestData("헬파이어 처지", new int[] { 5000 }));
        questList.Add(30, new QuestData("만두 장인에게 물약 전달하기", new int[] { 800 }));
        questList.Add(40, new QuestData("클리어!", new int[] { 0 }));
    }

    // NPC ID를 받고 퀘스트 번호를 반환하는 함수
    public int GetQuestTalkIndex(int id) {
        return questId + questActionIndex;
       
    }


    // 대화 진행을 위한 퀘스트 대화 순서를 올리는 함수
    public string CheckQuest(int id) {
        // 순서에 맞게 대화 했을 때만 대화 순서를 올리게
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;

        // 퀘스트 대화순서가 끝에 도달했을 때 퀘스트 번호 증가
        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        // 현재 퀘스트의 이름
        return questList[questId].questName;
    }

    // 퀘스트 이름만 가져오는 함수 : 오버로딩
    public string CheckQuest()
    {
        // 현재 퀘스트의 이름
        return questList[questId].questName;
    }

    void NextQuest() {
        questId += 10;  // 퀘스트 번호를 올려줌
        questActionIndex = 0;   // 0으로 초기화
    }
}
