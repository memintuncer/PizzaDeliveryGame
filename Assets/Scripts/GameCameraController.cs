using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    [SerializeField] Transform DestinationPosition, CameraStartPosition, NormalCameraPosition, EndingCameraPosition;
    [SerializeField] float CameraMovementSpeed;
    public Vector3 NormalCameraAngle, FinishCameraAngle;
    private void OnEnable()
    {

        
        EventManager.StartListening(GameConstants.GameEvents.GAME_STARTED, StartMoving);


    }
    private void OnDisable()
    {
        
        EventManager.StopListening(GameConstants.GameEvents.GAME_STARTED, StartMoving);

    }

   
    void StartMoving(EventParam param)
    {
        DestinationPosition = NormalCameraPosition;
        
    }
    void Start()
    {
        DestinationPosition = CameraStartPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, DestinationPosition.position, CameraMovementSpeed * Time.deltaTime);
    }
}
