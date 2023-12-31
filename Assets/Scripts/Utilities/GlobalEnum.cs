namespace GlobalEnum
{
    public enum BattleResult
    {
        LOSE,
        WIN
    }

    public enum CardSuit
    {
        Diamonds,
        Club,
        Hearts,
        Spades
    }

    public enum CardRank
    {
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace,
        Two
    }

    public enum CombinationType
    {
        None,
        Single,
        Pair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOFAKind,
        StraightFlush,
        RoyalFlush
    }

    public enum PlayerEmotion
    {
        IDLE,
        SMILE,
        SAD
    }
}