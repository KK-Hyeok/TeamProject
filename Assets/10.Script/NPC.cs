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
    private bool waitForInput = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 스페이스 바를 누르면 waitForInput를 true로 설정하여 코드 중단을 해제합니다.
            waitForInput = false;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            clickCount = 0;
        }
        if (Input.GetMouseButtonDown(0)) {
            if (clickCount == 0 && panel.activeSelf == true)
            {
                NPCText.text = "A";
                if (Input.GetMouseButtonDown(0))
                    { clickCount++; }
            }
            else if (clickCount == 1 && panel.activeSelf == true)
            {
                NPCText.text = "B";
                if (Input.GetMouseButtonDown(0))
                { clickCount++; }

            }

            else if (clickCount == 2 && panel.activeSelf == true)
            {
                NPCText.text = "C";
                panel.SetActive(false);
                waitForInput = false;
            }
        }
    }

    public void Enter()
    {
        panel.SetActive(true);
        StartWaitingForInput();
    }

    public void Next()
    {
        waitForInput = true;
    }
    IEnumerator WaitForInput()
    {
        // 특정 조건이 충족될 때까지 대기합니다.
        while (waitForInput)
        {
            yield return null; // 다음 프레임까지 대기
        }

        // 특정 조건이 충족되면 이곳에서 다음 코드를 실행합니다.
        Debug.Log("Input received!");
    }

    public void StartWaitingForInput()
    {
        waitForInput = true;
        StartCoroutine(WaitForInput());
    }
}
