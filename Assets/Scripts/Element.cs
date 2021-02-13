using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public ElementType type;
    public Side side;
    private Coroutine coroutine;

    public void OnPointerEnter()
    {
        Debug.Log("Pointer enter: " + type.ToString());
        coroutine = StartCoroutine(SelectCoroutine());
    }

    public void OnPointerExit()
    {
        Debug.Log("Pointer exit: " + type.ToString());
        StopCoroutine(coroutine);
    }

    private IEnumerator SelectCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Game.current.SelectElement(this);
    }
}
