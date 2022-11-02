using ShockwaveGORN.Hooks;
using UnityEngine;

namespace ShockwaveGORN.Managers
{
    internal static class M_Player
    {
        internal static void OnHeartBeat()
        {
            if (!Config.General.Allow_Player_Communication)
                return;
            M_HapticFeedback.heartBeat.Play();
        }

        internal static void HitSomething(DamagerRigidbody damager, Collision collision)
        {
            if (!Config.General.Allow_Player_Communication)
                return;

            GrabHand grabHand = damager.weaponBase.grabbedByHand;
            if (!grabHand.isPlayer
                || (grabHand.ownerFist == null))
                return;

            Vector3 velocity = collision.relativeVelocity - (Vector3)H_DamagerRigidbody.DamagerRigidbody_velocity.GetValue(damager);

            if (damager.isPlayerFist)
                M_Enemy.OnFistDamage(grabHand.ownerFist, velocity, DamageType.Blunt);
            else
                M_Enemy.OnWeaponDamage(damager.weaponBase, velocity, DamageType.Blunt, collision);
        }
    }
}