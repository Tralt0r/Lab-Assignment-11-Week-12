using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Task3 : MonoBehaviour
{
    List<string> deck = new List<string>();
    List<string> hand = new List<string>();

    string[] ranks = { "K", "Q", "J", "A" };
    string[] suits = { "\u2660", "\u2663", "\u2665", "\u2666" }; // ♠ ♣ ♥ ♦

    void Start()
    {
        CreateDeck();
        ShuffleDeck();
        DrawInitialHand();

        Debug.Log("I made the initial deck and draw. My hand is: " + string.Join(", ", hand));

        PlayGame();
    }

    void CreateDeck()
    {
        foreach (string rank in ranks)
        {
            foreach (string suit in suits)
            {
                deck.Add(rank + suit);
            }
        }
    }

    void ShuffleDeck()
    {
        deck = deck.OrderBy(x => Random.value).ToList();
    }

    void DrawInitialHand()
    {
        for (int i = 0; i < 4; i++)
        {
            DrawCard();
        }
    }

    void DrawCard()
    {
        if (deck.Count > 0)
        {
            hand.Add(deck[0]);
            deck.RemoveAt(0);
        }
    }

    bool IsWinningHand()
    {
        var suitCounts = hand
            .GroupBy(card => card.Last()) // group by suit
            .Select(group => group.Count());

        return suitCounts.Any(count => count >= 3);
    }

    void PlayGame()
    {
        while (true)
        {
            if (IsWinningHand())
            {
                Debug.Log("My hand is: " + string.Join(", ", hand) + ". The game is WON.");
                break;
            }

            if (deck.Count == 0)
            {
                Debug.Log("The deck is empty. The game is LOST.");
                break;
            }

            // Discard random card
            int discardIndex = Random.Range(0, hand.Count);
            string discarded = hand[discardIndex];
            hand.RemoveAt(discardIndex);

            // Draw new card
            string drawn = deck[0];
            DrawCard();

            Debug.Log($"I discarded {discarded} and drew {drawn}. My hand is: {string.Join(", ", hand)}. This is not a winning hand. I will attempt to play another round.");
        }
    }
}