using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오픈소스 사용
// 25번 ~ 92번째 줄 응용하여 사용
public class TalkManager : MonoBehaviour
{

    // 대화를 저장할 변수 ( ID, ID에 부합하는 Data )
    Dictionary<int, string[]> talkData;
    // 초상화를 저장하는 변수
    Dictionary<int, Sprite> portraitData;

    // 초상화 스프라이트를 저장할 배열
    public Sprite[] portraitArr;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    void GenerateData()
    {
        // TV와 Plyer의 대화
        // #1. 궁극의 만두 먹방을 보고 있는 토끼는 궁극의 만두 레시피를 찾으러 나선다.
        talkData.Add(0 + 100, new string[] { "안녕하세요 여러분! \n이번에 제가 먹을 음식부터 소개해 드리겠습니다!|0", "오늘 준비한 음식은 말이죠..|0", "무려 오브레 마을에서 만두 장인이 직접 만든!!|0","궁극의 만두입니다!!!|0" 
            , "우와.. 그냥 만두도 아니고 궁극의 만두라니..|1", "(꿀꺽) 맛있겠다..|1" , "음냠냠.. 쩝쩝..|0", "역시 궁극의 만두야..|0", "다른 만두는 생각도 안날정도로 맛있습니다..|0"
        ,"(꼬르륵) 나도 먹고싶다..|1", "시청자분들도 드시고 싶으신가요?|0","(격한 끄덕임)|1","그렇다면 오브레 마을에서 맛보실 수 있습니다!|0", "오브레마을?|1"
        ,"모두 재밌게 보셨다면 구독과 좋아ㅇ..|0", "안되겠다, 당장 오브레 마을로 갈 준비를 해야겠어!|1"});

        // #2. 마을로 가는 길 : 몬스터를 만나며, 몬스터가 당근을 떨어트린다.
        talkData.Add( 200, new string[] { "맞다, 여기는 몬스터가 자주 출몰하니까 조심해야해|1", "이 녀석들은 당근을 좋아하던데..|1","밟아보면 당근이 나올 지 몰라!|1"
            , "이왕 나온김에 당근도 좀 챙겨야겠다.|1"});

        // #3. 오브레 마을 도착한 토순이. 만두 장인에게 퀘스트를 받고 만두 레시피를 받음
        // 모자를 쓴 청년
        talkData.Add(10 + 300, new string[] { "혹시 만두 장인이 만든 만두를 먹으려고 왔는데 \n어디로 가면 되나요?|1", "맨 끝에 있는 집으로 가면 먹을 수 있어|0", "근데 지금은 먹기 힘들텐데..|0",
        "엥 어째서요?|1", "이유는 모르지만 딸이 병에 걸린 것 같더라고.|0", "자세한건 옆의 여인한테 물어봐. 그 분 딸의 친구거든.|0"});

        // 총을 든 헌터의 혼잣말
        talkData.Add(11 + 600, new string[] { "이 총이면 헬파이어를 잡을 수 있지 않을까?" , "헉, 무서워라..\n조심히 지나가야 겠다." });

        // 모자를 쓴 여인
        talkData.Add(11 + 400, new string[] {"흑흑..|2", "헉, 왜 울고 계세요?|1", "제 친구가 알 수 없는 병에 걸렸거든요..|2" ,"제가 그 숲에 가자고 하지 않았다면 걸리지 않았을텐데.. 흑..|2",
        "'그 숲' 이요?|1", "마을 바로 뒤에 숲이 있거든요..|2", "거기 숲에서 피는 꽃을 보러 갔다가 그만 몬스터에게 당했지 뭐에요..|2"
        , "헉 몬스터요?|1", "네.. 다 저 때문이에요.. 흑흑|2", "(여자는 눈물 때문에 말하기 힘들어 보인다.)|1", "(다른 마을 사람에게 가서 물어보자)|1"});

        // 마을청년
        talkData.Add(12 + 700, new string[] { "만두 장인님 집이 여긴가요?", "아니 좀 더 가야해", "감사합니다!" });

        // 만두 장인
        talkData.Add(13 + 500, new string[] {"혹시 만두 장인님의 가게인가요?|1", "맞아.. 우리 가게지..|3", "헉 만두 장인님이셨군요! \n괜찮으세요? 안색이 많이 안좋아보여요.|1",
        "우리 딸이 몇주째 일어나지 못하고 있어.|3", "의사의 말로는 헬파이어의 물약을 먹이는 것 밖에 없다는데|3", "우리 마을에선 헬파이어를 잡을만한 인물이 없어..|3",
        "(표정이 너무 안좋아보여.)|1", "(헬파이어를 잡아, 물약을 가져다 드려야겠다!)|1", "그럼 제가 그 물약을 가져올게요!|1", "뭐? 이렇게 조그만데 가져올 수 있겠어?|3", 
            "괜찮아요! 대신 만두 장인님의 만두를 먹을 수 있게 해주세요!|1", "만두가 뭐야, 더 큰 사례도 해줄 수 있어.|3", "아뇨, 저는 만두면 돼요! \n그럼 다녀올게요!|1",
        "정말 고맙다.. 몸 조심하거라.|3"});

        // #.4 물약을 찾고 만두 장인에게 가져다 준다.
        talkData.Add(20 + 5000, new string[] { "헬파이어의 물약이다!", "얼른 가져다 드려야겠다!"});

        talkData.Add(30 + 800, new string[] { "만두장인님! 제가 물약을 가져왔어요!!|1", "오오.. 정말 고맙구나.. 고마워..|3", "얼른 들어가 보세요!|1",
        "(한참 뒤)|1", "정말 너 덕분에 우리 딸의 목숨을 건졌어!\n어떻게 사례해야 될 지 모르겠구나..|3", "따님은 괜찮으세요?|1", 
            "그럼! 얼마만에 일어나있는 딸 아이의 모습을 본건지..|3", "너한테 정말 고맙구나.|3",
        "저 그러면 혹시 부탁 하나가 있는데.. \n들어주실 수 있나요?|1", "물론! 내가 할 수 있는 거라면 뭐든 해주마!|3", "그럼 궁극의 만두를 먹을 수 있도록 해주세요!|1",
        "하하하!!\n그게 부탁이라면 어려울 것도 없지!|3", "배가 터지도록 먹게해주마!|3", "와아 감사합니다!!|1"});


        // 표정이 여러개 일 경우
        // 플레이어의 기본 모습
        portraitData.Add(100 + 0, portraitArr[0]);
        portraitData.Add(100 + 1, portraitArr[1]);

        portraitData.Add(200 + 1, portraitArr[0]);

        portraitData.Add(300 + 0, portraitArr[0]);
        portraitData.Add(300 + 1, portraitArr[1]);

        portraitData.Add(400 + 1, portraitArr[1]);
        portraitData.Add(400 + 2, portraitArr[2]);


        portraitData.Add(500 + 1, portraitArr[1]);
        portraitData.Add(500 + 3, portraitArr[3]);


        portraitData.Add(800 + 1, portraitArr[1]);
        portraitData.Add(800 + 3, portraitArr[3]);

    }

    // id디로 대화를 얻어와서 talkIndext로 대화의 한 문장을 가져온다.
    public string GetTalk(int id, int talkIndex) {
        // 예외처리
        // talkData에 Key가있는지 없는지 검사
        if (!talkData.ContainsKey(id)) {

            // 해당 퀘스트 진행 순서 중 대사가 없을 때
            // 퀘스트 맨 처음의 대사를 가지고 온다.
            if (talkIndex == talkData[id - id % 10].Length)
                return null;
            else
                return talkData[id - id % 10][talkIndex];
        }

        //talkIndext는 string을 배열로 받아왔기 때문에 해당 id의 배열 수 만큼 반복
        // id의 배열 수와 같으면 null반환
        if (talkIndex == talkData[id].Length)
            return null;

        // 아니라면 계속 진행
        else 
            return talkData[id][talkIndex];
    }

    // 지정된 초상화 스프라이트를 반환할 함수
    public Sprite GetPortrait(int id, int portraitIndex) {
        return portraitData[id + portraitIndex];
    }
}
