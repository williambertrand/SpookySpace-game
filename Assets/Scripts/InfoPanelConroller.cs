using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class LevelInfo
{
    public string title;
    public int displayTime;
    public Sprite? Icon;
    string subtitle;
    string cta;

    public LevelInfo(string title, int t)
    {
        this.title = title;
        displayTime = t;
        Icon = null;
    }
}

public class InfoPanelConroller : MonoBehaviour
{

    private Camera cam;

    Dictionary<string, LevelInfo> infoItems;

    [SerializeField] private GameObject InfoPanel;
    [SerializeField] private GameObject InfoPanelIcon;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private Sprite SwitchSprite;
    [SerializeField] private Sprite TPSprite;


    // Start is called before the first frame update
    void Start()
    {
        infoItems = new Dictionary<string, LevelInfo>();

        infoItems.Add(
            "level0",
            new LevelInfo("Use WASD or the arrow keys to move.", 3)
        );

        infoItems.Add(
            "level1",
            new LevelInfo("Enemies move each time you do. Avoid them in the limited space you have to survive.", 4)
        );

        LevelInfo switchInfo = new LevelInfo("Switches only block movement temporarily. Time your moves right to make it past.", 6);
        switchInfo.Icon = SwitchSprite;

        infoItems.Add(
            "switchTut",
           switchInfo
        );

        LevelInfo tpInfo = new LevelInfo("When space is limited, teleporting can save the day.", 3);
        tpInfo.Icon = TPSprite;

        infoItems.Add(
            "teleportTut",
            tpInfo
        );

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleLevelSpawn(Level level)
    {
        if (!infoItems.ContainsKey(level.id)) return;

        LevelInfo info = infoItems[level.id];
        infoText.text = info.title;
        if(info.Icon != null)
        {
            InfoPanelIcon.SetActive(true);
            InfoPanelIcon.GetComponent<Image>().sprite = info.Icon;
        } else
        {
            InfoPanelIcon.SetActive(false);
        }
        ShowInfoPanel(info.displayTime);
    }

    private void ShowInfoPanel(int sec)
    {
        Vector3 infoShownPos = new Vector3(0, -166f, 0);
        Vector3 infoHiddenPos = new Vector3(0, -460f, 0);

        rectTransform.transform.localPosition = new Vector3(0f, -150f, 0f);
        rectTransform
            .DOAnchorPos(new Vector2(0f, 0f), 1.0f, false)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                rectTransform
                    .DOAnchorPos(new Vector2(0f, -150f), 0.5f, false)
                    .SetDelay(sec)
                    .SetEase(Ease.InSine);
            });
    }
}
