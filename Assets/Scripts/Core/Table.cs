// Table.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elevens.Core
{

public sealed class Table
{
    private readonly List<Card> _visibleCards = new();
    public int MaxCards { get; }

    public Table(int maxCards = 9)
    {
        if (maxCards <= 0) throw new ArgumentOutOfRangeException(nameof(maxCards)); //ht
        MaxCards = maxCards;
    }

    public IReadOnlyList<Card> Cards => _visibleCards;

    public int Count() => _visibleCards.Count;

    public bool IsEmpty() => _visibleCards.Count == 0;

    public void AddCard(Card card)
    {
        if (_visibleCards.Count >= MaxCards)
            throw new InvalidOperationException("Table is full."); //ht
        _visibleCards.Add(card);
    }

    public Card GetCardAt(int index)
    {
        if (index < 0 || index >= _visibleCards.Count) throw new ArgumentOutOfRangeException(nameof(index)); //ht
        return _visibleCards[index];
    }

    public List<Card> GetCardsByIndices(IEnumerable<int> indices)
    {
        var idx = indices.Distinct().OrderBy(i => i).ToList();
        if (idx.Count == 0) throw new ArgumentException("No indices selected.");
        if (idx.Any(i => i < 0 || i >= _visibleCards.Count)) throw new ArgumentOutOfRangeException(nameof(indices)); //ht
        return idx.Select(i => _visibleCards[i]).ToList();
    }

    public void RemoveCards(IEnumerable<Card> cards)
    {
        foreach (var c in cards)
        {
            bool removed = _visibleCards.Remove(c);
            if (!removed) //ht
                throw new InvalidOperationException("Attempted to remove a card not on the table.");
        }
    }
}
}
