using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Elevens.Core;
using System.Runtime.ExceptionServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

public class ElevensUnityUI : MonoBehaviour
{
    [Header("Card Slots (9)")]
    [SerializeField] private CardSlotUI[] cardSlots;
    [Header("Buttons")]
    
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button replaceButton;
    [SerializeField] private Button quitButton;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI deckText;
    [SerializeField] private TextMeshProUGUI stateText;

    private GameController game;

    private readonly HashSet<int> selectedIndices = new();

    void Start()
    {
        newGameButton.onClick.AddListener(OnNewGame);
        replaceButton.onClick.AddListener(OnReplace);
        quitButton.onClick.AddListener(OnQuit);

        OnNewGame();
    }

    public void OnNewGame()
    {
        game = new GameController();
        selectedIndices.Clear();

        game.StartGame();
        stateText.text = "New Game Started.";
        RefreshAllUI();

    }

    public void OnReplace()
    {
        if ( game == null) return;

        var ordered = selectedIndices.ToList();
        ordered.Sort();
        bool ok = game.SubmitSelection(ordered, out string message);
        stateText.text = message;

        slectedIndices.Clear();
        RefreshAllUI();

        if (game.State == GameState.Won)
        {
            statusText.text = "You win!";
        }
        else if (game.State == GameState.Lost)
        {
            statusText.text = "You lose!";
        }
    }

    public void OnQuit()
    {
        Debug.Log("Quit Game");

        #if UNITY_EDITOR
        UnityEngine.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    public void OnCardClicked(int index)
    {
        if (game == null) return;
        if (game.State != GameState.Running) return;
        if (selectedIndices.Contains(index))
        {
            selectedIndices.Remove(index);
        }
        else
        {
            selectedIndices.Add(index);
        }
        RefreshCardsSelections();
    }

    public void RefreshAllUI()
    {
        RefreshBoard();
        RefreshCardsSelections();
        RefreshInfo();
    }
    private void RefreshBoard()
    {
        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (game != null && i < game.Table.Cards.Count())
            {
                Card card = game.Table.GetCardAt(i);
                cardSlots[i].SetCard(card, i, this);
            }
            else
            {
                cardSlots[i].Clear();
            }
        }

    }
    private void RefreshCardsSelections()
    {
        for (int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].SetSelected(selectedIndices.Contains(i));

        }

    }
    private void RefreshInfo()
    {
        if (game == null)
        {
            stateText.text = "State: Not Started";
            deckText.text = "Deck 52";
            return;
        }
        stateText.text = $"State: {game.State}";
        deckText.text = $"Deck: {game.Deck.Count()}";
    }
}
