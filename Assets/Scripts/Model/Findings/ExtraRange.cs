namespace Assets.Scripts.Model.Findings
{
    public class ExtraRange : AbstractFinding
    {
        protected override void PowerUp(Player player)
        {
            player.bombRange++;
        }
    }
}
