using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 오픈소스를 응용하여 사용

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;    
    public TypeEffect talkText;     // 대화 텍스트 
    public GameObject scanObject;   // Player가 조사한 오브젝트
    public Animator talkPanel;    // 대화를 진행중일때만 판넬을 보이게 할 변수 지정

    // 페이드인,아웃 애니메이션과 오브젝트
    public Animator anim;
    public GameObject imageObject;

    public bool isAction;       // 상태 저장 변수
    public int talkIndex;
    public Image portraitImg;   // 초상화
    public string objName;  // 이름
    public Text nameText;

    // 생명 UI 변수와 당근을 기록하는 포인트 UI 변수
    public Image[] UIhealth;
    public Text UIPoint;
    public GameObject restartBtn;

    // 총 점수, 각 스테이지마다의 점수, 체력
    public int totalPotion;
    public int stagePoint;
    public int health;
    public PlayerMove player;

    public GameObject missionItem;
    public bool getItem;

    public QuestManager questManager;
    public Text questText;  // 퀘스트 내용
    public GameObject menuSet;   // 메뉴생성

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
            questText.text = "진행중인 퀘스트가 없습니다.";
        else
            questText.text = questManager.CheckQuest();
    }

    void Update()
    {
        UIPoint.text = (totalPotion + stagePoint).ToString();  
        
        if (Input.GetButtonDown("Cancel")) {
            // 메뉴가 켜져 있다면
            if (menuSet.activeSelf)
                menuSet.SetActive(false);

            // esc를 누르면 메뉴가 나오도록
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
    

    // 데미지를 입을경우 목숨 -1
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
            // 체력이 0이되면 플레이어의 죽음 함수 호출
            player.Die();

            // 죽었을 경우의 UI
            Debug.Log("죽었습니다.");
            restartBtn.SetActive(true);
            imageObject.SetActive(false);
        }
    }

    public void Action(GameObject scanObj) {
        
        scanObject = scanObj;
        objName = scanObject.name;
        // 선택한 오브젝트의 id와 isNpc의 값을 가져와
        // 그 값으로 TalkManager를 호출
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNpc);
        
        // Action에 따라가기 때문에 ture, false가 아닌 isAction을 넣음
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

        // 대화 끝남
        if (talkData == null) {
            isAction = false;   // 액션 종료
            talkIndex = 0;      // 다음 대화를 위한 talkIndex 초기화
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

        // Npc일 경우와 아닌 경우
        if (isNpc)
        {

            // 구분자는 |
            // 문자열 이기 때문에 구분자를 통해 배열로 나눔 => 배열이 됨
            talkText.SetMsg(talkData.Split('|')[0]);

            // NPC일 경우 Image가 보이도록
            // GetPortrait함수에 전달. 
            // Parse는 문자열을 해당 타입으로 변환시켜주는 함수 > int 형으로 변환
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split('|')[1]));

            // 1인 경우는 플레이어 아니라면 객체 이름 출력
            if (int.Parse(talkData.Split('|')[1]) == 1){
                nameText.text = "토순이";
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

            // 사물인 경우 0으로 보이지 않게 한다.
            portraitImg.color = new Color(1, 1, 1, 0);
        }

        isAction = true;
        // 다음 문장을 출력하기 위해 증가
        talkIndex++;
    }

    public void Restart() {
        // 재시작 하게 되면 시간복구
        Time.timeScale = 1;

        // 죽었던 스테이지에서 다시 시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // 게임종료
    public void GameExit() {
        Application.Quit();
    }
}
