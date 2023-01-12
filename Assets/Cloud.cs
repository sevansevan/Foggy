using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CloudType {LocalLeft, LocalRight, NoEffect, LastEffect, Fast, Elevator, Lava, Ground, Treasure, COUNT}

public class Cloud : MonoBehaviour
{
    public CloudType cloudType;
    public float ElevatorStrength = 20.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
