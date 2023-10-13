using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject StartButton;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStart()
    {
        Color color = StartButton.GetComponent<Image>().color;
        color.a = 0.7f;
        StartButton.GetComponent<Image>().color = color;
        Invoke("ButtonClickOut", 1f);
    }
    void ButtonClickOut()
    {
        Color color = StartButton.GetComponent<Image>().color;
        color.a = 1f;
        StartButton.GetComponent<Image>().color = color;
        SceneManager.LoadScene("TeamProject");
    }
}
