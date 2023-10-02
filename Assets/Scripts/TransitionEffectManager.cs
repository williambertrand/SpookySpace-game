using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransitionEffectManager : MonoBehaviour
{

    [SerializeField] private GameObject cloudLeft;
    [SerializeField] private GameObject cloudLeft2;
    [SerializeField] private GameObject cloudRight;
    [SerializeField] private GameObject cloudRight2;
    [SerializeField] private GameObject moon;

    private Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        //StartCoroutine(CloudsInAfter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CloudsInAfter()
    {
        yield return new WaitForSeconds(1.0f);
        CloudsTranstion();
    }

    public void CloudsTranstion()
    {
        cloudLeft.transform
            .DOMove(new Vector3(cam.transform.position.x - 1.5f, 2.5f, 0), 1.0f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => {
                cloudLeft.transform.DOMove(new Vector3(-15, 2.5f, 0), 0.75f).SetEase(Ease.InOutSine).SetDelay(1.0f);
            });

        cloudLeft2.transform
            .DOMove(new Vector3(cam.transform.position.x - 1.5f, 0.75f, 0), 1.2f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => {
                cloudLeft2.transform.DOMove(new Vector3(-15, 0.5f, 0), 0.75f).SetEase(Ease.InOutSine).SetDelay(0.8f);
            });

        cloudRight.transform.DOMove(new Vector3(cam.transform.position.x + 1.5f, 4.5f, 0), 1.0f).SetEase(Ease.InOutSine)
            .OnComplete(() => {
                cloudRight.transform.DOMove(new Vector3(15, 4.5f, 0), 0.75f).SetEase(Ease.InOutSine).SetDelay(1.0f);
            });

        cloudRight2.transform.DOMove(new Vector3(cam.transform.position.x + 1.5f, 0f, 0), 1.0f).SetEase(Ease.InOutSine)
            .OnComplete(() => {
                cloudRight2.transform.DOMove(new Vector3(15, 0f, 0), 0.75f).SetEase(Ease.InOutSine).SetDelay(1.0f);
            });

        moon.transform.DOMove(new Vector3(cam.transform.position.x - 1.75f, 4.5f, 0), 1.0f).SetEase(Ease.InOutSine)
            .OnComplete(() => {
                moon.transform.DOMove(new Vector3(cam.transform.position.x - 1.5f, 10f, 0), 0.75f).SetEase(Ease.InOutSine).SetDelay(1.0f);
            });
    }

    public void CloudsOut()
    {
        cloudLeft.transform.DOMove(new Vector3(-15, 0.5f, 0), 0.75f).SetEase(Ease.InOutSine);
        cloudRight.transform.DOMove(new Vector3(15, 3.5f, 0), 0.75f).SetEase(Ease.InOutSine);
    }
}
