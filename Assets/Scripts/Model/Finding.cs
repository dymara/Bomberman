namespace Assets.Scripts.Model
{
    public class Finding : StaticGameObject
    {
        public override void OnExplode()
        {
            Destroy(this.gameObject);
        }
    }
}
