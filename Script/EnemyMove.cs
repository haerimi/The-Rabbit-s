using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 오픈소스 사용
// 74번째 줄 ~ 마지막까지는 직접 코드 작성

public class EnemyMove : MonoBehaviour
{

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
   // CapsuleCollider2D collider;
    BoxCollider2D collideBox;
    // 아이템 프리펩을 저장할 변수 (당근, 미션아이템)
    public GameObject itemCarrot;
    //public GameObject itemMission;

    public GameObject missionItem;
    // 행동지표를 결정할 변수
    public int nextMove;
    Animator anim;

    // 0.8초후 사라짐
    public float DestroyTime = 0.8f; 
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //collider = GetComponent<CapsuleCollider2D>();
        collideBox = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        // 주어진 시간이 지난 뒤 지정된 함수를 실행
        // 지정한 초만큼 딜레이
        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        // 스스로 움직임 : 왼쪽은 -1 오른쪽은 1
        // 왼쪽 y방향으로 이동
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // 낭떠러지인지 아닌지 체크
        // 움직인 방향으로 체크해야 하기 때문에 nextMove변수 사용 (int라서 바로 적용 가능)
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.1f, rigid.position.y) ;
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platfrom", "Object"));

        // collider가 무조건 있어야 함
        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    // 행동 지표를 바꿔줄 함수이면서 재귀 함수
    void Think() {
        // nextMove가 왼쪽인지 오른쪽인지 멈추는지 랜덤으로 설정
        nextMove = Random.Range(-1, 2);    // 최솟값, 최대값 (최대값은 포함되지않음)
                                           // 따라서 1을 포함하기 위해 2로 지정
        float nextThinkTime = Random.Range(2f, 5f);     // 생각하는 시간 랜덤
        Invoke("Think", nextThinkTime);
    }
    
    void Turn() {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 2);
    }

    public void OnDamaged() {
        // 아이템 드랍률
        int ran = Random.Range(0, 8);
        // 30퍼 non Item
        if (ran < 3)
        {
            Debug.Log("Not Item");
        }
        // 50퍼 당근
        else if (ran < 8)
        {
            Instantiate(itemCarrot, transform.position, itemCarrot.transform.rotation);
            Debug.Log("Get!!");
        }

        // 투명하게 만들기
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        // Y축 뒤집기
        spriteRenderer.flipX = true;
        //콜라이더 종료
        collideBox.enabled = false;
        // 추락
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // 삭제
        Invoke("DeActive", 5);
    }

    public void OnDamagedBoss() {
        if (Health.health != 0) { 
            Health.health -= 10f;
            anim.SetTrigger("isHit");
        }
        else if (Health.health == 0) {
            anim.SetTrigger("isDie");
            Destroy(gameObject, DestroyTime);
            missionItem.SetActive(true);
           // Instantiate(itemCarrot, transform.position, itemCarrot.transform.rotation);
            Debug.Log("Get!!");
        }
    }

    void DeActive() {
        gameObject.SetActive(false);
    }

 
}
