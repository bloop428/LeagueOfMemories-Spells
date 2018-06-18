using LeagueSandbox.GameServer.Logic.API;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.Logic.Scripting.CSharp;
using System.Collections.Generic;
using System.Numerics;

namespace Spells
{
    class EvelynnQ : GameScript
    {
        public void ApplyEffects(Champion owner, AttackableUnit target, Spell spell, Projectile projectile)
        {
            if (owner.Team != target.Team)
            {
                var adScaling = (new float[] { .50f, .55f, .60f, .65f, .70f })[spell.Level - 1] * (owner.GetStats().AttackDamage.Total - owner.GetStats().AttackDamage.BaseValue);
                var apScaling = (new float[] { .35f, .40f, .45f, .50f, .55f })[spell.Level - 1] * owner.GetStats().AbilityPower.Total;
                var baseDamage = (new float[] { 30f, 45f, 60f, 75f, 90f })[spell.Level - 1];
                var damage = adScaling + apScaling + baseDamage;
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
        }

        public void OnActivate(Champion owner)
        {
        }

        public void OnDeactivate(Champion owner)
        {
        }

        public void OnFinishCasting(Champion owner, Spell spell, AttackableUnit target)
        {
            var ZoneCenter = new Target(owner.X, owner.Y);
            List<AttackableUnit> units = ApiFunctionManager.GetUnitsInRange(ZoneCenter, 300, true);

            if (units.Count > 1)
            {
                spell.spellAnimation("SPELL1", owner);

                var closest = units[1];
                var current = new Vector2(owner.X, owner.Y);
                var to = Vector2.Normalize(new Vector2(closest.X, closest.Y) - current);
                var range = to * 500;
                var trueCoords = current + range;
                ZoneCenter = new Target(trueCoords.X, trueCoords.Y);

                spell.AddProjectile("EvelynnQ", trueCoords.X, trueCoords.Y, true);
                Particle p = ApiFunctionManager.AddParticleTarget(owner, "Evelynn_Q_mis.troy", target);
            }
        }

        public void OnStartCasting(Champion owner, Spell spell, AttackableUnit target)
        {
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
