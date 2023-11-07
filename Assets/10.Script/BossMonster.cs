using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossMonster : MonoBehaviour
{
    public int MaxHealth;
    public int curHealth;
    public Slider Boss_HP;
    Transform target;
    public BoxCollider meleeArea1;
    public BoxCollider meleeArea2;
    // public Slider Boss_HP;
    public GameObject gameobject;
    public GameObject[] Item;
    public GameObject panel;
    public bool isStop;
    public bool isidle;
    public bool isAttack;
    bool isLook;
    bool isJump = false;
    bool isDefend = false;
    bool isheal = false;

    float ObjectDistance;
    public float targetRange;
    private Vector3 unitPlanePosition;

    Vector3 lookVec;
    Vector3 tauntVec;
    

    Material mat;
    Rigidbody rigid;
    BoxCollider boxCollider;
    NavMeshAgent nav;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        Invoke("ChaseStart", 2);
        StartCoroutine(Think());
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(4f);
        if (ObjectDistance > 12f && ObjectDistance < 60f && curHealth > 0 && !isAttack)
        {

            int ranAction = Random.Range(0, 6);
            switch (ranAction)
            {
                case 0:
                case 1:
                    StartCoroutine(Defend());
                    break;// ���


                case 2:
                case 3:
                case 4:
                    StartCoroutine(Jump());
                    break;// ����

                case 5:
                    StartCoroutine(Heal());
                    break;// ��ȯ
            }
        }
        else
        {
            yield return new WaitForSeconds(6f);
            StartCoroutine(Think());
        }


    }
    IEnumerator Defend()
    {

        anim.SetTrigger("Defend");
        isDefend = true;
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(2.8f);
        isDefend = false;
        StartCoroutine(Think());
    }
    IEnumerator Jump()
    {
        Stop();
        yield return new WaitForSeconds(0.6f);
        StopEnd();
        anim.SetTrigger("Jump");
        isJump = true;
        Vector3 JumpPos = target.position;
        nav.speed = 80;
        yield return new WaitForSeconds(0.9f);
        Stop();
        isJump = false;
        nav.speed = 6;
        StopEnd();
        yield return new WaitForSeconds(1.8f);

        StartCoroutine(Think());
    }
    IEnumerator Heal()
    {
        anim.SetTrigger("Heal");
        isheal = true;
        curHealth += (MaxHealth - curHealth)/5;
        yield return new WaitForSeconds(2.2f);
        isheal = false;
        StartCoroutine(Think());
    }

    void ChaseStart()
    {
        isidle = true;
        anim.SetBool("Walk", true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee" && gameObject.layer == 6)
        {
            Weapon weapon = other.GetComponent<Weapon>();
            if (isDefend) {
                curHealth -= (weapon.damage / 2);

            }
            else {
                curHealth -= weapon.damage;
            }

            Vector3 reactVec = transform.position - other.transform.position;

            StartCoroutine(OnDamage(reactVec));

        }
        else if (other.tag == "Bullet" && gameObject.layer == 6)
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (isDefend)
            {
                curHealth -= (bullet.damage / 2);

            }
            else
            {
                curHealth -= bullet.damage; 
            }
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);

            StartCoroutine(OnDamage(reactVec));

        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {
        mat.color = Color.red;
        gameObject.layer = 7;
        yield return new WaitForSeconds(0.2f);

        if (curHealth > 0)
        {
            mat.color = Color.white;
            gameObject.layer = 6;
        }
        else if (curHealth <= 0)
        {
            mat.color = Color.gray;

            gameObject.layer = 7; // EnemyDead layer

            isidle = false;
            nav.enabled = false;
            anim.SetTrigger("Die");

            reactVec = reactVec.normalized;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            Destroy(gameObject, 4);
        }
    }

    void OnDestroy()
    {
        if (curHealth <= 0)
        {
            DropItem();
        }
    }
    IEnumerator Attack()
    {
        int ranAttack = Random.Range(0, 2);

        if (curHealth > 0){
            
            isAttack = true;
            Stop();
            switch (ranAttack)
            {
                case 0:
                    anim.SetBool("Attack1", true);
                    yield return new WaitForSeconds(0.4f);
                    meleeArea1.enabled = true;
                    yield return new WaitForSeconds(0.1f);
                    meleeArea1.enabled = false;
                    yield return new WaitForSeconds(0.5f);
                    anim.SetBool("Attack1", false);
                    break;
                case 1:
                    anim.SetBool("Attack2", true);
                    yield return new WaitForSeconds(0.4f);
                    meleeArea2.enabled = true;
                    yield return new WaitForSeconds(0.1f);
                    meleeArea2.enabled = false;
                    yield return new WaitForSeconds(0.5f);
                    anim.SetBool("Attack2", false);
                    break;
            }
            isAttack = false;
            StopEnd();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = gameobject.GetComponent<BossMonster>().MaxHealth;
        InvokeRepeating("UpdateTarget", 0f, 0.25f);

    }
    private void UpdateTarget()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 50f);

        if (cols.Length > 0)
        {

            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].tag == "Player")
                {
                    target = cols[i].gameObject.transform;
                }
            }
        }
        else
        {
            target = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        ObjectDistance = Vector3.Distance(this.transform.position, target.position);
        if (curHealth > 0 )
        {
            if (ObjectDistance > 50f || isDefend || isheal)
            {
                Stop();
                anim.SetBool("Walk", false);
            }
            else
            {
                StopEnd();
                ChaseStart();

            }
        }
        if (ObjectDistance < 60f && curHealth > 1) { Enter(); }

        else if (ObjectDistance >= 60f || curHealth <= 0) { Exit(); }
        

        if (nav.enabled && !isJump)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isidle;
        }
        curHealth = gameobject.GetComponent<BossMonster>().curHealth;
        Boss_HP.maxValue = MaxHealth;
        Boss_HP.value = curHealth;

        if (isLook && !isJump)
        {
            transform.LookAt(target.position);
        }
        

    }

        public void DropItem()
    {
        // �������� ����� ��ġ�� �޾ƿͼ� �������� �����մϴ�.
        Instantiate(Item[Random.Range(0, Item.Length)], (this.gameobject.transform.position + Vector3.up), Quaternion.identity);
    }
    void FreezeVelocity()
    {
        if (!isidle)
        {
            nav.velocity = Vector3.zero;
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }
    void Targerting()
    {
        float targetRadius = 1.5f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
                                                     targetRadius,
                                                     transform.forward,
                                                     targetRange,
                                                     LayerMask.GetMask("Player"));

        if (rayHits.Length > 0)
        {
            StartCoroutine(Attack());
        }
    }
    void Stop()
    {
        isAttack = true;
        isidle = false;
        nav.enabled = false;
        nav.velocity = Vector3.zero;
        nav.updatePosition = false;
        nav.updateRotation = false;
    }
    void StopEnd()
    {
        nav.enabled = true;
        nav.updatePosition = true;
        nav.updateRotation = true;
        isidle = true;
        isAttack = false;
    }
    void FixedUpdate()
    {
        Targerting();
        FreezeVelocity();
    }
    public void Enter()
    {
        panel.SetActive(true);

    }

    public void Exit()
    {
        panel.SetActive(false);
    }
}

