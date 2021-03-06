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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FFACETools;
using Parsing.Abilities;
using Parsing.Types;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     Executor targeted or buffing actions. A fuller explanation
    ///     can be found here: https://github.com/EasyFarm/EasyFarm/wiki/Battle-List-Roles.
    ///     See "list types" for more details.
    /// </summary>
    public class Executor
    {
        #region Constructors

        public Executor(FFACE fface)
        {
            _fface = fface;
            _caster = new Caster(fface);
        }

        #endregion

        #region Member Variables

        private readonly FFACE _fface;
        private readonly Caster _caster;

        #endregion

        #region Buffing Methods

        /// <summary>
        ///     Executes moves without the need for a target.
        /// </summary>
        /// <param name="actions"></param>
        public void UseBuffingActions(IEnumerable<BattleAbility> actions)
        {
            if (actions == null) throw new ArgumentNullException("actions");

            var castables = actions.ToList();

            while (castables.Count > 0)
            {
                foreach (var action in castables.ToList())
                {
                    if (!ActionFilters.BuffingFilter(_fface, action))
                    {
                        castables.Remove(action);
                        continue;
                    }

                    // Try to cast the spell. On failure, 
                    // continue and recast at a later time. 
                    if (!_caster.CastSpell(action)) continue;

                    // Remove spell from castables so that it 
                    // will not be casted again. 
                    castables.Remove(action);

                    // Increase usage count to limit number of usages. 
                    action.Usages++;

                    // Sleep until a spell is recastable. 
                    Thread.Sleep(Config.Instance.GlobalCooldown);
                }
            }
        }

        /// <summary>
        ///     Execute actions that are not target oriented.
        /// </summary>
        /// <param name="actions"></param>
        public void UseBuffingActions(IEnumerable<Ability> actions)
        {
            if (actions == null) throw new ArgumentNullException("actions");
            UseBuffingActions(actions.Select(x => new BattleAbility {Ability = x}));
        }

        /// <summary>
        ///     Execute a signle buffing type action.
        /// </summary>
        /// <param name="action"></param>
        public void UseBuffingAction(BattleAbility action)
        {
            if (action == null) throw new ArgumentNullException("action");
            UseBuffingActions(new List<BattleAbility> {action});
        }

        /// <summary>
        ///     Execute a single buffing type action.
        /// </summary>
        /// <param name="action"></param>
        public void UseBuffingAction(Ability action)
        {
            if (action == null) throw new ArgumentNullException("action");

            // Create new new ability and set its basic required information. 
            var baction = new BattleAbility
            {
                Name = action.English,
                IsEnabled = true,
                Ability = action
            };

            // Convert ability to new battleability object. 
            UseBuffingActions(new List<BattleAbility> {baction});
        }

        #endregion

        #region Targeted Methods

        /// <summary>
        ///     Execute targeted actions.
        /// </summary>
        /// <param name="actions"></param>
        /// <param name="target"></param>
        public void UseTargetedActions(IEnumerable<BattleAbility> actions, Unit target)
        {
            // Logic error to call this without setting a target first. 
            if (actions == null) throw new ArgumentNullException("actions");
            if (target == null) throw new ArgumentNullException("target");

            foreach (var action in actions)
            {
                MoveIntoActionRange(target, action);

                // Face unit
                _fface.Navigator.FaceHeading(target.Position);

                // Target mob if not currently targeted. 
                SetTarget(target);

                if (CompositeAbilityTypes.IsSpell.HasFlag(action.Ability.AbilityType))
                {
                    _caster.CastSpell(action);
                }
                else
                {
                    _caster.CastAbility(action);
                }

                // Increase usage count to limit number of usages. 
                action.Usages++;
                Thread.Sleep(Config.Instance.GlobalCooldown);
            }
        }

        /// <summary>
        ///     Place cursor on unit
        /// </summary>
        /// <param name="target"></param>
        private void SetTarget(Unit target)
        {
            if (target.Id != _fface.Target.ID)
            {
                _fface.Target.SetNPCTarget(target.Id);
                _fface.Windower.SendString("/ta <t>");
            }
        }

        /// <summary>
        ///     Move close enough to mob to use an ability.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="action"></param>
        private void MoveIntoActionRange(Unit target, BattleAbility action)
        {
            // Move to target if out of distance. 
            if (target.Distance > action.Distance)
            {
                // Move to unit at max buff distance. 
                var oldTolerance = _fface.Navigator.DistanceTolerance;
                _fface.Navigator.DistanceTolerance = action.Distance;
                _fface.Navigator.GotoNPC(target.Id);
                _fface.Navigator.DistanceTolerance = oldTolerance;
            }
        }

        /// <summary>
        ///     Execute a single action targeted type action.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="target"></param>
        public void UseTargetedAction(BattleAbility action, Unit target)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (action == null) throw new ArgumentNullException("action");
            UseTargetedActions(new List<BattleAbility> {action}, target);
        }

        #endregion
    }
}