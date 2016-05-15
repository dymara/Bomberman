namespace Assets.Scripts.Model.Findings
{
    public class ExtraLive : Finding
    {
        protected override void PowerUp(Player player)
        {
            player.remainingLives++;
        }
    }
}
