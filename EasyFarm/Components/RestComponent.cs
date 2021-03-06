﻿/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;
using EasyFarm.Classes;
using FFACETools;

namespace EasyFarm.Components
{
    /// <summary>
    ///     Behavior for resting our character.
    /// </summary>
    public class RestComponent : MachineComponent
    {
        private readonly FFACE _fface;

        /// <summary>
        ///     Retrieves aggroing creature.
        /// </summary>
        private readonly UnitService _units;

        public RestComponent(FFACE fface)
        {
            _fface = fface;
            _units = new UnitService(fface);
        }

        /// <summary>
        ///     Does our player have a status effect that prevents him
        /// </summary>
        /// <returns></returns>
        public bool IsRestingBlocked
        {
            get
            {
                var restBlockingDebuffs = new List<StatusEffect>
                {
                    StatusEffect.Poison,
                    StatusEffect.Bio,
                    StatusEffect.Sleep,
                    StatusEffect.Sleep2,
                    StatusEffect.Poison,
                    StatusEffect.Petrification,
                    StatusEffect.Stun,
                    StatusEffect.Charm1,
                    StatusEffect.Charm2,
                    StatusEffect.Terror,
                    StatusEffect.Frost,
                    StatusEffect.Burn,
                    StatusEffect.Choke,
                    StatusEffect.Rasp,
                    StatusEffect.Shock,
                    StatusEffect.Drown,
                    StatusEffect.Dia,
                    StatusEffect.Requiem,
                    StatusEffect.Lullaby
                };

                return restBlockingDebuffs.Intersect(_fface.Player.StatusEffects).Count() != 0;
            }
        }

        /// <summary>
        ///     Determines if we should rest up our health or magic.
        /// </summary>
        /// <returns></returns>
        public override bool CheckComponent()
        {
            // Check for effects taht stop resting. 
            if (ProhibitEffects.ProhibitEffectsDots
                .Intersect(_fface.Player.StatusEffects).Any()) return false;

            // Do not rest if we are being attacked. 
            if (_units.HasAggro) return false;

            // Check if we are fighting. 
            if (_fface.Player.Status == Status.Fighting) return false;

            // Check if we should rest for health.
            if (ShouldRestForHealth(
                _fface.Player.HPPCurrent,
                _fface.Player.Status)) return true;

            // Check if we should rest for magic. 
            if (ShouldRestForMagic(
                _fface.Player.MPPCurrent,
                _fface.Player.Status)) return true;

            // We do not meet the conditions for resting. 
            return false;
        }

        /// <summary>
        ///     Begin resting our health and magic.
        /// </summary>
        public override void RunComponent()
        {
            RestingUtils.Rest(_fface);
        }

        /// <summary>
        ///     Force the player to stand up before attempting anything else.
        /// </summary>
        public override void ExitComponent()
        {
            while (_fface.Player.Status == Status.Healing)
            {
                RestingUtils.Stand(_fface);
            }
        }

        /// <summary>
        ///     Tells us when we should rest mp.
        /// </summary>
        /// <param name="magic"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool ShouldRestForMagic(int magic, Status status)
        {
            return (Config.Instance.IsMagicEnabled &&
                    (IsMagicLow(magic) || !IsMagicHigh(magic) && status == Status.Healing));
        }

        /// <summary>
        ///     Given a value, is our mp low?
        /// </summary>
        /// <param name="magic"></param>
        /// <returns></returns>
        public bool IsMagicLow(int magic)
        {
            return magic <= Config.Instance.LowMagic;
        }

        /// <summary>
        ///     Given a value, is our mp high?
        /// </summary>
        /// <param name="magic"></param>
        /// <returns></returns>
        public bool IsMagicHigh(int magic)
        {
            return magic >= Config.Instance.HighMagic;
        }

        /// <summary>
        ///     Should we rest up our magic?
        /// </summary>
        /// <param name="health"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool ShouldRestForHealth(int health, Status status)
        {
            // Rest while low and while not high
            return (Config.Instance.IsHealthEnabled &&
                    (IsHealthLow(health) || !IsHealthHigh(health) && status == Status.Healing));
        }

        /// <summary>
        ///     Given a value, is our health low?
        /// </summary>
        /// <param name="health"></param>
        /// <returns></returns>
        public bool IsHealthLow(int health)
        {
            return health <= Config.Instance.LowHealth;
        }

        /// <summary>
        ///     Given a value, is our health high?
        /// </summary>
        /// <param name="health"></param>
        /// <returns></returns>
        public bool IsHealthHigh(int health)
        {
            return health >= Config.Instance.HighHealth;
        }
    }
}