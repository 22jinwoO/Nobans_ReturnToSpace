using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginData : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(GetLoginData());
    }


    void Update()
    {

    }
    IEnumerator GetLoginData()  //유저가 로그인한 아이디의 정보를 가져오는 함수
    {
        yield return new WaitForSeconds(0.7f);
        StartManager.instance.loadingDataTxt.text = "데이터 불러오는중...";
        yield return new WaitForSeconds(0.5f);
        print("로그인 데이터 불러오기");
        yield return new WaitForSeconds(1.4f);
        PlayFabManager.instance.GetStat(StartManager.instance.playerHighTxt);
        yield return new WaitForSeconds(0.5f);
        print("로그인성공");



        StartManager.instance.loginRegisterPanel.SetActive(false);

        StartManager.instance.ShowPlayerRank(PlayFabManager.instance.playerMaxHeight, StartManager.instance.playerRankImg);
        Debug.Log("Congratulations, you made your first successful API call!");

    }

}
