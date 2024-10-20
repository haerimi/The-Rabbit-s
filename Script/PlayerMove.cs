using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 153번 ~ 171번번째 줄, 238번 ~ 278번째 줄은 오픈소스 응용하여 사용
// 나머지는 직접 작성

public class PlayerMove : MonoBehaviour
{
    public GameManager manager;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    BoxCollider2D collideBox;
    Animator anim;
    float h;

    bool jump;
    GameObject scanObject;

    // 현재 바라보고 있는 방향 값을 가진 변수
    Vector3 dirVec;

    public bool attacked = false;
    public int atkDmg;
    int boss = 1;

    // 페이드 애니메이션
    public Animator faed;

    // 오디오
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioDie;
    public AudioClip audioItem;
    AudioSource audioSource;

    void Start()
    {
        atkDmg = 10;
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanObject = GetComponent<GameObject>();
        collideBox = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {

        // Jump
        // isAction이 false인 경우 false로 Player의 점프를 멈추게 한다.
        jump = manager.isAction ? false : Input.GetKeyDown(KeyCode.LeftControl);
        if (jump && !anim.GetBool("isJump"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
            PlaySound("JUMP");
        }

        // 점프키를 누르지 않을경우 떨어지는 애니메이션 
        if (Input.GetKeyUp(KeyCode.LeftControl))
            anim.SetBool("isFall", true);

        // Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            // normalized : 벡터 크기를 1로 만든 상태 (단위벡터)
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y); ;
        }

        // 방향전환 Direction Sprite
        // flip은 bool 값
        if (Input.GetButtonDown("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        // Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.3) 
            anim.SetBool("isWalking", false);
        
        else
            anim.SetBool("isWalking", true);
         

        // Direction (좌, 우)
        if (h == 1)
            dirVec = Vector3.right;
        else if (h == -1)
            dirVec = Vector3.left;

        // Scan Object
        // Space를 누르고 객체가 null이 아니라면 매니저에 오브젝트 이름을 넘긴다.
        if (Input.GetKeyDown(KeyCode.Space) && scanObject != null)
        {
            manager.Action(scanObject);
            Debug.Log(scanObject);
        }

        // 위쪽 키 + isBoard인 경우 다음 스테이지로 이동
        if (Input.GetKeyDown(KeyCode.UpArrow) && isBoard) {
            Fade();                 // 페이드인
            manager.faedStart();
        }
    }

    void Fade()
    {
        faed.SetBool("Faed", true);
    }

    void FixedUpdate()
    {

        // Move By Key Control
        // Move Speed
        // isAction이 false인 경우 0으로 Player의 움직임을 멈추게 한다.
        h = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // Max Speed
        // velocity는 리지드바디의 현재 속도
        if (rigid.velocity.x > maxSpeed)    // Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))    // Left Max Speed, 왼쪽은 음수이기 때문에 -1을 곱함
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        // Ray (player 앞의 인식이 가능한 선)
        Debug.DrawRay(rigid.position, dirVec * 2.0f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 2.0f, LayerMask.GetMask("Object", "NPC", "MissionItem"));

        // 사물이 있을 경우
        if (rayHit.collider != null)
        {
            // scanObject에 저장
            scanObject = rayHit.collider.gameObject;
        }
        else
            scanObject = null;


        //Landing Platfrom
        // Player가 바닥에서만 진행할 수 있도록
        if (rigid.velocity.y < 0)
        {
            // Player를 감지하는 것이 아니라 바닥을 감지해야 함
            // 따라서 LayerMask를 사용해 Paltfrom을 구별
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHitDown = Physics2D.Raycast(rigid.position, Vector3.down, 3, LayerMask.GetMask("Platfrom", "Object"));
            //RaycastHit2D rayHitStage = Physics2D.Raycast(rigid.position, Vector3.down, 6, LayerMask.GetMask("Next"));
            if (rayHitDown.collider != null)
            {
                Debug.Log(rayHitDown.distance);
                // 닿은 거리가 플레이어 크기의 반 보다 작을 경우
                if (rayHitDown.distance < 4.5f)
                {
                    anim.SetBool("isJump", false);
                    anim.SetBool("isFall", false);
                }
            }
        }
    }

    public bool isBoard;

    // 접촉한 콜라이더의 정보를 가져온다.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 스테이지 이동
        if (collision.CompareTag("Next"))
        {
            isBoard = true;
        }

        // 아이템 충돌
        if (collision.gameObject.tag == "Item") {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type) {
                // 당근 아이템을 획득하면 당근 갯수 1 증가
                case "Carrot":
                    manager.stagePoint += 1;
                    break;   
            }
            Destroy(collision.gameObject);
            PlaySound("ITEM");
        }

        if (collision.gameObject.tag == "MissionItem")
            manager.getItem = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss" || collision.gameObject.tag == "Fire")
        {
            // 몬스터를 밟을 경우
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                if (collision.gameObject.tag == "Boss")
                    boss = 0;
                OnAttack(collision.transform, collision.transform.position, boss);
            }

            else
                // 데미지를 맞을 경우
                OnDamaged(collision.transform.position);
        }
    }

    void OnAttack(Transform enemy, Vector2 targetPosition, int boss) {

        // 몬스터 죽음
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        if (boss == 1)
            enemyMove.OnDamaged();
        else if (boss == 0) {
            enemyMove.OnDamagedBoss();
        }
        PlaySound("ATTACK");

        // 튕겨나감
        int dirc = transform.position.x - targetPosition.x > 0 ? 1 : 1;
        rigid.AddForce(new Vector2(dirc, 1) * 20, ForceMode2D.Impulse);

    }

    void OnDamaged(Vector2 targetPosition) {

        // 체력 감소
        manager.HealthDown();

        // 플레이어 데미지 레이어로 변경
        gameObject.layer = 14;

        // 데미지를 맞았을 경우 플레이어의 색을 투명하게 만듬 = 무적상태
        spriteRenderer.color = new Color(1, 1, 1, 0.8f);

        // 데미지를 맞으면 튕겨나감
        int dirc = transform.position.x - targetPosition.x > 0 ? 1 : 1;
        rigid.AddForce(new Vector2(dirc, 1) * 12f, ForceMode2D.Impulse);

        // 데미지 애니메이션
        anim.SetTrigger("doDamaged");

        // 시간차
        Invoke("OffDamaged", 2);

        PlaySound("DAMAGED");
    }


    // 무적 해제 함수
    void OffDamaged() {
        gameObject.layer = 13;
        spriteRenderer.color = new Color(1, 1, 1, 1);

    }

    // 죽었을 경우
    public void Die() {
        // 투명하게 만들기
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        //콜라이더 종료
        collideBox.enabled = false;
        // 추락
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        PlaySound("DIE");
    }

    // Audio
    void PlaySound(string action) {
        switch (action) {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
        }
        audioSource.Play();
    }
}

