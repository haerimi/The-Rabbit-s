using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���¼ҽ� ���
// 23�� ~ 32��°�� �����Ͽ� ���

public class QuestManager : MonoBehaviour
{
    public int questId; //�������� ����Ʈ id
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
        // int���� ������ NPC ID�� �Է�
        questList.Add(0, new QuestData("���극���� ã�ư���", new int[] { 100 }));
        questList.Add(10, new QuestData("���� ���� ã��", new int[] { 300, 400, 700, 500}));
        questList.Add(20, new QuestData("�����̾� ó��", new int[] { 5000 }));
        questList.Add(30, new QuestData("���� ���ο��� ���� �����ϱ�", new int[] { 800 }));
        questList.Add(40, new QuestData("Ŭ����!", new int[] { 0 }));
    }

    // NPC ID�� �ް� ����Ʈ ��ȣ�� ��ȯ�ϴ� �Լ�
    public int GetQuestTalkIndex(int id) {
        return questId + questActionIndex;
       
    }


    // ��ȭ ������ ���� ����Ʈ ��ȭ ������ �ø��� �Լ�
    public string CheckQuest(int id) {
        // ������ �°� ��ȭ ���� ���� ��ȭ ������ �ø���
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;

        // ����Ʈ ��ȭ������ ���� �������� �� ����Ʈ ��ȣ ����
        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        // ���� ����Ʈ�� �̸�
        return questList[questId].questName;
    }

    // ����Ʈ �̸��� �������� �Լ� : �����ε�
    public string CheckQuest()
    {
        // ���� ����Ʈ�� �̸�
        return questList[questId].questName;
    }

    void NextQuest() {
        questId += 10;  // ����Ʈ ��ȣ�� �÷���
        questActionIndex = 0;   // 0���� �ʱ�ȭ
    }
}
