using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressManager : MonoBehaviour
{
    [SerializeField] GameObject[] LevelPrefabs;
    public enum LevelStates
    {
        Idle,
        LevelStarted,
        LevelRunning,
        
    }

    private LevelStates levelState;
    InputManager inputManager;

   

    void Start()
    {
        inputManager = InputManager.Instance;
        levelState = LevelStates.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
    }

    void StateMachine()
    {
        switch (levelState)
        {
            case LevelStates.Idle:
                if (inputManager.isPointerClick)
                {
                    levelState = LevelStates.LevelStarted;
                }
                break;
            case LevelStates.LevelStarted:
                EventManager.TriggerEvent(GameConstants.GameEvents.GAME_STARTED, new EventParam());
                levelState = LevelStates.LevelRunning;
                break;
            case LevelStates.LevelRunning:
                break;
            

        }
    }



    public void RestartLevel()
    {
        Application.LoadLevel(0);
    }
}
