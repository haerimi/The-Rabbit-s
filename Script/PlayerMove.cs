using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 153�� ~ 171����° ��, 238�� ~ 278��° ���� ���¼ҽ� �����Ͽ� ���
// �������� ���� �ۼ�

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

    // ���� �ٶ󺸰� �ִ� ���� ���� ���� ����
    Vector3 dirVec;

    public bool attacked = false;
    public int atkDmg;
    int boss = 1;

    // ���̵� �ִϸ��̼�
    public Animator faed;

    // �����
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
        // isAction�� false�� ��� false�� Player�� ������ ���߰� �Ѵ�.
        jump = manager.isAction ? false : Input.GetKeyDown(KeyCode.LeftControl);
        if (jump && !anim.GetBool("isJump"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
            PlaySound("JUMP");
        }

        // ����Ű�� ������ ������� �������� �ִϸ��̼� 
        if (Input.GetKeyUp(KeyCode.LeftControl))
            anim.SetBool("isFall", true);

        // Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            // normalized : ���� ũ�⸦ 1�� ���� ���� (��������)
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y); ;
        }

        // ������ȯ Direction Sprite
        // flip�� bool ��
        if (Input.GetButtonDown("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        // Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.3) 
            anim.SetBool("isWalking", false);
        
        else
            anim.SetBool("isWalking", true);
         

        // Direction (��, ��)
        if (h == 1)
            dirVec = Vector3.right;
        else if (h == -1)
            dirVec = Vector3.left;

        // Scan Object
        // Space�� ������ ��ü�� null�� �ƴ϶�� �Ŵ����� ������Ʈ �̸��� �ѱ��.
        if (Input.GetKeyDown(KeyCode.Space) && scanObject != null)
        {
            manager.Action(scanObject);
            Debug.Log(scanObject);
        }

        // ���� Ű + isBoard�� ��� ���� ���������� �̵�
        if (Input.GetKeyDown(KeyCode.UpArrow) && isBoard) {
            Fade();                 // ���̵���
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
        // isAction�� false�� ��� 0���� Player�� �������� ���߰� �Ѵ�.
        h = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // Max Speed
        // velocity�� ������ٵ��� ���� �ӵ�
        if (rigid.velocity.x > maxSpeed)    // Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))    // Left Max Speed, ������ �����̱� ������ -1�� ����
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        // Ray (player ���� �ν��� ������ ��)
        Debug.DrawRay(rigid.position, dirVec * 2.0f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 2.0f, LayerMask.GetMask("Object", "NPC", "MissionItem"));

        // �繰�� ���� ���
        if (rayHit.collider != null)
        {
            // scanObject�� ����
            scanObject = rayHit.collider.gameObject;
        }
        else
            scanObject = null;


        //Landing Platfrom
        // Player�� �ٴڿ����� ������ �� �ֵ���
        if (rigid.velocity.y < 0)
        {
            // Player�� �����ϴ� ���� �ƴ϶� �ٴ��� �����ؾ� ��
            // ���� LayerMask�� ����� Paltfrom�� ����
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHitDown = Physics2D.Raycast(rigid.position, Vector3.down, 3, LayerMask.GetMask("Platfrom", "Object"));
            //RaycastHit2D rayHitStage = Physics2D.Raycast(rigid.position, Vector3.down, 6, LayerMask.GetMask("Next"));
            if (rayHitDown.collider != null)
            {
                Debug.Log(rayHitDown.distance);
                // ���� �Ÿ��� �÷��̾� ũ���� �� ���� ���� ���
                if (rayHitDown.distance < 4.5f)
                {
                    anim.SetBool("isJump", false);
                    anim.SetBool("isFall", false);
                }
            }
        }
    }

    public bool isBoard;

    // ������ �ݶ��̴��� ������ �����´�.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �������� �̵�
        if (collision.CompareTag("Next"))
        {
            isBoard = true;
        }

        // ������ �浹
        if (collision.gameObject.tag == "Item") {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type) {
                // ��� �������� ȹ���ϸ� ��� ���� 1 ����
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
            // ���͸� ���� ���
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                if (collision.gameObject.tag == "Boss")
                    boss = 0;
                OnAttack(collision.transform, collision.transform.position, boss);
            }

            else
                // �������� ���� ���
                OnDamaged(collision.transform.position);
        }
    }

    void OnAttack(Transform enemy, Vector2 targetPosition, int boss) {

        // ���� ����
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        if (boss == 1)
            enemyMove.OnDamaged();
        else if (boss == 0) {
            enemyMove.OnDamagedBoss();
        }
        PlaySound("ATTACK");

        // ƨ�ܳ���
        int dirc = transform.position.x - targetPosition.x > 0 ? 1 : 1;
        rigid.AddForce(new Vector2(dirc, 1) * 20, ForceMode2D.Impulse);

    }

    void OnDamaged(Vector2 targetPosition) {

        // ü�� ����
        manager.HealthDown();

        // �÷��̾� ������ ���̾�� ����
        gameObject.layer = 14;

        // �������� �¾��� ��� �÷��̾��� ���� �����ϰ� ���� = ��������
        spriteRenderer.color = new Color(1, 1, 1, 0.8f);

        // �������� ������ ƨ�ܳ���
        int dirc = transform.position.x - targetPosition.x > 0 ? 1 : 1;
        rigid.AddForce(new Vector2(dirc, 1) * 12f, ForceMode2D.Impulse);

        // ������ �ִϸ��̼�
        anim.SetTrigger("doDamaged");

        // �ð���
        Invoke("OffDamaged", 2);

        PlaySound("DAMAGED");
    }


    // ���� ���� �Լ�
    void OffDamaged() {
        gameObject.layer = 13;
        spriteRenderer.color = new Color(1, 1, 1, 1);

    }

    // �׾��� ���
    public void Die() {
        // �����ϰ� �����
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        //�ݶ��̴� ����
        collideBox.enabled = false;
        // �߶�
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

