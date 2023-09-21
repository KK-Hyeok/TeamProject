using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MovePoint : MonoBehaviour
{

    public CharacterController characterController; // 캐릭터 컨트롤러
    public Vector3 movePoint; // 이동 위치 저장
    public Vector3 AttackPoint;
    public Camera mainCamera; // 메인 카메라
    public Vector3 cameraOffset; // 카메라 
    public Animator animator; // 애니메이터
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    private Vector3 unitPlanePosition; // 유닛의 바닥 위치 ( 거리 계산할 때 사용 )
    private NavMeshAgent agent;
    public float speed;      // 캐릭터 움직임 스피드
    public float rotateSpeed;
    private float CountTime;
    private RaycastHit hit;

    public int ammo;
    public float health;
    public int hasGrenade;

    public int maxAmmo;
    public float maxHealth;
    public int maxGrenade;

    bool isDodge;
    bool DodgeStop;
    bool isSwap;
    bool isReload;
    bool isFireReady = true;
    bool isDamage;
    bool isStop;
    Rigidbody rigid;
    MeshRenderer[] meshs;
    GameObject ItemObject;
    public GameObject equipWeapon;
    Weapon UseWeapon;
    float fireDelay;
    void Start()
    {
        speed = 12.0f;
        rotateSpeed = 0.1f;
        mainCamera = Camera.main;
        characterController = GetComponent<CharacterController>();

        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<MeshRenderer>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;
    }


    void Update()
    {

        // 마우스 우클릭 이벤트가 들어왔다면
        if (Input.GetMouseButton(1))
        {
            // 카메라에서 레이저를 쏜다.
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // 레이저가 뭔가에 맞았다면
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                // 레이저가 맞은 위치를 목적지로 저장
                movePoint = raycastHit.point;

            }
        }
        // 목적지까지 거리가 0.5f 보다 멀다면
        if (Vector3.Distance(unitPlanePosition, movePoint) > 0.5f)
        {

            // Move()함수 실행
            Move();
        }

        // 오브젝트가 앞에 있으면
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * 3, Color.green);
        isStop = Physics.Raycast(transform.position + Vector3.up, transform.forward, 3, LayerMask.GetMask("Water"));

        if (Physics.Raycast(transform.position, transform.forward))
        {
            if (isStop)
            {
                movePoint = transform.position;
            }
        }


        Dodge();
        GetItem();
        Swap();
        Attack();
        Reload();
    }

    void Move()
    {
        if (isSwap || !isFireReady || isReload)
            movePoint = transform.position;
        if (Vector3.Distance(movePoint, transform.position + new Vector3(0, 1.7f, 0)) < 1.9f)
            movePoint = transform.position;
        //agent의 위치를 계산해 movePoint로 이동
        agent.SetDestination(movePoint);
        //movePoint와 Player의 백터값을 계산
        animator.SetBool("is Run", Vector3.Distance(movePoint, transform.position + new Vector3(0,1.7f,0)) >= 1.9f);
        animator.SetBool("is idle", Vector3.Distance(movePoint, transform.position + new Vector3(0, 1.7f, 0)) < 1.9f);
    }

    void StopToWall()
    {
        
    }

    void Dodge () {
        if (Input.GetKeyDown(KeyCode.Space) &&isDodge != true && Vector3.Distance(movePoint , transform.position) >= 1f) {
            if(!isSwap){
                agent.speed *= 3f;
                isDodge = true;
                DodgeStop = true;
                DodgeStop = false;
                animator.SetBool("is Dodge", true);
                Invoke("DodgeOut", 0.4f);
            }

        }
    }

    void DodgeOut ()
    {
        
        agent.speed /= 3f;
        StartCoroutine(DodgeDelay());
        
    }

    void Swap(){
    
        int WeaponIndex = -1;
        if(DodgeStop){
            if(Input.GetKeyDown(KeyCode.Alpha1) && hasWeapons[0] == true ) WeaponIndex = 0;

            else if(Input.GetKeyDown(KeyCode.Alpha1) && (equipWeapon == weapons[WeaponIndex])){
                return;}
            if(Input.GetKeyDown(KeyCode.Alpha2) && hasWeapons[1] == true ) WeaponIndex = 1;

            else if(Input.GetKeyDown(KeyCode.Alpha2) && (equipWeapon == weapons[WeaponIndex])){
                return;}
            if(Input.GetKeyDown(KeyCode.Alpha3) && hasWeapons[2] == true ) WeaponIndex = 2;

            else if(Input.GetKeyDown(KeyCode.Alpha3) && (equipWeapon == weapons[WeaponIndex])){
                return;}
            if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3)){
                if(!isReload){
                    if(equipWeapon != null )
                        equipWeapon.SetActive(false);
            
                    weapons[WeaponIndex].SetActive(true);
                    UseWeapon = weapons[WeaponIndex].GetComponent<Weapon>();
                    if(equipWeapon != weapons[WeaponIndex]){
                        animator.SetTrigger("is Swap");
                        isSwap = true;
                        Invoke("SwapOut", 0.01f);
                    }
                    equipWeapon = weapons[WeaponIndex];
            }
        }
        }
   
    }
    void SwapOut()
    {
        StartCoroutine(SwapDelay());
    }

    void Attack()
    {
        if(UseWeapon == null)
        return;

        fireDelay += Time.deltaTime;
        isFireReady = UseWeapon.rate < fireDelay;

        if(Input.GetMouseButton(0) && isFireReady && !isSwap &&!isReload && DodgeStop && UseWeapon.curAmmo != 0){
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                AttackPoint = raycastHit.point;
                agent.enabled = false;
                AttackPoint.y = (transform.position.y + 0.4f);
                transform.LookAt(AttackPoint);
                agent.enabled = true;

            }
            UseWeapon.Use();
            animator.SetTrigger(UseWeapon.type == Weapon.Type.Melee ? "is Swing" : "is Shot");
            fireDelay = 0;
        }
    }
    
    void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R) && UseWeapon.type == Weapon.Type.Range && !isSwap && isFireReady && DodgeStop){
            animator.SetTrigger("is Reload");
            isReload = true;
            Invoke("ReloadOut", 2.5f);
        }
    }

    void ReloadOut()
    {
        int reAmmo = ammo < UseWeapon.maxAmmo ? ammo : UseWeapon.maxAmmo;
        UseWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }
    
    IEnumerator DodgeDelay()
    {
    yield return new WaitForSeconds(0.5f);    
    DodgeStop = true;
    yield return new WaitForSeconds(3f);
    isDodge = false;
    }

    IEnumerator SwapDelay()
    {

    yield return new WaitForSeconds(0.01f);

    isSwap = false;

    }
    
    IEnumerator OnDamage()
    {
        isDamage = true;
        foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.yellow;
        }
        yield return new WaitForSeconds(1f);

        isDamage = false;
        foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.white;
        }

    }

    void GetItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && ItemObject != null)
        {
            if (ItemObject.tag == "Weapon")
            {
                Item item = ItemObject.GetComponent<Item>();
                int WeaponIndex = item.value;
                hasWeapons[WeaponIndex] = true;

                Destroy(ItemObject);
            }
            else if (ItemObject.tag == "NPC")
            {
                NPC npc = ItemObject.GetComponent<NPC>();
                npc.Enter();
            }
        }
    }
     
    void OnTriggerStay(Collider other)
    {

        if(other.tag == "Weapon")
            ItemObject = other.gameObject;

        else if(other.tag == "NPC")
            ItemObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item") {
            Item item = other.GetComponent<Item>();
            switch(item.type){
                case Item.Type.Ammo:
                    ammo += item.value;
                    if(ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if(health > maxHealth)
                        health = maxHealth;
                    break;
                case Item.Type.Grenade:
                    hasGrenade += item.value;
                    if(hasGrenade > maxGrenade)
                        hasGrenade = maxGrenade;
                    break;
                

            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "EnemyBullet"){
            if(!isDamage){
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;
                StartCoroutine(OnDamage());
            }

        
        }
    }
}