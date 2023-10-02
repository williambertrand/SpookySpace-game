using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    [SerializeField] private GameObject camera;
    [SerializeField] private float effect;
    private float startPos;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float dist = (camera.transform.position.x * effect);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
