using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("AudioManager.instance is null. Attempting to locate AudioManager in the scene.");
                _instance = FindObjectOfType<AudioManager>();
                if(!_instance)
                {
                    Debug.Log("Unable to locate AudioManager in the scene. Creating a new AudioManager.");
                    var go = (GameObject)Instantiate(Resources.Load("AudioManager"), Vector3.zero, Quaternion.identity);
                    _instance = go.GetComponent<AudioManager>();
                }

                Debug.Log("AudioManager is " + _instance.transform.name);
            }
            return _instance;
        }
        protected set
        {
            _instance = value;
        }
    }

    public AudioMixer mixer;
    public GameObject audioSourcePrefab;

    public string AmbianceSoundInfoID, MusicSoundInfoID;

    private Dictionary<string, SoundInfo> sounds = new Dictionary<string, SoundInfo>();
    private List<AudioSource> audioSources = new List<AudioSource>();

    public float GlobalVolume { get; protected set; }
    public float MusicVolume { get; protected set; }
    public float SFXVolume { get; protected set; }
    public float EnvironmentVolume { get; protected set; }

    private const int NUMBER_OF_AUDIO_SOURCES = 16;

    public void RegisterSoundInfo(SoundInfo soundInfo)
    {
        sounds[soundInfo.id] = soundInfo;
    }

    public AudioSource PlaySound(string id) {
        return Play(id);
    }

    public AudioSource Play2D(string id, bool loop = false, float pitchMin = 1.0f, float pitchMax = 1.0f, float volumeMin = 1.0f, float volumeMax = 1.0f, bool isMusic = false, Vector3? position = null, float minDistance = 0.01f, float maxDistance = 20.0f)
    {
        if (position != null)
        {
            position = new(Camera.main.transform.position.x, position.Value.y, Camera.main.transform.position.z);
        }
        return Play(id, loop, pitchMin, pitchMax, volumeMin, volumeMax, isMusic, position, minDistance, maxDistance);
    }

    public AudioSource Play(string id, bool loop = false, float pitchMin = 1.0f, float pitchMax = 1.0f, float volumeMin = 1.0f, float volumeMax = 1.0f, bool isMusic = false, Vector3? position = null, float minDistance = 1, float maxDistance = 1000)
    {
        AudioSource audioSource = null;
        for (int i = 0; i < NUMBER_OF_AUDIO_SOURCES; i++)
        {
            if(!audioSources[i].isPlaying)
            {
                audioSource = audioSources[i];
                break;
            }
        }

        if(audioSource == null)
        {
            Debug.Log("Unable to play " + id + " as there are no audio sources available.");
            return audioSource;
        }

        if(!sounds.ContainsKey(id))
        {
            Debug.Log("Unable to play " + id + " as there is no sound info associated with the id.");
            return audioSource;
        }

        SoundInfo soundInfo = sounds[id];
        if(soundInfo.audioClips.Length <= 0)
        {
            Debug.Log("Unable to play " + id + " as there are no clips associated with the id.");
            return audioSource;
        }

        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.volume = Random.Range(volumeMin, volumeMax);
        audioSource.clip = soundInfo.audioClips[Random.Range(0, soundInfo.audioClips.Length)];
        audioSource.outputAudioMixerGroup = soundInfo.audioMixerGroup;
        audioSource.loop = loop;

        if(position != null)
        {
            audioSource.transform.position = position.Value;
            audioSource.spatialBlend = 1.0f;
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
        }
        else
        {
            audioSource.transform.localPosition = Vector3.zero;
            audioSource.spatialBlend = 0.0f;
        }

        audioSource.Play();

        if (isMusic)
        {
            _musicSource = audioSource;
        }

        return audioSource;
    }

    AudioSource _musicSource;

    public void StopMusic()
    {
        if(!_musicSource)
        {
            return;
        }
        _musicSource.Stop();
    }

    void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Debug.LogWarning("Only one audio manager can exist. Destroying one instance.");
            Destroy(this.gameObject);
            return;
        }
        _instance = this;

        GameObject audioSourcesGO = new GameObject();
        audioSourcesGO.transform.name = "Audio Source Pool";
        audioSourcesGO.transform.parent = transform;
        for (int i = 0; i < NUMBER_OF_AUDIO_SOURCES; i++)
        {
            var go = Instantiate(audioSourcePrefab);
            go.transform.parent = audioSourcesGO.transform;
            go.transform.name = "Audio Source " + i;
            var audioSource = go.GetComponent<AudioSource>();
            audioSources.Add(audioSource);
        }
    }

    private void Start()
    {
        InitializeVolumes();
        Play(AmbianceSoundInfoID, true, volumeMin: 0.25f, volumeMax: 0.25f);
        Play(MusicSoundInfoID, true, volumeMin: 0.5f, volumeMax: 0.5f, isMusic: true);
    }

    float GetVolumeFromPrefs(string volumeKey)
    {
        if (!PlayerPrefs.HasKey(volumeKey)) {
            PlayerPrefs.SetFloat(volumeKey, 1.0f);
            PlayerPrefs.Save();
            Debug.Log(volumeKey + " = 1.0f");
            return 1.0f;
        } else
        {
            Debug.Log(volumeKey + " = " + PlayerPrefs.GetFloat(volumeKey));
            return PlayerPrefs.GetFloat(volumeKey);
        }
    }

    void InitializeVolumes()
    {
        SetGlobalVol(GetVolumeFromPrefs("globalVol"));
        SetMusicVol(GetVolumeFromPrefs("musicVol"));
        SetSFXVol(GetVolumeFromPrefs("sfxVol"));
        SetEnvironmentVol(GetVolumeFromPrefs("environmentVol"));
    }

    public void SetGlobalVol(float volume)
    {
        GlobalVolume = volume;
        PlayerPrefs.SetFloat("globalVol", GlobalVolume);
        PlayerPrefs.Save();

        mixer.SetFloat("globalVol", Mathf.Log10(GlobalVolume) * 20);
        Debug.Log("Set global volume = " + volume);
    }

    public void SetMusicVol(float volume)
    {
        MusicVolume = volume;
        PlayerPrefs.SetFloat("musicVol", MusicVolume);
        PlayerPrefs.Save();

        mixer.SetFloat("musicVol", Mathf.Log10(MusicVolume) * 20);
        Debug.Log("Set music volume = " + volume);
    }

    public void SetSFXVol(float volume)
    {
        SFXVolume = volume;
        PlayerPrefs.SetFloat("sfxVol", SFXVolume);
        PlayerPrefs.Save();

        mixer.SetFloat("sfxVol", Mathf.Log10(SFXVolume) * 20);
        Debug.Log("Set sfx volume = " + volume);
    }

    public void SetEnvironmentVol(float volume)
    {
        EnvironmentVolume = volume;
        PlayerPrefs.SetFloat("environmentVol", EnvironmentVolume);
        PlayerPrefs.Save();

        mixer.SetFloat("environmentVol", Mathf.Log10(EnvironmentVolume) * 20);
        Debug.Log("Set environment volume = " + volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
