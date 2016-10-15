using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Luola.Entities;
using Luola.Weapons;

namespace luola.Weapons
{
    public class WeaponManager
    {
        private readonly Dictionary<String, Tuple<float, Type>> _weaponTypes;
        private readonly Random _random;
        private float _totalChanceToAppear;

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
            ConstructorInfo ctor = cls.GetConstructor(new[] {typeof(Ship)});

            if (ctor == null)
                return null;

            return (Weapon) ctor.Invoke(new object[] {ship});
        }
    }
}