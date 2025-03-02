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

   
    public IEnumerator ExpandFonts()    // ���� ������ ��Ʈ �÷��ִ� �Լ�
    {
        float valueY = 1.5f;
        yield return new WaitForSeconds(1.2f);
        Invoke("FireWork", 0.8f);

        while (endingTxtImg.rectTransform.sizeDelta.x<875f) // �ؽ�Ʈ �̹��� x�� ũ�Ⱑ 875f �������� �� ����
        {
            if (endingTxtImg.rectTransform.sizeDelta.y>=131f)
            {
                valueY = 0;
            }
            endingTxtImg.rectTransform.sizeDelta += new Vector2(10, valueY);
            yield return null;
        }

        yield return new WaitForSeconds(12f);
        PlayFabManager.instance.loginData.SetActive(false); // �α��� �����͸� ��Ȱ��ȭ ��Ű�� 
        SceneManager.LoadScene("1.StartScene"); // Ÿ��Ʋȭ�� ������ ���ư�
        AudioManager.instance.gameObject.SetActive(true);   // ����� �Ŵ��� ������Ʈ Ȱ��ȭ

        AudioManager.instance.StartCoroutine("SoundFade", AudioManager.instance.soundsBgm[0]);  // ����ȭ�� ��� ����

    }
}
