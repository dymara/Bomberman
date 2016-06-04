namespace Assets.Scripts.Model.Findings
{
    public class ExtraLife : AbstractFinding
    {
        protected override void PowerUp(Player player)
        {
            player.remainingLives++;
        }
    }
}
