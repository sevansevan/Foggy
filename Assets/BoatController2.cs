using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController2 : MonoBehaviour
{
    private float ForwardSpeed = 0.0f;
    public float ForwardSpeedBase = 0.3f;
    public float BackwardSpeed = 0.0f;
    public float Speed = 0.0f;
    public float RotationSpeed = 0.01f;

    public float pitch = 0;
    public float yaw = 0;

    public GameObject Trail;
    public Transform CameraTransform;
    public int TrailEveryN = 70;
    private int FrameCounter = 0;
    
    public float MaxFallSpeed = 0.2f;

    public Vector3 PilotOffset;
    public Vector3 LastGroundPosition;
    public float LastGroundYaw;
    [HideInInspector] public Cloud currentCloud;
    [HideInInspector] public CloudType currentCloudType;
    [HideInInspector] public float CloudRotation;
    // Start is called before the first frame update
    public GameObject TextFound;
    public GameObject TextNotFound;
    private int DigFrameElapsed;
    private int BoostFrameElapsed = 100000;


    void Start()
    {
        currentCloudType = CloudType.NoEffect;
        TextFound.SetActive(false);
        TextNotFound.SetActive(false);
        ForwardSpeed = ForwardSpeedBase;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Speed = Input.GetAxis("Vertical");
        Speed = (Speed > 0) ? Speed * ForwardSpeed : Speed * BackwardSpeed;  
        this.transform.position += transform.forward * Speed;

      //  this.transform.Rotate(0, RotationSpeed * 50f * Input.GetAxis("Horizontal"), 0);
    
        if (currentCloudType == CloudType.LocalLeft) CloudRotation = 0.2f;
        if (currentCloudType == CloudType.LocalRight) CloudRotation = -0.2f;
        if (currentCloudType == CloudType.LastEffect) {} ;
        if (currentCloudType == CloudType.NoEffect) CloudRotation = 0f;

        yaw += RotationSpeed * Input.GetAxis("Horizontal2");

        pitch += -Input.GetAxis("Vertical2");
        pitch = Mathf.Clamp(pitch, -30f, 13f);
 
        //CameraTransform.rotation = 


        //CameraTransform.eulerAngles = new Vector3(Mathf.Clamp(transform.eulerAngles.x, -45 , 45), CameraTransform.eulerAngles.y, CameraTransform.eulerAngles.z);

//        GameObject Vortex = GameObject.Find("Vortex");

//        Vector2 VortexDirection;
        RaycastDown();

        if (currentCloudType == CloudType.Lava) {
            this.transform.position = LastGroundPosition;
            yaw = LastGroundYaw;
        }


        //DoTrail();

        this.transform.rotation = Quaternion.Euler(0.0f, yaw, 0.0f);
        CameraTransform.rotation = Quaternion.Euler(pitch, yaw, 0.0f);

        CheckTreasure();
        Boost();


    }
    void RaycastDown(){
        RaycastHit RayHit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down) * 1000, out RayHit)){
          //  Destroy(RayHit.transform.gameObject);
            currentCloud = RayHit.transform.gameObject.GetComponent<Cloud>();
            if (currentCloud) {
                currentCloudType = currentCloud.cloudType;
            }
            else
            {
                currentCloudType = CloudType.NoEffect;
                }

            Vector3 NewPosition;
            NewPosition.x = this.transform.position.x;
            Transform GroundTransform = RayHit.transform.gameObject.transform;

            NewPosition.y = GroundTransform.position.y + GroundTransform.localScale.y /2.0f;
            NewPosition.z = this.transform.position.z;
            NewPosition += PilotOffset;

            NewPosition.y = Mathf.Max(this.transform.position.y - MaxFallSpeed, NewPosition.y);

            if (currentCloudType == CloudType.Ground){
                LastGroundPosition.x = GroundTransform.position.x;
                LastGroundPosition.y = NewPosition.y + 15.0f;
                LastGroundPosition.z = GroundTransform.position.z;
                LastGroundYaw = yaw;
            }

            if (currentCloudType == CloudType.Elevator){
                NewPosition.y = GroundTransform.position.y + GroundTransform.localScale.y /2.0f + currentCloud.ElevatorStrength;
                NewPosition.y = Mathf.Min(this.transform.position.y + MaxFallSpeed, NewPosition.y);
            };

            this.transform.position = NewPosition;
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

    void CheckTreasure(){
                DigFrameElapsed += 1;
        //if (currentCloudType == CloudType.Treasure)
            if(Input.GetButtonDown("Submit")){
                DigFrameElapsed = 0;
            //TextFound.SetActive()
                if (currentCloudType == CloudType.Treasure){
                TextFound.SetActive(true);
                TextNotFound.SetActive(false);
                }
                else
                {
                TextNotFound.SetActive(true);
                TextFound.SetActive(false);
                }
            }
            if (DigFrameElapsed > 120){
                TextNotFound.SetActive(false);
                TextFound.SetActive(false);
            };
    }

    void Boost(){
            BoostFrameElapsed += 1;
        //if (currentCloudType == CloudType.Treasure)
            if(Input.GetAxis("Fire1") == 1 && BoostFrameElapsed > 120){
                BoostFrameElapsed = 0;
            //TextFound.SetActive()
                ForwardSpeed = ForwardSpeedBase * 5f;
            };

            if (BoostFrameElapsed > 60) ForwardSpeed = ForwardSpeedBase ;


    }

}
