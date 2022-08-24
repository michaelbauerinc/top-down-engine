using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{

    // public float horizontal;
    // public float vertical;
    public bool inputBuffer = false;
    // public int inputBufferMax = 90;

    public (int, int) selectedCharacter = (0, 0);
    private VisualElement selectedFrame;

    private Vector2 input;
    public bool confirm = false;
    private int maxColumns = 2;
    private int maxRows = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        var vertical = input.y;
        var horizontal = input.x;
        if (!inputBuffer)
        {
            inputBuffer = true;
            if (vertical > 0)
            {
                selectedCharacter.Item1--;
            }
            else if (vertical < 0)
            {
                selectedCharacter.Item1++;
            }

            if (horizontal > 0)
            {
                selectedCharacter.Item2++;
            }
            else if (horizontal < 0)
            {
                selectedCharacter.Item2--;
            }

            if (selectedCharacter.Item1 > maxRows - 1)
            {
                selectedCharacter.Item1 = 0;
            }
            else if (selectedCharacter.Item1 < 0)
            {
                selectedCharacter.Item1 = maxRows - 1;
            }

            if (selectedCharacter.Item2 > maxColumns - 1)
            {
                selectedCharacter.Item2 = 0;
            }
            else if (selectedCharacter.Item2 < 0)
            {
                selectedCharacter.Item2 = maxColumns - 1;
            }
        }

    }

    public void Confirm(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            confirm = true;
        }
    }

    public void GetMovement(InputAction.CallbackContext value)
    {

        if (value.started)
        {
            inputBuffer = false;
        }
        else
        {
            Vector2 vector = value.ReadValue<Vector2>();
            input = vector;
        }
    }
}
