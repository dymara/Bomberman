namespace Assets.Scripts.Model.Findings
{
    public class ExtraRange : Finding
    {
        protected override void PowerUp(Player player)
        {
            player.bombRange++;
        }
    }
}
