using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.ComponentModel.Design;

public class GameManager : MonoBehaviour
{

    [Header("�÷��̾� �÷���Ÿ�� �ؽ�Ʈ")]
    public TextMeshProUGUI playerTimeTxt;


    [Header("�÷��̾� ���� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI playerHighTxt;

    [Header("�÷��̾� ��ũ��Ʈ")]
    [SerializeField]
    private Player playerCs;

    [Header("�÷��̾� ������Ʈ Ʈ������")]
    [SerializeField]
    private Transform playerTr;


    public TextMeshProUGUI nowHighTxt;

    public TextMeshProUGUI bestHighTxt;

    public TextMeshProUGUI endPlayerTimeTxt;

    public int stage;
    private bool isStage1_over;
    private bool isStage2_over;

    [Header("�������� 1 ���� ������")]
    [SerializeField]
    private GameObject boardPref_Stg1;

    [Header("�������� 2 ���� ������")]
    [SerializeField]
    private GameObject boardPref_Stg2;

    [Header("�������� 3 ���� ������")]
    [SerializeField]
    private GameObject boardPref_Stg3;

    [Header("�������� 3 �ر� ���� ������")]
    [SerializeField]
    private GameObject boardPref_Stg3_break;


    [Header("UiManager ��ũ��Ʈ")]
    [SerializeField]
    private UiManager uimagerCs;

    // ���� ���� ������
    [Header("�������� 1 ���� �����յ� ������Ʈ Ǯ��")]
    [SerializeField]
    private GameObject[] boardObjects_Stg1;

    // ���� ���� ������
    [Header("�������� 2 ���� �����յ� ������Ʈ Ǯ��")]
    [SerializeField]
    private GameObject[] boardObjects_Stg2;

    // ���� ���� ������
    [Header("�������� 3 ���� �����յ� ������Ʈ Ǯ��")]
    [SerializeField]
    private GameObject[] boardObjects_Stg3;

    // ���� ���� ������
    [Header("�������� 3 �ر� ���� �����յ� ������Ʈ Ǯ��")]
    [SerializeField]
    private GameObject[] boardObjects_Stg3_break;
    

    [Header("���� ���� ����")]
    [SerializeField]
    private int boardCnt;   // ���� ���� ����

    [Header("�ر� ���� ����")]
    [SerializeField]
    private int breakBoardCnt;   // ���� ���� ����

    [Header("���� �ִ� ����(Y��)")]
    [SerializeField]
    private int maxfootBoard_Ypos;

    [Header("���� �̹�����")]
    [SerializeField]
    private Sprite[] footBoard_sprite = new Sprite[3];

    [Header("������ ����")]
    [SerializeField]
    private GameObject[] itemObjects;

    [Header("���� �ν��� ������ ����")]
    [SerializeField]
    private int booster_itemCnt;   // ���� �ν��� ������ ����

    [Header("���� ���� ������ ����")]
    [SerializeField]
    private int shield_itemCnt;   // ���� ���� ������ ����

    [Header("���� ��Ʈ ������ ����")]
    [SerializeField]
    private int heart_itemCnt;   // ���� ��Ʈ ������ ����


    [Header("������ �̹�����")]
    [SerializeField]
    private Sprite[] item_sprite = new Sprite[2];

    // ������ ���� ������
    [Header("�ν��� ������ ������ ���ӿ�����Ʈ")]
    [SerializeField]
    private GameObject booster_ItemPref;

    [Header("���� ������ ������ ���ӿ�����Ʈ")]
    [SerializeField]
    private GameObject shield_ItemPref;

    [Header("��Ʈ ������ ������ ���ӿ�����Ʈ")]
    [SerializeField]
    private GameObject heart_ItemPref;

    [Header("�ν��� ������ ������ ���ӿ�����Ʈ")]
    [SerializeField]
    private GameObject[] booster_ItemObjects;

    [Header("���� ������ ������ ���ӿ�����Ʈ")]
    [SerializeField]
    private GameObject[] shield_ItemObjects;

    [Header("��Ʈ ������ ������ ���ӿ�����Ʈ")]
    [SerializeField]
    private GameObject[] heart_ItemObjects;

    [Header("��Ʈ ������ ������ ���ӿ�����Ʈ")]
    [SerializeField]
    private GameObject itemPref;



    // ���ؿ�ҵ�

    [Header("���� ���ӿ�����Ʈ")]
    [SerializeField]
    private GameObject enemyComet;

    [Header("�ΰ����� ���ӿ�����Ʈ")]
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

        for (int i = 0; i < 200; i++)   // ���� ���� ������Ʈ Ǯ��
        {
            GameObject boardObject = Instantiate(boardPref_Stg1);
            boardObjects_Stg1[i] = boardObject;
            boardObjects_Stg1[i].SetActive(false);
        }

        for (int i = 0; i < 200; i++)   // ���� ���� ������Ʈ Ǯ��
        {
            GameObject boardObject = Instantiate(boardPref_Stg2);
            boardObjects_Stg2[i] = boardObject;
            boardObjects_Stg2[i].SetActive(false);
        }

        for (int i = 0; i < 200; i++)   // ���� ���� ������Ʈ Ǯ��
        {
            GameObject boardObject = Instantiate(boardPref_Stg3);
            boardObjects_Stg3[i] = boardObject;
            boardObjects_Stg3[i].SetActive(false);
        }

        for (int i = 0; i < 100; i++)   // ���� �ر����� ������Ʈ Ǯ��
        {
            GameObject boardObject = Instantiate(boardPref_Stg3_break);
            boardObjects_Stg3_break[i] = boardObject;
            boardObjects_Stg3_break[i].SetActive(false);
        }

        // ������ ������Ʈ Ǯ��
        itemObjects = new GameObject[100];

        for (int i = 0; i < 100; i++)
        {
            GameObject itemObject = Instantiate(itemPref);
            itemObjects[i] = itemObject;
            itemObjects[i].SetActive(false);
        }

        // �ν��� ������ ������Ʈ Ǯ��
        booster_ItemObjects = new GameObject[30];

        for (int i = 0; i < 30; i++)
        {
            GameObject booster = Instantiate(booster_ItemPref);
            booster_ItemObjects[i] = booster;
            booster_ItemObjects[i].SetActive(false);
        }

        // ���� ������ ������Ʈ Ǯ��
        shield_ItemObjects = new GameObject[30];

        for (int i = 0; i < 30; i++)
        {
            GameObject shield = Instantiate(shield_ItemPref);
            shield_ItemObjects[i] = shield;
            shield_ItemObjects[i].SetActive(false);
        }

        // ��Ʈ ������ ������Ʈ Ǯ��
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

        uimagerCs.StartCoroutine("FadeInFadeOut", true);    // ���̵� �ξƿ� �Լ� ����

        // ���� �����ϴ� �ڵ�
        int count = 4;
        while (count<=80)
        {
            int rand0 = Random.Range(2, 5);
            if (rand0==3)
            {
                continue;
            }

            // ���� ����
            if (count % rand0 == 0)
            {
                int rand = Random.Range(2, 5);  // �����Ǵ� ���� ���� ����
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
        playerHighTxt.text = "���� : "+Mathf.FloorToInt(playerCs.MaxY).ToString()+"km";

        // ������� �ʴ� �������� 1�ܰ� ������Ʈ Ǯ�� ���� �ı�
        if (playerCs.MaxY > 65f && !isStage1_over)
        {
            for (int i = 0; i < boardObjects_Stg1.Length; i++)
            {
                Destroy(boardObjects_Stg1[i]);
            }
            isStage1_over = true;
        }

        // ������� �ʴ� �������� 2�ܰ� ������Ʈ Ǯ�� ���� �ı�

        if (playerCs.MaxY>115f&&!isStage2_over)
        {
            for (int i = 0; i < boardObjects_Stg2.Length; i++)
            {
                Destroy(boardObjects_Stg2[i]);
            }
            isStage2_over = true;
        }
    }
    public void Create_FootBoard()  // ���� ���� �����ϴ� �Լ�
    {
        if (!isGameOver)
        {
            //�ѹ��� ����ǰ� �ؾ���
            if (playerCs.MaxY*4f > maxfootBoard_Ypos - 40f)  // ���� �÷��̾��� �ִ� ���̰� ������ ������ y��ǥ -40���� ũ�ٸ�
            {
                int count = 2;
                while (count <= 80)
                {
                    int rand0 = Random.Range(2, 5);
                    if (rand0 == 3) //2�� 4�� ���������ϱ� ���� ����
                    {
                        continue;
                    }
                    // ���� ����
                    if ((maxfootBoard_Ypos + count) % rand0 == 0)
                    {

                        int stageBoard = 4;

                        int rand = Random.Range(2, stageBoard);  // �����Ǵ� ���� ���� ����
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



                            // * �÷��̾� ���̿� ���� ���� ��������Ʈ�� �ݶ��̴� ��ġ ����
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
                                int breakRand = Random.Range(0, 5);  // ������ ���� ����
                                int breakRange = 1;
                                if (maxfootBoard_Ypos + count >= 400f)
                                {
                                    breakRange = 3;
                                }
                                if (breakRand < breakRange) // ������ ���� ����
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

    // ���� Ȱ��ȭ �ϴ� �Լ�
    private void SetActiveBoardPref(List<float> boardPosList ,GameObject boardPref,int countNum)
    {

        int x = Random.Range(0, boardPosList.Count);
        boardPref.SetActive(true);

        // ���� ��ġ�� ���� ���� ���� �ʰ� ����
        boardPref.transform.position = new Vector3(boardPosList[x], maxfootBoard_Ypos + countNum, 0);
        boardPosList.Remove(boardPosList[x]);
        CreateItem(boardPref.transform);
    }
        public void CreateItem(Transform boardTr)   // ������ �����ϴ� �Լ�
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

            if (rand<2) // �ν�Ʈ ������ �����ϴ� �ڵ�, Ȯ�� 2/10
            {
                
                if (boardTr.position.x<=-3f|| boardTr.position.x >= 2.8f) // ���� ������ �����Ǿ��� ��
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
            else if (rand < 8) // ���� ������ �����ϴ� �ڵ�, Ȯ�� 7/10
            {
                

                if (boardTr.position.x <= -3f || boardTr.position.x >= 2.8f) // ���� ������ �����Ǿ��� ��
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
            else if (rand <10) // ��Ʈ ������ �����ϴ� �ڵ�, Ȯ�� 1/10
            {
                if (boardTr.position.x <= -3f || boardTr.position.x >= 2.8f) // ���� ������ �����Ǿ��� ��
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
    public IEnumerator CreateFallingEnemy() // ���ؿ�� � �����ϴ� �Լ�
    {
        if (!isGameOver)
        {
            print("������");
            int createRandEnemyCnt = Random.Range(1, 4);    // ��� 1~3�� �� �������� ����
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
                yield return new WaitForSeconds(rand0); //���� �ֱ� ����
            }
            float rand = Random.Range(1.5f, 6f);
            yield return new WaitForSeconds(rand);//�Լ� ���� �ֱ� ����
            StartCoroutine("CreateFallingEnemy");
        }
        
        
    }
    public void CreateMoveEnemy()  // �����̴� ��(�����, ����) �����ϴ� �Լ�
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
