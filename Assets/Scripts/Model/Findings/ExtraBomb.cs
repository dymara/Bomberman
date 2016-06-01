namespace Assets.Scripts.Model.Findings
{
    public class ExtraBomb : AbstractFinding
    {
        protected override void PowerUp(Player player)
        {
            player.bombs++;
            player.maximumBombsCount++;
        }
    }
}
