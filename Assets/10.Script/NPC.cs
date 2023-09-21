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
            // �����̽� �ٸ� ������ waitForInput�� true�� �����Ͽ� �ڵ� �ߴ��� �����մϴ�.
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
        // Ư�� ������ ������ ������ ����մϴ�.
        while (waitForInput)
        {
            yield return null; // ���� �����ӱ��� ���
        }

        // Ư�� ������ �����Ǹ� �̰����� ���� �ڵ带 �����մϴ�.
        Debug.Log("Input received!");
    }

    public void StartWaitingForInput()
    {
        waitForInput = true;
        StartCoroutine(WaitForInput());
    }
}
