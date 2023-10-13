using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<UiManager>();
            }
            return m_instance;
        }
    }
    private static UiManager m_instance;

    public TextMeshProUGUI HandGunBullets;
    public TextMeshProUGUI SubMachineGunBullets;
    public TextMeshProUGUI BulletText;
    public GameObject Number1;
    public GameObject Number2;
    public GameObject Number3;
    public GameObject gameoverUI;
    public GameObject Handgun;
    public GameObject Submachinegun;
    GameObject Player;

    // Start is called before the first frame update


    void Start()
    {
        Player = GameObject.Find("Luna");

    }

    // Update is called once per frame
    void Update()
    {
        MovePoint move = GetComponent<MovePoint>();
        BulletText.text = Player.GetComponent<MovePoint>().ammo.ToString();

        if(Player.GetComponent<MovePoint>().equipWeapon == Player.GetComponent<MovePoint>().weapons[0])
                {
                    Number1.gameObject.SetActive(true);
                    HandGunBullets.text = " ";
                    Number2.gameObject.SetActive(false);
                    Number3.gameObject.SetActive(false);
                }

        else if (Player.GetComponent<MovePoint>().equipWeapon == Player.GetComponent<MovePoint>().weapons[1])
        {
            Number2.gameObject.SetActive(true);
            HandGunBullets.text = Handgun.GetComponent<Weapon>().curAmmo.ToString() + "/12";
            Number1.gameObject.SetActive(false);
            Number3.gameObject.SetActive(false);
        }

        else if (Player.GetComponent<MovePoint>().equipWeapon == Player.GetComponent<MovePoint>().weapons[2])
        {
            Number3.gameObject.SetActive(true);
            SubMachineGunBullets.text = Submachinegun.GetComponent<Weapon>().curAmmo.ToString() + "/30";
            Number1.gameObject.SetActive(false);
            Number2.gameObject.SetActive(false);
        }

        else
        {
            Number1.gameObject.SetActive(false);
            Number2.gameObject.SetActive(false);
            Number3.gameObject.SetActive(false);
        }

    }

    public void GameOver()
    {
        gameoverUI.gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("StartScene");
    }

    
    public void GameEnd()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void TestS()
    {
        SceneManager.LoadScene("DataHoldTest");
    }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
