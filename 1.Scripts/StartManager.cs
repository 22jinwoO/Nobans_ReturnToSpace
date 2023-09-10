using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;




public class StartManager : MonoBehaviour
{

    static public StartManager instance; //�̱������� ����� ���� ����

    [Header("���� ��ư")]
    [SerializeField]
    Button startBtn;    // ���� ��ư

    [Header("�÷��̾� �г��� �ؽ�Ʈ")]
    public TextMeshProUGUI playerNicknameTxt;

    // �÷��̾�
    [Header("�÷��̾� �ְ���")]
    public TextMeshProUGUI playerHighTxt;

    [Header("�÷��̾� Ƽ�� �̹���")]
    [SerializeField]
    Sprite[] rankImgs;

    [Header("�÷��̾� Ƽ�� �̹��� ��������Ʈ ������")]
    public Image playerRankImg;

    // �α��� �� �ʿ��� ��ǲ�ʵ�
    [Header("�α��� â �̸��� ��ǲ�ʵ�")]
    public TMP_InputField loginEmailInput;

    [Header("�α��� â �н����� ��ǲ�ʵ�")]
    public TMP_InputField loginPasswordInput;

    [Header("ȸ������ â �̸��� ��ǲ�ʵ�")]
    public TMP_InputField emailInput;

    [Header("ȸ������ â �н����� ��ǲ�ʵ�")]
    public TMP_InputField passwordInput;

    [Header("ȸ������â ���� ��ǲ�ʵ�")]
    public TMP_InputField userNameInput;

    [Header("ȸ������ â �г��� ��ǲ�ʵ�")]
    public TMP_InputField nickNameInput;

    [Header("�༺ �̹��� �迭")]
    public Image[] ranks_Img;

    [Header("��ŷ���� ��ư")]
    public Button showRankBtn;

    [Header("��ŷ���� �ݱ� ��ư")]
    [SerializeField]
    Button showRankCloseBtn;

    [Header("��ŷ���� �ݱ��ư �̹���")]
    [SerializeField]
    Image showRankCloseBtnImg;

    [Header("��ŷ ����ǥ ���ӿ�����Ʈ")]
    public GameObject showRankObject;

    // ��ŷ���� ������ ������Ʈ
    public GameObject contentOb;    // ��ũ�Ѻ� ������ ������Ʈ

    public GameObject rankLinePrefab;   // ��ũ�Ѻ� �������� �ڽ����� �÷��̾��� �������� ���� ������

    // ��ŷ ����
    public List<RankLine> rankLines = new List<RankLine>();
    public Scrollbar scrollbar;

    // �˾�â ����
    [Header("�ݱ� ��ư ��������Ʈ �迭")]
    [SerializeField]
    Sprite[] closeBtnSprs;

    [Header("�˾� â �ݴ� ��ư")]
    public Button closePopUpBtn;

    [Header("�˾� â �ݴ� ��ư �̹���")]
    public Image closePopUpBtnImg;

    // �α���, ȸ������ ����
    [Header("�α��� ��ư")]
    public Button loginTxtBtn;

    [Header("���۷α��� ��ư")]
    public Button googleloginTxtBtn;


    [Header("ȸ������â Ȱ��ȭ �ϴ� ��ư")]
    public Button registerTxtBtn;

    [Header("ȸ������ϴ� ��ư")]
    public Button memberRegisterBtn;

    [Header("ȸ������ â �ݴ� ��ư")]
    public Button registerCloseBtn;

    [Header("ȸ������ â �ݴ� ��ư �̹���")]
    public Image registerCloseBtnImg;


    [Header("ȸ������ �� Ȱ��ȭ�Ǵ� ���ӿ�����Ʈ")]
    public GameObject registerPopUp;

    [Header("�α���, ȸ������ ���� �� Ȱ��ȭ�Ǵ� ���ӿ�����Ʈ")]
    public GameObject popUpObj;

    [Header("�α���, ȸ������ ���� �� �ۼ��Ǵ� �ؽ�Ʈ")]
    public TextMeshProUGUI popUptxt;

    [Header("�α���, ȸ������ ȭ�� �г� ������Ʈ")]
    public GameObject loginRegisterPanel;

    [Header("�α���, ȸ������ �ڽ� ������Ʈ")]
    public GameObject loginRegisterBox;

    [Header("�α���, ȸ������ ȭ�� ������ �ҷ����� �ؽ�Ʈ ������Ʈ")]
    public TextMeshProUGUI loadingDataTxt;


    // �α׾ƿ� ����
    [Header("�α׾ƿ� ��ư")]
    [SerializeField]
    Button logOutBtn;

    [SerializeField]
    Button gameOffBtn;

    [SerializeField]
    Image gameOffBtnImg;

    [SerializeField]
    Sprite[] gameOffs;

    // ���ǥ ����
    [SerializeField]
    GameObject rankTierOb;

    [SerializeField]
    Button rankTierBtn;

    [SerializeField]
    Button closeRankTierBtn;

    [SerializeField]
    Image closeRankTierBtnImg;


    private void Awake()
    {
        instance = this;

        startBtn.onClick.AddListener(ClickGameStartBtn);    // ���� ���� ��ư�� ���ξ����� �Ѿ�� �Լ� ����


        googleloginTxtBtn.onClick.AddListener(() => PlayFabManager.instance.StartCoroutine("GoogleLogin"));    // ���� �α��� ��ư�� �÷����� �Ŵ����� ���۷α��� �ڷ�ƾ �Լ� ����
        loginTxtBtn.onClick.AddListener(PlayFabManager.instance.ClickLoginBtn); // �α��� �ؽ�Ʈ ��ư��  �÷����� �Ŵ����� Ŭ�� �α��� ��ư �Լ� ����
        closePopUpBtn.onClick.AddListener(() => StartCoroutine(PressedClickcloseUIBtn(closePopUpBtn,closePopUpBtnImg,popUpObj))); //�˾�â �ݱ�
        registerTxtBtn.onClick.AddListener(ClickRegisterBtn);   // ȸ������â Ȱ��ȭ �Ǵ� �Լ� ��ư�� ����
        registerCloseBtn.onClick.AddListener(() => StartCoroutine(PressedClickcloseUIBtn(registerCloseBtn, registerCloseBtnImg, registerPopUp))); // ȸ������â �ݱ�

        memberRegisterBtn.onClick.AddListener(PlayFabManager.instance.ClickRegisterBtn);
        showRankBtn.onClick.AddListener(() => PlayFabManager.instance.StartCoroutine("GetLeaderboard"));
        showRankCloseBtn.onClick.AddListener(() => StartCoroutine(PressedClickcloseUIBtn(showRankCloseBtn, showRankCloseBtnImg, showRankObject))); // ��ŷ���� ������Ʈ �ݱ�
        logOutBtn.onClick.AddListener(ClickLogOutBtn);  //�α׾ƿ� ��ư�� �α׾ƿ� �Լ� ����

        gameOffBtn.onClick.AddListener(() => StartCoroutine("ClickGameOffBtn"));

        //���ǥ ���� ��ư �Լ� ����
        rankTierBtn.onClick.AddListener(()=>ClickcloseOpenObjectBtn(rankTierOb,true));
        closeRankTierBtn.onClick.AddListener(() => StartCoroutine(PressedClickcloseUIBtn(closeRankTierBtn, closeRankTierBtnImg, rankTierOb))); // ��ŷ���� ������Ʈ �ݱ�

        //PlayerPrefs.SetFloat("BestHigh", 0);
        Time.timeScale = 1f;

        if (PlayFabManager.instance.isLogin)
        {
            loginRegisterPanel.SetActive(true);
            loadingDataTxt.text = "������ �ҷ�������...";
            loadingDataTxt.gameObject.SetActive(true);

            loginRegisterBox.SetActive(false);
            PlayFabManager.instance.loginData.SetActive(true);
        }

        //�ػ� ���� �Լ�
        ScreenResolution();
    }

    public void ScreenResolution()  // ȭ�� ���� �����ϴ� �Լ�
    {
        var camera = Camera.main;
        var r = camera.rect;
        var scaleheight = ((float)Screen.width / Screen.height) / (9f / 16f);
        var scalewidth = 1f / scaleheight;
        if (scaleheight < 1f)
        {
            r.height = scaleheight;
            r.y = (1f - scaleheight) / 2f;
        }
        else
        {
            r.width = scalewidth;
            print(r.width);
            r.x = (1f - scalewidth) / 2f;
            print(r.x);
        }

        camera.rect = r;

    }
    public void OnPreCull() => GL.Clear(true, true, Color.black);

    // �÷��̾� ���̿� ���� �༺ �̹����� ��������ִ� �Լ�
    public void ShowPlayerRank(int Height, Image rankImage)
    {
        print("��ŷ �̹��� Ȱ��ȭ�Ǵ� �Լ� ����" + Height.ToString());

        rankImage.gameObject.SetActive(true);
        if (Height <= 0)
        {
            print("������");
            rankImage.gameObject.SetActive(false);
        }

        else if (Height > 0f && Height <= 149f)
        {
            rankImage.sprite = rankImgs[0];
            print("����");
        }

        else if (Height <= 299f)
        {
            rankImage.sprite = rankImgs[1];
            print("�ݼ�");
        }

        else if (Height <= 449f)
        {
            rankImage.sprite = rankImgs[2];
        }

        else if (Height <= 599f)
        {
            rankImage.sprite = rankImgs[3];
        }


        else if (Height <= 749f)
        {
            rankImage.sprite = rankImgs[4];
        }

        else if (Height <= 899f)
        {
            rankImage.sprite = rankImgs[5];
        }

        else if (Height <= 1049f)
        {
            rankImage.sprite = rankImgs[6];
        }

        else if (Height <= 1199f)
        {
            rankImage.sprite = rankImgs[7];
        }

        else if (Height > 1199f)
        {
            rankImage.sprite = rankImgs[8];
        }

    }
    private void ClickGameStartBtn()    // ���ӽ��� ��ư ������ �� ȣ��Ǵ� �Լ�
    {
        PlayFabManager.instance.isLogin = true;
        AudioManager.instance.Sound_ClickBtn(AudioManager.instance.soundsBtn[0]);
        // �ε����̶� �� ���� �� �ҷ�����
        LoadingManager.LoadScene("2.MainScene");
    }

    private void ClickRegisterBtn() // ȸ������â ��ư Ŭ������ �� ȣ��Ǵ� �Լ�
    {
        registerPopUp.SetActive(true);
    }

    private void CloseRegisterPopUp()   // ȸ������â�� �ݾ��� �� ȣ��Ǵ� �Լ�
    {

        emailInput.text = null; ;

        passwordInput.text = null; ;

        userNameInput.text = null; ;

        nickNameInput.text = null; ;
        ClickcloseOpenObjectBtn(registerPopUp, false);
    }

    #region
    public void ClickcloseOpenObjectBtn(GameObject Object, bool closeOropen) // �ݱ� ��ư�� ���� �� ȣ��Ǵ� �Լ�
    {
        Object.SetActive(closeOropen);

    }

    IEnumerator PressedClickcloseUIBtn(Button BtnObject, Image BtnImage, GameObject Object)
    {
        BtnObject.interactable = false;
        BtnImage.sprite = closeBtnSprs[1];
        yield return new WaitForSeconds(0.15f);

        BtnImage.sprite = closeBtnSprs[0];

        BtnObject.interactable = true;
        Object.SetActive(false);
    }

    #endregion
    public void ClickLogOutBtn()    // �α׾ƿ� ��ư Ŭ���� ȣ��Ǵ� �Լ�
    {
        print("�α׾ƿ� ��ư ����");
        loginEmailInput.text = null;
        loginPasswordInput.text = null; ;

        emailInput.text = null; ;

        passwordInput.text = null; ;

        userNameInput.text = null; ;

        nickNameInput.text = null; ;
        if (PlayFabManager.instance.isGoogleLogin)
        {
            PlayFabManager.instance.GoogleLogout();
        }
        PlayFabManager.instance.loginData.SetActive(false);
        ClickcloseOpenObjectBtn(loginRegisterPanel, true);
        loadingDataTxt.gameObject.SetActive(false);
        ClickcloseOpenObjectBtn(loginRegisterBox, true);
    }
    
    private IEnumerator ClickGameOffBtn()   // ���� ���� ��ư�� ����Ǵ� �Լ�
    {
        gameOffBtnImg.sprite = gameOffs[1];
        gameOffBtn.interactable = false;
        yield return new WaitForSeconds(0.15f);
        gameOffBtnImg.sprite = gameOffs[0];

        yield return new WaitForSeconds(0.5f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
