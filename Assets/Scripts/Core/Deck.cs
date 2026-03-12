// Deck.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Elevens.Core
{

public sealed class Deck
{
    private readonly List<Card> _cards = new();

    public Deck()
    {
        foreach (Suit s in Enum.GetValues(typeof(Suit)))
            for (int r = 1; r <= 13; r++)
                _cards.Add(new Card(r, s));
    }

    public int Count => _cards.Count;

    public bool IsEmpty() => _cards.Count == 0;

    public void Shuffle()
    {
        // Fisher-Yates using cryptographic RNG (stable + fair for demos)
        for (int i = _cards.Count - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
        }
    }

    public Card DealCard()
    {
        if (_cards.Count == 0) throw new InvalidOperationException("Deck is empty."); //ht
        Card top = _cards[^1];
        _cards.RemoveAt(_cards.Count - 1);
        return top;
    }
}
}
