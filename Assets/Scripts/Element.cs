using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public ElementType type;
    private Coroutine coroutine;

    [HideInInspector]
    public bool selected = false;
    private bool focused = false;

    public void OnPointerEnter()
    {
        Debug.Log("Pointer enter: " + type.ToString());
        focused = true;
        if (!selected)
        {
            coroutine = StartCoroutine(SelectCoroutine());
        }
    }

    public void OnPointerExit()
    {
        Debug.Log("Pointer exit: " + type.ToString());
        focused = false;
        StopCoroutine(coroutine);
    }

    private IEnumerator SelectCoroutine()
    {
        yield return new WaitForSeconds(2f);
        selected = true;
        Game.current.SelectElement(this);
    }
}
