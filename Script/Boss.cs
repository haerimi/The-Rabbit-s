using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���¼ҽ� ���
// 53��° �� ~ 91��° ���� �����Ͽ� ���
// 113��° �� ���ʹ� ���� �ۼ�

public class Boss : MonoBehaviour
{
    PlayerMove playerMove;
	public float movePower = 1f;
    Animator anim;
	Vector3 movement;
	// 0�� �⺻, 1�� ������, 2�� ������
	int movementFlag = 0;
    GameObject traceTarge;
    public bool isTracing;
    public GameObject fire;
    Rigidbody2D rigid;
    Vector3 moveVelovity;
    GameObject scanObject;
    public bool value;

    void Start()
    {
		anim = gameObject.GetComponentInChildren<Animator>();
        traceTarge = GetComponent<GameObject>();
        playerMove = GetComponent<PlayerMove>();
        rigid = GetComponent<Rigidbody2D>();
        scanObject = GetComponent<GameObject>();
        StartCoroutine("Change");
    }

    // �ڷ�ƾ�� �̿��� ������
    public IEnumerator Change() {

        // �������� ����ǰ�
        movementFlag = Random.Range(0, 3);

        // �ִϸ��̼�
        if (movementFlag == 0)
            anim.SetBool("isWalk", false);
        else 
            anim.SetBool("isWalk", true);

        // 3�� ������
        yield return new WaitForSeconds(3f);

        // ��ȣ��
        StartCoroutine("Change");
    }
    void Move()
    {
        moveVelovity = Vector3.zero;
        string dist = "";

        if (isTracing)
        {
            Vector3 playerPos = traceTarge.transform.position;

            if (playerPos.x < transform.position.x)
                dist = "Right";
            else if (playerPos.x > transform.position.x)
                dist = "Left";
        }

        else
        {
            if (movementFlag == 1)
                dist = "Right";
            else if (movementFlag == 2)
                dist = "Left";
        }

        // ���������� �̵�
        if (dist == "Right")
        {
            moveVelovity = Vector3.left;
            transform.localScale = new Vector3(-8, 8, 1);
        }

        // �������� �̵�
        else if (dist == "Left")
        {
            moveVelovity = Vector3.right;
            transform.localScale = new Vector3(8, 8, 1);
        }

        transform.position += moveVelovity * movePower * Time.deltaTime;
    }

    void FixedUpdate()
    {
        Move();  
        // Ray (player ���� �ν��� ������ ��)
        Debug.DrawRay(rigid.position, moveVelovity * 4.0f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, moveVelovity, 4.0f, LayerMask.GetMask("Player"));

        // �繰�� ���� ���
        if (rayHit.collider != null)
        {
            value = true;
            Invoke("AttackTarget", 0.5f);
        }

        else
            notAttack();
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾ �����ߴ��� Ȯ��
        if (collision.gameObject.tag == "Player") {
            traceTarge = collision.gameObject;
            StopCoroutine("Change");
        }
    }

    // �����ȿ� ���� �� �÷��̾ ����
     void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            isTracing = true;
            anim.SetBool("isWalk", true);
        }
    }

    // ���� ������ ��� �� �ٽ� Chang�� Ȱ��ȭ
     void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTracing = false;
            StartCoroutine("Change");
            anim.SetBool("doAttack", false);
        }
    }

    void AttackTarget() {
        if (value) {
            anim.SetBool("doAttack", true);
            fire.SetActive(true);
        }
        value = false;
    }

    void notAttack() {
        if (value == false) { 
            anim.SetBool("doAttack", false);
            fire.SetActive(false);
        }
    }
}
