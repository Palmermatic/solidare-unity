using Assets.Scripts.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    private GameObject Canvas;
    public GameObject ZoomedCardPrefab;

    private GameObject card;


    public void Awake()
    {
        Canvas = GameObject.Find("Canvas");
    }

    public void OnHoverEnter()
    {
        card = Instantiate(
            ZoomedCardPrefab,
            new Vector2(Input.mousePosition.x + 100, Input.mousePosition.y + 100),
            Quaternion.identity,
            Canvas.transform
        );

        card.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
    }

    public void OnHoverExit()
    {
        Destroy(card);
    }
}
