using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class TabBetween : MonoBehaviour
{
    public InputField nextField;
    private InputField myField;

    // Start is called before the first frame update
    private void Start()
    {
        if (nextField == null)
        {
            Destroy(this);
            return;
        }

        myField = GetComponent<InputField>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (myField.isFocused && Input.GetKeyDown(KeyCode.Tab))
        {
            nextField.ActivateInputField();
        }
    }
}