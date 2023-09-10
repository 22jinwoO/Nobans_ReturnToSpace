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
        if (gameObject.CompareTag("BoostItem")&& collision.CompareTag("PlayerItemRange")) // �ν�Ʈ ������ �Ծ��� ��
        {
            playerCs.StartCoroutine("EatBoost");
            gameObject.SetActive(false);
        }

        else if (gameObject.CompareTag("HeartItem") && collision.CompareTag("PlayerItemRange"))   // ��� ������ �Ծ��� ��
        {
            playerCs.StartCoroutine("EatHeart");
            gameObject.SetActive(false);
        }

        else if (gameObject.CompareTag("ShieldItem") && collision.CompareTag("PlayerItemRange"))   // ���� ������ �Ծ��� ��
        {
            playerCs.usingShieldtime=0;
            playerCs.isShield=true;
            playerCs.StartCoroutine("EatShiled");
            gameObject.SetActive(false);
        }
    }
}
