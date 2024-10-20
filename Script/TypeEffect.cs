using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ������ ���� ���ӿ��� Ȱ���� �ڵ� ���

public class TypeEffect : MonoBehaviour
{
    public GameObject TextIcon;
    public int CharPerSeconds;  //���� ��� �ӵ��� ���� ����
    public bool isAnim;
 
    // ���� ��� ����
    string targetMsg;

    // �� ��������, ���� ��� �ӵ��� ���� ����
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
        // ������ ��������
        msgText.text = "";
        // ������
        index = 0;
        TextIcon.SetActive(false) ;


        // ���� �ִϸ��̼�
        interval = 1.0f / CharPerSeconds;
        isAnim = true;
        Invoke("Effecting", interval);  //1���ڰ� ������ ������(1/CharPerSeconds)
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
        Invoke("Effecting", interval);  //1���ڰ� ������ ������(1/CharPerSeconds)


    }
    void EffectEnd()
    {
        // Text�� ��ġ�ϸ� ����
        TextIcon.SetActive(true);
        isAnim = false;

    }
}
