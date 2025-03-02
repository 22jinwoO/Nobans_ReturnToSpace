using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapParent : MonoBehaviour
{
    [SerializeField]
    private Player playerCs;

    private float startPosY;    // 초기 배경 위치
    private float addPosY;  // 배경위치에 더해지는 값
    private float backgroundSize;   // 그림 높이

    [SerializeField]
    private Transform[] sprites;    // 앞배경 이미지 배열

    [SerializeField]
    private Transform[] backSprites;   // 뒷배경이미지 배열

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
