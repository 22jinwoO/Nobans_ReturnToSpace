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
    private float speed;    // �� �̵� �ӵ�

    [SerializeField]
    private Transform[] sprites;    // �չ�� �̹��� �迭

    [SerializeField]
    private Sprite[] backSprites;   // �޹���̹��� �迭

    private SpriteRenderer spr; // ��������Ʈ ������
    private float viewHeight;


    [SerializeField]
    Rigidbody2D playerRigd; // �÷��̾� ������ٵ�


    /*
    �ʿ�����Ʈ�� ��ġ�� ī�޶��� ��ġ�� ������Ʈ������ ��� �ݿ�
    �չ�� ������Ʈ�� �̹���    
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

            // �׸����� 38.4f
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

