using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.Model.Findings
{
    public class FasterMoving : AbstractFinding
    {
        protected override void PowerUp(Player player)
        {
            PSMoveFirstPersonController fpc = player.gameObject.GetComponent<PSMoveFirstPersonController>();
            fpc.SetWalkSpeed(fpc.GetWalkSpeed() * GameManager.instance.GetSpeedMultiplier());
            player.speed *= GameManager.instance.GetSpeedMultiplier();
        }
    }
}
