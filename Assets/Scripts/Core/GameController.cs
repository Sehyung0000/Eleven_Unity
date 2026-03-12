// GameController.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elevens.Core
{

public enum GameState { NotStarted, Running, Won, Lost }

public sealed class GameController
{
    private readonly Deck _deck;
    private readonly Table _table;
    private readonly MoveValidator _validator;

    public GameState State { get; private set; } = GameState.NotStarted;

    public Deck Deck => _deck;
    public Table Table => _table;

    public GameController(Deck deck = null, Table table = null, MoveValidator validator = null)
    {
        _deck = deck ?? new Deck();
        _table = table ?? new Table(9);
        _validator = validator ?? new MoveValidator();
    }

    public void StartGame()
    {
        _deck.Shuffle();
        // Fill table to 9
        RefillTableToNine();
        State = GameState.Running;
        CheckEndState();
    }

    // Orchestration method: collaboration Deck + Table
    public void RefillTableToNine()
    {
        while (_table.Count() < _table.MaxCards && !_deck.IsEmpty())
        {
            var card = _deck.DealCard();
            _table.AddCard(card);
        }
    }

    // Orchestration method: selection → validate → remove → refill → end-state
    public bool SubmitSelection(IReadOnlyList<int> indices, out string message)
    {
        message = "";

        if (State != GameState.Running)
        {
            message = "Game is not running.";
            return false;
        }

        List<Card> selected;
        try
        {
            selected = _table.GetCardsByIndices(indices);
        }
        catch (Exception ex)
        {
            message = $"Invalid selection: {ex.Message}";
            return false;
        }

        if (!_validator.IsValidSelection(selected))
        {
            message = "Invalid move. Select 2 cards summing to 11, or 3 cards that are J-Q-K.";
            return false;
        }

        // State change belongs to Table; orchestrated here
        _table.RemoveCards(selected);

        // Replace removed cards if possible
        RefillTableToNine();

        // Update game state
        CheckEndState();

        message = "Move accepted.";
        return true;
    }

    public void CheckEndState()
    {
        if (CheckWin())
        {
            State = GameState.Won;
            return;
        }

        if (CheckLose())
        {
            State = GameState.Lost;
            return;
        }

        State = GameState.Running;
    }

    public bool CheckWin()
    {
        // Win if deck empty and table empty
        return _deck.IsEmpty() && _table.IsEmpty();
    }

    public bool CheckLose()
    {
        // Lose if deck empty and no legal moves on table
        return !_validator.HasLegalMoves(_table.Cards);
    }
}
}
