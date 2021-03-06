﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    FIRE,
    EARTH,
    WATER,
    AIR,
    WIND,
    ALCOHOL,
    LAVA,
    MOLOTOV_COCKTAIL,
    ENERGY,
    STORM,
    SWAMP,
    TYPHOON,
    STEAM,
    GEYSER,
    DUST,
    GUNPOWDER
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
    public static Game current;

    private Combination[] combinations = {
        new Combination(ElementType.AIR, ElementType.AIR, ElementType.WIND),
        new Combination(ElementType.FIRE, ElementType.WATER, ElementType.ALCOHOL),
        new Combination(ElementType.FIRE, ElementType.EARTH, ElementType.LAVA),
        new Combination(ElementType.FIRE, ElementType.ALCOHOL, ElementType.MOLOTOV_COCKTAIL),
        new Combination(ElementType.FIRE, ElementType.AIR, ElementType.ENERGY),
        new Combination(ElementType.ENERGY, ElementType.AIR, ElementType.STORM),
        new Combination(ElementType.EARTH, ElementType.WATER, ElementType.SWAMP),
        new Combination(ElementType.STORM, ElementType.WATER, ElementType.TYPHOON), //8
        new Combination(ElementType.AIR, ElementType.WATER, ElementType.STEAM),
        new Combination(ElementType.STEAM, ElementType.EARTH, ElementType.GEYSER),
        new Combination(ElementType.AIR, ElementType.EARTH, ElementType.DUST),
        new Combination(ElementType.DUST, ElementType.FIRE, ElementType.GUNPOWDER),
    };

    private GameObject defaultResource;
    private List<ElementType> elements = new List<ElementType>();
    private int step = 2;

    // left shelf:  (-4, 1.5, -11)
    // right shelf: ( 4, 1.5, -11)

    private Element leftElement = null;
    private Element rightElement = null;

    private GameObject leftSelector;
    private GameObject rightSelector;

    private void Awake()
    {
        GameObject selector = Resources.Load("selector") as GameObject;

        leftSelector = Instantiate(selector);
        InitSelector(leftSelector);

        rightSelector = Instantiate(selector);
        InitSelector(rightSelector);

        defaultResource = Resources.Load("default") as GameObject;
        current = this;
        createElement(ElementType.FIRE);
        createElement(ElementType.WATER);
        createElement(ElementType.EARTH);
        createElement(ElementType.AIR);
    }

    private void InitSelector(GameObject selector)
    {
        selector.SetActive(false);
    }

    private void createElement(ElementType type)
    {
        GameObject prefab = Resources.Load(type.ToString()) as GameObject;
        if (prefab == null)
        {
            Debug.Log("Could not load resource: " + type.ToString());
            prefab = defaultResource;
        }
        elements.Add(type);
        CreateSidedElement(prefab, type, Side.LEFT, elements.Count - 1);
        CreateSidedElement(prefab, type, Side.RIGHT, elements.Count - 1);
    }

    private GameObject CreateSidedElement(GameObject prefab, ElementType type, Side side, int offset)
    {
        GameObject obj = Instantiate(prefab);
        CreateTextMesh(obj, type, side, offset);

        obj.transform.Translate(new Vector3(GetXBySide(side), 1.65f, -11f + offset * step));
        Element element = obj.GetComponent<Element>();
        element.type = type;
        element.side = side;
        return obj;
    }

    private void CreateTextMesh(GameObject parent, ElementType type, Side side, int offset)
    {
        GameObject textObj = new GameObject();
        textObj.transform.parent = parent.transform;
        textObj.transform.Translate(new Vector3(0, 1f + (offset % 2) * 0.5f, 0));
        TextMesh text = textObj.AddComponent<TextMesh>() as TextMesh;
        text.fontSize = 80;
        text.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        text.text = type.ToString();
        text.anchor = TextAnchor.LowerCenter;

        switch (side)
        {
            case Side.LEFT:
                textObj.transform.Rotate(0, 90, 0);
                break;
            case Side.RIGHT:
                textObj.transform.Rotate(0, -90, 0);
                break;
        }
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
                MarkAsSelected(leftElement, leftSelector);
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
                MarkAsSelected(rightElement, rightSelector);
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
            MarkAsUnselected(leftElement, leftSelector);
            leftElement = null;
        }
    }

    private void UnselectRight()
    {
        if (rightElement != null)
        {
            MarkAsUnselected(rightElement, rightSelector);
            rightElement = null;
        }
    }

    private void MarkAsSelected(Element element, GameObject selector)
    {
        selector.transform.position = element.gameObject.transform.position;
        selector.transform.Translate(new Vector3(0, 0.9f, 0));
        selector.SetActive(true);
    }

    private void MarkAsUnselected(Element element, GameObject selector)
    {
        selector.SetActive(false);
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
                UnselectLeft();
                UnselectRight();
                Debug.Log("Combination created: " + comb.result.ToString());
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
