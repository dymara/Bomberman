using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.Model.Findings
{
    public class FasterMoving : Finding
    {
        protected override void PowerUp(Player player)
        {
            FirstPersonController fpc = player.gameObject.GetComponent<FirstPersonController>();
            fpc.SetWalkSpeed(fpc.GetWalkSpeed() * GameManager.instance.GetSpeedMultiplier());
            player.speed *= GameManager.instance.GetSpeedMultiplier();
        }
    }
}
