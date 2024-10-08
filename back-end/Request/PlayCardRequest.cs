namespace back_end.Request
{
    public class PlayCardRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int CardId { get; set; }

        public int BoardSlotIndex { get; set; }

    }
}