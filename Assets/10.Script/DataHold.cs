using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHold : MonoBehaviour
{
    // ������ ������
    public string objectInfo;

    // ������ ������Ʈ�� ���� ������ �ű�
    void Awake()
    {
        // �̹� �� ��ȯ�� ��ģ ���, �ߺ� ���� ����
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
