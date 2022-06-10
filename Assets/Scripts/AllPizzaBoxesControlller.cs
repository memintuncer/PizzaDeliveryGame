using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPizzaBoxesControlller : MonoBehaviour
{

    

    [SerializeField] List<PizzaBoxes> AllPizzaBoxesList = new List<PizzaBoxes>();
    int currenPizzaBoxIndex =0,currentWeightValue;
    [SerializeField] int TotalPizzaBoxCount ;
    [SerializeField] PizzaBoxes PizzaBoxPrefab;
    [SerializeField] PlayerContoller playerContoller;

    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.SWIPE_LEFT,SendPizzaBoxToPlayer);
        EventManager.StartListening(GameConstants.GameEvents.SWIPE_RIGHT,SendPizzaBoxToPlayer);
        EventManager.StartListening(GameConstants.GameEvents.PIZZA_BOX_COLLECTED, ActivateNextPizzaBox);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.SWIPE_LEFT, SendPizzaBoxToPlayer);
        EventManager.StopListening(GameConstants.GameEvents.SWIPE_RIGHT, SendPizzaBoxToPlayer);
        EventManager.StopListening(GameConstants.GameEvents.PIZZA_BOX_COLLECTED, ActivateNextPizzaBox);
    }


    void SendPizzaBoxToPlayer(EventParam param)
    {
        if (currenPizzaBoxIndex < AllPizzaBoxesList.Count)
        {
            EventParam pizzaBoxParam = new EventParam();
            pizzaBoxParam.pickupDirection = param.pickupDirection;
            pizzaBoxParam.PizzaBox = AllPizzaBoxesList[currenPizzaBoxIndex];
            pizzaBoxParam.PizzaBox.DisableWeightText();
            pizzaBoxParam.PizzaBox.DisableCollider();
            
            EventManager.TriggerEvent(GameConstants.GameEvents.SEND_PIZZABOX_TO_PLAYER, pizzaBoxParam);
        }
       
        

    }

    void ActivateNextPizzaBox(EventParam param)
    {
        PlayerContoller playerContoller = param.playerContoller;
        currenPizzaBoxIndex++;
        if (currenPizzaBoxIndex < AllPizzaBoxesList.Count)
        {
            
            int new_weight = 0;
            if (currenPizzaBoxIndex > 5)
            {

                
                if(playerContoller.GetLeftWeight()!=0 && playerContoller.GetRightWeight() != 0)
                {
                    int player_weight_diff = playerContoller.GetWeightDiff();
                    new_weight = Random.Range(player_weight_diff - 5, player_weight_diff + 5);
                    AllPizzaBoxesList[currenPizzaBoxIndex].SetWeight(new_weight);

                }
                else
                {
                    new_weight = Random.Range(0, 5);
                }
                
                
                AllPizzaBoxesList[currenPizzaBoxIndex].CreatePizzaBoxes();

            }
            
            AllPizzaBoxesList[currenPizzaBoxIndex].transform.position = playerContoller.transform.position +
                new Vector3(0, 0, 25);
            AllPizzaBoxesList[currenPizzaBoxIndex].gameObject.SetActive(true);
            if (currenPizzaBoxIndex == 19)
            {
                EventManager.TriggerEvent(GameConstants.GameEvents.FINISHED_PIZZA_BOXES, new EventParam());
            }
        }
       
    }

    void Start()
    {
        CreateAllPizzaBoxes();
        AllPizzaBoxesList[0].gameObject.SetActive(true);
        
    }

    void CreateAllPizzaBoxes()
    {
        for(int i = 0; i < TotalPizzaBoxCount; i++)
        {
           

            PizzaBoxes new_pizza_box = Instantiate(PizzaBoxPrefab, transform.position, Quaternion.identity);
            if (i <= 5)
            {
                currentWeightValue = Random.Range(0, 5);
                new_pizza_box.SetWeight(currentWeightValue);
                new_pizza_box.CreatePizzaBoxes();
            }
            else
            {
                
                currentWeightValue = 0;
            }
           
            new_pizza_box.transform.parent = transform;
            
            
            new_pizza_box.gameObject.SetActive(false);
            AllPizzaBoxesList.Add(new_pizza_box);
        }
    }


}
