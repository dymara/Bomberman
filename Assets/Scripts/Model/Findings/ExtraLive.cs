namespace Assets.Scripts.Model.Findings
{
    public class ExtraLive : AbstractFinding
    {
        protected override void PowerUp(Player player)
        {
            player.remainingLives++;
        }
    }
}
