using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NPC : MonoBehaviour
{
    int clickCount = 0;
    
    GameObject Object;
    public GameObject Npc;
    public TextMeshProUGUI NPCText;
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ESC를 누르면 waitForInput를 true로 설정하여 코드 중단을 해제합니다.
            Exit();
        }
        if (Input.GetKeyDown(KeyCode.E) && panel.activeSelf == false)
        {
            clickCount = 0;
        }
        if (Input.GetMouseButtonDown(0)) {
            if (clickCount == 0 && panel.activeSelf == true)
            {
                NPCText.text = "A";
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
                    { clickCount++; }
            }
            else if (clickCount == 1 && panel.activeSelf == true)
            {
                NPCText.text = "B";
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
                { clickCount++; }

            }

            else if (clickCount == 2 && panel.activeSelf == true)
            {
                NPCText.text = "C";
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
                { clickCount++; }

            }
            else if (clickCount == 3 && panel.activeSelf == true)
            {
                Exit();
            }
        }
    }
    public void Enter()
    {
        panel.SetActive(true);
        Time.timeScale = 0;

    }

    public void Exit()
    {
        panel.SetActive(false);
        NPCText.text = " ";
        Time.timeScale = 1;
        clickCount = 0;
    }
}
