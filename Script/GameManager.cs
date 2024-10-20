using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ���¼ҽ��� �����Ͽ� ���

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;    
    public TypeEffect talkText;     // ��ȭ �ؽ�Ʈ 
    public GameObject scanObject;   // Player�� ������ ������Ʈ
    public Animator talkPanel;    // ��ȭ�� �������϶��� �ǳ��� ���̰� �� ���� ����

    // ���̵���,�ƿ� �ִϸ��̼ǰ� ������Ʈ
    public Animator anim;
    public GameObject imageObject;

    public bool isAction;       // ���� ���� ����
    public int talkIndex;
    public Image portraitImg;   // �ʻ�ȭ
    public string objName;  // �̸�
    public Text nameText;

    // ���� UI ������ ����� ����ϴ� ����Ʈ UI ����
    public Image[] UIhealth;
    public Text UIPoint;
    public GameObject restartBtn;

    // �� ����, �� �������������� ����, ü��
    public int totalPotion;
    public int stagePoint;
    public int health;
    public PlayerMove player;

    public GameObject missionItem;
    public bool getItem;

    public QuestManager questManager;
    public Text questText;  // ����Ʈ ����
    public GameObject menuSet;   // �޴�����

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
            questText.text = "�������� ����Ʈ�� �����ϴ�.";
        else
            questText.text = questManager.CheckQuest();
    }

    void Update()
    {
        UIPoint.text = (totalPotion + stagePoint).ToString();  
        
        if (Input.GetButtonDown("Cancel")) {
            // �޴��� ���� �ִٸ�
            if (menuSet.activeSelf)
                menuSet.SetActive(false);

            // esc�� ������ �޴��� ��������
            else
                menuSet.SetActive(true);
        }
    }

    public void faedStart() {
        totalPotion += stagePoint;
        stagePoint = 0;
        Invoke("NextStage", 1f);
    }

    public void NextStage()
    {
        for (int i = 0; i <= 6; i++) {
            if (SceneManager.GetActiveScene().buildIndex == i && anim.GetBool("Faed") == true) {
                anim.SetBool("Faed", false);
                SceneManager.LoadScene(i+1);
            }
        }  
    }

    public void LastStage()
    {
        anim.SetBool("Faed", false);
        SceneManager.LoadScene(7);
    }
    

    // �������� ������� ��� -1
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            HealthDown();
    }

    public void HealthDown() {
        if (health > 1) {
            health--;
            UIhealth[health+1].color = new Color(0, 0, 0, 0);
        }

        else {
            UIhealth[health].color = new Color(0, 0, 0, 0);
            // ü���� 0�̵Ǹ� �÷��̾��� ���� �Լ� ȣ��
            player.Die();

            // �׾��� ����� UI
            Debug.Log("�׾����ϴ�.");
            restartBtn.SetActive(true);
            imageObject.SetActive(false);
        }
    }

    public void Action(GameObject scanObj) {
        
        scanObject = scanObj;
        objName = scanObject.name;
        // ������ ������Ʈ�� id�� isNpc�� ���� ������
        // �� ������ TalkManager�� ȣ��
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNpc);
        
        // Action�� ���󰡱� ������ ture, false�� �ƴ� isAction�� ����
        talkPanel.SetBool("isShow", isAction);
    }

    void Talk(int id, bool isNpc) {

        // set talk data
        int questTalkIndex = 0;
        string talkData = "";

        if (talkText.isAnim)
        {
            talkText.SetMsg("");
            return;
        }
        else { 
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }

        // ��ȭ ����
        if (talkData == null) {
            isAction = false;   // �׼� ����
            talkIndex = 0;      // ���� ��ȭ�� ���� talkIndex �ʱ�ȭ
            missionItem.SetActive(false);
            if (id == 800)
            {
                anim.SetBool("Faed", true);
                Invoke("LastStage", 1);
            }
            else if (id == 100)
                questText.text = questManager.CheckQuest(id - 10);
            else
                questText.text = questManager.CheckQuest(id);

            
            return;
        }

        // Npc�� ���� �ƴ� ���
        if (isNpc)
        {

            // �����ڴ� |
            // ���ڿ� �̱� ������ �����ڸ� ���� �迭�� ���� => �迭�� ��
            talkText.SetMsg(talkData.Split('|')[0]);

            // NPC�� ��� Image�� ���̵���
            // GetPortrait�Լ��� ����. 
            // Parse�� ���ڿ��� �ش� Ÿ������ ��ȯ�����ִ� �Լ� > int ������ ��ȯ
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split('|')[1]));

            // 1�� ���� �÷��̾� �ƴ϶�� ��ü �̸� ���
            if (int.Parse(talkData.Split('|')[1]) == 1){
                nameText.text = "�����";
            }
            else{
                nameText.text = objName;
            }
            portraitImg.color = new Color(1, 1, 1, 1);
        }

        else
        {
            nameText.text = "";

            talkText.SetMsg(talkData);

            // �繰�� ��� 0���� ������ �ʰ� �Ѵ�.
            portraitImg.color = new Color(1, 1, 1, 0);
        }

        isAction = true;
        // ���� ������ ����ϱ� ���� ����
        talkIndex++;
    }

    public void Restart() {
        // ����� �ϰ� �Ǹ� �ð�����
        Time.timeScale = 1;

        // �׾��� ������������ �ٽ� ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // ��������
    public void GameExit() {
        Application.Quit();
    }
}
