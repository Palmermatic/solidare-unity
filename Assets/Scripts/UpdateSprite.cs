using Assets.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSprite : MonoBehaviour
{
    public Sprite CardFace, CardBack;

    private Image cardImage;
    private Selectable selectable;
    private Solitaire solitaire;
    private UserInput userInput;


    // Start is called before the first frame update
    void Start()
    {
        //List<Card> deck = new List<Card>();
        //deck.Unwrap();
        //solitaire = FindObjectOfType<Solitaire>();

        userInput = FindObjectOfType<UserInput>();
        cardImage = GetComponent<Image>();
        selectable = GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {

        cardImage.sprite = selectable.IsFaceUp ? CardFace : CardBack;
    }
}
