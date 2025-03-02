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
    IEnumerator GetLoginData()  //������ �α����� ���̵��� ������ �������� �Լ�
    {
        yield return new WaitForSeconds(0.7f);
        StartManager.instance.loadingDataTxt.text = "������ �ҷ�������...";
        yield return new WaitForSeconds(0.5f);
        print("�α��� ������ �ҷ�����");
        yield return new WaitForSeconds(1.4f);
        PlayFabManager.instance.GetStat(StartManager.instance.playerHighTxt);
        yield return new WaitForSeconds(0.5f);
        print("�α��μ���");



        StartManager.instance.loginRegisterPanel.SetActive(false);

        StartManager.instance.ShowPlayerRank(PlayFabManager.instance.playerMaxHeight, StartManager.instance.playerRankImg);
        Debug.Log("Congratulations, you made your first successful API call!");

    }

}
