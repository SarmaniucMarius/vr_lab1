using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    FIRE,
    EARTH,
    WATER,
    AIR,
    WIND
}

public class Game : MonoBehaviour
{
    // {ELEM1, ELEM2, RESULT}
    public ElementType[,] combinations = {
        { ElementType.AIR, ElementType.AIR, ElementType.WIND}
    };


    ElementType[] elements = { ElementType.FIRE, ElementType.WATER, ElementType.EARTH, ElementType.AIR };
    int step = 2;
    // left shelf:  (-4, 1.5, -11)
    // right shelf: ( 4, 1.5, -11)
    public Transform initialPos;

    public GameObject newElementPrefab;

    public static Game current;

    private void Awake()
    {
        current = this;
        for (int i = 0; i < elements.Length; i++)
        {
            GameObject left = Instantiate(newElementPrefab);
            left.transform.Translate(new Vector3(-4f, 1.5f, -11f + i * step));
            left.GetComponent<Element>().type = elements[i];

            GameObject right = Instantiate(newElementPrefab);
            right.transform.Translate(new Vector3(4f, 1.5f, -11f + i * step));
            right.GetComponent<Element>().type = elements[i];
            Debug.Log(elements[i].ToString());
        }
    }

    private GameObject newElement = null;
    private Element previousSelectedElement = null;
    public void SelectElement(Element newSelectedElement)
    {
        Debug.Log("Select Element: " + newSelectedElement.type.ToString());
        //if(previousSelectedElement != null && previousSelectedElement != newSelectedElement)
        //{
        //    // newElement = Instantiate(newElementPrefab, thirdElementPos);

        //    newSelectedElement.isSelected = false;
        //    previousSelectedElement.isSelected = false;
        //    previousSelectedElement = null;
        //}
        //else
        //{
        //    if (newElement)
        //        Destroy(newElement);

        //    previousSelectedElement = newSelectedElement;
        //    newSelectedElement.isSelected = true;
        //}
    }
}
