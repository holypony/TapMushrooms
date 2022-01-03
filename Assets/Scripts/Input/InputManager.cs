using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    private Mashrooms_Screen_input inputActios;

    [SerializeField] private BoolValueSO boolValueBtn1;
    [SerializeField] private BoolValueSO boolValueBtn2;
    [SerializeField] private BoolValueSO boolValueBtn3;
    [SerializeField] private BoolValueSO boolValueBtn4;
    [SerializeField] private BoolValueSO boolValueBtn5;
    [SerializeField] private BoolValueSO boolValueBtn6;
    [SerializeField] private BoolValueSO boolValueBtn7;
    [SerializeField] private BoolValueSO boolValueBtn8;
    [SerializeField] private BoolValueSO boolValueBtn9;


    private void Awake()
    {
        inputActios = new Mashrooms_Screen_input();
    }


    void Start()
    {
        inputActios.Player.Btn1.started += ctx => btn1Start(ctx);
        inputActios.Player.Btn1.canceled += ctx => btn1Cancel(ctx);

        inputActios.Player.Btn2.started += ctx => btn2Start(ctx);
        inputActios.Player.Btn2.canceled += ctx => btn2Cancel(ctx);

        inputActios.Player.Btn3.started += ctx => btn3Start(ctx);
        inputActios.Player.Btn3.canceled += ctx => btn3Cancel(ctx);

        inputActios.Player.Btn4.started += ctx => btn4Start(ctx);
        inputActios.Player.Btn4.canceled += ctx => btn4Cancel(ctx);

        inputActios.Player.Btn5.started += ctx => btn5Start(ctx);
        inputActios.Player.Btn5.canceled += ctx => btn5Cancel(ctx);

        inputActios.Player.Btn6.started += ctx => btn6Start(ctx);
        inputActios.Player.Btn6.canceled += ctx => btn6Cancel(ctx);

        inputActios.Player.Btn7.started += ctx => btn7Start(ctx);
        inputActios.Player.Btn7.canceled += ctx => btn7Cancel(ctx);

        inputActios.Player.Btn8.started += ctx => btn8Start(ctx);
        inputActios.Player.Btn8.canceled += ctx => btn8Cancel(ctx);

        inputActios.Player.Btn9.started += ctx => btn9Start(ctx);
        inputActios.Player.Btn9.canceled += ctx => btn9Cancel(ctx);

    }

    private void btn1Cancel(InputAction.CallbackContext ctx)
    {
        boolValueBtn1.Value = false;
    }

    private void btn1Start(InputAction.CallbackContext ctx)
    {
        boolValueBtn1.Value = true;
    }

    private void btn2Cancel(InputAction.CallbackContext ctx)
    {
        boolValueBtn2.Value = false;
    }

    private void btn2Start(InputAction.CallbackContext ctx)
    {
        boolValueBtn2.Value = true;
    }

    private void btn3Cancel(InputAction.CallbackContext ctx)
    {
        boolValueBtn3.Value = false;
    }

    private void btn3Start(InputAction.CallbackContext ctx)
    {
        boolValueBtn3.Value = true;
    }

    private void btn4Cancel(InputAction.CallbackContext ctx)
    {
        boolValueBtn4.Value = false;
    }

    private void btn4Start(InputAction.CallbackContext ctx)
    {
        boolValueBtn4.Value = true;
    }

    private void btn5Cancel(InputAction.CallbackContext ctx)
    {
        boolValueBtn5.Value = false;
    }

    private void btn5Start(InputAction.CallbackContext ctx)
    {
        boolValueBtn5.Value = true;
    }

    private void btn6Cancel(InputAction.CallbackContext ctx)
    {
        boolValueBtn6.Value = false;
    }

    private void btn6Start(InputAction.CallbackContext ctx)
    {
        boolValueBtn6.Value = true;
    }

    private void btn7Cancel(InputAction.CallbackContext ctx)
    {
        boolValueBtn7.Value = false;
    }

    private void btn7Start(InputAction.CallbackContext ctx)
    {
        boolValueBtn7.Value = true;
    }

    private void btn8Cancel(InputAction.CallbackContext ctx)
    {
        boolValueBtn8.Value = false;
    }

    private void btn8Start(InputAction.CallbackContext ctx)
    {
        boolValueBtn8.Value = true;
    }

    private void btn9Cancel(InputAction.CallbackContext ctx)
    {
        boolValueBtn9.Value = false;
    }

    private void btn9Start(InputAction.CallbackContext ctx)
    {
        boolValueBtn9.Value = true;
    }





    private void OnEnable()
    {
        inputActios.Enable();
    }

    private void OnDisable()
    {
        inputActios.Disable();
    }
}
