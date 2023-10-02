using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioEvent
{
    MENU_LOAD,
    BUTTON_CLICK,
    START_GAME,

    PLAYER_MOVE_1,
    PLAYER_MOVE_2,
    PLAYER_MOVE_3,
    PLAYER_MOVE_4,

    Player_NO_Move,
    
    // TODO :Add mroe move sounds
    ENEMY_MOVE,

    PLAYER_LOSE,

    LEVEL_COMPLETE
}

public enum MusicType
{
    MENU,
}

public class AudioManager : MonoBehaviour
{

    public delegate void OnMusicLoaded();
    public OnMusicLoaded onLoad;

    public static AudioManager Instance;

    public bool hasLoaded;

    Dictionary<AudioEvent, AudioClip> sfxClips;
    Dictionary<MusicType, AudioClip> musicClips;

    private AudioSource _audioSFX;
    private AudioSource _audioMusic;

    // TODO: smooth music transitions
    // https://www.youtube.com/watch?v=c3NdUYDyRhE&ab_channel=PawelMakesGames

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        DontDestroyOnLoad(gameObject);

        _audioSFX = gameObject.AddComponent<AudioSource>();
        _audioMusic = gameObject.AddComponent<AudioSource>();
        _audioMusic.volume = 0.7f;
        _audioSFX.volume = 0.7f;

        sfxClips = new Dictionary<AudioEvent, AudioClip>();

        //sfxClips.Add(AudioEvent.BUTTON_HOVER, loadClip("UI/UI_Button_hover_01"));
        //sfxClips.Add(AudioEvent.BUTTON_CLICK, loadClip("UI/UI_Button_click_01"));
        sfxClips.Add(AudioEvent.MENU_LOAD, loadClip("amb"));

        // TODO: Better game start strum
        sfxClips.Add(AudioEvent.START_GAME, loadClip("complete1"));

        sfxClips.Add(AudioEvent.PLAYER_MOVE_1, loadClip("strum1"));
        sfxClips.Add(AudioEvent.PLAYER_MOVE_2, loadClip("strum2"));
        sfxClips.Add(AudioEvent.PLAYER_MOVE_3, loadClip("strum3"));
        sfxClips.Add(AudioEvent.PLAYER_MOVE_4, loadClip("strum4"));
        sfxClips.Add(AudioEvent.PLAYER_LOSE, loadClip("death1"));

        sfxClips.Add(AudioEvent.LEVEL_COMPLETE, loadClip("complete1"));

        sfxClips.Add(AudioEvent.Player_NO_Move, loadClip("blocked1"));

        musicClips = new Dictionary<MusicType, AudioClip>();

        musicClips.Add(MusicType.MENU, loadClip("bgMusic1"));

        Debug.Log("~~ Audio files loaded! ~~");
        hasLoaded = true;

        onLoad?.Invoke();
    }

    public void PlayOneShot(AudioEvent ev)
    {
        AudioClip clip;
        if (!sfxClips.TryGetValue(ev, out clip))
        {
            Debug.Log("Clip not loaded for event: " + ev.ToString());
            return;
        }

        _audioSFX.PlayOneShot(clip);
    }

    public void PlayOneShotForMoveDir(int dir)
    {
        switch (dir)
        {
            case 1:
                PlayOneShot(AudioEvent.PLAYER_MOVE_1);
                break;
            case 2:
                PlayOneShot(AudioEvent.PLAYER_MOVE_2);
                break;
            case 3:
                PlayOneShot(AudioEvent.PLAYER_MOVE_3);
                break;
            default:
                PlayOneShot(AudioEvent.PLAYER_MOVE_4);
                break;
        }
    }

    public void PlayMusic(MusicType music)
    {
        AudioClip clip;
        if (!musicClips.TryGetValue(music, out clip))
        {
            Debug.Log("Clip not loaded for music: " + music.ToString());
            return;
        }

        _audioMusic.clip = clip;
        _audioMusic.loop = true;
        _audioMusic.Play();
    }

    public void StopAll()
    {
        _audioSFX.Stop();
        _audioMusic.Stop();
    }

    private AudioClip loadClip(string name)
    {
        return (AudioClip)Resources.Load("Audio/" + name);
    }
}