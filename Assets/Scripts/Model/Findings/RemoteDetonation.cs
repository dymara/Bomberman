namespace Assets.Scripts.Model.Findings
{
    public class RemoteDetonation : Finding
    {
        protected override void PowerUp(Player player)
        {
            player.remoteDetonationBonus = true;
        }
    }
}
