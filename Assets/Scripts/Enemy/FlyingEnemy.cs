using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : BaseEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override Point PickNextMove()
    {
        int dir = Random.Range(0, 3);

        return new Point(0,0);
    }
}
