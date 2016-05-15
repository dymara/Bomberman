namespace Assets.Scripts.Model.Findings
{
    public class ExtraBomb : Finding
    {
        protected override void PowerUp(Player player)
        {
            player.bombs++;
            player.maximumBombsCount++;
        }
    }
}
