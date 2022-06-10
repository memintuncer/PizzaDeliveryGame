using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InputManager : SingletonComponent<InputManager>, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    #region PRIVATE VARIABLES
    private bool _isBeginDrag = false;
    private bool _isDrag = false;
    private bool _isEndDrag = false;

    private bool _isPointerDown = false;
    private bool _isPointerClick = false;
    private bool _isPointerUp = false;

    private Vector3 _deltaPos = Vector3.zero;

    private Canvas canvas = null;
    #endregion

    #region PUBLIC VARIABLES
    public bool isBeginDrag
    {
        get
        {
            return _isBeginDrag;
        }
    }
    public bool isDrag
    {
        get
        {
            return _isDrag;
        }
    }
    public bool isEndDrag
    {
        get
        {
            return _isEndDrag;
        }
    }

    public bool isPointerDown
    {
        get
        {
            return _isPointerDown;
        }
    }
    public bool isPointerUp
    {
        get
        {
            return _isPointerUp;
        }
    }
    public bool isPointerClick
    {
        get
        {
            return _isPointerClick;
        }
    }

    public Vector3 deltaPos
    {
        get
        {
            return _deltaPos;
        }
    }
    #endregion

    #region EVENTSYSTEM FUNCTIONS
    public void OnBeginDrag(PointerEventData eventData)
    {
        _isBeginDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _isDrag = true;
        _deltaPos = eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDrag = false;
        _isEndDrag = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isPointerClick = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerUp = true;
    }
    #endregion

    #region UPDATE
    private void LateUpdate()
    {
        if (_isPointerDown)
        {
            _isPointerDown = false;
        }

        if (_isPointerUp)
        {
            _isPointerUp = false;
        }

        if (_isPointerClick)
        {
            _isPointerClick = false;
        }

        if (_isBeginDrag)
        {
            _isBeginDrag = false;
        }

        if (_isEndDrag)
        {
            _isEndDrag = false;
        }
    }
    #endregion

    #region LEVELSTARTED

    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.LEVEL_STARTED, onLevelStarted);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.LEVEL_STARTED, onLevelStarted);

    }

    private void onLevelStarted(EventParam param)
    {
        canvas = GetComponent<Canvas>();

        if (canvas != null)
        {
            canvas.worldCamera = Camera.main;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
    }

    #endregion
}

