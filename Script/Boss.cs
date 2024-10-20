using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 오픈소스 사용
// 53번째 줄 ~ 91번째 줄은 응용하여 사용
// 113번째 줄 부터는 직접 작성

public class Boss : MonoBehaviour
{
    PlayerMove playerMove;
	public float movePower = 1f;
    Animator anim;
	Vector3 movement;
	// 0은 기본, 1은 오른쪽, 2는 오른쪽
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

    // 코루틴을 이용한 딜레이
    public IEnumerator Change() {

        // 랜덤으로 변경되게
        movementFlag = Random.Range(0, 3);

        // 애니메이션
        if (movementFlag == 0)
            anim.SetBool("isWalk", false);
        else 
            anim.SetBool("isWalk", true);

        // 3초 딜레이
        yield return new WaitForSeconds(3f);

        // 재호출
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

        // 오른쪽으로 이동
        if (dist == "Right")
        {
            moveVelovity = Vector3.left;
            transform.localScale = new Vector3(-8, 8, 1);
        }

        // 왼쪽으로 이동
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
        // Ray (player 앞의 인식이 가능한 선)
        Debug.DrawRay(rigid.position, moveVelovity * 4.0f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, moveVelovity, 4.0f, LayerMask.GetMask("Player"));

        // 사물이 있을 경우
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
        // 플레이어가 진입했는지 확인
        if (collision.gameObject.tag == "Player") {
            traceTarge = collision.gameObject;
            StopCoroutine("Change");
        }
    }

    // 범위안에 있을 때 플레이어를 추적
     void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            isTracing = true;
            anim.SetBool("isWalk", true);
        }
    }

    // 범위 밖으로 벗어날 때 다시 Chang를 활성화
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
