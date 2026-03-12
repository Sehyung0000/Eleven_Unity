// MoveValidator.cs
using System.Collections.Generic;
using System.Linq;

namespace Elevens.Core
{

public sealed class MoveValidator
{
    public bool IsValidSelection(IReadOnlyList<Card> selected)
    {
        if (selected == null) return false;
        if (selected.Count == 2) return IsValidPair(selected[0], selected[1]);
        if (selected.Count == 3) return IsValidTriple(selected);
        return false;
    }

    private bool IsValidPair(Card a, Card b)
    {
        // J/Q/K have value 0 and cannot be used for sum=11 pair.
        return a.ValueForEleven > 0 && b.ValueForEleven > 0 && (a.ValueForEleven + b.ValueForEleven == 11);
    }

    private bool IsValidTriple(IReadOnlyList<Card> three)
    {
        // Must be exactly one J, one Q, one K
        bool hasJ = three.Any(c => c.IsJack);
        bool hasQ = three.Any(c => c.IsQueen);
        bool hasK = three.Any(c => c.IsKing);
        return hasJ && hasQ && hasK;
    }

    public bool HasLegalMoves(IReadOnlyList<Card> tableCards)
    {
        if (tableCards == null || tableCards.Count == 0) return false;

        // Any valid pair sum=11?
        for (int i = 0; i < tableCards.Count; i++)
        for (int j = i + 1; j < tableCards.Count; j++)
            if (IsValidSelection(new List<Card> { tableCards[i], tableCards[j] }))
            {
                //Console.WriteLine($"{i}, {j}");
                return true;
            }

        // Any JQK triple?
        // Quick check: must contain J, Q, K somewhere
        bool hasJ = tableCards.Any(c => c.IsJack);
        bool hasQ = tableCards.Any(c => c.IsQueen);
        bool hasK = tableCards.Any(c => c.IsKing);
        if (hasJ && hasQ && hasK)
        {
            //Console.WriteLine("has j q k");
            return true;
        }
        else 
            return false;
    }
}
}
