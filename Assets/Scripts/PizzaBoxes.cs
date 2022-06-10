using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PizzaBoxes : MonoBehaviour
{
    public enum PickupDirection
    {
        Idle,
        Left,
        Right,
    }

    [SerializeField] int Weight;
    [SerializeField] GameObject PizzaBoxModel;
    [SerializeField] TextMeshProUGUI WeightText;
    public PickupDirection pizzaDirection;
    Collider selfCollider;
    [SerializeField] Rigidbody[] modelsRBs;
    void Start()
    {
        
        selfCollider = GetComponent<Collider>();
        
        
    }

    public void CreatePizzaBoxes()
    {
       
        if (Weight >= 0)
        {
            if (Weight == 0)
            {
                Weight = 1;
            }
            WeightText.text = "+" + Weight.ToString();
        }
        else
        {
            WeightText.text =Weight.ToString();
        }
        
        for(int i = 0; i < Weight; i++)
        {
            
            GameObject temp_pizza_box_model = Instantiate(PizzaBoxModel, transform.position,Quaternion.Euler(0,180,0));
            temp_pizza_box_model.SetActive(true);
            temp_pizza_box_model.transform.parent = transform.GetChild(0);
            
            temp_pizza_box_model.transform.localPosition += new Vector3(0, 0.1f * i, 0f);
        }

        SetModelsRBs();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            EventManager.TriggerEvent(GameConstants.GameEvents.HITTED_TO_PIZZABOX, new EventParam());
        }
    }

    public void SetWeight(int weight)
    {
        Weight = weight;
    }

    public int GetWeight()
    {
        return Weight;
    }

    

    public void DisableWeightText()
    {
        WeightText.gameObject.SetActive(false);
    }

    public void DisableCollider()
    {
        selfCollider.enabled = false;
    }

  

    public void ActivateModelsGravity()
    {

        foreach(Rigidbody model_rb in modelsRBs)
        {
            model_rb.useGravity = true;
        }
    }

    public void SetModelsRBs()
    {
        modelsRBs = transform.GetChild(0).GetComponentsInChildren<Rigidbody>();
    }


    public IEnumerator SendModelsToDestinationCRT(Transform target_destination)
    {
        Transform models_parent = transform.GetChild(0);
        int model_count = models_parent.childCount;
        for (int i = 0; i < model_count; i++)
        {
            Transform models_to_send = models_parent.GetChild(0);
            models_to_send.parent = target_destination;
            
            if (target_destination.childCount > 1)
            {
                models_to_send.transform.localPosition = target_destination.GetChild(target_destination.childCount - 2).localPosition +
                    new Vector3(-0.1f, 0, 0);
                
                models_to_send.localRotation = Quaternion.Euler(90, 270, 0);


            }
            else
            {
                models_to_send.localPosition = Vector3.zero;
                models_to_send.localRotation = Quaternion.Euler(90, 270, 0);
            }
            yield return new WaitForSeconds(0.05f);

        }
    }

   
}
