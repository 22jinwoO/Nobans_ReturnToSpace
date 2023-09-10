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
    public static PlayFabManager instance; //�̱������� ����� ���� ����


    public bool isLogin;    // �α��� Ȯ�� ���� �Ǵ��ϴ� ����

    public int playerMaxHeight;
    public string playerNickname;
    public bool isGoogleLogin;


    [SerializeField]
    string rankPlayerNickname;

    [SerializeField]
    Dictionary<string , int> rankPlayersTimes= new Dictionary<string, int>();

    // �α��� ������
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
    public void SetResoulution()    // ȭ�� �����ϴ� �Լ�
    {
        int setWidth = 1080;
        int setHeight = 1920;

        Screen.SetResolution(setWidth, setHeight, false);
    }
    #region  # ClickLoginBtn() - �α��� ��ư�� �����ϴ� �Լ� , StartManager�� loginTxtBtn�� ����Ǿ� ȣ��
    public void ClickLoginBtn() // �α��� ��ư�� �����ϴ� �Լ�
    {
        StartManager.instance.googleloginTxtBtn.interactable = false;
        StartManager.instance.loginTxtBtn.interactable = false;
        StartManager.instance.registerTxtBtn.interactable = false;
        var request = new LoginWithEmailAddressRequest { Email = StartManager.instance.loginEmailInput.text, Password = StartManager.instance.loginPasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }
    #endregion

    #region #  ClickRegisterBtn() - ȸ������ ��ư�� �����ϴ� �Լ�, StartManager�� memberRegisterBtn�� ����Ǿ� ȣ��
    public void ClickRegisterBtn()  // ȸ������ ��ư�� �����ϴ� �Լ�
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
    //public void GoogleRegister(string googleID,string userNickname) //���۷α��ν� �ڵ� ȸ�����ԵǴ� �Լ�
    //{
    //    StartManager.instance.loginTxtBtn.interactable = false;
    //    StartManager.instance.registerTxtBtn.interactable = false;
    //    var request = new RegisterPlayFabUserRequest { Email = googleID+"@rand.com", Password = googleID, Username = googleID + "@rand.com".Substring(0, StartManager.instance.emailInput.text.LastIndexOf('@')), DisplayName = userNickname }; //
    //    PlayFabClientAPI.RegisterPlayFabUser(request, GooglePlayFabLogin(), OnRegisterFailure);
    //}
    //#endregion

    void Init() //�����÷��� �α��� �ϱ��� ȣ��Ǵ� �Լ�
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
                //string Check = Regex.Replace(StartManager.instance.popUptxt.text, @"[^a-zA-Z0-9��-�R]", "", RegexOptions.Singleline);
                //Check = Regex.Replace(StartManager.instance.popUptxt.text, @"[^\w-\-.-_-#-$-%-^-&*]", "", RegexOptions.Singleline);

                //if (StartManager.instance.popUptxt.text.Equals(Check) == true)
                //{
                //    Debug.Log("Ư�����ڴ� ����� �� �����ϴ�.");
                //    return;
                //}
                StartManager.instance.popUptxt.text = "�α��ο� �����ϼ̽��ϴ�.";
            };
        });
    }

    public void GoogleLogout()  //���� �α׾ƿ� �� ȣ���ϴ� �Լ�
    {
        isGoogleLogin = false;
        ((PlayGamesPlatform)Social.Active).SignOut();
    }

    public void GooglePlayFabLogin()    //���� �α��� ���� �� �÷����� �ڵ����� �α����ϵ��� ����, ���н� GooglePlayFabRegister �ݹ��Լ� ȣ��
    {
        string userId = Social.localUser.id.Replace("_", "").Substring(0, 5);
        var request = new LoginWithEmailAddressRequest { Email = userId + "@rand.com", Password = userId + "#$%" };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => OnLoginSuccess(result), (error) => GooglePlayFabRegister());
        isGoogleLogin = true;
    }

    public void GooglePlayFabRegister() // �����÷��� �α��� �������� �� �÷����� �������� ȸ�������� �ȵǾ����� ��� �ڵ����� ȸ�������ϵ��� ����
    {
        string userId = Social.localUser.id.Replace("_", "").Substring(0, 5);
        var request = new RegisterPlayFabUserRequest { Email = userId + "@rand.com", Password = userId + "#$%", Username = userId, DisplayName = Social.localUser.userName };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { GooglePlayFabLogin(); },
            (error) => {
                StartManager.instance.googleloginTxtBtn.interactable = true;
                StartManager.instance.loginTxtBtn.interactable = true;
                StartManager.instance.registerTxtBtn.interactable = true;
                StartManager.instance.popUpObj.SetActive(true);
                StartManager.instance.popUptxt.text = $"ȸ�����Կ� �����ϼ̽��ϴ�.";
            });
    }

    #region #  GetPlayerProfile() - �÷��̾� �г��� �������� �Լ�, PlayFabManager�� LoginSuccess �Լ����� ȣ��
    void GetPlayerProfile(string playFabId) // �÷��̾� �г��� �������� �Լ�
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
        print("�ƹ��ų�" + playerNickname);
    }
    #endregion

    #region #  OnLoginSuccess() - �α��� ���� �� ȣ��Ǵ� �Լ�, PlayFabClientAPI.LoginWithEmailAddress���� ȣ��
    private void OnLoginSuccess(LoginResult result) // �α��� ���� �� ȣ��Ǵ� �Լ�
    {
        StartManager.instance.loadingDataTxt.text = "�α��� ����!";
        //StartManager.instance.popUpObj.SetActive(true);
        //StartManager.instance.popUptxt.text = "�α��� ����!";
        StartManager.instance.googleloginTxtBtn.interactable = true;
        StartManager.instance.loginTxtBtn.interactable = true;
        StartManager.instance.registerTxtBtn.interactable = true;
        StartManager.instance.loginRegisterBox.SetActive(false);
        StartManager.instance.loadingDataTxt.gameObject.SetActive(true);
        GetPlayerProfile(result.PlayFabId); // �÷��̾� �г��� �������� �Լ�        
        //�ð��߰�
        loginData.SetActive(true);

    }
    #endregion

    private void OnLoginFailure(PlayFabError error) // �α��� ���� �� ���� �ݹ� �Լ�
    {
        StartManager.instance.googleloginTxtBtn.interactable = true;
        StartManager.instance.loginTxtBtn.interactable = true;
        StartManager.instance.registerTxtBtn.interactable = true;
        print("�α��ν���");
        StartManager.instance.popUpObj.SetActive(true);
        StartManager.instance.popUptxt.text = "�α��ο� �����ϼ̽��ϴ�.";

        if (!StartManager.instance.loginEmailInput.text.Contains(".com") || StartManager.instance.loginEmailInput.text.Contains("!") || StartManager.instance.loginEmailInput.text.Contains("#")
            || StartManager.instance.loginEmailInput.text.Contains("$") || StartManager.instance.loginEmailInput.text.Contains("%") || StartManager.instance.loginEmailInput.text.Contains("^") || StartManager.instance.loginEmailInput.text.Contains("&")
            || StartManager.instance.loginEmailInput.text.Contains("(") || StartManager.instance.loginEmailInput.text.Contains(")") || StartManager.instance.loginEmailInput.text.Contains("-")
            || StartManager.instance.loginEmailInput.text.Contains("_") || StartManager.instance.loginEmailInput.text.Contains(" "))
        {
            StartManager.instance.popUptxt.text += "\n �̸��� ������ �߸��Ǿ����ϴ�.\n(Ư������ �� ������ �Է��� �Ұ����մϴ�.)";
        }

        else
        {
            print("��й�ȣ����");
            StartManager.instance.popUptxt.text += "\n ��й�ȣ�� �߸� �Է��ϼ̰ų�\n��ϵ��� ���� �̸����Դϴ�.";
        }
    }


    private void OnRegisterSuccess(RegisterPlayFabUserResult result)    // ȸ������ ���� �� ȣ��Ǵ� �Լ�
    {
        print("ȸ�� ���� ����");
        StartManager.instance.googleloginTxtBtn.interactable = true;
        StartManager.instance.loginTxtBtn.interactable = true;
        StartManager.instance.registerTxtBtn.interactable = true;
        StartManager.instance.ClickcloseOpenObjectBtn(StartManager.instance.registerPopUp, false); // ȸ�� ���� â �ݱ� closePopUpBtn, closePopUpBtnImg,
        StartManager.instance.popUpObj.SetActive(true);
        StartManager.instance.popUptxt.text = "ȸ������ �Ϸ�!";
    }


    private void OnRegisterFailure(PlayFabError error)   // ȸ�� ���� ��� ���� �� ȣ��Ǵ� �Լ�
    {
        StartManager.instance.googleloginTxtBtn.interactable = true;
        StartManager.instance.loginTxtBtn.interactable = true;
        StartManager.instance.registerTxtBtn.interactable = true;
        print("ȸ�� ���� ����");
        StartManager.instance.popUpObj.SetActive(true);
        StartManager.instance.popUptxt.text = "ȸ�����Կ� �����ϼ̽��ϴ�.";
        if (!StartManager.instance.emailInput.text.Contains(".com") || StartManager.instance.emailInput.text.Contains("!") || StartManager.instance.emailInput.text.Contains("#") || StartManager.instance.emailInput.text.Contains("&") ||
            StartManager.instance.emailInput.text.Contains("$") || StartManager.instance.emailInput.text.Contains("%") || StartManager.instance.emailInput.text.Contains("^") || StartManager.instance.emailInput.text.Contains("(") || StartManager.instance.emailInput.text.Contains(")") || StartManager.instance.emailInput.text.Contains("-") || StartManager.instance.emailInput.text.Contains("_") || StartManager.instance.emailInput.text.Contains(" "))
        {
            StartManager.instance.popUptxt.text += "\n �̸��� ������ �߸��Ǿ����ϴ�.\n.com���� �������� �ۼ����ּ���.\n(Ư������ �� ������ �Է��� �Ұ���)";
        }
        Debug.LogError(error.GenerateErrorReport());
    }

    public void SetStat(float higher)   // ���� ���� �� ����
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
        (result) => { print("�������"); },
        (error) => { print("�� ���� ����"); });
    }

    public void GetStat(TextMeshProUGUI txt)   // �� �ҷ�����
    {

        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            (result) =>
            {
                StartManager.instance.playerNicknameTxt.text = playerNickname;
                print("����Ʈ 0��° ��" + result.Statistics);
                print("����Ʈ 0��° ��" + result.Statistics.Count);
                if (result.Statistics.Count == 0)
                {
                    print(playerNickname);
                    playerMaxHeight = 0;
                    txt.text = $"���� ����� �����ϴ�!\n���ο� ����� �����ּ���!";
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
            (error) => { print("�� �ҷ����� ����"); });
        print("�ƹ��ų�" + playerNickname);
    }

    public void SendLeaderboard(float score,float time)    // ����ǥ�� �÷��̾� ���ھ� �����ϱ�
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

    public IEnumerator GetLeaderboard() // ���������� �� 
    {
        
        StartManager.instance.showRankBtn.interactable = false;
        StartManager.instance.popUptxt.text = "��ŷ �ҷ�������...";
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
        


        print("��ŷ �����ֱ�");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log(" ��� ����");
    }

    
    public struct LeaderBoardValue  // ��ŷ ����ǥ�� ������ ������ ���� ����ü
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
    private void OnLeaderboardGetHeight(GetLeaderboardResult result)  //�������� �� �ҷ����� �Լ�
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
                print(i + "��°");
                print(rankOb.transform.GetComponentInChildren<Image>());
                print("���" + (i + 1));
                print(result.Leaderboard[i]);
                print(result.Leaderboard[i].StatValue);
                // height_rank �������� i ��°�� �г����� �޾ƿ��� �� �� for���� ������ time_rank ���������� i ��° �г��ӵ�� ���� �� ���� �г����� ������ i��°�� time_rank.StatValue ���� �������½����� ����
                StartManager.instance.ShowPlayerRank(result.Leaderboard[i].StatValue, rankOb.transform.GetComponentInChildren<Image>());
                LeaderBoardValue ranker = new LeaderBoardValue(rankOb.transform.GetComponentInChildren<Image>(), (i + 1), result.Leaderboard[i].DisplayName, result.Leaderboard[i].StatValue, rankPlayersTimes[result.Leaderboard[i].DisplayName]);
                // ������ RankLine�� �� �Է����ֱ�
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
    void OnLeaderboardGetTime(GetLeaderboardResult result)  //�������� �� �ҷ����� �Լ�
    {
        print("�÷��� Ÿ�� �ҷ����� �Լ� ����");
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
