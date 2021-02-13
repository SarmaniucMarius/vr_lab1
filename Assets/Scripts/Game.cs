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

public enum Side
{
    LEFT,
    RIGHT
}

public class Combination
{
    public ElementType e1;
    public ElementType e2;
    public ElementType result;

    public Combination(ElementType e1, ElementType e2, ElementType result)
    {
        this.e1 = e1;
        this.e2 = e2;
        this.result = result;
    }

    public bool match(ElementType ne1, ElementType ne2)
    {
        return e1 == ne1 && e2 == ne2 || e1 == ne2 && e2 == ne1;
    }
}

public class Game : MonoBehaviour
{
    public GameObject newElementPrefab;

    public static Game current;

    private Combination[] combinations = {
        new Combination(ElementType.AIR, ElementType.AIR, ElementType.WIND)
    };

    private List<ElementType> elements = new List<ElementType>();
    private int step = 2;

    // left shelf:  (-4, 1.5, -11)
    // right shelf: ( 4, 1.5, -11)

    private Element leftElement = null;
    private Element rightElement = null;

    private void Awake()
    {
        current = this;
        createElement(ElementType.FIRE);
        createElement(ElementType.WATER);
        createElement(ElementType.EARTH);
        createElement(ElementType.AIR);
    }

    private void createElement(ElementType type)
    {
        elements.Add(type);
        CreateSidedElement(type, Side.LEFT, elements.Count - 1);
        CreateSidedElement(type, Side.RIGHT, elements.Count - 1);
    }

    private GameObject CreateSidedElement(ElementType type, Side side, int offset)
    {
        GameObject obj = Instantiate(newElementPrefab);
        obj.transform.Translate(new Vector3(GetXBySide(side), 1.5f, -11f + offset * step));
        Element element = obj.GetComponent<Element>();
        element.type = type;
        element.side = side;
        return obj;
    }

    private float GetXBySide(Side side)
    {
        if (side == Side.LEFT)
        {
            return 4f;
        }
        if (side == Side.RIGHT)
        {
            return -4f;
        }
        return 0;
    }

    public void SelectElement(Element newSelectedElement)
    {
        Debug.Log("Select Element: " + newSelectedElement.type.ToString());
        if (newSelectedElement.side == Side.LEFT)
        {
            if (newSelectedElement == leftElement)
            {
                UnselectLeft();
            }
            else
            {
                UnselectLeft();
                leftElement = newSelectedElement;
                MarkAsSelected(leftElement);
            }
        }

        if (newSelectedElement.side == Side.RIGHT)
        {
            if (newSelectedElement == rightElement)
            {
                UnselectRight();
            }
            else
            {
                UnselectRight();
                rightElement = newSelectedElement;
                MarkAsSelected(rightElement);
            }
        }


        if (leftElement != null && rightElement != null)
        {
            tryCombination();
        }
    }

    private void UnselectLeft()
    {
        if (leftElement != null)
        {
            MarkAsUnselected(leftElement);
            leftElement = null;
        }
    }

    private void UnselectRight()
    {
        if (rightElement != null)
        {
            MarkAsUnselected(rightElement);
            rightElement = null;
        }
    }

    private void MarkAsSelected(Element element)
    {
        element.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }

    private void MarkAsUnselected(Element element)
    {
        element.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
    }

    private void tryCombination()
    {
        Debug.Log("Try Combination: (" + leftElement.type.ToString() + ", " + rightElement.type.ToString() + ")");
        Combination comb = Array.Find<Combination>(combinations, combination =>
        {
            return combination.match(leftElement.type, rightElement.type);
        });
        if (comb != null)
        {
            if (!elements.Contains(comb.result))
            {
                createElement(comb.result);
                Debug.Log("Combination created: " + comb.result.ToString());
                UnselectLeft();
                UnselectRight();
            }
            else
            {
                Debug.Log("Combination already exists");
            }
        }
        else
        {
            Debug.Log("No combination available");
        }
    }
}
