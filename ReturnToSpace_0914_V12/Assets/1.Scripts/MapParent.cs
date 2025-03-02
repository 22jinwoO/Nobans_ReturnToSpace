using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapParent : MonoBehaviour
{
    [SerializeField]
    private Player playerCs;

    private float startPosY;    // �ʱ� ��� ��ġ
    private float addPosY;  // �����ġ�� �������� ��
    private float backgroundSize;   // �׸� ����

    [SerializeField]
    private Transform[] sprites;    // �չ�� �̹��� �迭

    [SerializeField]
    private Transform[] backSprites;   // �޹���̹��� �迭

    void Awake()
    {
        startPosY = 12.2f;
        
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].localPosition = new Vector3(0,startPosY+addPosY,10);
            backSprites[i].localPosition = new Vector3(0, startPosY + addPosY, 10);
            addPosY +=38.4f;
        }
    }

    // Update is called once per frame
    void Update()
    {
            transform.position = Camera.main.transform.position;
    }
}
