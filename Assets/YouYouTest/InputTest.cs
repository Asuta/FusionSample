using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour

{
    private static InputTest _instance;
    public static InputTest Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputTest>();
            }
            return _instance;
        }
    }
    public XRIDefaultInputActions inputActions;
    public InputActionAsset inputActionAsset;
    public Vector2 leftStick;
    public Vector2 rightStick;
    public bool buttonA;
    public float buttonFloat;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }


        inputActions = new XRIDefaultInputActions();

    }
   private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        // if (inputActions.KeyBoardTest.Newaction.triggered)
        // {
        //     Debug.Log("KeyBoardTest");
        // }
        // if (inputActions.KeyBoardTest.Newaction.WasPressedThisFrame())
        // {
        //     Debug.Log("2222222  ");
        // }
        // if (inputActions.KeyBoardTest.Newaction.IsPressed())
        // {
        //     Debug.Log("33333333333");
        // }
        leftStick = inputActions.XRILeftHandInteraction.StickVector2.ReadValue<Vector2>();
        rightStick = inputActions.XRIRightHandInteraction.StickVector2.ReadValue<Vector2>();
        buttonA = inputActions.XRIRightHandInteraction.ButtonA.inProgress;
        
        if (buttonA)
        {
            Debug.Log("buttonA");
        }

    }


}
