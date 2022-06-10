using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    public enum StickStates
    {
        NoDanger,
        Danger,
        Failed
    }
    private StickStates stickState = StickStates.NoDanger;
    [SerializeField] Transform StickTransform;
    [SerializeField] Transform PickupPositions;
    [SerializeField] float RotationMultiplier,weigthDiff;
    bool Failed = false;
    public float dangerTime = 0;
    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.PIZZA_BOX_COLLECTED, RotateStick);
    }
    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.PIZZA_BOX_COLLECTED, RotateStick);
    }
    void RotateStick(EventParam param)
    {
        PlayerContoller playerContoller = param.playerContoller;
        weigthDiff = playerContoller.GetWeightDiff();
        int weight_diff = playerContoller.GetWeightDiff();

        
        Vector3 targetRotation = StickTransform.localRotation.eulerAngles;

        targetRotation = new Vector3(targetRotation.x,Mathf.Clamp( 90 -weight_diff * 4,40,140), targetRotation.z);

        StickTransform.localRotation = Quaternion.Euler(targetRotation);
        
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
    }


    void checkDangerTimer()
    {
        
        dangerTime += Time.deltaTime;
        if (dangerTime >= 2f)
        {
            if(Mathf.Abs(weigthDiff) < 6)
            {
                stickState = StickStates.NoDanger;
                return;
                
            }
            Failed = true;
        }
        
    }

    void StateMachine()
    {
        switch (stickState)
        {
            case StickStates.NoDanger:
                if(Mathf.Abs(weigthDiff) >=8)
                {
                    stickState = StickStates.Danger;
                }
                
                
                break;
            case StickStates.Danger:
                checkDangerTimer();
                if (Failed)
                {
                    EventManager.TriggerEvent(GameConstants.GameEvents.STICK_DANGER_TIMEOUT, new EventParam());
                    stickState = StickStates.Failed;
                }
                break;
            case StickStates.Failed:
                break;
        }
    }
}
