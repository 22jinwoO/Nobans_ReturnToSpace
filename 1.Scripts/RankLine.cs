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

    public void SetData(PlayFabManager.LeaderBoardValue ranker) // ����ü�� ���� ���� ������ �ݿ����ִ� �Լ�
    {
        int min= ranker.rankPlayTime/60;
        int sec= ranker.rankPlayTime%60;
        rankImg = ranker.rankImg;
        rankTxt.text = ranker.rankNum.ToString();
        print("���� �۾�"+rankTxt.text);
        print(ranker.rankNum);
        rankNicknameTxt.text = ranker.rankNickname;
        rankHighTxt.text = ranker.rankHeight.ToString() + "km";
        rankPlayTimeTxt.text = $"{min.ToString("00")} : {sec.ToString("00")}";
    }
}
