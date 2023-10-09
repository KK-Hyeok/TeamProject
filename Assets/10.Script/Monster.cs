using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{

    public int MaxHealth;
    public int curHealth;
    public Transform target;
    public BoxCollider meleeArea;
    public Slider HP_slider;
    public GameObject gameobject;
    public GameObject[] Item;
    public bool isChase;
    public bool isAttack;
    public float targetRange;
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
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee"){
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            
            StartCoroutine(OnDamage(reactVec));
            
        }
        else if (other.tag == "Bullet"){
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Debug.Log("Bullet : " + curHealth);
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);

            StartCoroutine(OnDamage(reactVec));
            
        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.2f);

        if(curHealth > 0){
            mat.color = Color.white;
        }
        else if (curHealth <= 0){
            mat.color = Color.gray;

            gameObject.layer = 7; // EnemyDead layer
            
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("isDie");

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
        if (curHealth > 0)
        {
            isChase = false;
            isAttack = true;
            anim.SetBool("isAttack", true);
            nav.velocity = Vector3.zero;
            nav.updatePosition = false;
            nav.updateRotation = false;
            yield return new WaitForSeconds(0.4f);
            meleeArea.enabled = true;

            yield return new WaitForSeconds(0.8f);
            meleeArea.enabled = false;

            yield return new WaitForSeconds(1f); ;
            isAttack = false;
            nav.updatePosition = true;
            nav.updateRotation = true;
            anim.SetBool("isAttack", false);
            isChase = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = gameobject.GetComponent<Monster>().MaxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if(nav.enabled){
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
        curHealth = gameobject.GetComponent<Monster>().curHealth;
        HP_slider.maxValue = MaxHealth;
        HP_slider.value = curHealth;

        if (isAttack == true)
        {

        }
    }

    public void DropItem()
    {
        // 아이템을 드롭할 위치를 받아와서 아이템을 생성합니다.
        Instantiate(Item[Random.Range(0, Item.Length)], (this.gameobject.transform.position + Vector3.up), Quaternion.identity);
    }
    void FreezeVelocity()
    {
        if(!isChase){
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
    
        if(rayHits.Length > 0 && !isAttack){
            StartCoroutine(Attack());
        }
    }

    void FixedUpdate()
    {
        Targerting();
        FreezeVelocity();
    }
}
