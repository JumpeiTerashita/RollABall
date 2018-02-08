using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [Resources/Sounds]フォルダにあるすべての音源アセットを管理します。
/// </summary>
public class SoundManager : MonoBehaviour
{

    #region Singleton
    /// <summary>
    /// Singleton instance.
    /// </summary>
    private static SoundManager instance;

    /// <summary>
    /// [instance] property.
    /// </summary>
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                var ob = new GameObject();
                DontDestroyOnLoad(ob);
                ob.name = "SoundManager";
                ob.AddComponent<AudioSource>();
                instance = ob.AddComponent<SoundManager>();
                //instance.Init();
                return instance;
            }
            else
            {
                return instance;
            }
        }
    }
    #endregion

    private Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, GameObject> seObjectDictionary = new Dictionary<string, GameObject>();
    private bool isInit = false;

    /// <summary>
    /// Play bgm.
    /// </summary>
    /// <param name="bgmname">Bgm name.(Asset name)</param>
    /// <param name="loop">True = Loop bgm.</param>
    /// <param name="volume">Bgm volume.</param>
    public void PlayBgm(string bgmname, bool loop, float volume)
    {
        if (!clipDictionary.ContainsKey(bgmname)) return;
        var source = gameObject.GetComponent<AudioSource>();
        if (source.isPlaying)
        {
            StopBgm();
        }

        source.clip = clipDictionary[bgmname];
        source.loop = loop;
        source.volume = volume;
        source.Play();
    }

    /// <summary>
    /// Play se.
    /// </summary>
    /// <param name="sename">Se name.(Asset name)</param>
    /// <param name="loop">True = Loop se.</param>
    /// <param name="pos">Se sound source point.</param>
    /// <param name="volume">Se volume.</param>
    public void PlaySe(string sename, bool loop, Vector3 pos, float volume = 1.0f)
    {
        if (!clipDictionary.ContainsKey(sename)) return;

        if (!seObjectDictionary.ContainsKey(sename))
        {
            seObjectDictionary[sename] = new GameObject();
            seObjectDictionary[sename].name = "SE : "+sename;
            DontDestroyOnLoad(seObjectDictionary[sename]); 
            seObjectDictionary[sename].AddComponent<AudioSource>();
        }

        var source = seObjectDictionary[sename].GetComponent<AudioSource>();
        source.gameObject.transform.position = pos;
        if (!source.loop)
        {
            source.loop = loop;
            source.clip = clipDictionary[sename];
            source.volume = volume;
            source.Play();
        }
    }

    /// <summary>
    /// Stop bgm.
    /// </summary>
    public void StopBgm()
    {
        var source = gameObject.GetComponent<AudioSource>();
        if (source.isPlaying)
        {
            source.Stop();
        }
    }

    /// <summary>
    /// Stop se.
    /// </summary>
    /// <param name="sename">Se name.</param>
    public void StopSe(string sename)
    {
        if (seObjectDictionary.ContainsKey(sename))
        {
            var source = seObjectDictionary[sename].GetComponent<AudioSource>();
            source.loop = false;
            source.Stop();
        }
    }

    /// <summary>
    /// Initialize all assets in [Resources/Sounds] to SoundClip.
    /// </summary>
    public void Init()
    {
        if (isInit)
        {
            return;
        }

        AudioClip[] ob = Resources.LoadAll<AudioClip>("Sounds/");
        foreach (var clip in ob)
        {
            clipDictionary.Add(clip.name, clip);
        }
        isInit = true;
    }
}