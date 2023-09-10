using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.ComponentModel.Design;

public class GameManager : MonoBehaviour
{

    [Header("플레이어 플레이타임 텍스트")]
    public TextMeshProUGUI playerTimeTxt;


    [Header("플레이어 높이 텍스트")]
    [SerializeField]
    private TextMeshProUGUI playerHighTxt;

    [Header("플레이어 스크립트")]
    [SerializeField]
    private Player playerCs;

    [Header("플레이어 오브젝트 트랜스폼")]
    [SerializeField]
    private Transform playerTr;


    public TextMeshProUGUI nowHighTxt;

    public TextMeshProUGUI bestHighTxt;

    public TextMeshProUGUI endPlayerTimeTxt;

    public int stage;
    private bool isStage1_over;
    private bool isStage2_over;

    [Header("스테이지 1 발판 프리팹")]
    [SerializeField]
    private GameObject boardPref_Stg1;

    [Header("스테이지 2 발판 프리팹")]
    [SerializeField]
    private GameObject boardPref_Stg2;

    [Header("스테이지 3 발판 프리팹")]
    [SerializeField]
    private GameObject boardPref_Stg3;

    [Header("스테이지 3 붕괴 발판 프리팹")]
    [SerializeField]
    private GameObject boardPref_Stg3_break;


    [Header("UiManager 스크립트")]
    [SerializeField]
    private UiManager uimagerCs;

    // 발판 관련 변수들
    [Header("스테이지 1 발판 프리팹들 오브젝트 풀링")]
    [SerializeField]
    private GameObject[] boardObjects_Stg1;

    // 발판 관련 변수들
    [Header("스테이지 2 발판 프리팹들 오브젝트 풀링")]
    [SerializeField]
    private GameObject[] boardObjects_Stg2;

    // 발판 관련 변수들
    [Header("스테이지 3 발판 프리팹들 오브젝트 풀링")]
    [SerializeField]
    private GameObject[] boardObjects_Stg3;

    // 발판 관련 변수들
    [Header("스테이지 3 붕괴 발판 프리팹들 오브젝트 풀링")]
    [SerializeField]
    private GameObject[] boardObjects_Stg3_break;
    

    [Header("현재 발판 갯수")]
    [SerializeField]
    private int boardCnt;   // 현재 발판 갯수

    [Header("붕괴 발판 갯수")]
    [SerializeField]
    private int breakBoardCnt;   // 현재 발판 갯수

    [Header("발판 최대 높이(Y값)")]
    [SerializeField]
    private int maxfootBoard_Ypos;

    [Header("발판 이미지들")]
    [SerializeField]
    private Sprite[] footBoard_sprite = new Sprite[3];

    [Header("아이템 관련")]
    [SerializeField]
    private GameObject[] itemObjects;

    [Header("현재 부스터 아이템 갯수")]
    [SerializeField]
    private int booster_itemCnt;   // 현재 부스터 아이템 갯수

    [Header("현재 쉴드 아이템 갯수")]
    [SerializeField]
    private int shield_itemCnt;   // 현재 쉴드 아이템 갯수

    [Header("현재 하트 아이템 갯수")]
    [SerializeField]
    private int heart_itemCnt;   // 현재 하트 아이템 갯수


    [Header("아이템 이미지들")]
    [SerializeField]
    private Sprite[] item_sprite = new Sprite[2];

    // 아이템 관련 변수들
    [Header("부스터 아이템 프리팹 게임오브젝트")]
    [SerializeField]
    private GameObject booster_ItemPref;

    [Header("쉴드 아이템 프리팹 게임오브젝트")]
    [SerializeField]
    private GameObject shield_ItemPref;

    [Header("하트 아이템 프리팹 게임오브젝트")]
    [SerializeField]
    private GameObject heart_ItemPref;

    [Header("부스터 아이템 프리팹 게임오브젝트")]
    [SerializeField]
    private GameObject[] booster_ItemObjects;

    [Header("쉴드 아이템 프리팹 게임오브젝트")]
    [SerializeField]
    private GameObject[] shield_ItemObjects;

    [Header("하트 아이템 프리팹 게임오브젝트")]
    [SerializeField]
    private GameObject[] heart_ItemObjects;

    [Header("하트 아이템 프리팹 게임오브젝트")]
    [SerializeField]
    private GameObject itemPref;



    // 방해요소들

    [Header("유성 게임오브젝트")]
    [SerializeField]
    private GameObject enemyComet;

    [Header("인공위성 게임오브젝트")]
    [SerializeField]
    private GameObject enemySetlite;



    public bool isGameOver;


    private bool isFallingEnemy;

    private void Awake()
    {
        boardObjects_Stg1 = new GameObject[200];
        boardObjects_Stg2 = new GameObject[200];
        boardObjects_Stg3 = new GameObject[200];
        boardObjects_Stg3_break = new GameObject[100];

        for (int i = 0; i < 200; i++)   // 바위 발판 오브젝트 풀링
        {
            GameObject boardObject = Instantiate(boardPref_Stg1);
            boardObjects_Stg1[i] = boardObject;
            boardObjects_Stg1[i].SetActive(false);
        }

        for (int i = 0; i < 200; i++)   // 구름 발판 오브젝트 풀링
        {
            GameObject boardObject = Instantiate(boardPref_Stg2);
            boardObjects_Stg2[i] = boardObject;
            boardObjects_Stg2[i].SetActive(false);
        }

        for (int i = 0; i < 200; i++)   // 우주 발판 오브젝트 풀링
        {
            GameObject boardObject = Instantiate(boardPref_Stg3);
            boardObjects_Stg3[i] = boardObject;
            boardObjects_Stg3[i].SetActive(false);
        }

        for (int i = 0; i < 100; i++)   // 우주 붕괴발판 오브젝트 풀링
        {
            GameObject boardObject = Instantiate(boardPref_Stg3_break);
            boardObjects_Stg3_break[i] = boardObject;
            boardObjects_Stg3_break[i].SetActive(false);
        }

        // 아이템 오브젝트 풀링
        itemObjects = new GameObject[100];

        for (int i = 0; i < 100; i++)
        {
            GameObject itemObject = Instantiate(itemPref);
            itemObjects[i] = itemObject;
            itemObjects[i].SetActive(false);
        }

        // 부스터 아이템 오브젝트 풀링
        booster_ItemObjects = new GameObject[30];

        for (int i = 0; i < 30; i++)
        {
            GameObject booster = Instantiate(booster_ItemPref);
            booster_ItemObjects[i] = booster;
            booster_ItemObjects[i].SetActive(false);
        }

        // 쉴드 아이템 오브젝트 풀링
        shield_ItemObjects = new GameObject[30];

        for (int i = 0; i < 30; i++)
        {
            GameObject shield = Instantiate(shield_ItemPref);
            shield_ItemObjects[i] = shield;
            shield_ItemObjects[i].SetActive(false);
        }

        // 하트 아이템 오브젝트 풀링
        heart_ItemObjects = new GameObject[30];

        for (int i = 0; i < 30; i++)
        {
            GameObject heart = Instantiate(heart_ItemPref);
            heart_ItemObjects[i] = heart;
            heart_ItemObjects[i].SetActive(false);
        }

    }
    private void Start()
    {
        playerCs = GameObject.FindObjectOfType<Player>();

        uimagerCs.StartCoroutine("FadeInFadeOut", true);    // 페이드 인아웃 함수 실행

        // 발판 생성하는 코드
        int count = 4;
        while (count<=80)
        {
            int rand0 = Random.Range(2, 5);
            if (rand0==3)
            {
                continue;
            }

            // 동작 수행
            if (count % rand0 == 0)
            {
                int rand = Random.Range(2, 5);  // 생성되는 발판 랜덤 갯수
                List<float> boardPositionList = new List<float>() { -3.08f, -1.88f, -0.68f, 0.52f, 1.72f, 2.92f}; //- 2.56f, -1.92f, -1.28f, -0.64f, 0f, 0.64f, 1.28f, 1.92f, 2.56f
                for (int j = 0; j < rand; j++)
                {
                    int boardPosRand = Random.Range(0, boardPositionList.Count);
                    boardObjects_Stg1[boardCnt].SetActive(true);
                    boardObjects_Stg1[boardCnt].transform.position = new Vector3(boardPositionList[boardPosRand], count, 0);
                    boardPositionList.Remove(boardPositionList[boardPosRand]);
                    CreateItem(boardObjects_Stg1[boardCnt].transform);
                    boardCnt += 1;

                }
            }
            
            
            maxfootBoard_Ypos = count;
            count++;
        }
        AudioManager.instance.StartCoroutine("SoundFade", AudioManager.instance.soundsBgm[1]);
        Time.timeScale = 1.3f;
    }

    void Update()
    {

        if (playerCs.MaxY > 100f&&!isFallingEnemy)
        {
            StartCoroutine(CreateFallingEnemy());

            isFallingEnemy = true;
        }
        playerHighTxt.text = "높이 : "+Mathf.FloorToInt(playerCs.MaxY).ToString()+"km";

        // 사용하지 않는 스테이지 1단계 오브젝트 풀링 발판 파괴
        if (playerCs.MaxY > 65f && !isStage1_over)
        {
            for (int i = 0; i < boardObjects_Stg1.Length; i++)
            {
                Destroy(boardObjects_Stg1[i]);
            }
            isStage1_over = true;
        }

        // 사용하지 않는 스테이지 2단계 오브젝트 풀링 발판 파괴

        if (playerCs.MaxY>115f&&!isStage2_over)
        {
            for (int i = 0; i < boardObjects_Stg2.Length; i++)
            {
                Destroy(boardObjects_Stg2[i]);
            }
            isStage2_over = true;
        }
    }
    public void Create_FootBoard()  // 랜덤 발판 생성하는 함수
    {
        if (!isGameOver)
        {
            //한번만 실행되게 해야함
            if (playerCs.MaxY*4f > maxfootBoard_Ypos - 40f)  // 만약 플레이어의 최대 높이가 마지막 발판의 y좌표 -40보다 크다면
            {
                int count = 2;
                while (count <= 80)
                {
                    int rand0 = Random.Range(2, 5);
                    if (rand0 == 3) //2와 4만 나오도록하기 위한 조건
                    {
                        continue;
                    }
                    // 동작 수행
                    if ((maxfootBoard_Ypos + count) % rand0 == 0)
                    {

                        int stageBoard = 4;

                        int rand = Random.Range(2, stageBoard);  // 생성되는 발판 랜덤 갯수
                        List<float> boardPositionList = new List<float>() { -3.08f, -1.88f, -0.68f, 0.52f, 1.72f, 2.92f};

                        for (int j = 0; j < rand; j++)
                        {
                            if (boardCnt >= 200)
                            {
                                boardCnt = 0;
                            }
                            else if (breakBoardCnt >= 100)
                            {
                                breakBoardCnt = 0;
                            }



                            // * 플레이어 높이에 따라 발판 스프라이트와 콜라이더 위치 변경
                            if ((maxfootBoard_Ypos + count) / 4f >= 0 && ((maxfootBoard_Ypos + count) / 4f < 50f))
                            {
                                SetActiveBoardPref(boardPositionList,boardObjects_Stg1[boardCnt], count);
                            }
                            else if ((maxfootBoard_Ypos + count)/4f < 100f)
                            {
                                SetActiveBoardPref(boardPositionList,boardObjects_Stg2[boardCnt], count);
                            }
                            else if ((maxfootBoard_Ypos + count)/4f >=100f)
                            {
                                int breakRand = Random.Range(0, 5);  // 깨지는 발판 생성
                                int breakRange = 1;
                                if (maxfootBoard_Ypos + count >= 400f)
                                {
                                    breakRange = 3;
                                }
                                if (breakRand < breakRange) // 깨지는 발판 생성
                                {
                                    SetActiveBoardPref(boardPositionList,boardObjects_Stg3_break[breakBoardCnt], count);
                                    breakBoardCnt++;
                                }

                                else
                                {
                                    SetActiveBoardPref(boardPositionList,boardObjects_Stg3[boardCnt], count);
                                }

                            }
                            boardCnt += 1;

                        }
                    }

                    count++;
                }
                maxfootBoard_Ypos += 80;

            }
        }
        
    }

    // 발판 활성화 하는 함수
    private void SetActiveBoardPref(List<float> boardPosList ,GameObject boardPref,int countNum)
    {

        int x = Random.Range(0, boardPosList.Count);
        boardPref.SetActive(true);

        // 같은 위치에 발판 생성 되지 않게 설정
        boardPref.transform.position = new Vector3(boardPosList[x], maxfootBoard_Ypos + countNum, 0);
        boardPosList.Remove(boardPosList[x]);
        CreateItem(boardPref.transform);
    }
        public void CreateItem(Transform boardTr)   // 아이템 생성하는 함수
    {
        int crateItemRand = Random.Range(0, 8);

        if (crateItemRand==0)
        {
            int rand = Random.Range(0, 10);

            if (booster_itemCnt >= 30)
            {
                booster_itemCnt = 0;
            }
            else if (shield_itemCnt >= 30)
            {
                shield_itemCnt = 0;
            }
            else if (heart_itemCnt >= 30)
            {
                heart_itemCnt = 0;
            }

            if (rand<2) // 부스트 아이템 생성하는 코드, 확률 2/10
            {
                
                if (boardTr.position.x<=-3f|| boardTr.position.x >= 2.8f) // 벽에 가까이 생성되었을 때
                {
                    booster_ItemObjects[booster_itemCnt].SetActive(true);
                    booster_ItemObjects[booster_itemCnt].transform.position = new Vector3(boardTr.position.x + Random.Range(-0.3f, 0.3f), boardTr.position.y + Random.Range(-1f, 2.5f), 0);
                    
                }
                else
                {
                    booster_ItemObjects[booster_itemCnt].SetActive(true);
                    booster_ItemObjects[booster_itemCnt].transform.position = new Vector3(boardTr.position.x + Random.Range(-1f, 1.1f), boardTr.position.y + Random.Range(-1f, 2.5f), 0);
                }
                booster_ItemObjects[booster_itemCnt].tag = "BoostItem";
                booster_itemCnt += 1;
            }
            else if (rand < 8) // 방패 아이템 생성하는 코드, 확률 7/10
            {
                

                if (boardTr.position.x <= -3f || boardTr.position.x >= 2.8f) // 벽에 가까이 생성되었을 때
                {                   
                    shield_ItemObjects[shield_itemCnt].SetActive(true);
                    shield_ItemObjects[shield_itemCnt].transform.position = new Vector3(boardTr.position.x + Random.Range(-0.3f, 0.3f), boardTr.position.y + Random.Range(-1f, 2.5f), 0);
                }
                else
                {
                    shield_ItemObjects[shield_itemCnt].SetActive(true);
                    shield_ItemObjects[shield_itemCnt].transform.position = new Vector3(boardTr.position.x + Random.Range(-1f, 1.1f), boardTr.position.y + Random.Range(-1f, 2.5f), 0);
                }
                shield_ItemObjects[shield_itemCnt].tag = "ShieldItem";
                shield_itemCnt += 1;
            }
            else if (rand <10) // 하트 아이템 생성하는 코드, 확률 1/10
            {
                if (boardTr.position.x <= -3f || boardTr.position.x >= 2.8f) // 벽에 가까이 생성되었을 때
                {
                    heart_ItemObjects[heart_itemCnt].SetActive(true);
                    heart_ItemObjects[heart_itemCnt].transform.position = new Vector3(boardTr.position.x + Random.Range(-0.3f, 0.3f), boardTr.position.y + Random.Range(-1f, 2.5f), 0);
                }
                else
                {
                    heart_ItemObjects[heart_itemCnt].SetActive(true);
                    heart_ItemObjects[heart_itemCnt].transform.position = new Vector3(boardTr.position.x + Random.Range(-1f, 1.1f), boardTr.position.y + Random.Range(-1f, 2.5f), 0);
                }
                heart_ItemObjects[heart_itemCnt].tag = "HeartItem";
                heart_itemCnt += 1;
            }
        }
    }
    public IEnumerator CreateFallingEnemy() // 방해요소 운석 생성하는 함수
    {
        if (!isGameOver)
        {
            print("적생성");
            int createRandEnemyCnt = Random.Range(1, 4);    // 운석은 1~3개 중 랜덤으로 생성
            for (int i = 0; i < createRandEnemyCnt; i++)
            {
                GameObject enemyCometPref = Instantiate(enemyComet);
                int rand2 = Random.Range(0, 3);
                switch (rand2)
                {
                    case 0:
                        enemyCometPref.transform.localPosition = new Vector3(Random.Range(-5f,-4.3f), Random.Range(Camera.main.transform.position.y + 2, Camera.main.transform.position.y + 10), 0);
                        break;

                    case 1:
                        enemyCometPref.transform.localPosition = new Vector3(Random.Range(4.3f,5f), Random.Range(Camera.main.transform.position.y + 2, Camera.main.transform.position.y + 10), 0);
                        break;
                    case 2:
                        enemyCometPref.transform.localPosition = new Vector3(Random.Range(-3.2f,3.2f), Camera.main.transform.position.y + 10, 0);
                        break;
                }
                float rand0 = Random.Range(0.3f, 1.5f);
                yield return new WaitForSeconds(rand0); //생성 주기 랜덤
            }
            float rand = Random.Range(1.5f, 6f);
            yield return new WaitForSeconds(rand);//함수 실행 주기 랜덤
            StartCoroutine("CreateFallingEnemy");
        }
        
        
    }
    public void CreateMoveEnemy()  // 움직이는 적(비행기, 위성) 생성하는 함수
    {
        if (!isGameOver)
        {

            GameObject enemySetlitePref = Instantiate(enemySetlite);
            int rand2 = Random.Range(0, 2);
            switch (rand2)
            {
                case 0:
                    enemySetlitePref.transform.position = new Vector3(-5f, Random.Range(Camera.main.transform.position.y - 6, Camera.main.transform.position.y + 7), 0);
                    break;

                case 1:
                    enemySetlitePref.transform.position = new Vector3(5f, Random.Range(Camera.main.transform.position.y - 6, Camera.main.transform.position.y + 7), 0);
                    break;
            }



            Invoke("CreateMoveEnemy", 8f);


        }
    }
        

}
