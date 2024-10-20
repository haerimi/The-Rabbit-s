using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    public string questName;
    public int[] npcId;
    // 구조체 생성을 위한 매개변수 생성자
    public QuestData(string name, int[] npc) {
        questName = name;
        npcId = npc;
    }
}
