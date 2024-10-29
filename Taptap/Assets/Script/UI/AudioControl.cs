using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    public static AudioControl instance;

    public static AudioControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindAnyObjectByType<AudioControl>();
            }
            return instance;
        }
    }
    
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Slider _slider;
    [SerializeField] private AudioSource audioSourceBuild;
    [SerializeField] private AudioSource audioSourceAttack; 
    [SerializeField] private AudioSource audioSourceTheme; 
    private AudioSource currentSource; //正在播放的音频
    private AudioSource nextSource; //准备播放的音频
    private AudioClip newClip;
    private bool isSwitching = false; //是否正在切换音乐
    
    private void Awake()
    {
        _toggle.onValueChanged.AddListener(isOn => ControlAudio());
        _slider.onValueChanged.AddListener(value =>Volume(value));
        currentSource = audioSourceTheme;
        nextSource = audioSourceBuild;
        
        currentSource.Play();
    }
    
    private void ControlAudio()
    {
        if (_toggle.isOn)
        {
            currentSource.Play();
        }
        else
        {
            currentSource.Stop();
        }
    }

    private void Volume(float value)
    {
        currentSource.volume = value;
    }

    private void Update()
    {
        
    }
    
    private void SetNextClip()
    {
        if (UIManager.Instance.isSpawning == true)
        {
            newClip = audioSourceAttack.clip;
        }

        if (UIManager.Instance.isSpawning == false)
        {
            newClip = audioSourceBuild.clip;
            Debug.Log("here");
        }
    }
    
    public void SwitchMusic(float fadeDuration = 0.01f)
    {
        if (isSwitching) return;
        
        SetNextClip();
        
        //切换前设置新音频源的Clip，并同步播放进度
        nextSource.clip = newClip;
       
        nextSource.time = currentSource.time; //保持同步的播放进度
        nextSource.volume = 0f; //设置音量为0，准备淡入

        nextSource.Play(); //开始播放新音频
        Debug.Log(nextSource.clip);
        StartCoroutine(FadeMusic(fadeDuration)); //开启协程处理淡入淡出
    }
    
    public void Switch()
    {
        isSwitching = false;
        
        Debug.Log("newnewnew");
        
        currentSource.Stop();
        
        nextSource.Play(); 
        audioSourceBuild.volume = 1f;
        //StartCoroutine(FadeMusic(fadeDuration)); //开启协程处理淡入淡出
    }
    
    //淡入淡出
    private System.Collections.IEnumerator FadeMusic(float duration)
    {
        isSwitching = true;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            //float t = timeElapsed / duration;

            currentSource.volume = 0f;//Mathf.Lerp(0.01f, 0f, t);
            nextSource.volume = 1f;//Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        
        currentSource.Stop();
        
        (currentSource, nextSource) = (nextSource, currentSource);

        isSwitching = false;
    }
}
