using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Mono : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Assembly info = typeof(int).Assembly;
        Console.WriteLine(info); //attribute: see metadata
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = 0.0f; //property: signifies where the sprite is
        if (Keyboard.current.leftArrowKey.isPressed) horizontal = -1.0f; //event: causes movement
        else if (Keyboard.current.rightArrowKey.isPressed) horizontal = 1.0f;
        Debug.Log(horizontal);
        Vector2 position = transform.position;
        position.x = position.x + 0.1f * horizontal;
        transform.position = position; //out parameter: executes event
    }
}
