using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private List<Player> players;
    [SerializeField] private List<Card> initialDeck = new List<Card>();
    [SerializeField] private List<Card> lastCardInTable = new List<Card>();
    [SerializeField] private Transform tableParent;

    [SerializeField] private CardFactory cardFactory;
    [SerializeField] private PlayerInputController inputController;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Image gameOverAvatar;
    [SerializeField] private TextMeshProUGUI gameOverTitle;

    private static readonly List<string> names = new List<string>
    {
        "Alex",
        "Sam",
        "Max",
        "Charlie",
        "Taylor",
        "Jordan",
        "Jamie",
        "Morgan",
        "Casey",
        "Riley",
        "Quinn",
        "Skyler",
        "Dakota",
        "Peyton",
        "Drew",
        "Cameron",
        "Logan",
        "Avery",
        "Harper",
        "Emerson"
    };

    [SerializeField] private State currentState;
    enum State
    {
        Initializing,
        PlayerTurn,
        BotTurn,
        RoundEnd,
        Scoring,
        GameEnd
    }

    private int currentPlayerIndex = 0;
    private int passCount = 0;
    private Player roundWinner = null;
    private const int CARDS_PER_PLAYER = 13;

    // Start is called before the first frame update
    void Start()
    {
        SetState(State.Initializing);
    }

    private void InitializeGame()
    {
        // initialize players
        InitializePlayer();

        // initiate the deck
        initialDeck = cardFactory.CreateDeck();

        // shuffle the deck
        ShuffleDeck();

        // distribute cards to the player
        DistributeCards();
    }

    private void InitializePlayer()
    {
        for(int i = 0; i < players.Count; i++)
        {
            if(i > 0)
            {
                // this is bot
                string botName = names[Random.Range(0, names.Count)];
                int botMoney = Random.Range(100000, 500000);

                players[i].Initialize(botName, false, botMoney, PlayerInfo.Instance.GetAllAvatarData()[i].avatarId);
            }
            else
            {
                players[i].Initialize(PlayerInfo.Instance.GetPlayerId, true, PlayerInfo.Instance.GetPlayerCash(), PlayerInfo.Instance.GetPlayerAvatarId());
            }
        }
    }
    
    private void DistributeCards()
    {
        int currentPlayerIndex = 0;
        foreach (Card card in initialDeck)
        {
            players[currentPlayerIndex].ReceiveCard(card);
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

            if (currentPlayerIndex == 0 && players[0].Hand.Count == CARDS_PER_PLAYER)
            {
                break;
            }
        }

        initialDeck.RemoveRange(0, CARDS_PER_PLAYER * players.Count);

        for(int i = 0; i < players.Count; i++)
        {
            players[i].SortCard();
        }
    }

    private void SetNextPlayerTurn()
    {
        do
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        } while (players[currentPlayerIndex].isPass);

        if (!players[currentPlayerIndex].IsHuman)
        {
            StartCoroutine(BotTurn());
        }
        else
        {
            ChangeStateToPlayerTurn();
        }
    }

    private IEnumerator BotTurn()
    {
        ChangeStateToBotTurn();

        yield return new WaitForSeconds(Random.Range(1f, 2f));

        BotMakeMove(players[currentPlayerIndex]);
    }

    private void BotMakeMove(Player botPlayer)
    {
        var cardsOnTable = GetCardInTheTable();
        var playableCards = botPlayer.GetPlayableCards(cardsOnTable);

        if (playableCards.Any())
        {
            bool isPairOnTable = cardsOnTable.Count == 2 && cardsOnTable[0].Rank == cardsOnTable[1].Rank;
            List<Card> cardsToSubmit = new List<Card>();

            if (isPairOnTable)
            {
                var pairs = playableCards.GroupBy(c => c.Rank).FirstOrDefault(g => g.Count() >= 2);
                if (pairs != null)
                {
                    cardsToSubmit = pairs.Take(2).ToList();
                }
                else
                {
                    botPlayer.PlayerPass();
                }
            }
            else
            {
                cardsToSubmit = new List<Card> { playableCards.First() };
            }

            SubmitCard(cardsToSubmit);
        }
        else
        {
            botPlayer.PlayerPass();
        }
    }

    private void ChangeStateToBotTurn()
    {
        SetState(State.BotTurn);
    }

    private void ChangeStateToPlayerTurn()
    {
        SetState(State.PlayerTurn);
    }

    void SetState(State newState)
    {
        currentState = newState;
        OnStateChange(newState);
    }

    void OnStateChange(State newState)
    {
        switch (newState)
        {
            case State.Initializing:
                inputController.EnableButton(false);
                InitializeGame();
                SetState(State.PlayerTurn);
                break;
            case State.PlayerTurn:
                inputController.EnableButton(true);
                break;
            case State.BotTurn:
                inputController.EnableButton(false);
                break;
            case State.RoundEnd:
                break;
            case State.Scoring:
                CalculateScores();
                break;
            case State.GameEnd:
                EndGame();
                break;
            default:
                break;
        }
    }

    public void SubmitCard(List<Card> cardTobeSubmitted)
    {
            for(int i = 0 ; i < lastCardInTable.Count; i++)
            {
                lastCardInTable[i].gameObject.SetActive(false);
            }

            lastCardInTable = cardTobeSubmitted;

            for(int i = 0; i < lastCardInTable.Count; i++)
            {
                lastCardInTable[i].transform.SetParent(tableParent);
                lastCardInTable[i].FlipCard(false);
                lastCardInTable[i].ResetCard();
            }

            if(players[currentPlayerIndex].Hand.Count == 0)
            {
                // Game End
                SetState(State.Scoring);
            }
            else
            {
                // pass to next
                SetNextPlayerTurn();
            }
    }

    private void CalculateScores()
    {
        for(int i = 0; i < players.Count; i++)
        {
            if(i != currentPlayerIndex)
            {
                players[currentPlayerIndex].AddMoney(players[i].Bet);
                players[i].PenaltyMoney();
            }
        }

        SetState(State.GameEnd);
    }

    private void EndGame()
    {
        gameOverPanel.SetActive(true);

        if(players[currentPlayerIndex].IsHuman)
        {
            gameOverTitle.text = "YOU WIN";
            gameOverAvatar.sprite = PlayerInfo.Instance.GetSmileFaceById(PlayerInfo.Instance.GetPlayerAvatarId());
            PlayerInfo.Instance.AddMatchHistory(GlobalEnum.BattleResult.WIN);
        }
        else
        {
            // player lose
            gameOverTitle.text = "YOU LOSE";
            gameOverAvatar.sprite = PlayerInfo.Instance.GetSadFaceById(PlayerInfo.Instance.GetPlayerAvatarId());
            PlayerInfo.Instance.AddMatchHistory(GlobalEnum.BattleResult.LOSE);
        }
        StartCoroutine(GoToMainMenu());
    }

    private IEnumerator GoToMainMenu()
    {
        yield return new WaitForSeconds(3);
        SceneController.Instance.LoadScene("Main Menu");
    }

    public void PlayerPass(Player player)
    {
        passCount++;

        if (passCount >= 3)
        {
            roundWinner = GetPlayerWhoHasNotPassed();
            EndRound();
        }
        else
        {
            AdvanceToNextPlayerTurn();
        }
    }

    private Player GetPlayerWhoHasNotPassed()
    {
        return players.FirstOrDefault(p => !p.isPass);
    }

    private void EndRound()
    {
        SetState(State.RoundEnd);
        foreach (var player in players)
        {
            player.isPass = false;
        }

        passCount = 0;

        if (roundWinner.Hand.Count == 0)
        {
            DeclareWinner(roundWinner);
        }
        else
        {
            StartNewRound();
        }

        for(int i = 0 ; i < lastCardInTable.Count; i++)
        {
            lastCardInTable[i].gameObject.SetActive(false);
        }
    }

    private void AdvanceToNextPlayerTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        if (players[currentPlayerIndex].isPass)
        {
            PlayerPass(players[currentPlayerIndex]);
        }
        else
        {
            ChangeStateToPlayerOrBotTurn();
        }
    }

    private void ChangeStateToPlayerOrBotTurn()
    {
        if (players[currentPlayerIndex].IsHuman)
        {
            ChangeStateToPlayerTurn();

        }
        else
        {
            StartCoroutine(BotTurn());
        }
    }

    private void DeclareWinner(Player player)
    {
        Debug.Log("Round Winner: " + player.Name);
    }

    private void StartNewRound()
    {
        currentPlayerIndex = 0;
        lastCardInTable.Clear();
        ChangeStateToPlayerOrBotTurn();
    }

    public List<Card> GetCardInTheTable()
    {
        return lastCardInTable;
    }

    private void ShuffleDeck()
    {
        int n = initialDeck.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Card value = initialDeck[k];
            initialDeck[k] = initialDeck[n];
            initialDeck[n] = value;
        }
    }
}
