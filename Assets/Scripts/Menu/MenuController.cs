using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(MusicType.MENU);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPressPlay()
    {
        AudioManager.Instance.StopAll();
        AudioManager.Instance.PlayOneShot(AudioEvent.START_GAME);
        SceneManager.LoadScene(GameConstants.Scenes.Game);
    }
}
