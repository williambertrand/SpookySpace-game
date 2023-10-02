using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuScript : MonoBehaviour
{

    public GameObject achievementBadge;
    public GameObject noAchievementBadge;

    // Start is called before the first frame update
    void Start()
    {

        if(GameConstants.playerDidDie == false)
        {
            achievementBadge.SetActive(true);
            noAchievementBadge.SetActive(false);
        }
    }

    public void OnPlayAgain()
    {
        GameConstants.playerDidDie = false;
        SceneManager.LoadScene(GameConstants.Scenes.Game);
    }

    public void OnMenu()
    {

    }
}
