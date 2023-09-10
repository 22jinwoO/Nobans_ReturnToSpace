using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    [SerializeField]
    Image endingTxtImg;

    [SerializeField]
    GameObject fireworkPref;

    [SerializeField]
    GameObject fireworkPref2;

    [SerializeField]
    GameObject fireworkOb;
    
    [SerializeField]
    GameObject fireworkOb2;

    [SerializeField]
    ParticleSystem fireWork;

    [SerializeField]
    ParticleSystem fireWork2;
    private void Awake()
    {
        var camera = Camera.main;
        var r = camera.rect;
        var scaleheight = ((float)Screen.width / Screen.height) / (9f / 16f);
        var scalewidth = 1f / scaleheight;
        if (scaleheight < 1f)
        {
            r.height = scaleheight;
            r.y = (1f - scaleheight) / 2f;
            OnPreCull();
        }
        else
        {
            r.width = scalewidth;
            print(r.width);
            r.x = (1f - scalewidth) / 2f;
            print(r.x);
            OnPreCull();
        }

        camera.rect = r;

        OnPreCull();

}
    // Start is called before the first frame update
    void Start()
    {
        fireworkOb = Instantiate(fireworkPref);
        fireworkOb2 = Instantiate(fireworkPref2);

        fireWork =fireworkOb.GetComponent<ParticleSystem>();
        fireWork2 =fireworkOb2.GetComponent<ParticleSystem>();
        fireWork.Stop();
        fireWork2.Stop();


        StartCoroutine("ExpandFonts");
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);

    void FireWork()
    {

        GameObject fireworkPrefabOb = Instantiate(fireworkPref);
        GameObject fireworkPrefabOb2 = Instantiate(fireworkPref2);
        fireworkPrefabOb.transform.position = new Vector3(Random.Range(-1.5f,1.5f), Random.Range(1.8f, 4f), 0);
        fireworkPrefabOb2.transform.position = fireworkPrefabOb.transform.position;

        Invoke("FireWork", Random.Range(0.3f, 0.6f));
    }

   
    public IEnumerator ExpandFonts()    // 엔딩 씬에서 폰트 늘려주는 함수
    {
        float valueY = 1.5f;
        yield return new WaitForSeconds(1.2f);
        Invoke("FireWork", 0.8f);

        while (endingTxtImg.rectTransform.sizeDelta.x<875f) // 텍스트 이미지 x의 크기가 875f 보다작을 때 동안
        {
            if (endingTxtImg.rectTransform.sizeDelta.y>=131f)
            {
                valueY = 0;
            }
            endingTxtImg.rectTransform.sizeDelta += new Vector2(10, valueY);
            yield return null;
        }

        yield return new WaitForSeconds(12f);
        PlayFabManager.instance.loginData.SetActive(false); // 로그인 데이터를 비활성화 시키고 
        SceneManager.LoadScene("1.StartScene"); // 타이틀화면 씬으로 돌아감
        AudioManager.instance.gameObject.SetActive(true);   // 오디오 매니저 오브젝트 활성화

        AudioManager.instance.StartCoroutine("SoundFade", AudioManager.instance.soundsBgm[0]);  // 시작화면 브금 실행

    }
}
