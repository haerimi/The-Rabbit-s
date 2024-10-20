using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// ���¼ҽ� ���
// 74��° �� ~ ������������ ���� �ڵ� �ۼ�

public class EnemyMove : MonoBehaviour
{

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
   // CapsuleCollider2D collider;
    BoxCollider2D collideBox;
    // ������ �������� ������ ���� (���, �̼Ǿ�����)
    public GameObject itemCarrot;
    //public GameObject itemMission;

    public GameObject missionItem;
    // �ൿ��ǥ�� ������ ����
    public int nextMove;
    Animator anim;

    // 0.8���� �����
    public float DestroyTime = 0.8f; 
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //collider = GetComponent<CapsuleCollider2D>();
        collideBox = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        // �־��� �ð��� ���� �� ������ �Լ��� ����
        // ������ �ʸ�ŭ ������
        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        // ������ ������ : ������ -1 �������� 1
        // ���� y�������� �̵�
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // ������������ �ƴ��� üũ
        // ������ �������� üũ�ؾ� �ϱ� ������ nextMove���� ��� (int�� �ٷ� ���� ����)
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.1f, rigid.position.y) ;
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platfrom", "Object"));

        // collider�� ������ �־�� ��
        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    // �ൿ ��ǥ�� �ٲ��� �Լ��̸鼭 ��� �Լ�
    void Think() {
        // nextMove�� �������� ���������� ���ߴ��� �������� ����
        nextMove = Random.Range(-1, 2);    // �ּڰ�, �ִ밪 (�ִ밪�� ���Ե�������)
                                           // ���� 1�� �����ϱ� ���� 2�� ����
        float nextThinkTime = Random.Range(2f, 5f);     // �����ϴ� �ð� ����
        Invoke("Think", nextThinkTime);
    }
    
    void Turn() {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 2);
    }

    public void OnDamaged() {
        // ������ �����
        int ran = Random.Range(0, 8);
        // 30�� non Item
        if (ran < 3)
        {
            Debug.Log("Not Item");
        }
        // 50�� ���
        else if (ran < 8)
        {
            Instantiate(itemCarrot, transform.position, itemCarrot.transform.rotation);
            Debug.Log("Get!!");
        }

        // �����ϰ� �����
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        // Y�� ������
        spriteRenderer.flipX = true;
        //�ݶ��̴� ����
        collideBox.enabled = false;
        // �߶�
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // ����
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
