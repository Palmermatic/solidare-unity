using Assets.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer != null)
            spriteRenderer.sprite = selectable.IsFaceUp ? cardFace : cardBack;

        if (name == userInput.slot1.name)
        {
            spriteRenderer.color = Color.yellow;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
