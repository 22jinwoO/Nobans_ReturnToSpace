using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [Header("������� ����� �ҽ�")]
    public AudioSource audioBgm;

    [Header("���� ���� ����� �ҽ�")]
    public AudioSource jumpSound;

    [Header("ȿ���� ����� �ҽ�")]
    public AudioSource audioItemEffectSound;

    [Header("������� ����� Ŭ�� �迭")]
    public AudioClip[] soundsBgm;

    [Header("���� ����� Ŭ�� �迭")]
    public AudioClip[] soundsJump;

    [Header("������ȿ���� ����� Ŭ�� �迭")]
    public AudioClip[] soundsItem;

    [Header("��ưȿ���� ����� Ŭ�� �迭")]
    public AudioClip[] soundsBtn;


    static public AudioManager instance;
    void Awake()
    {
        if (null == instance)
        {
            //�� Ŭ���� �ν��Ͻ��� ź������ �� �������� instance�� ���ӸŴ��� �ν��Ͻ��� ������� �ʴٸ�, �ڽ��� �־��ش�.
            instance = this;

            //�� ��ȯ�� �Ǵ��� �ı����� �ʰ� �Ѵ�.
            //gameObject�����ε� �� ��ũ��Ʈ�� ������Ʈ�μ� �پ��ִ� Hierarchy���� ���ӿ�����Ʈ��� ��������, 
            //���� �򰥸� ������ ���� this�� �ٿ��ֱ⵵ �Ѵ�.
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� GameMgr�� ������ ���� �ִ�.
            //�׷� ��쿣 ���� ������ ����ϴ� �ν��Ͻ��� ��� ������ִ� ��찡 ���� �� ����.
            //�׷��� �̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ�(���ο� ���� GameMgr)�� �������ش�.
            Destroy(this.gameObject);
        }

    }

    void Start()
    {
        audioBgm = GetComponents<AudioSource>()[0];   // ù��° ����� �ҽ� ��������
        jumpSound = GetComponents<AudioSource>()[1]; // �ι�° ����� �ҽ� ��������
        audioItemEffectSound = GetComponents<AudioSource>()[2]; // ����° ����� �ҽ� ��������
        Sound_Bgm(soundsBgm[0]);    //ù ���� BGM ���� ����
    }

    public void Sound_Bgm(AudioClip bgmSound)   //BGM ���� �����ϴ� �Լ�(�Ű������� ����� Ŭ��)
    {
        audioBgm.clip = bgmSound;
        audioBgm.Play();
    }

    public void Sound_Jump(AudioClip orginJumpSound) // �����Ҷ� ���� ���尡 ����Ǵ� �Լ�(�Ű����� ����� Ŭ��)
    {
        jumpSound.clip = orginJumpSound;
        jumpSound.PlayOneShot(orginJumpSound);
    }

    public void Sound_Item(AudioClip itemSound) // ������ ���� ���尡 ����Ǵ� �Լ� (�Ű������� ����� Ŭ��)
    {
        audioItemEffectSound.clip = itemSound;
        audioItemEffectSound.Play();
    }


    public void Sound_ClickBtn(AudioClip clickBtnSound) // ��ư Ŭ�� �� ����Ǵ� �Լ� (�Ű������� ����� Ŭ��)
    {
        audioItemEffectSound.clip = clickBtnSound;
        audioItemEffectSound.Play();
    }

    public IEnumerator SoundFade(AudioClip fadeSound)   // ���� ���̵� �ξƿ� �Լ�
    {
        while (audioBgm.volume >0f)
        {
            audioBgm.volume -= 0.1f;
            yield return new WaitForSeconds(0.2f);
            yield return null;
        }

        audioBgm.clip = fadeSound;
        if (audioBgm.clip!=null)
        {
            audioBgm.Play();

        }
        while (audioBgm.volume<0.6f)
        {
            audioBgm.volume += 0.1f;
            yield return new WaitForSeconds(0.2f);
            yield return null;
        }
    }
}