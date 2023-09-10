using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySystem : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigd;

    [SerializeField]
    private int atkDmg; // ������ ����

    private SpriteRenderer spr; // ��������Ʈ ������

    [SerializeField]
    private Sprite[] enemySprites;  // ���ʹ� �̹��� �迭

    private BoxCollider2D boxCollider;  // �ڽ��ݶ��̴�

    Vector3 startPosition;

    private void Awake()
    {
        atkDmg = 1;
        rigd = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        if (gameObject.tag== "enemySetlite"&& transform.position.y <= 300)  // ���� ������Ʈ�̰� ���� ������ �����ƴٸ�
        {
            spr.sprite = enemySprites[0];
            boxCollider.offset = new Vector2(-0.04f, 0);
            boxCollider.size = new Vector2(2.2f, 1.39f);

            if (transform.position.x < -3f)
            {
                spr.flipX = false;
            }
            else if (transform.position.x > 3f)
            {
                spr.flipX = true;
            }
        }
        else if (gameObject.tag == "enemySetlite" && transform.position.y > 300)    //���� ������Ʈ�̰� ������ ������ �����ƴٸ�
        {
            spr.sprite = enemySprites[1];
            boxCollider.offset = new Vector2(-0.16f, 0);
            boxCollider.size = new Vector2(1.1f, 1.39f);
            if (transform.position.x < -3f)
            {
                spr.flipX = true;
                print(spr.flipX);
            }
            else if (transform.position.x > 3f)
            {
                spr.flipX = false;
            }
        }

        if (gameObject.tag == "enemyComet")
        {
            if (transform.position.x < 0f)
            {
                spr.flipX = true;
            }

            else if (transform.position.x >= 0f)
            {
                spr.flipX = false;
            }
            startPosition = transform.position;

        }
    }
    private void FixedUpdate()
    {
        switch (gameObject.tag)
        {
            case "enemySetlite":    // ���ӿ�����Ʈ �±װ� �����϶�
                
                if (rigd.velocity.x<4)
                {
                    if (transform.position.x<-3f)
                    {
                        rigd.AddForce(new Vector2(100f, 0) * Time.deltaTime, ForceMode2D.Force);
                    }

                    else if (transform.position.x > 3f)
                    {
                        rigd.AddForce(new Vector2(-100f, 0) * Time.deltaTime, ForceMode2D.Force);
                    }
                }
                Destroy(gameObject, 4.5f);
                break;

            case "enemyComet":  // ���ӿ�����Ʈ �±װ� ������ ��

                if (startPosition.x < 0f)
                {
                    rigd.AddForce(new Vector2(1f, -1f) * 200f * Time.deltaTime, ForceMode2D.Force);
                }

                else if (startPosition.x >= 0)
                {
                    rigd.AddForce(new Vector2(-1f, -1f) * 200f * Time.deltaTime, ForceMode2D.Force);
                }


                Destroy(gameObject, 7f);
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerItemRange") // �÷��̾� ��Ʈ������ �浹���� ��
        {
            collision.transform.parent.GetComponent<Player>().StartCoroutine("PlayerGetDamaged",atkDmg); //�÷��̾� ������ �Դ� �Լ� ȣ��
            gameObject.SetActive(false);
        }
        
    }

}
