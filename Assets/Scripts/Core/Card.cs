// Card.cs
using System;

namespace Elevens.Core
{

public enum Suit { Clubs, Diamonds, Hearts, Spades } //ht

public sealed class Card
{
    public int Rank { get; }          // 1..13 (A..K)
    public Suit Suit { get; }

    public Card(int rank, Suit suit)
    {
        if (rank < 1 || rank > 13) throw new ArgumentOutOfRangeException(nameof(rank));
        Rank = rank;
        Suit = suit;
    }

    // For Elevens rule: number cards have face value; J/Q/K are treated as 0 (cannot be used in sum=11).
    public int ValueForEleven => (Rank >= 1 && Rank <= 10) ? Rank : 0;

    public bool IsJack => Rank == 11;
    public bool IsQueen => Rank == 12;
    public bool IsKing => Rank == 13;

    public override string ToString()
    {
        string r = Rank switch
        {
            1 => "A",
            11 => "J",
            12 => "Q",
            13 => "K",
            _ => Rank.ToString()
        };
        string suit = Suit switch
        {
            Suit.Spades => "♠",
            Suit.Clubs => "♣",
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            _ => "?"
        };

        return $"{r}{suit}";
    }
}
}
