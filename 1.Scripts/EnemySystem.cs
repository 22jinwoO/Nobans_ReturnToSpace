using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySystem : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigd;

    [SerializeField]
    private int atkDmg; // 데미지 변수

    private SpriteRenderer spr; // 스프라이트 렌더러

    [SerializeField]
    private Sprite[] enemySprites;  // 에너미 이미지 배열

    private BoxCollider2D boxCollider;  // 박스콜라이더

    Vector3 startPosition;

    private void Awake()
    {
        atkDmg = 1;
        rigd = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        if (gameObject.tag== "enemySetlite"&& transform.position.y <= 300)  // 위성 오브젝트이고 왼쪽 벽에서 생성됐다면
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
        else if (gameObject.tag == "enemySetlite" && transform.position.y > 300)    //위성 오브젝트이고 오른쪽 벽에서 생성됐다면
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
            case "enemySetlite":    // 게임오브젝트 태그가 위성일때
                
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

            case "enemyComet":  // 게임오브젝트 태그가 유성일 때

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
        if (collision.tag == "PlayerItemRange") // 플레이어 히트범위와 충돌했을 때
        {
            collision.transform.parent.GetComponent<Player>().StartCoroutine("PlayerGetDamaged",atkDmg); //플레이어 데미지 입는 함수 호출
            gameObject.SetActive(false);
        }
        
    }

}
