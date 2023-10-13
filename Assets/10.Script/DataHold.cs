using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHold : MonoBehaviour
{
    // 저장할 데이터
    public string objectInfo;

    // 데이터 오브젝트를 다음 씬으로 옮김
    void Awake()
    {
        // 이미 씬 전환을 거친 경우, 중복 생성 방지
        if (GameObject.FindGameObjectsWithTag("Player").Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
