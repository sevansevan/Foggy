using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float ForwardSpeed = 0.1f;
    public float RotationSpeed = 0.01f;

    public GameObject Trail;
    public int TrailEveryN = 70;
    private int FrameCounter = 0;
    

    [HideInInspector] public Cloud currentCloud;
    [HideInInspector] public CloudType currentCloudType;
    [HideInInspector] public float CloudRotation;
    // Start is called before the first frame update


    void Start()
    {
        currentCloudType = CloudType.NoEffect;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        this.transform.position += transform.forward * ForwardSpeed;



      //  this.transform.Rotate(0, RotationSpeed * 50f * Input.GetAxis("Horizontal"), 0);
    
        if (currentCloudType == CloudType.LocalLeft) CloudRotation = 0.2f;
        if (currentCloudType == CloudType.LocalRight) CloudRotation = -0.2f;
        if (currentCloudType == CloudType.LastEffect) {} ;
        if (currentCloudType == CloudType.NoEffect) CloudRotation = 0f;

        float ActiveRotation = RotationSpeed * Input.GetAxis("Horizontal") + CloudRotation;
        Debug.Log(ActiveRotation);
        this.transform.Rotate(0, ActiveRotation, 0);

//        GameObject Vortex = GameObject.Find("Vortex");

//        Vector2 VortexDirection;
        RaycastDown();
        DoTrail();

    }
    void RaycastDown(){
        RaycastHit RayHit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down) * 1000, out RayHit)){
          //  Destroy(RayHit.transform.gameObject);
            currentCloud = RayHit.transform.gameObject.GetComponent<Cloud>();
            if (currentCloud) currentCloudType = currentCloud.cloudType;

            // Debug.Log("Did hit");
        } else {
            // currentCloudType = CloudType.NoEffect;  
        }
    }
    void DoTrail(){
        FrameCounter += 1;
        if (FrameCounter >= TrailEveryN){
            Instantiate(Trail, this.transform.position, Quaternion.identity);
            FrameCounter -= TrailEveryN;
        }

    }
}
