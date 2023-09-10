using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [Header("������ٵ� 2D ����")]
    public Rigidbody2D rigd;   //������ٵ� 2D ����

    [Header("���о����� ���ӿ�����Ʈ")]
    [SerializeField]
    private GameObject shield;  // ���� ������

    [Header("�÷��̾� X �̵� ��")]
    private float moveX;    // �÷��̾� X �̵� ��

    [Header("�÷��̾� ������ �ӵ�")]
    public float moveSpeed = 12f; // �÷��̾� ������ �ӵ�

    [Header("�÷��̾� ���� �Ŀ�")]
    public float jumpForce;  // ���� �Ŀ�

    private int playerLayer;    // �÷��̾� ���̾� ��
    private int groundLayer;    // �׶��� ���̾� ��

    [Header("�÷��̾� �⺻ ü�� ��")]
    [SerializeField]
    private int playerHeart=4; // �÷��̾� �⺻ ü�� ��


    [Header("���� ���� �� �����ϴ� ���� ��ư")]
    [SerializeField]
    private Button jumpBtn; // ���� ���۹�ư

    
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
    private bool isUp; // ���� �������� Ȯ���ϴ� ����

    private float maxY; //�÷��̾� �ְ� ���̰�

    private float playTimeSec;
    private float playTimeMin;

    public float MaxY   // �б� �������θ� �� ��, �� ���� �ҷ��͵� ���� ��ȭ�� ���� ���ϰ� �б�θ� 
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

    // ����Ʈ

    [SerializeField]
    private GameObject[] itemEffects; //0: ��Ʈ�Ծ��� �� , 1: �¾��� ��

    public bool isShield;
    public IEnumerator eatShieldItem;

    private void Awake()
    {
        rigd = GetComponent<Rigidbody2D>();
        isDamage = true;
        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");
        jumpBtn.onClick.AddListener(ClickJumpBtn);  // �������� ��ư�� �Լ� ����
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
                playerJumpRange.offset = new Vector2(0f, -0.8f); //���� �ڽ� ��ġ����
                playerItemRange.offset = new Vector2(0f, -0.4f); // ������ ��Ʈ �ڽ� ��ġ����
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
            // ���� ���� �Լ� ȣ��
            
        }

    }

    private void FixedUpdate()
    {
        moveX = x * moveSpeed * Time.deltaTime; // �¿� �Է� �� �޴� �ڵ�   
        
    }

    void ClickJumpBtn() // ���� ���� ��ư
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
            if (!isUp)  // is������ false �϶��� �׶���� �浹�� ����
            {
                AudioManager.instance.Sound_Jump(AudioManager.instance.soundsJump[0]);  // ���� ���� ����
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
    public IEnumerator EatBoost()   // �ν�Ʈ ������ ���� �� ȣ��Ǵ� �Լ�
    {
        rigd.velocity = Vector2.zero;

        isDamage = false;
        AudioManager.instance.Sound_Jump(AudioManager.instance.soundsJump[1]);  // ���� ���� ����
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

    public IEnumerator EatHeart()  // ��Ʈ ������ �Ծ��� �� ȣ��Ǵ� �Լ�
    {
        GameObject heartEfct=Instantiate(itemEffects[0],transform);
        heartEfct.transform.localPosition = new Vector3(0.18f, 0.04f, 0f);
        print(itemEffects[0]);
        heartEfct.SetActive(true);
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[1]);  // ��� ������ ���� ����
        if (playerHeart<4)
        {
            playerHeart += 1;
            playerHearts[playerHeart].SetActive(true);
        }

        yield return null;
        Destroy(heartEfct,5f);

    }

    public IEnumerator EatShiled()  // ���� ������ �Ծ��� �� ȣ��Ǵ� �Լ�
    {      
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[2]);  // ���� ������ ���� ����
        yield return null;
        isShield = false;
        shield.SetActive(true);
        isDamage = false;

        while (usingShieldtime <= 3f)
        {
            if (isShield)
            {
                Debug.LogWarning("isShield ����");
                break;
            }
            print("�÷��̾� ������");
            usingShieldtime += Time.deltaTime;
            yield return null;
        }
        print("�÷��̾� ������");
        isDamage = true;
        shield.SetActive(false);

    }

    public IEnumerator EatFever()  // ���� ������ �Ծ��� �� ȣ��Ǵ� �Լ�
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

    public IEnumerator PlayerGetDamaged(int enemyatkDmg)  // �÷��̾ ���ظ� ���� �� ȣ��Ǵ� �Լ�
    {
        if (isDamage&&!gmCs.isGameOver)
        {
            anim.SetTrigger("playerHurt");
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[0]);  // ��� ������ ���� ����
            playerHearts[playerHeart].SetActive(false);
            print("���̱��� : " + playerHeart);            
            playerHeart -= enemyatkDmg;
            print("���̱� �� : " + playerHeart);
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

    IEnumerator PlayerDeath()   // ����� ��Ʈ ������ 0�� �Ǿ��� �� ȣ��Ǵ� �Լ�
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
        
        AudioManager.instance.Sound_Jump(AudioManager.instance.soundsJump[2]);  // ���� ���� ����
        
        gameOver.SetActive(true);
        yield return new WaitForSeconds(1f);
        gmCs.nowHighTxt.text = "��� :   " + Mathf.FloorToInt(maxY).ToString()+"km";
        gmCs.bestHighTxt.text = "�ְ� ��� :   "+ PlayFabManager.instance.playerMaxHeight.ToString()+"km";
        gmCs.endPlayerTimeTxt.text = "("+gmCs.playerTimeTxt.text+")";
        
        
        yield return null;
        playerItemRange.enabled = false;
    }

    IEnumerator PlayerWin() // ����� ��ǥ����(1200km)�� �������� �� ����Ǵ� �Լ�
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
