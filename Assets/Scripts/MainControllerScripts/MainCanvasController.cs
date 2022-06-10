using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvasController : MonoBehaviour
{
    
    
    [SerializeField]  GameObject TapToPlayUI,TryAgainUI,VictoryUI;

    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.GAME_STARTED, StartTextAnimaton);
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.LEVEL_FAILED, ActivateTryAgainButton);
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, ActivateVictoryUI);
    }
    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.GAME_STARTED, StartTextAnimaton);
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.LEVEL_FAILED, ActivateTryAgainButton);
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, ActivateVictoryUI);
    }

    void ActivateVictoryUI(EventParam param)
    {
        VictoryUI.SetActive(true);
        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(1f);
        Application.LoadLevel(0);
    }

    void StartTextAnimaton(EventParam param)
    {
        TapToPlayUI.SetActive(false);
        
    }

    void ActivateTryAgainButton(EventParam param)
    {
        TryAgainUI.SetActive(true);
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
