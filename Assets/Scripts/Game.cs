using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    NO_TYPE,
    FIRE,
    EARTH,
    WATER,
    AIR,

    SOMETHING_ELSE,
}

public class Game : MonoBehaviour
{
    string[] elements = { "earth", "fire", "water", "air" };
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
            GameObject a = Instantiate(newElementPrefab);
            a.transform.Translate(new Vector3(-4f, 1.5f, -11f + i * step));
            a.name = elements[i];
            a.GetComponent<Element>().name = a.name;
        }
    }

    private GameObject newElement = null;
    private Element previousSelectedElement = null;
    public void SelectElement(Element newSelectedElement)
    {
        if(previousSelectedElement != null && previousSelectedElement != newSelectedElement)
        {
            // newElement = Instantiate(newElementPrefab, thirdElementPos);

            newSelectedElement.isSelected = false;
            previousSelectedElement.isSelected = false;
            previousSelectedElement = null;
        }
        else
        {
            if (newElement)
                Destroy(newElement);

            previousSelectedElement = newSelectedElement;
            newSelectedElement.isSelected = true;
        }
    }
}
