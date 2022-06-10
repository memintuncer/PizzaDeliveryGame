using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public enum SwipeStates
    {
        Idle,
        SwipeLeft,
        SwipeRight,
        SwipeFinished
    }
    [SerializeField] bool CanSwipe;
    InputManager inputManager;
    [SerializeField] SwipeStates SwipeState = SwipeStates.Idle;
    // Start is called before the first frame update
    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.GAME_STARTED, StartSwiping);
        EventManager.StartListening(GameConstants.GameEvents.FINISHED_PIZZA_BOXES, FinishSwipeCheck);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.GAME_STARTED, StartSwiping);
        EventManager.StopListening(GameConstants.GameEvents.FINISHED_PIZZA_BOXES, FinishSwipeCheck);
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    void StartSwiping(EventParam param)
    {
        CanSwipe = true;
    }
     void FinishSwipeCheck(EventParam param)
    {
        SwipeState = SwipeStates.SwipeFinished;
    }

    void StateMachine()
    {
        EventParam direction_param = new EventParam();
        switch (SwipeState)
        {
            case SwipeStates.Idle:
                if (CanSwipe)
                {
                    CheckSwipeDirection();
                }
                break;
            case SwipeStates.SwipeRight:
                
                direction_param.pickupDirection = EventParam.PickupDirection.Right;
                EventManager.TriggerEvent(GameConstants.GameEvents.SWIPE_RIGHT, direction_param);
                
                SwipeState = SwipeStates.Idle;
                break;
            case SwipeStates.SwipeLeft:
                
                direction_param.pickupDirection = EventParam.PickupDirection.Left;
                EventManager.TriggerEvent(GameConstants.GameEvents.SWIPE_LEFT, direction_param);
                SwipeState = SwipeStates.Idle;
                break;
            case SwipeStates.SwipeFinished:
                break;

        }
    }


    void CheckSwipeDirection()
    {
        if (inputManager.isBeginDrag)
        {
            if (inputManager.deltaPos.x < -0.5f)
            {
                SwipeState = SwipeStates.SwipeLeft;
            } 
            if (inputManager.deltaPos.x >0.5f)
            {
                SwipeState = SwipeStates.SwipeRight;
            }
        }
    }

   

    // Update is called once per frame
    void Update()
    {
        StateMachine();
    }
}
