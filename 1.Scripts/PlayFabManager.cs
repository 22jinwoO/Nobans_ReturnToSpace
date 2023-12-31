using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance; //싱글톤으로 만들기 위해 선언


    public bool isLogin;    // 로그인 확인 유무 판단하는 변수

    public int playerMaxHeight;
    public string playerNickname;
    public bool isGoogleLogin;


    [SerializeField]
    string rankPlayerNickname;

    [SerializeField]
    Dictionary<string , int> rankPlayersTimes= new Dictionary<string, int>();

    // 로그인 데이터
    public GameObject loginData;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

    }
    private void Start()
    {
        //SetResoulution();
 
    }
    public void SetResoulution()    // 화면 고정하는 함수
    {
        int setWidth = 1080;
        int setHeight = 1920;

        Screen.SetResolution(setWidth, setHeight, false);
    }
    #region  # ClickLoginBtn() - 로그인 버튼에 연결하는 함수 , StartManager의 loginTxtBtn에 연결되어 호출
    public void ClickLoginBtn() // 로그인 버튼에 연결하는 함수
    {
        StartManager.instance.googleloginTxtBtn.interactable = false;
        StartManager.instance.loginTxtBtn.interactable = false;
        StartManager.instance.registerTxtBtn.interactable = false;
        var request = new LoginWithEmailAddressRequest { Email = StartManager.instance.loginEmailInput.text, Password = StartManager.instance.loginPasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }
    #endregion

    #region #  ClickRegisterBtn() - 회원가입 버튼에 연결하는 함수, StartManager의 memberRegisterBtn에 연결되어 호출
    public void ClickRegisterBtn()  // 회원가입 버튼에 연결하는 함수
    {
        StartManager.instance.googleloginTxtBtn.interactable = false;
        StartManager.instance.loginTxtBtn.interactable = false;
        StartManager.instance.registerTxtBtn.interactable = false;
        var request = new RegisterPlayFabUserRequest { Email = StartManager.instance.emailInput.text, Password = StartManager.instance.passwordInput.text, Username = StartManager.instance.emailInput.text.Substring(0, StartManager.instance.emailInput.text.LastIndexOf('@')), DisplayName = StartManager.instance.nickNameInput.text }; //
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);

    }
    #endregion

    //private void GooglePlayFabLogin()
    //{
    //    GPGSBinder.Inst.GoogleLogin((success, localuser) =>
    //        playerID=localuser.id, playerNickname = localuser.userName);

    //    var request = new LoginWithEmailAddressRequest { Email = Social.localUser.id + "@rand.com", Password = Social.localUser.id };
    //    PlayFabClientAPI.LoginWithEmailAddress(request, (result) => { OnLoginSuccess(result); },(error)=>GoogleRegister(Social.localUser.id,Social.localUser.userName));
    //}

    //#region
    //public void GoogleRegister(string googleID,string userNickname) //구글로그인시 자동 회원가입되는 함수
    //{
    //    StartManager.instance.loginTxtBtn.interactable = false;
    //    StartManager.instance.registerTxtBtn.interactable = false;
    //    var request = new RegisterPlayFabUserRequest { Email = googleID+"@rand.com", Password = googleID, Username = googleID + "@rand.com".Substring(0, StartManager.instance.emailInput.text.LastIndexOf('@')), DisplayName = userNickname }; //
    //    PlayFabClientAPI.RegisterPlayFabUser(request, GooglePlayFabLogin(), OnRegisterFailure);
    //}
    //#endregion

    void Init() //구글플레이 로그인 하기전 호출되는 함수
    {
        var config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public IEnumerator GoogleLogin()
    {
        Init();
        yield return null;
        Social.localUser.Authenticate((success) =>
        {
            if (success)
            {
                StartManager.instance.googleloginTxtBtn.interactable = false;
                StartManager.instance.loginTxtBtn.interactable = false;
                StartManager.instance.registerTxtBtn.interactable = false;
                GooglePlayFabLogin();
            }
            else
            {
                StartManager.instance.googleloginTxtBtn.interactable = true;
                StartManager.instance.loginTxtBtn.interactable = true;
                StartManager.instance.registerTxtBtn.interactable = true;
                StartManager.instance.popUpObj.SetActive(true);
                //string Check = Regex.Replace(StartManager.instance.popUptxt.text, @"[^a-zA-Z0-9가-힣]", "", RegexOptions.Singleline);
                //Check = Regex.Replace(StartManager.instance.popUptxt.text, @"[^\w-\-.-_-#-$-%-^-&*]", "", RegexOptions.Singleline);

                //if (StartManager.instance.popUptxt.text.Equals(Check) == true)
                //{
                //    Debug.Log("특수문자는 사용할 수 없습니다.");
                //    return;
                //}
                StartManager.instance.popUptxt.text = "로그인에 실패하셨습니다.";
            };
        });
    }

    public void GoogleLogout()  //구글 로그아웃 시 호출하는 함수
    {
        isGoogleLogin = false;
        ((PlayGamesPlatform)Social.Active).SignOut();
    }

    public void GooglePlayFabLogin()    //구글 로그인 성공 시 플레이팹 자동으로 로그인하도록 설정, 실패시 GooglePlayFabRegister 콜백함수 호출
    {
        string userId = Social.localUser.id.Replace("_", "").Substring(0, 5);
        var request = new LoginWithEmailAddressRequest { Email = userId + "@rand.com", Password = userId + "#$%" };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => OnLoginSuccess(result), (error) => GooglePlayFabRegister());
        isGoogleLogin = true;
    }

    public void GooglePlayFabRegister() // 구글플레이 로그인 성공했을 때 플레이팹 서버에서 회원가입이 안되어있을 경우 자동으로 회원가입하도록 설정
    {
        string userId = Social.localUser.id.Replace("_", "").Substring(0, 5);
        var request = new RegisterPlayFabUserRequest { Email = userId + "@rand.com", Password = userId + "#$%", Username = userId, DisplayName = Social.localUser.userName };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { GooglePlayFabLogin(); },
            (error) => {
                StartManager.instance.googleloginTxtBtn.interactable = true;
                StartManager.instance.loginTxtBtn.interactable = true;
                StartManager.instance.registerTxtBtn.interactable = true;
                StartManager.instance.popUpObj.SetActive(true);
                StartManager.instance.popUptxt.text = $"회원가입에 실패하셨습니다.";
            });
    }

    #region #  GetPlayerProfile() - 플레이어 닉네임 가져오는 함수, PlayFabManager의 LoginSuccess 함수에서 호출
    void GetPlayerProfile(string playFabId) // 플레이어 닉네임 가져오는 함수
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }

        },
        (result) => playerNickname = result.PlayerProfile.DisplayName,
        (error) => Debug.LogWarning(error.GenerateErrorReport()));
        print("아무거나" + playerNickname);
    }
    #endregion

    #region #  OnLoginSuccess() - 로그인 성공 시 호출되는 함수, PlayFabClientAPI.LoginWithEmailAddress에서 호출
    private void OnLoginSuccess(LoginResult result) // 로그인 성공 시 호출되는 함수
    {
        StartManager.instance.loadingDataTxt.text = "로그인 성공!";
        //StartManager.instance.popUpObj.SetActive(true);
        //StartManager.instance.popUptxt.text = "로그인 성공!";
        StartManager.instance.googleloginTxtBtn.interactable = true;
        StartManager.instance.loginTxtBtn.interactable = true;
        StartManager.instance.registerTxtBtn.interactable = true;
        StartManager.instance.loginRegisterBox.SetActive(false);
        StartManager.instance.loadingDataTxt.gameObject.SetActive(true);
        GetPlayerProfile(result.PlayFabId); // 플레이어 닉네임 가져오는 함수        
        //시간추가
        loginData.SetActive(true);

    }
    #endregion

    private void OnLoginFailure(PlayFabError error) // 로그인 실패 시 에러 콜백 함수
    {
        StartManager.instance.googleloginTxtBtn.interactable = true;
        StartManager.instance.loginTxtBtn.interactable = true;
        StartManager.instance.registerTxtBtn.interactable = true;
        print("로그인실패");
        StartManager.instance.popUpObj.SetActive(true);
        StartManager.instance.popUptxt.text = "로그인에 실패하셨습니다.";

        if (!StartManager.instance.loginEmailInput.text.Contains(".com") || StartManager.instance.loginEmailInput.text.Contains("!") || StartManager.instance.loginEmailInput.text.Contains("#")
            || StartManager.instance.loginEmailInput.text.Contains("$") || StartManager.instance.loginEmailInput.text.Contains("%") || StartManager.instance.loginEmailInput.text.Contains("^") || StartManager.instance.loginEmailInput.text.Contains("&")
            || StartManager.instance.loginEmailInput.text.Contains("(") || StartManager.instance.loginEmailInput.text.Contains(")") || StartManager.instance.loginEmailInput.text.Contains("-")
            || StartManager.instance.loginEmailInput.text.Contains("_") || StartManager.instance.loginEmailInput.text.Contains(" "))
        {
            StartManager.instance.popUptxt.text += "\n 이메일 형식이 잘못되었습니다.\n(특수문자 및 공백은 입력이 불가능합니다.)";
        }

        else
        {
            print("비밀번호오류");
            StartManager.instance.popUptxt.text += "\n 비밀번호를 잘못 입력하셨거나\n등록되지 않은 이메일입니다.";
        }
    }


    private void OnRegisterSuccess(RegisterPlayFabUserResult result)    // 회원가입 성공 시 호출되는 함수
    {
        print("회원 가입 성공");
        StartManager.instance.googleloginTxtBtn.interactable = true;
        StartManager.instance.loginTxtBtn.interactable = true;
        StartManager.instance.registerTxtBtn.interactable = true;
        StartManager.instance.ClickcloseOpenObjectBtn(StartManager.instance.registerPopUp, false); // 회원 가입 창 닫기 closePopUpBtn, closePopUpBtnImg,
        StartManager.instance.popUpObj.SetActive(true);
        StartManager.instance.popUptxt.text = "회원가입 완료!";
    }


    private void OnRegisterFailure(PlayFabError error)   // 회원 가입 등록 실패 시 호출되는 함수
    {
        StartManager.instance.googleloginTxtBtn.interactable = true;
        StartManager.instance.loginTxtBtn.interactable = true;
        StartManager.instance.registerTxtBtn.interactable = true;
        print("회원 가입 실패");
        StartManager.instance.popUpObj.SetActive(true);
        StartManager.instance.popUptxt.text = "회원가입에 실패하셨습니다.";
        if (!StartManager.instance.emailInput.text.Contains(".com") || StartManager.instance.emailInput.text.Contains("!") || StartManager.instance.emailInput.text.Contains("#") || StartManager.instance.emailInput.text.Contains("&") ||
            StartManager.instance.emailInput.text.Contains("$") || StartManager.instance.emailInput.text.Contains("%") || StartManager.instance.emailInput.text.Contains("^") || StartManager.instance.emailInput.text.Contains("(") || StartManager.instance.emailInput.text.Contains(")") || StartManager.instance.emailInput.text.Contains("-") || StartManager.instance.emailInput.text.Contains("_") || StartManager.instance.emailInput.text.Contains(" "))
        {
            StartManager.instance.popUptxt.text += "\n 이메일 형식이 잘못되었습니다.\n.com으로 끝나도록 작성해주세요.\n(특수문자 및 공백은 입력이 불가능)";
        }
        Debug.LogError(error.GenerateErrorReport());
    }

    public void SetStat(float higher)   // 유저 높이 값 저장
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName ="height_rank" ,
                    Value = (int)higher
                }
            }
        },
        (result) => { print("값저장됨"); },
        (error) => { print("값 저장 실패"); });
    }

    public void GetStat(TextMeshProUGUI txt)   // 값 불러오기
    {

        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            (result) =>
            {
                StartManager.instance.playerNicknameTxt.text = playerNickname;
                print("리스트 0번째 값" + result.Statistics);
                print("리스트 0번째 값" + result.Statistics.Count);
                if (result.Statistics.Count == 0)
                {
                    print(playerNickname);
                    playerMaxHeight = 0;
                    txt.text = $"현재 기록이 없습니다!\n새로운 기록을 남겨주세요!";
                }
                else
                {
                    foreach (var eachStat in result.Statistics)
                    {
                        if (eachStat.StatisticName== "height_rank")
                        {
                            print(playerNickname);
                            txt.text = eachStat.Value.ToString() + "km";

                            playerMaxHeight = eachStat.Value;
                            print(eachStat.StatisticName);
                            print(eachStat.Value);
                        }

                    }
                }
            },
            (error) => { print("값 불러오기 실패"); });
        print("아무거나" + playerNickname);
    }

    public void SendLeaderboard(float score,float time)    // 순위표에 플레이어 스코어 전달하기
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate {
                    StatisticName = "height_rank",
                    Value = (int)score,
                },
                new StatisticUpdate {
                    StatisticName = "time_rank",
                    Value = (int)time,
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }

    public IEnumerator GetLeaderboard() // 리더보드의 값 
    {
        
        StartManager.instance.showRankBtn.interactable = false;
        StartManager.instance.popUptxt.text = "랭킹 불러오는중...";
        StartManager.instance.popUpObj.SetActive(true);
        StartManager.instance.closePopUpBtn.gameObject.SetActive(false);
        
        var request = new GetLeaderboardRequest
        {
            StatisticName = "height_rank",

            StartPosition = 0,
            MaxResultsCount = 100,
        };

        var request2 = new GetLeaderboardRequest
        {
            StatisticName = "time_rank",

            StartPosition = 0,
            MaxResultsCount = 100,
        };
        PlayFabClientAPI.GetLeaderboard(request2, OnLeaderboardGetTime, OnError);
        yield return new WaitForSeconds(1f);
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGetHeight, OnError);
        


        print("랭킹 보여주기");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log(" 등록 성공");
    }

    
    public struct LeaderBoardValue  // 랭킹 순위표에 데이터 전달을 위한 구조체
    {
        public Image rankImg;

        public int rankNum;

        public string rankNickname;

        public int rankHeight;

        public int rankPlayTime;

        public LeaderBoardValue(Image rankImg, int rankNum, string rankNickname, int rankHeight, int rankPlayTime)
        {
            this.rankImg = rankImg;
            this.rankNum = rankNum;
            this.rankNickname = rankNickname;
            this.rankHeight = rankHeight;
            this.rankPlayTime = rankPlayTime;
        }

    }
    private void OnLeaderboardGetHeight(GetLeaderboardResult result)  //리더보드 값 불러오는 함수
    {
        print(result.Leaderboard);
        foreach (var item in result.Leaderboard)
        {
            Debug.LogWarning(item);
        }

        bool isNew = false;
        if (StartManager.instance.rankLines.Count != 0)
        {
            for (int i = 0; i < result.Leaderboard.Count; i++)
            {
                if (StartManager.instance.rankLines[i].rankNicknameTxt.text != result.Leaderboard[i].DisplayName)
                {
                    isNew = true;
                    for (int j = 0; j < StartManager.instance.rankLines.Count; j++)
                    {
                        Destroy(StartManager.instance.rankLines[j].gameObject);
                    }
                    StartManager.instance.rankLines.Clear();
                }
            }

        }
        else
        {
            isNew = true;
        }

        if (isNew)
        {
            
            for (int i = 0; i < result.Leaderboard.Count; i++)
            {
                GameObject rankOb = Instantiate(StartManager.instance.rankLinePrefab, StartManager.instance.contentOb.transform);
                RankLine rankline = rankOb.GetComponent<RankLine>();
                StartManager.instance.rankLines.Add(rankline);
                print(result.Leaderboard.Count);
                print(i + "번째");
                print(rankOb.transform.GetComponentInChildren<Image>());
                print("등수" + (i + 1));
                print(result.Leaderboard[i]);
                print(result.Leaderboard[i].StatValue);
                // height_rank 리더보드 i 번째의 닉네임을 받아오고 난 후 for문을 돌려서 time_rank 리더보드의 i 번째 닉네임들과 비교한 후 같은 닉네임이 잇으면 i번째의 time_rank.StatValue 값을 가져오는식으로 구현
                StartManager.instance.ShowPlayerRank(result.Leaderboard[i].StatValue, rankOb.transform.GetComponentInChildren<Image>());
                LeaderBoardValue ranker = new LeaderBoardValue(rankOb.transform.GetComponentInChildren<Image>(), (i + 1), result.Leaderboard[i].DisplayName, result.Leaderboard[i].StatValue, rankPlayersTimes[result.Leaderboard[i].DisplayName]);
                // 복사한 RankLine에 값 입력해주기
                rankline.SetData(ranker);

            }
        }

        StartManager.instance.closePopUpBtn.gameObject.SetActive(true);
        StartManager.instance.popUpObj.SetActive(false);        
        StartManager.instance.showRankBtn.interactable = true;
        StartManager.instance.showRankObject.SetActive(true);
        StartManager.instance.scrollbar.value = 1;

        //foreach(var item in result.Leaderboard)
        //{
        //    Debug.Log(item.Position +  " "+item.DisplayName+ " " + item.StatValue);
        //}
    }
    void OnLeaderboardGetTime(GetLeaderboardResult result)  //리더보드 값 불러오는 함수
    {
        print("플레이 타임 불러오는 함수 실행");
        rankPlayersTimes.Clear();
        for (int i = 0; i < result.Leaderboard.Count; i++)
        {
            rankPlayersTimes.Add(result.Leaderboard[i].DisplayName, result.Leaderboard[i].StatValue);
            print(rankPlayersTimes[result.Leaderboard[i].DisplayName]);
        }
    }

    private void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        //Debug.LogError(error.GenerateErrorReport());
    }
}
