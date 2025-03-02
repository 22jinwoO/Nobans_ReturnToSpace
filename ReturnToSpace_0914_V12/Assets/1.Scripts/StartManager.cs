using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;




public class StartManager : MonoBehaviour
{

    static public StartManager instance; //싱글톤으로 만들기 위해 선언

    [Header("시작 버튼")]
    [SerializeField]
    Button startBtn;    // 시작 버튼

    [Header("플레이어 닉네임 텍스트")]
    public TextMeshProUGUI playerNicknameTxt;

    // 플레이어
    [Header("플레이어 최고기록")]
    public TextMeshProUGUI playerHighTxt;

    [Header("플레이어 티어 이미지")]
    [SerializeField]
    Sprite[] rankImgs;

    [Header("플레이어 티어 이미지 스프라이트 렌더러")]
    public Image playerRankImg;

    // 로그인 시 필요한 인풋필드
    [Header("로그인 창 이메일 인풋필드")]
    public TMP_InputField loginEmailInput;

    [Header("로그인 창 패스워드 인풋필드")]
    public TMP_InputField loginPasswordInput;

    [Header("회원가입 창 이메일 인풋필드")]
    public TMP_InputField emailInput;

    [Header("회원가입 창 패스워드 인풋필드")]
    public TMP_InputField passwordInput;

    [Header("회원가입창 유저 인풋필드")]
    public TMP_InputField userNameInput;

    [Header("회원가입 창 닉네임 인풋필드")]
    public TMP_InputField nickNameInput;

    [Header("행성 이미지 배열")]
    public Image[] ranks_Img;

    [Header("랭킹보기 버튼")]
    public Button showRankBtn;

    [Header("랭킹보기 닫기 버튼")]
    [SerializeField]
    Button showRankCloseBtn;

    [Header("랭킹보기 닫기버튼 이미지")]
    [SerializeField]
    Image showRankCloseBtnImg;

    [Header("랭킹 순위표 게임오브젝트")]
    public GameObject showRankObject;

    // 랭킹보기 콘텐츠 오브젝트
    public GameObject contentOb;    // 스크롤뷰 콘텐츠 오브젝트

    public GameObject rankLinePrefab;   // 스크롤뷰 콘텐츠의 자식으로 플레이어의 정보들이 들어가는 프리팹

    // 랭킹 보기
    public List<RankLine> rankLines = new List<RankLine>();
    public Scrollbar scrollbar;

    // 팝업창 관련
    [Header("닫기 버튼 스프라이트 배열")]
    [SerializeField]
    Sprite[] closeBtnSprs;

    [Header("팝업 창 닫는 버튼")]
    public Button closePopUpBtn;

    [Header("팝업 창 닫는 버튼 이미지")]
    public Image closePopUpBtnImg;

    // 로그인, 회원가입 관련
    [Header("로그인 버튼")]
    public Button loginTxtBtn;

    [Header("구글로그인 버튼")]
    public Button googleloginTxtBtn;


    [Header("회원가입창 활성화 하는 버튼")]
    public Button registerTxtBtn;

    [Header("회원등록하는 버튼")]
    public Button memberRegisterBtn;

    [Header("회원가입 창 닫는 버튼")]
    public Button registerCloseBtn;

    [Header("회원가입 창 닫는 버튼 이미지")]
    public Image registerCloseBtnImg;


    [Header("회원가입 시 활성화되는 게임오브젝트")]
    public GameObject registerPopUp;

    [Header("로그인, 회원가입 실패 시 활성화되는 게임오브젝트")]
    public GameObject popUpObj;

    [Header("로그인, 회원가입 실패 시 작성되는 텍스트")]
    public TextMeshProUGUI popUptxt;

    [Header("로그인, 회원가입 화면 패널 오브젝트")]
    public GameObject loginRegisterPanel;

    [Header("로그인, 회원가입 박스 오브젝트")]
    public GameObject loginRegisterBox;

    [Header("로그인, 회원가입 화면 데이터 불러오는 텍스트 오브젝트")]
    public TextMeshProUGUI loadingDataTxt;


    // 로그아웃 관련
    [Header("로그아웃 버튼")]
    [SerializeField]
    Button logOutBtn;

    [SerializeField]
    Button gameOffBtn;

    [SerializeField]
    Image gameOffBtnImg;

    [SerializeField]
    Sprite[] gameOffs;

    // 등급표 관련
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

        startBtn.onClick.AddListener(ClickGameStartBtn);    // 게임 시작 버튼에 메인씬으로 넘어가는 함수 연결


        googleloginTxtBtn.onClick.AddListener(() => PlayFabManager.instance.StartCoroutine("GoogleLogin"));    // 구글 로그인 버튼에 플레이팹 매니저의 구글로그인 코루틴 함수 연결
        loginTxtBtn.onClick.AddListener(PlayFabManager.instance.ClickLoginBtn); // 로그인 텍스트 버튼에  플레이팹 매니저의 클릭 로그인 버튼 함수 연결
        closePopUpBtn.onClick.AddListener(() => StartCoroutine(PressedClickcloseUIBtn(closePopUpBtn,closePopUpBtnImg,popUpObj))); //팝업창 닫기
        registerTxtBtn.onClick.AddListener(ClickRegisterBtn);   // 회원가입창 활성화 되는 함수 버튼에 연결
        registerCloseBtn.onClick.AddListener(() => StartCoroutine(PressedClickcloseUIBtn(registerCloseBtn, registerCloseBtnImg, registerPopUp))); // 회원가입창 닫기

        memberRegisterBtn.onClick.AddListener(PlayFabManager.instance.ClickRegisterBtn);
        showRankBtn.onClick.AddListener(() => PlayFabManager.instance.StartCoroutine("GetLeaderboard"));
        showRankCloseBtn.onClick.AddListener(() => StartCoroutine(PressedClickcloseUIBtn(showRankCloseBtn, showRankCloseBtnImg, showRankObject))); // 랭킹보기 오브젝트 닫기
        logOutBtn.onClick.AddListener(ClickLogOutBtn);  //로그아웃 버튼에 로그아웃 함수 연결

        gameOffBtn.onClick.AddListener(() => StartCoroutine("ClickGameOffBtn"));

        //등급표 관련 버튼 함수 연결
        rankTierBtn.onClick.AddListener(()=>ClickcloseOpenObjectBtn(rankTierOb,true));
        closeRankTierBtn.onClick.AddListener(() => StartCoroutine(PressedClickcloseUIBtn(closeRankTierBtn, closeRankTierBtnImg, rankTierOb))); // 랭킹보기 오브젝트 닫기

        //PlayerPrefs.SetFloat("BestHigh", 0);
        Time.timeScale = 1f;

        if (PlayFabManager.instance.isLogin)
        {
            loginRegisterPanel.SetActive(true);
            loadingDataTxt.text = "데이터 불러오는중...";
            loadingDataTxt.gameObject.SetActive(true);

            loginRegisterBox.SetActive(false);
            PlayFabManager.instance.loginData.SetActive(true);
        }

        //해상도 대응 함수
        ScreenResolution();
    }

    public void ScreenResolution()  // 화면 비율 조정하는 함수
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

    // 플레이어 높이에 따라 행성 이미지를 적용시켜주는 함수
    public void ShowPlayerRank(int Height, Image rankImage)
    {
        print("랭킹 이미지 활성화되는 함수 실행" + Height.ToString());

        rankImage.gameObject.SetActive(true);
        if (Height <= 0)
        {
            print("값없음");
            rankImage.gameObject.SetActive(false);
        }

        else if (Height > 0f && Height <= 149f)
        {
            rankImage.sprite = rankImgs[0];
            print("수성");
        }

        else if (Height <= 299f)
        {
            rankImage.sprite = rankImgs[1];
            print("금성");
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
    private void ClickGameStartBtn()    // 게임시작 버튼 눌렀을 때 호출되는 함수
    {
        PlayFabManager.instance.isLogin = true;
        AudioManager.instance.Sound_ClickBtn(AudioManager.instance.soundsBtn[0]);
        // 로딩씬이랑 그 다음 씬 불러오기
        LoadingManager.LoadScene("2.MainScene");
    }

    private void ClickRegisterBtn() // 회원가입창 버튼 클릭했을 때 호출되는 함수
    {
        registerPopUp.SetActive(true);
    }

    private void CloseRegisterPopUp()   // 회원가입창을 닫았을 때 호출되는 함수
    {

        emailInput.text = null; ;

        passwordInput.text = null; ;

        userNameInput.text = null; ;

        nickNameInput.text = null; ;
        ClickcloseOpenObjectBtn(registerPopUp, false);
    }

    #region
    public void ClickcloseOpenObjectBtn(GameObject Object, bool closeOropen) // 닫기 버튼을 누를 때 호출되는 함수
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
    public void ClickLogOutBtn()    // 로그아웃 버튼 클릭시 호출되는 함수
    {
        print("로그아웃 버튼 눌림");
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
    
    private IEnumerator ClickGameOffBtn()   // 게임 종료 버튼에 연결되는 함수
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
