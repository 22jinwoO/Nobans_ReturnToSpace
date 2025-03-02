using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankLine : MonoBehaviour
{
    public Image rankImg;
    public TextMeshProUGUI rankTxt;
    public TextMeshProUGUI rankNicknameTxt;
    public TextMeshProUGUI rankHighTxt;
    public TextMeshProUGUI rankPlayTimeTxt;
    public int rankCount;

    public void SetData(PlayFabManager.LeaderBoardValue ranker) // 구조체로 전달 받은 데이터 반영해주는 함수
    {
        int min= ranker.rankPlayTime/60;
        int sec= ranker.rankPlayTime%60;
        rankImg = ranker.rankImg;
        rankTxt.text = ranker.rankNum.ToString();
        print("순위 글씨"+rankTxt.text);
        print(ranker.rankNum);
        rankNicknameTxt.text = ranker.rankNickname;
        rankHighTxt.text = ranker.rankHeight.ToString() + "km";
        rankPlayTimeTxt.text = $"{min.ToString("00")} : {sec.ToString("00")}";
    }
}
