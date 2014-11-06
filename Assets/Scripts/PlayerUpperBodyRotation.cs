using UnityEngine;
using System.Collections;

public class PlayerUpperBodyRotation : MonoBehaviour
{
    public Transform playerCameraPivot;

    Transform PlayerBone;
    Transform UpperBodyBone;
    Transform HeadBone;
    Transform LeftArmBone;
    Transform RightArmBone;

    Vector2 camRotation = Vector2.zero;
    float cameraAngleX;  
    
    Transform targetObj;

    RotationType rotType;

    public enum RotationType
    {
        Camera = 0,
        Target
    }

    void Start () {
        string playerBoneName = "BaseModel/PlayerAni/chr_ground_point/Bip01";
        string upperBodyName = "Bip01 Spine/Bip01 Spine1/";
        string headName = "Bip01 Spine/Bip01 Spine1/Bip01 Head";
        string leftArmName = "Bip01 Spine/Bip01 Spine1/Bip01 L Clavicle";
        string rightArmName = "Bip01 Spine/Bip01 Spine1/Bip01 R Clavicle";
        string targetObjName = "BaseModel/Camera/TargetObj";

        PlayerBone = GameObject.Find( playerBoneName ).transform;
        UpperBodyBone = PlayerBone.FindChild(upperBodyName);
        HeadBone = PlayerBone.FindChild(headName);
        LeftArmBone = PlayerBone.FindChild(leftArmName);
        RightArmBone = PlayerBone.FindChild(rightArmName);

        targetObj = GameObject.Find( targetObjName ).transform;

        rotType = RotationType.Target;
	}
	
	void Update () {
        
	}

    void LateUpdate ()
    {
        MouseInput();
        
        if (Input.GetMouseButton(0))
        {
            playerCameraRotation(touchRotation);
        }

        switch (rotType)
        {
            case RotationType.Camera:
                UseCameraUpperRotation(playerCameraPivot);
                break;
            case RotationType.Target:
                UseTargetUpperRotation(targetObj);
                break;
        }        
    }

    void UseCameraUpperRotation(Transform _cameraPivot)
    {
        float afterCameraAngleX = _cameraPivot.rotation.eulerAngles.x;
        cameraAngleX = Mathf.LerpAngle(cameraAngleX, afterCameraAngleX, Time.deltaTime * 10);

        UpperBodyBone.localRotation = Quaternion.AngleAxis(-cameraAngleX, Vector3.forward) * UpperBodyBone.localRotation;
        //LeftArmBone.localRotation = Quaternion.AngleAxis(-cameraAngleX, Vector3.forward) * LeftArmBone.localRotation;
        //RightArmBone.localRotation = Quaternion.AngleAxis(-cameraAngleX, Vector3.forward) * RightArmBone.localRotation;
        //HeadBone.localRotation = Quaternion.AngleAxis(-cameraAngleX, Vector3.forward) * HeadBone.localRotation;
    }

    void UseTargetUpperRotation(Transform _tTargetObj)
    {
        Quaternion q = Quaternion.FromToRotation(Vector3.forward, _tTargetObj.position - UpperBodyBone.position);
        UpperBodyBone.rotation = q * UpperBodyBone.rotation;     
    }

    void playerCameraRotation(Vector2 _touchRotation)
    {
        Quaternion tempRotation = playerCameraPivot.rotation * Quaternion.Euler(-_touchRotation.y, 0, 0);

        if (tempRotation.eulerAngles.x <= 30 || tempRotation.eulerAngles.x >= 315)
        {
            _touchRotation.x = 0;
            playerCameraPivot.Rotate(-_touchRotation.y, _touchRotation.x, 0);
            playerCameraPivot.localRotation = new Quaternion(playerCameraPivot.localRotation.x, playerCameraPivot.localRotation.y, 0, 1);
        }
    }
    
    Vector2 beganTouchPos;
    Vector2 nowTouchPos;
    Vector2 touchRotation;

    void TouchInput()
    {
        int touchCount = Input.touchCount;

        Vector2 flickingPos = Vector2.zero;
       
        for(int i = 0 ; i < touchCount ; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                beganTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                nowTouchPos = touch.position;
                touchRotation = new Vector2(GetTouchAngle(nowTouchPos.x, beganTouchPos.x, Screen.width), GetTouchAngle(nowTouchPos.y, beganTouchPos.y, Screen.height));
                //touchRotation.x = touchRotation.x * ROTATIONSPEED;
                //touchRotation.y = touchRotation.y * ROTATIONSPEED;
            }
        }	     
    }

    void MouseInput()
    {
        int LeftButton = 0;     
       
        Vector2 flickingPos = Vector2.zero;

        if (Input.GetMouseButtonDown(LeftButton))
        {
            beganTouchPos = Input.mousePosition;         
        }
        else if (Input.GetMouseButton(LeftButton))
        {            
            if (Vector2.Distance(beganTouchPos, Input.mousePosition) >= 20)         
                beganTouchPos = Input.mousePosition;

            nowTouchPos = Input.mousePosition;
            
            touchRotation = new Vector2(GetTouchAngle(nowTouchPos.x, beganTouchPos.x, Screen.width), GetTouchAngle(nowTouchPos.y, beganTouchPos.y, Screen.height));
            //touchRotation.x = touchRotation.x * ROTATIONSPEED;
            //touchRotation.y = touchRotation.y * ROTATIONSPEED;
        }
        else if (Input.GetMouseButtonUp(LeftButton))
        {
            beganTouchPos = Input.mousePosition;
        } 
    }

    private float sensitivity = 40;
    public float zoomRatio = 1f;
    
    float GetTouchAngle(float x1, float x2, int distance)
    {
        sensitivity = 40;

	    if(distance == Screen.width){
		    sensitivity *= 5;
	    }

        sensitivity *= zoomRatio;
	  
	    return ( ( x1 - x2 ) / ( distance / sensitivity ) );
    }

    void OnGUI()
    {
        Debug.DrawLine(UpperBodyBone.position, targetObj.position, Color.red);
    }


    //float nPlayerRotationSpeed = 0;
    float ROTATIONSPEED = 1.4f;
}
