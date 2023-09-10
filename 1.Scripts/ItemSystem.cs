using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem : MonoBehaviour
{
    private Player playerCs;
    private void Awake()
    {
        playerCs= GameObject.FindObjectOfType<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("BoostItem")&& collision.CompareTag("PlayerItemRange")) // 부스트 아이템 먹었을 때
        {
            playerCs.StartCoroutine("EatBoost");
            gameObject.SetActive(false);
        }

        else if (gameObject.CompareTag("HeartItem") && collision.CompareTag("PlayerItemRange"))   // 산소 아이템 먹었을 때
        {
            playerCs.StartCoroutine("EatHeart");
            gameObject.SetActive(false);
        }

        else if (gameObject.CompareTag("ShieldItem") && collision.CompareTag("PlayerItemRange"))   // 쉴드 아이템 먹었을 떄
        {
            playerCs.usingShieldtime=0;
            playerCs.isShield=true;
            playerCs.StartCoroutine("EatShiled");
            gameObject.SetActive(false);
        }
    }
}
