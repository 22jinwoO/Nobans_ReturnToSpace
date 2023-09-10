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
    public static PlayFabManager instance; //½Ì±ÛÅæÀ¸·Î ¸¸µé±â À§ÇØ ¼±¾ð


    public bool isLogin;    // ·Î±×ÀÎ È®ÀÎ À¯¹« ÆÇ´ÜÇÏ´Â º¯¼ö

    public int playerMaxHeight;
    public string playerNickname;
    public bool isGoogleLogin;


    [SerializeField]
    string rankPlayerNickname;

    [SerializeField]
    Dictionary<string , int> rankPlayersTimes= new Dictionary<string, int>();

    // ·Î±×ÀÎ µ¥ÀÌÅÍ
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
    public void SetResoulution()    // È­¸é °íÁ¤ÇÏ´Â ÇÔ¼ö
    {
        int setWidth = 1080;
        int setHeight = 1920;

        Screen.SetResolution(setWidth, setHeight, false);
    }
    #region  # ClickLoginBtn() - ·Î±×ÀÎ ¹öÆ°¿¡ ¿¬°áÇÏ´Â ÇÔ¼ö , StartManagerÀÇ loginTxtBtn¿¡ ¿¬°áµÇ¾î È£Ãâ
    public void ClickLoginBtn() // ·Î±×ÀÎ ¹öÆ°¿¡ ¿¬°áÇÏ´Â ÇÔ¼ö
    {
        StartManager.instance.googleloginTxtBtn.interactable = false;
        StartManager.instance.loginTxtBtn.interactable = false;
        StartManager.instance.registerTxtBtn.interactable = false;
        var request = new LoginWithEmailAddressRequest { Email = StartManager.instance.loginEmailInput.text, Password = StartManager.instance.loginPasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }
    #endregion

    #region #  ClickRegisterBtn() - È¸¿ø°¡ÀÔ ¹öÆ°¿¡ ¿¬°áÇÏ´Â ÇÔ¼ö, StartManagerÀÇ memberRegisterBtn¿¡ ¿¬°áµÇ¾î È£Ãâ
    public void ClickRegisterBtn()  // È¸¿ø°¡ÀÔ ¹öÆ°¿¡ ¿¬°áÇÏ´Â ÇÔ¼ö
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
    //public void GoogleRegister(string googleID,string userNickname) //±¸±Û·Î±×ÀÎ½Ã ÀÚµ¿ È¸¿ø°¡ÀÔµÇ´Â ÇÔ¼ö
    //{
    //    StartManager.instance.loginTxtBtn.interactable = false;
    //    StartManager.instance.registerTxtBtn.interactable = false;
    //    var request = new RegisterPlayFabUserRequest { Email = googleID+"@rand.com", Password = googleID, Username = googleID + "@rand.com".Substring(0, StartManager.instance.emailInput.text.LastIndexOf('@')), DisplayName = userNickname }; //
    //    PlayFabClientAPI.RegisterPlayFabUser(request, GooglePlayFabLogin(), OnRegisterFailure);
    //}
    //#endregion

    void Init() //±¸±ÛÇÃ·¹ÀÌ ·Î±×ÀÎ ÇÏ±âÀü È£ÃâµÇ´Â ÇÔ¼ö
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
                //string Check = Regex.Replace(StartManager.instance.popUptxt.text, @"[^a-zA-Z0-9°¡-ÆR]", "", RegexOptions.Singleline);
                //Check = Regex.Replace(StartManager.instance.popUptxt.text, @"[^\w-\-.-_-#-$-%-^-&*]", "", RegexOptions.Singleline);

                //if (StartManager.instance.popUptxt.text.Equals(Check) == true)
                //{
                //    Debug.Log("Æ¯¼ö¹®ÀÚ´Â »ç¿ëÇÒ ¼ö ¾ø½À´Ï´Ù.");
                //    return;
                //}
                StartManager.instance.popUptxt.text = "·Î±×ÀÎ¿¡ ½ÇÆÐÇÏ¼Ì½À´Ï´Ù.";
            };
        });
    }

    public void GoogleLogout()  //±¸±Û ·Î±×¾Æ¿ô ½Ã È£ÃâÇÏ´Â ÇÔ¼ö
    {
        isGoogleLogin = false;
        ((PlayGamesPlatform)Social.Active).SignOut();
    }

    public void GooglePlayFabLogin()    //±¸±Û ·Î±×ÀÎ ¼º°ø ½Ã ÇÃ·¹ÀÌÆÕ ÀÚµ¿À¸·Î ·Î±×ÀÎÇÏµµ·Ï ¼³Á¤, ½ÇÆÐ½Ã GooglePlayFabRegister ÄÝ¹éÇÔ¼ö È£Ãâ
    {
        string userId = Social.localUser.id.Replace("_", "").Substring(0, 5);
        var request = new LoginWithEmailAddressRequest { Email = userId + "@rand.com", Password = userId + "#$%" };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => OnLoginSuccess(result), (error) => GooglePlayFabRegister());
        isGoogleLogin = true;
    }

    public void GooglePlayFabRegister() // ±¸±ÛÇÃ·¹ÀÌ ·Î±×ÀÎ ¼º°øÇßÀ» ¶§ ÇÃ·¹ÀÌÆÕ ¼­¹ö¿¡¼­ È¸¿ø°¡ÀÔÀÌ ¾ÈµÇ¾îÀÖÀ» °æ¿ì ÀÚµ¿À¸·Î È¸¿ø°¡ÀÔÇÏµµ·Ï ¼³Á¤
    {
        string userId = Social.localUser.id.Replace("_", "").Substring(0, 5);
        var request = new RegisterPlayFabUserRequest { Email = userId + "@rand.com", Password = userId + "#$%", Username = userId, DisplayName = Social.localUser.userName };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { GooglePlayFabLogin(); },
            (error) => {
                StartManager.instance.googleloginTxtBtn.interactable = true;
                StartManager.instance.loginTxtBtn.interactable = true;
                StartManager.instance.registerTxtBtn.interactable = true;
                StartManager.instance.popUpObj.SetActive(true);
                StartManager.instance.popUptxt.text = $"È¸¿ø°¡ÀÔ¿¡ ½ÇÆÐÇÏ¼Ì½À´Ï´Ù.";
            });
    }

    #region #  GetPlayerProfile() - ÇÃ·¹ÀÌ¾î ´Ð³×ÀÓ °¡Á®¿À´Â ÇÔ¼ö, PlayFabManagerÀÇ LoginSuccess ÇÔ¼ö¿¡¼­ È£Ãâ
    void GetPlayerProfile(string playFabId) // ÇÃ·¹ÀÌ¾î ´Ð³×ÀÓ °¡Á®¿À´Â ÇÔ¼ö
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
        print("¾Æ¹«°Å³ª" + playerNickname);
    }
    #endregion

    #region #  OnLoginSuccess() - ·Î±×ÀÎ ¼º°ø ½Ã È£ÃâµÇ´Â ÇÔ¼ö, PlayFabClientAPI.LoginWithEmailAddress¿¡¼­ È£Ãâ
    private void OnLoginSuccess(LoginResult result) // ·Î±×ÀÎ ¼º°ø ½Ã È£ÃâµÇ´Â ÇÔ¼ö
    {
        StartManager.instance.loadingDataTxt.text = "·Î±×ÀÎ ¼º°ø!";
        //StartManager.instance.popUpObj.SetActive(true);
        //StartManager.instance.popUptxt.text = "·Î±×ÀÎ ¼º°ø!";
        StartManager.instance.googleloginTxtBtn.interactable = true;
        StartManager.instance.loginTxtBtn.interactable = true;
        StartManager.instance.registerTxtBtn.interactable = true;
        StartManager.instance.loginRegisterBox.SetActive(false);
        StartManager.instance.loadingDataTxt.gameObject.SetActive(true);
        GetPlayerProfile(result.PlayFabId); // ÇÃ·¹ÀÌ¾î ´Ð³×ÀÓ °¡Á®¿À´Â ÇÔ¼ö        
        //½Ã°£Ãß°¡
        loginData.SetActive(true);

    }
    #endregion

    private void OnLoginFailure(PlayFabError error) // ·Î±×ÀÎ ½ÇÆÐ ½Ã ¿¡·¯ ÄÝ¹é ÇÔ¼ö
    {
        StartManager.instance.googleloginTxtBtn.interactable = true;
        StartManager.instance.loginTxtBtn.interactable = true;
        StartManager.instance.registerTxtBtn.interactable = true;
        print("·Î±×ÀÎ½ÇÆÐ");
        StartManager.instance.popUpObj.SetActive(true);
        StartManager.instance.popUptxt.text = "·Î±×ÀÎ¿¡ ½ÇÆÐÇÏ¼Ì½À´Ï´Ù.";

        if (!StartManager.instance.loginEmailInput.text.Contains(".com") || StartManager.instance.loginEmailInput.text.Contains("!") || StartManager.instance.loginEmailInput.text.Contains("#")
            || StartManager.instance.loginEmailInput.text.Contains("$") || StartManager.instance.loginEmailInput.text.Contains("%") || StartManager.instance.loginEmailInput.text.Contains("^") || StartManager.instance.loginEmailInput.text.Contains("&")
            || StartManager.instance.loginEmailInput.text.Contains("(") || StartManager.instance.loginEmailInput.text.Contains(")") || StartManager.instance.loginEmailInput.text.Contains("-")
            || StartManager.instance.loginEmailInput.text.Contains("_") || StartManager.instance.loginEmailInput.text.Contains(" "))
        {
            StartManager.instance.popUptxt.text += "\n ÀÌ¸ÞÀÏ Çü½ÄÀÌ Àß¸øµÇ¾ú½À´Ï´Ù.\n(Æ¯¼ö¹®ÀÚ ¹× °ø¹éÀº ÀÔ·ÂÀÌ ºÒ°¡´ÉÇÕ´Ï´Ù.)";
        }

        else
        {
            print("ºñ¹Ð¹øÈ£¿À·ù");
            StartManager.instance.popUptxt.text += "\n ºñ¹Ð¹øÈ£¸¦ Àß¸ø ÀÔ·ÂÇÏ¼Ì°Å³ª\nµî·ÏµÇÁö ¾ÊÀº ÀÌ¸ÞÀÏÀÔ´Ï´Ù.";
        }
    }


    private void OnRegisterSuccess(RegisterPlayFabUserResult result)    // È¸¿ø°¡ÀÔ ¼º°ø ½Ã È£ÃâµÇ´Â ÇÔ¼ö
    {
        print("È¸¿ø °¡ÀÔ ¼º°ø");
        StartManager.instance.googleloginTxtBtn.interactable = true;
        StartManager.instance.loginTxtBtn.interactable = true;
        StartManager.instance.registerTxtBtn.interactable = true;
        StartManager.instance.ClickcloseOpenObjectBtn(StartManager.instance.registerPopUp, false); // È¸¿ø °¡ÀÔ Ã¢ ´Ý±â closePopUpBtn, closePopUpBtnImg,
        StartManager.instance.popUpObj.SetActive(true);
        StartManager.instance.popUptxt.text = "È¸¿ø°¡ÀÔ ¿Ï·á!";
    }


    private void OnRegisterFailure(PlayFabError error)   // È¸¿ø °¡ÀÔ µî·Ï ½ÇÆÐ ½Ã È£ÃâµÇ´Â ÇÔ¼ö
    {
        StartManager.instance.googleloginTxtBtn.interactable = true;
        StartManager.instance.loginTxtBtn.interactable = true;
        StartManager.instance.registerTxtBtn.interactable = true;
        print("È¸¿ø °¡ÀÔ ½ÇÆÐ");
        StartManager.instance.popUpObj.SetActive(true);
        StartManager.instance.popUptxt.text = "È¸¿ø°¡ÀÔ¿¡ ½ÇÆÐÇÏ¼Ì½À´Ï´Ù.";
        if (!StartManager.instance.emailInput.text.Contains(".com") || StartManager.instance.emailInput.text.Contains("!") || StartManager.instance.emailInput.text.Contains("#") || StartManager.instance.emailInput.text.Contains("&") ||
            StartManager.instance.emailInput.text.Contains("$") || StartManager.instance.emailInput.text.Contains("%") || StartManager.instance.emailInput.text.Contains("^") || StartManager.instance.emailInput.text.Contains("(") || StartManager.instance.emailInput.text.Contains(")") || StartManager.instance.emailInput.text.Contains("-") || StartManager.instance.emailInput.text.Contains("_") || StartManager.instance.emailInput.text.Contains(" "))
        {
            StartManager.instance.popUptxt.text += "\n ÀÌ¸ÞÀÏ Çü½ÄÀÌ Àß¸øµÇ¾ú½À´Ï´Ù.\n.comÀ¸·Î ³¡³ªµµ·Ï ÀÛ¼ºÇØÁÖ¼¼¿ä.\n(Æ¯¼ö¹®ÀÚ ¹× °ø¹éÀº ÀÔ·ÂÀÌ ºÒ°¡´É)";
        }
        Debug.LogError(error.GenerateErrorReport());
    }

    public void SetStat(float higher)   // À¯Àú ³ôÀÌ °ª ÀúÀå
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
        (result) => { print("°ªÀúÀåµÊ"); },
        (error) => { print("°ª ÀúÀå ½ÇÆÐ"); });
    }

    public void GetStat(TextMeshProUGUI txt)   // °ª ºÒ·¯¿À±â
    {

        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            (result) =>
            {
                StartManager.instance.playerNicknameTxt.text = playerNickname;
                print("¸®½ºÆ® 0¹øÂ° °ª" + result.Statistics);
                print("¸®½ºÆ® 0¹øÂ° °ª" + result.Statistics.Count);
                if (result.Statistics.Count == 0)
                {
                    print(playerNickname);
                    playerMaxHeight = 0;
                    txt.text = $"ÇöÀç ±â·ÏÀÌ ¾ø½À´Ï´Ù!\n»õ·Î¿î ±â·ÏÀ» ³²°ÜÁÖ¼¼¿ä!";
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
            (error) => { print("°ª ºÒ·¯¿À±â ½ÇÆÐ"); });
        print("¾Æ¹«°Å³ª" + playerNickname);
    }

    public void SendLeaderboard(float score,float time)    // ¼øÀ§Ç¥¿¡ ÇÃ·¹ÀÌ¾î ½ºÄÚ¾î Àü´ÞÇÏ±â
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

    public IEnumerator GetLeaderboard() // ¸®´õº¸µåÀÇ °ª 
    {
        
        StartManager.instance.showRankBtn.interactable = false;
        StartManager.instance.popUptxt.text = "·©Å· ºÒ·¯¿À´ÂÁß...";
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
        


        print("·©Å· º¸¿©ÁÖ±â");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log(" µî·Ï ¼º°ø");
    }

    
    public struct LeaderBoardValue  // ·©Å· ¼øÀ§Ç¥¿¡ µ¥ÀÌÅÍ Àü´ÞÀ» À§ÇÑ ±¸Á¶Ã¼
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
    private void OnLeaderboardGetHeight(GetLeaderboardResult result)  //¸®´õº¸µå °ª ºÒ·¯¿À´Â ÇÔ¼ö
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
                print(i + "¹øÂ°");
                print(rankOb.transform.GetComponentInChildren<Image>());
                print("µî¼ö" + (i + 1));
                print(result.Leaderboard[i]);
                print(result.Leaderboard[i].StatValue);
                // height_rank ¸®´õº¸µå i ¹øÂ°ÀÇ ´Ð³×ÀÓÀ» ¹Þ¾Æ¿À°í ³­ ÈÄ for¹®À» µ¹·Á¼­ time_rank ¸®´õº¸µåÀÇ i ¹øÂ° ´Ð³×ÀÓµé°ú ºñ±³ÇÑ ÈÄ °°Àº ´Ð³×ÀÓÀÌ ÀÕÀ¸¸é i¹øÂ°ÀÇ time_rank.StatValue °ªÀ» °¡Á®¿À´Â½ÄÀ¸·Î ±¸Çö
                StartManager.instance.ShowPlayerRank(result.Leaderboard[i].StatValue, rankOb.transform.GetComponentInChildren<Image>());
                LeaderBoardValue ranker = new LeaderBoardValue(rankOb.transform.GetComponentInChildren<Image>(), (i + 1), result.Leaderboard[i].DisplayName, result.Leaderboard[i].StatValue, rankPlayersTimes[result.Leaderboard[i].DisplayName]);
                // º¹»çÇÑ RankLine¿¡ °ª ÀÔ·ÂÇØÁÖ±â
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
    void OnLeaderboardGetTime(GetLeaderboardResult result)  //¸®´õº¸µå °ª ºÒ·¯¿À´Â ÇÔ¼ö
    {
        print("ÇÃ·¹ÀÌ Å¸ÀÓ ºÒ·¯¿À´Â ÇÔ¼ö ½ÇÇà");
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
