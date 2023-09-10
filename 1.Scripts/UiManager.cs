using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UiManager : MonoBehaviour
{
    [SerializeField]
    private Image fadeInFadeOutImg;    // ���̵��� ���̵� �ƿ� �̹���

    [SerializeField]
    private GameObject settingUiGameObject;

    [SerializeField]
    private Button setBtn;

    [SerializeField]
    private Image setBtnImg;

    [SerializeField]
    private Button closeBtn;

    [SerializeField]
    private Image closeBtnImg;

    [SerializeField]
    private GameObject settingPopUp;

    [SerializeField]
    private Button retryBtn;    //����â �ٽ��ϱ� ��ư

    [SerializeField]
    private Image retryBtnImg;

    [SerializeField]
    private Button goHomeBtn;   //����â Ȩ��ư

    [SerializeField]
    private Image goHomeBtnImg;


    [SerializeField]
    private Button retryBtn2;

    [SerializeField]
    private Image retryBtn2Img;

    [SerializeField]
    private Button goHomeBtn2;

    [SerializeField]
    private Image goHomeBtn2Img;

    [SerializeField]
    private Sprite[] pressedBtnImages;

    public bool isStop;
    void Awake()
    {
        setBtn.onClick.AddListener(ClickSetBtn);
        closeBtn.onClick.AddListener(ClickCloseSetBtn);
        goHomeBtn.onClick.AddListener(()=>ClickGoHomeBtn(goHomeBtn,goHomeBtnImg));
        goHomeBtn2.onClick.AddListener(()=>ClickGoHomeBtn(goHomeBtn2, goHomeBtn2Img));
        retryBtn.onClick.AddListener(()=>ClickRetry(retryBtn, retryBtnImg));
        retryBtn2.onClick.AddListener(()=>ClickRetry(retryBtn2, retryBtn2Img));

        StartManager.instance.ScreenResolution();
    }




    void ClickSetBtn()  // ����â ����
    {
        setBtn.interactable = false;
        setBtnImg.sprite = pressedBtnImages[1];
        StartCoroutine("PressedSetBtn");
        
    }

    IEnumerator PressedSetBtn() //��ư�� ������ ȣ��Ǵ� �Լ�
    {
        yield return new WaitForSeconds(0.15f);
        setBtnImg.sprite = pressedBtnImages[0];
        isStop = true;
        settingPopUp.SetActive(true);
        Time.timeScale = 0;
        
    }

    void ClickCloseSetBtn() //����â ���� ��
    {
        Time.timeScale = 1.3f;
        closeBtn.interactable = false;
        closeBtnImg.sprite = pressedBtnImages[3];
        StartCoroutine("PressedCloseSetBtn");
        
    }
    IEnumerator PressedCloseSetBtn()    // �ݱ� ��ư�� ������ �� ����Ǵ� �Լ�
    {
        yield return new WaitForSeconds(0.15f);
        closeBtnImg.sprite = pressedBtnImages[2];
        setBtn.interactable = true;
        print("����â ����");
        Time.timeScale = 1.3f;
        isStop = false;
        closeBtn.interactable = true;
        settingPopUp.SetActive(false);
    }

    void ClickGoHomeBtn(Button homeBtn, Image homeBtnImg)   // Ȩ��ư�� Ŭ���ϸ� ȣ��Ǵ� �Լ�
    {
        Time.timeScale = 1.3f;
        homeBtnImg.sprite = pressedBtnImages[5];
        homeBtn.interactable = false;
        StartCoroutine(PressedClickGomeHomeBtn(homeBtn, homeBtnImg));
    }

    IEnumerator PressedClickGomeHomeBtn(Button homeBtn, Image homeBtnImg)   // Ȩ��ư�� Ŭ������ �� ȣ��Ǵ� �Լ�
    {
        yield return new WaitForSeconds(0.15f);
        AudioManager.instance.StartCoroutine("SoundFade", AudioManager.instance.soundsBgm[0]);
        print("Ȩ��ư Ŭ��");
        Time.timeScale = 1;
        //Destroy(PlayFabManager.instance.gameObject);
        PlayFabManager.instance.loginData.SetActive(false); // �α��� �����͸� ��Ȱ��ȭ ��Ű�� 
        homeBtnImg.sprite = pressedBtnImages[4];
        homeBtn.interactable = true;
        SceneManager.LoadScene("1.StartScene");
    }

    void ClickRetry(Button retryBtn, Image retryBtnImg) //  �ٽ��ϱ� ��ư�� ������ �� ȣ��Ǵ� �Լ�
    {
        Time.timeScale = 1.3f;
        retryBtnImg.sprite = pressedBtnImages[7];
        retryBtn.interactable = false;
        StartCoroutine(PressedClickRetryBtn(retryBtn, retryBtnImg));
    }       

    IEnumerator PressedClickRetryBtn(Button retryBtn, Image retryBtnImg)
    {
        yield return new WaitForSeconds(0.15f);
        retryBtnImg.sprite = pressedBtnImages[6];
        retryBtn.interactable = true;
        Time.timeScale = 1;
        LoadingManager.LoadScene("2.MainScene");
    }

    public IEnumerator FadeInFadeOut(bool FadeInOutCheck)  // ȭ�� ���̵��� ���̵� �ƿ� ���ִ� �Լ�
    {
        Color color = fadeInFadeOutImg.color;
        fadeInFadeOutImg.gameObject.SetActive(true);

        if (FadeInOutCheck) // ���̵� �� ��Ȳ�� ��
        {
            float time = 1;
            while (time >= 0f)
            {
                color.a = time;
                fadeInFadeOutImg.color = color;
                time -= Time.deltaTime * 0.3f;
                yield return null;
            }
            fadeInFadeOutImg.gameObject.SetActive(false);

        }
        else  // ���̵� �ƿ� ��Ȳ�� ��
        {
            float time = 0;
            while (time <= 1f)
            {
                print("���̵�ƿ�");
                color.a = time;
                fadeInFadeOutImg.color = color;
                time += Time.deltaTime * 0.3f;
                yield return null;
            }
            
        }

    }
}
