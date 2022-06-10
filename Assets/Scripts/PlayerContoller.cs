using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;

public class PlayerContoller : MonoBehaviour
{
    public enum PlayerStates
    {
        Idle,
        Moving,
        Fail,
        Success
    }

    [SerializeField] private PlayerStates PlayerState = PlayerStates.Idle;
    [SerializeField] private float ForwardMovementSpeed;
    [SerializeField]
    Transform LeftStickPosition,
        RightStickPosition;
    
    private bool canMove,HittedToPizzaBox,ReachedFinish,stickDanger = false;
    [SerializeField] Animator PlayerAnimator;

    [SerializeField] private Rigidbody[] ragdollRBs;
    private Collider[] ragdollColliders;
    [SerializeField] GameObject PlayerModel;
    private int LeftWeight, RightWeight, WeightDiffirence;
    [SerializeField] private Transform StickLeftTransform, StickRightTransform;
    [SerializeField] TextMeshPro LeftWeightText, RightWeightText;
    [SerializeField] Transform Stick;
    
    List<PizzaBoxes> PizzaBoxes = new List<PizzaBoxes>();
    
    
    private void OnEnable()
    {
        
        EventManager.StartListening(GameConstants.GameEvents.SEND_PIZZABOX_TO_PLAYER, PizzaBoxPickedUp);
        EventManager.StartListening(GameConstants.GameEvents.GAME_STARTED, StartMoving);
        EventManager.StartListening(GameConstants.GameEvents.HITTED_TO_PIZZABOX,PizzaBoxHitted);
        EventManager.StartListening(GameConstants.GameEvents.STICK_DANGER_TIMEOUT,PizzaBoxHitted);
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.REACHED_FINISH,ReachedFinishPoint);
      

    }
    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.SEND_PIZZABOX_TO_PLAYER, PizzaBoxPickedUp);
        EventManager.StopListening(GameConstants.GameEvents.GAME_STARTED, StartMoving);
        EventManager.StopListening(GameConstants.GameEvents.HITTED_TO_PIZZABOX, PizzaBoxHitted);
        EventManager.StopListening(GameConstants.GameEvents.STICK_DANGER_TIMEOUT, PizzaBoxHitted);
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.REACHED_FINISH, ReachedFinishPoint);
    }

    void PizzaBoxPickedUp(EventParam param)
    {
        
        SendPizzaBoxToStick(param.PizzaBox, param);
        
    }


    void SendPizzaBoxToStick(PizzaBoxes pizza_box,EventParam param)
    {
        pizza_box.DisableCollider();
        if (param.pickupDirection == EventParam.PickupDirection.Left)
        {
            LeftWeight += pizza_box.GetWeight();
            
            if (LeftWeight < 0)
            {
                LeftWeight = 0;
            }
            
            SetPizzaBox(pizza_box, PizzaBoxes, LeftWeight, LeftWeightText, StickLeftTransform);
            
        }
        if (param.pickupDirection == EventParam.PickupDirection.Right)
        {
            
            RightWeight += pizza_box.GetWeight();
            if (RightWeight < 0)
            {
                RightWeight = 0;
            }
            
            SetPizzaBox(pizza_box, PizzaBoxes, RightWeight, RightWeightText, StickRightTransform);

        }
        WeightDiffirence = RightWeight - LeftWeight;

        EventParam new_param = new EventParam();
        new_param.playerContoller = this;
        
        EventManager.TriggerEvent(GameConstants.GameEvents.PIZZA_BOX_COLLECTED, new_param);
        
    }

    void ReachedFinishPoint(EventParam param)
    {
        ReachedFinish = true;
        PlayerModel.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        
        ForwardMovementSpeed = 0;
        PlayerAnimator.SetTrigger("Dance");
       
    }
 
    public int GetWeightDiff()
    {
        return WeightDiffirence;
    }

   
    void SetPizzaBox(PizzaBoxes pizza_box_to_send, List<PizzaBoxes> pizza_box_list,int weight, TextMeshPro weight_text,Transform StickDirectionTransform)
    {
        
        weight_text.text = weight.ToString();
        

        pizza_box_to_send.transform.position = StickDirectionTransform.position;
       
        pizza_box_list.Add(pizza_box_to_send);
        
        if (pizza_box_to_send.GetWeight() < 0)
        {
            
            for (int i = 0; i < MathF.Abs(pizza_box_to_send.GetWeight()); i++)
            {
                
                if (StickDirectionTransform.childCount >= 1)
                {
                    StickDirectionTransform.GetChild(StickDirectionTransform.childCount - 1).parent = null;
                }
                
                
               
            }
        }
        else
        {
            StartCoroutine(pizza_box_to_send.SendModelsToDestinationCRT(StickDirectionTransform));
        }
        
        
        
    }

    void PizzaBoxHitted(EventParam param)
    {
        ToggleRagdoll(true);
        canMove = false;
        HittedToPizzaBox = true;
        Stick.transform.parent = transform.parent;
        
    }

 
    void StartMoving(EventParam param)
    {
        canMove = true;
        PlayerAnimator.SetTrigger("Move");

    }

    void Start()
    {
        ragdollRBs = PlayerModel.GetComponentsInChildren<Rigidbody>();
        ragdollColliders = PlayerModel.GetComponentsInChildren<Collider>();
        ToggleRagdoll(false);
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
    }

    void StateMachine()
    {
        switch (PlayerState)
        {
            case PlayerStates.Idle:
                if (canMove)
                {
                    PlayerState = PlayerStates.Moving;
                }
                    
                break;
            case PlayerStates.Moving:
                PlayerMovement();
                if (HittedToPizzaBox || stickDanger)
                {
                    PlayerState = PlayerStates.Fail;
                }
                if (ReachedFinish)
                {
                    PlayerState = PlayerStates.Success;
                }
                break;
            case PlayerStates.Fail:
                EventManager.TriggerEvent(GameConstants.LEVEL_EVENTS.LEVEL_FAILED,new EventParam());
                foreach(PizzaBoxes p_b in PizzaBoxes)
                {
                    p_b.ActivateModelsGravity();
                }
                break;
            case PlayerStates.Success:
                EventManager.TriggerEvent(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, new EventParam());
                break;
            
        }
    }

    

    void PlayerMovement()
    {
        transform.Translate(transform.forward * ForwardMovementSpeed * Time.deltaTime);
    }

    void ToggleRagdoll(bool state)
    {
        
        PlayerAnimator.enabled = !state;
        foreach (Rigidbody rb in ragdollRBs)
        {
            rb.mass = 1;
            rb.isKinematic = !state;
        }
       
        foreach (Collider col in ragdollColliders)
            if (GetComponent<Collider>() != col)
            {
                
                col.isTrigger = !state;
                
            }

    }

    public int GetLeftWeight()
    {
        return LeftWeight;

    }
    public int GetRightWeight()
    {
        return RightWeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            EventManager.TriggerEvent(GameConstants.LEVEL_EVENTS.REACHED_FINISH, new EventParam());
        }
    }
}
