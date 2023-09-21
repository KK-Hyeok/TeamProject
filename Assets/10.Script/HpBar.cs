using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Image RedBar;
    public float health;
    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        RedBar = GetComponent<Image>();


    }

    // Update is called once per frame
    void Update()
    {
        MovePoint movepoint = GameObject.Find("Luna").GetComponent<MovePoint>();
        RedBar.fillAmount = movepoint.health / movepoint.maxHealth;
    }
}
