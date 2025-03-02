using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [Header("플레이어 위치 트랜스폼")]
    public Transform player;
    private Player playerCs;    //플레이어 스크립트

    void Start()
    {
        playerCs = player.GetComponent<Player>();
    }

    void Update()
    {
        Vector3 targetPos = new Vector3(0, player.transform.position.y - 15f, -10f);
        Vector3 myPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (playerCs.rigd.velocity.y < 1.5f)
        {
            transform.position = Vector3.Lerp(myPos, targetPos, Time.deltaTime * 0.8f);
        }
        else if (playerCs.rigd.velocity.y >= 1.5f)
        {
            transform.position = Vector3.Lerp(myPos, targetPos, Time.deltaTime * 2f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")|| collision.CompareTag("breakGround") || collision.CompareTag("HeartItem")|| collision.CompareTag("ShieldItem")|| collision.CompareTag("BoostItem"))
        {
            collision.gameObject.SetActive(false);
        }

    }
}
