using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 이전에 만든 게임에서 활용한 코드 사용

public class TypeEffect : MonoBehaviour
{
    public GameObject TextIcon;
    public int CharPerSeconds;  //글자 재생 속도를 위한 변수
    public bool isAnim;
 
    // 원래 대사 저장
    string targetMsg;

    // 몇 글자인지, 글자 재생 속도를 위한 변수
    int index;

    Text msgText;
    float interval;

    PlayerMove player;
    AudioSource audioText;

    private void Awake()
    {
        msgText = GetComponent<Text>();
        audioText = GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    public void SetMsg( string msg )
    {
        if (isAnim)
        {
            msgText.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        }
        else { 
            targetMsg = msg;
            EffectStart();
        }
    }

    // Update is called once per frame
    void EffectStart()
    {
        // 공백을 가져야함
        msgText.text = "";
        // 개수비교
        index = 0;
        TextIcon.SetActive(false) ;


        // 시작 애니메이션
        interval = 1.0f / CharPerSeconds;
        isAnim = true;
        Invoke("Effecting", interval);  //1글자가 나오는 딜레이(1/CharPerSeconds)
    }

    void Effecting()
    {
        if (msgText.text == targetMsg) {
            EffectEnd();
            return;
        }
        msgText.text += targetMsg[index];

        if (targetMsg[index] != ' ' || targetMsg[index] != '.') {
            audioText.Play();
        }
        index++;
        Invoke("Effecting", interval);  //1글자가 나오는 딜레이(1/CharPerSeconds)


    }
    void EffectEnd()
    {
        // Text가 일치하면 종료
        TextIcon.SetActive(true);
        isAnim = false;

    }
}
