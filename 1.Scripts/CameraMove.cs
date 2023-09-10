using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("플레이어 위치 트랜스폼")]
    public Transform player;

    // 플레이어 스크립트
    private Player playerCs;

    void Start()
    {
        playerCs = player.GetComponent<Player>();
    }

    void Update()
    {
        if (playerCs.transform.position.y>4f)   // 플레이어가 점프를 하기 시작했다면
        {
            Vector3 targetPos = new Vector3(0, player.transform.position.y + 1.5f, -10);
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
        

    }
}
