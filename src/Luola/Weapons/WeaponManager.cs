#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Luola.Entities;

namespace Luola.Weapons
{
    public class WeaponManager
    {
        private readonly Random _random;
        private readonly float _totalChanceToAppear;
        private readonly Dictionary<string, Tuple<float, Type>> _weaponTypes;

        public WeaponManager()
        {
            _totalChanceToAppear = 0;
            _random = new Random();
            _weaponTypes = new Dictionary<string, Tuple<float, Type>>();
            var weapons = Assembly.GetAssembly(typeof(Weapon)).GetTypes().Where(t => t.IsSubclassOf(typeof(Weapon)));
            foreach (var weapon in weapons)
            {
                var name = weapon.GetField("Name").GetValue(null) as string;
                var chance = (float) weapon.GetField("ChanceToAppear").GetValue(null);
                if (name != null)
                {
                    _weaponTypes[name] = new Tuple<float, Type>(chance, weapon);
                    _totalChanceToAppear += chance;
                }
            }
        }

        public string RandomWeaponName()
        {
            var choice = (float) _random.NextDouble()*_totalChanceToAppear;
            var total = 0f;
            foreach (var tuple in _weaponTypes)
            {
                total += tuple.Value.Item1;
                if (choice < total)
                    return tuple.Key;
            }

            return null;
        }

        public Weapon InitWeapon(string name, Ship ship)
        {
            var cls = _weaponTypes[name].Item2;
            var ctor = cls.GetConstructor(new[] {typeof(Ship)});

            if (ctor == null)
                return null;

            return (Weapon) ctor.Invoke(new object[] {ship});
        }
    }
}