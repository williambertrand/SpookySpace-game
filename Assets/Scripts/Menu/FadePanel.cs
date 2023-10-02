using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // TO fade in scene, just add a black panel that fades out and destroys itself
    public void FadeOut()
    {
        panel.DOFade(0.0f, 0.75f);
    }
}
