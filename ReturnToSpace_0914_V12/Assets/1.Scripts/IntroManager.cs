using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




public class IntroManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI storyTxt; // ���丮 �ؽ�Ʈ

    // ��Ʈ�� ����(�ؽ�Ʈ�� �ε������� �ؽ�Ʈ ���)
    [SerializeField]
    bool isStoryEnd;

    [SerializeField]
    int dialogueCount = 0;

    [System.Serializable]
    private class Dialogue
    {
        [TextArea]
        public string storyText;
    }

    [SerializeField]
    private Dialogue[] dialgoue;

    [SerializeField]
    private Button skipTextBtn;

    [SerializeField]
    private Image backgroundImg;

    private void Awake()
    {
        skipTextBtn.onClick.AddListener(ClickSkipBtn);
        
        // ȭ�� ���� ����
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


        StartCoroutine("ShowStoryText");

    }
    public void OnPreCull() => GL.Clear(true, true, Color.black);

    private IEnumerator ShowStoryText() // ��Ʈ�ο��� ���丮�� �����ִ� �ؽ�Ʈ �����ϴ� �Լ�
    {

        if (dialogueCount<dialgoue.Length)
        {
            
            float textColoA=0f;
            storyTxt.color = new Color(1,1,1,0);
            storyTxt.text = dialgoue[dialogueCount].storyText;
            dialogueCount++;

            while (textColoA <= 1f)
            {
                if (dialogueCount == 3)
                {
                    backgroundImg.color = new Color(1, 1, 1, textColoA);
                    storyTxt.color = new Color(1, 1, 1, textColoA);
                    textColoA += 0.05f;
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    storyTxt.color = new Color(1, 1, 1, textColoA);
                    textColoA += 0.05f;
                    yield return new WaitForSeconds(0.1f);
                }

            }
            yield return new WaitForSeconds(1.5f);
            while (textColoA >= 0f&& dialogueCount<3)
            {
                storyTxt.color = new Color(1, 1, 1, textColoA);
                textColoA -= 0.2f;
                yield return new WaitForSeconds(0.1f);
            }
            StartCoroutine("ShowStoryText");
        }
        else
        {
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("1.StartScene");
        }    

    }

    private void ClickSkipBtn() //��ŵ ��ư Ŭ�� �� Ÿ��Ʋ ȭ������ �� �̵�
    {
        SceneManager.LoadScene("1.StartScene");
    }
}


