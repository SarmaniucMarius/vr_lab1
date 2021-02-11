using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public string name;
    public ElementType type;
    
    [HideInInspector]
    public bool isSelected = false;

#if false
    private Vector3 initial_pos;
    private void Awake()
    {
        initial_pos = transform.position;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(isSelectable)
            {
                Game.current.SelectElement(this);
            }
        }

        if(isSelected)
        {
            transform.Translate(transform.up * Mathf.Sin(Time.time * 5) * Time.deltaTime);
        }
        else
        {
            transform.position = initial_pos;
        }
    }
#endif
    private bool isFocused = false;
    public void OnPointerEnter()
    {
        isFocused = true;
    }

    public void OnPointerExit()
    {
        isFocused = false;
    }

    private bool isSelectable = false;
    private void OnTriggerEnter(Collider other)
    {
        isSelectable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isSelectable = false;
    }
}
