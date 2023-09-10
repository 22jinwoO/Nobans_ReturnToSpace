using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [Header("리지드바디 2D 변수")]
    public Rigidbody2D rigd;   //리지드바디 2D 변수

    [Header("방패아이템 게임오브젝트")]
    [SerializeField]
    private GameObject shield;  // 방패 아이템

    [Header("플레이어 X 이동 값")]
    private float moveX;    // 플레이어 X 이동 값

    [Header("플레이어 움직임 속도")]
    public float moveSpeed = 12f; // 플레이어 움직임 속도

    [Header("플레이어 점프 파워")]
    public float jumpForce;  // 점프 파워

    private int playerLayer;    // 플레이어 레이어 값
    private int groundLayer;    // 그라운드 레이어 값

    [Header("플레이어 기본 체력 값")]
    [SerializeField]
    private int playerHeart=4; // 플레이어 기본 체력 값


    [Header("게임 시작 시 출현하는 점프 버튼")]
    [SerializeField]
    private Button jumpBtn; // 점프 시작버튼

    
    public float usingShieldtime;

    [SerializeField]
    private Image oxygenImg;

    [SerializeField]
    private TextMeshProUGUI oxygenTxt;



    [SerializeField]
    private float x;

    [SerializeField]
    private SpriteRenderer spr;

    [SerializeField]
    private GameObject[] playerHearts;

    public bool isDamage;
    private bool isJump;

    [SerializeField]
    private bool isUp; // 점프 가능한지 확인하는 변수

    private float maxY; //플레이어 최고 높이값

    private float playTimeSec;
    private float playTimeMin;

    public float MaxY   // 읽기 전용으로만 쓸 때, 이 값을 불러와도 값에 변화를 주지 못하게 읽기로만 
    { get { return maxY; } }

    private bool isLeft;
    private bool isRight;

    [SerializeField]
    private GameObject smokeGm;

    [SerializeField]
    private Sprite[] moveSpr;

    [SerializeField]
    private GameManager gmCs;

    [SerializeField]
    private UiManager uiManagerCs;

    [SerializeField]
    private GameObject gameOver;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private GameObject clearOb;

    

    public bool isDeath;
    [SerializeField]
    private bool isEnd;

    public bool isGameStart;

    [SerializeField]
    private int playerMaxScore;

    [SerializeField]
    private BoxCollider2D playerJumpRange;

    [SerializeField]
    private CircleCollider2D playerItemRange;

    // 이펙트

    [SerializeField]
    private GameObject[] itemEffects; //0: 하트먹었을 때 , 1: 맞았을 때

    public bool isShield;
    public IEnumerator eatShieldItem;

    private void Awake()
    {
        rigd = GetComponent<Rigidbody2D>();
        isDamage = true;
        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");
        jumpBtn.onClick.AddListener(ClickJumpBtn);  // 점프시작 버튼에 함수 연결
        spr = GetComponent<SpriteRenderer>();
        isUp = true;
        anim = GetComponent<Animator>();
        jumpForce = 500f;
        x = 0;        
    }
    
    void Update()
    {

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3.3f, 3.4f),transform.position.y,transform.position.z);

        if (!isDeath&&!isEnd)
        {

            if (isGameStart)
            {
                playTimeSec += Time.deltaTime;

                gmCs.playerTimeTxt.text = $"{((int)playTimeSec / 60).ToString("00")} : {((int)playTimeSec % 60).ToString("00")}";
            }

    

            //x = Input.GetAxisRaw("Horizontal");
            if (x == -1)
            {                
                anim.SetBool("isWalk", true);
                playerJumpRange.offset = new Vector2(0f, -0.8f); //점프 박스 위치조정
                playerItemRange.offset = new Vector2(0f, -0.4f); // 아이템 히트 박스 위치조정
                spr.flipX = false;
            }

            if (x == 0)
            {
                anim.SetBool("isWalk", false);

            }
            if (x == 1)
            {
                anim.SetBool("isWalk", true);
                playerJumpRange.offset = new Vector2(0f, -0.8f);
                playerItemRange.offset = new Vector2(0f, -0.4f);
                spr.flipX = true;
            }
            rigd.velocity = new Vector2(moveX, rigd.velocity.y);
        }

        

        if (rigd.velocity.y <0f)
        {
            //anim.SetBool("isJump", false);
            if (maxY<transform.position.y/4)
            {
                
                maxY = transform.position.y/4;
                if (maxY>= 1200f)
                {
                    StartCoroutine("PlayerWin");
                    maxY = 1200f;
                    PlayFabManager.instance.SendLeaderboard(maxY, playTimeSec);
                    isEnd = true;
                }
                else
                {
                    gmCs.Create_FootBoard();
                }
                
            }
            anim.SetBool("isJump", false);
            smokeGm.SetActive(false);
            isUp = false;
            // 발판 생성 함수 호출
            
        }

    }

    private void FixedUpdate()
    {
        moveX = x * moveSpeed * Time.deltaTime; // 좌우 입력 값 받는 코드   
        
    }

    void ClickJumpBtn() // 게임 시작 버튼
    {
        isGameStart = true;
        jumpForce = 700f;

        gmCs.Invoke("CreateMoveEnemy", 8f);

        jumpBtn.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")|| collision.CompareTag("breakGround"))
        {
            if (!isUp)  // is점프가 false 일때만 그라운드와 충돌이 가능
            {
                AudioManager.instance.Sound_Jump(AudioManager.instance.soundsJump[0]);  // 점프 사운드 실행
                anim.SetBool("isJump",true);
                rigd.velocity = Vector2.zero;
                smokeGm.SetActive(true);
                rigd.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
                if (collision.CompareTag("breakGround"))
                {
                    collision.gameObject.SetActive(false);
                }
                isUp = true;
            }
        }

        if (collision.CompareTag("DeathZone"))
        {
            for (int i = 0; i < playerHearts.Length; i++)
            {
                playerHearts[i].SetActive(false);
            }
            StartCoroutine("PlayerDeath");
        }

        if (collision.CompareTag("enemyComet")|| collision.CompareTag("enemySetlite"))
        {
            GameObject hitEft = Instantiate(itemEffects[1], collision.transform.position, Quaternion.identity);
            hitEft.SetActive(true);
            Destroy(hitEft,4f);
        }
    }
    public IEnumerator EatBoost()   // 부스트 아이템 먹을 떄 호출되는 함수
    {
        rigd.velocity = Vector2.zero;

        isDamage = false;
        AudioManager.instance.Sound_Jump(AudioManager.instance.soundsJump[1]);  // 죽음 사운드 실행
        jumpForce = 1200f;
        rigd.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
        yield return new WaitForSeconds(0.3f);
        jumpForce = 700f;
        yield return new WaitForSeconds(1.3f);
        if (shield.activeSelf==false)
        {
            isDamage = true;
        }
        
    }

    public IEnumerator EatHeart()  // 하트 아이템 먹었을 때 호출되는 함수
    {
        GameObject heartEfct=Instantiate(itemEffects[0],transform);
        heartEfct.transform.localPosition = new Vector3(0.18f, 0.04f, 0f);
        print(itemEffects[0]);
        heartEfct.SetActive(true);
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[1]);  // 산소 아이템 사운드 실행
        if (playerHeart<4)
        {
            playerHeart += 1;
            playerHearts[playerHeart].SetActive(true);
        }

        yield return null;
        Destroy(heartEfct,5f);

    }

    public IEnumerator EatShiled()  // 방패 아이템 먹었을 때 호출되는 함수
    {      
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[2]);  // 쉴드 아이템 사운드 실행
        yield return null;
        isShield = false;
        shield.SetActive(true);
        isDamage = false;

        while (usingShieldtime <= 3f)
        {
            if (isShield)
            {
                Debug.LogWarning("isShield 적용");
                break;
            }
            print("플레이어 무적중");
            usingShieldtime += Time.deltaTime;
            yield return null;
        }
        print("플레이어 무적끝");
        isDamage = true;
        shield.SetActive(false);

    }

    public IEnumerator EatFever()  // 방패 아이템 먹었을 때 호출되는 함수
    {
        Time.timeScale = 2f;
        yield return new WaitForSeconds(7);
        while(Time.timeScale>1f)
        {
            Time.timeScale -= 0.1f;
            yield return new WaitForSeconds(0.2f);
            yield return null;
        }
        Time.timeScale = 1f;

    }

    public IEnumerator PlayerGetDamaged(int enemyatkDmg)  // 플레이어가 피해를 받을 때 호출되는 함수
    {
        if (isDamage&&!gmCs.isGameOver)
        {
            anim.SetTrigger("playerHurt");
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[0]);  // 산소 아이템 사운드 실행
            playerHearts[playerHeart].SetActive(false);
            print("깎이기전 : " + playerHeart);            
            playerHeart -= enemyatkDmg;
            print("깎이기 후 : " + playerHeart);
            if (playerHeart <= -1)
            {
                gmCs.isGameOver = true;
                StartCoroutine("PlayerDeath");
            }
            yield return null;
        }
    }

    public void LeftMoving()
    {
        x = -1;
    }
    public void NoInput()
    {
        x = 0;
    }
    public void RightMoving()
    {
        x = 1;
    }

    IEnumerator PlayerDeath()   // 노반의 하트 개수가 0이 되었을 때 호출되는 함수
    {

        anim.SetTrigger("playerDeath");
        uiManagerCs.StartCoroutine("FadeInFadeOut", false);
        isDeath = true;
        rigd.velocity = Vector2.zero;
        rigd.gravityScale = 0;

        if (maxY > PlayFabManager.instance.playerMaxHeight)
        {
            PlayFabManager.instance.SendLeaderboard(maxY, playTimeSec);
            PlayFabManager.instance.playerMaxHeight = (int)maxY;
        }
        yield return new WaitForSeconds(2f);
        
        AudioManager.instance.Sound_Jump(AudioManager.instance.soundsJump[2]);  // 죽음 사운드 실행
        
        gameOver.SetActive(true);
        yield return new WaitForSeconds(1f);
        gmCs.nowHighTxt.text = "기록 :   " + Mathf.FloorToInt(maxY).ToString()+"km";
        gmCs.bestHighTxt.text = "최고 기록 :   "+ PlayFabManager.instance.playerMaxHeight.ToString()+"km";
        gmCs.endPlayerTimeTxt.text = "("+gmCs.playerTimeTxt.text+")";
        
        
        yield return null;
        playerItemRange.enabled = false;
    }

    IEnumerator PlayerWin() // 노반이 목표지점(1200km)에 도달했을 때 실행되는 함수
    {
        rigd.velocity = Vector2.zero;
        rigd.gravityScale = 0;
        playerItemRange.enabled = false;
        PlayFabManager.instance.SendLeaderboard(maxY, playTimeSec);
        uiManagerCs.StartCoroutine("FadeInFadeOut", false);

        yield return new WaitForSeconds(3.5f);
        AudioManager.instance.gameObject.SetActive(false);
        SceneManager.LoadScene("3.EndingScene");
    }
}
