using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMove : MonoBehaviour
{
    [SerializeField]
    private Player playerCs;

    [SerializeField]
    private Transform playerTr;

    [SerializeField]
    private float camera;

    [SerializeField]
    private float cameralong;

    [SerializeField]
    private float back_pivot;
    private float back_height;
    private float target_height;

    [SerializeField]
    private int startIndex;

    [SerializeField]
    private int endIndex;

    [SerializeField]
    private float speed;    // 맵 이동 속도

    [SerializeField]
    private Transform[] sprites;    // 앞배경 이미지 배열

    [SerializeField]
    private Sprite[] backSprites;   // 뒷배경이미지 배열

    private SpriteRenderer spr; // 스프라이트 렌더러
    private float viewHeight;


    [SerializeField]
    Rigidbody2D playerRigd; // 플레이어 리지드바디


    /*
    맵오브젝트의 위치를 카메라의 위치로 업데이트문에서 계속 반영
    앞배경 오브젝트의 이미지    
    */

    private void Awake()
    {
        viewHeight = Camera.main.orthographicSize;
        playerCs = GameObject.FindObjectOfType<Player>();
        spr= GetComponent<SpriteRenderer>();
        if (gameObject.CompareTag("BackGround"))
        {
            speed = 0.25f;
        }

    }
    void Update()
    {
        if (playerRigd.velocity.y>=0f&&playerCs.isGameStart)
        {
            Vector3 curPos = transform.position;
            Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
            transform.position = curPos + nextPos;

            // 그림높이 38.4f
            if (transform.localPosition.y < -26.2f)
            {
                if (spr.sprite == backSprites[0])
                {
                    spr.sprite = backSprites[2];
                }
                else if (spr.sprite == backSprites[1])
                {
                    spr.sprite = backSprites[3];
                }
                transform.localPosition = new Vector3(0, 89f, 10);
            }
        }
        
        
    }
    }

