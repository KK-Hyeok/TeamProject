using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Monster : MonoBehaviour
{
    public int MaxHealth;
    public int curHealth;
    public Transform target;
    public BoxCollider meleeArea;
    public bool isChase;
    public bool isAttack;
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
        else {
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

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);
        
        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(1f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nav.enabled){
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }

    void FreezeVelocity()
    {
        if(isChase){
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targerting()
    {
        float targetRadius = 1.5f;
        float targetRange = 2f;

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
