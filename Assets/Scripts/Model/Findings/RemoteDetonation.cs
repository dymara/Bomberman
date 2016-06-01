namespace Assets.Scripts.Model.Findings
{
    public class RemoteDetonation : AbstractFinding
    {
        protected override void PowerUp(Player player)
        {
            player.remoteDetonationBonus = true;
        }
    }
}
