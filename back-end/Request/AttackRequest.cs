namespace back_end.Request
{
    public class AttackRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int BoardSlotId { get; set; }  // Emplacement de la carte qui attaque
        public int TargetBoardSlotId { get; set; }  // Carte cible
    }
}
