using System;
using System.Linq;

namespace VenomCrosshairs
{
    public static class TF2Weapons
    {
        // Classes
        private static readonly string[] tf2Classes = { "Scout", "Soldier", "Pyro", "Demoman", "Heavy", "Engineer", "Medic", "Sniper", "Spy" };

        // Scout
        private static readonly string[] tf2ScoutWeapons = { "Scattergun, Back Scatter, Force-A-Nature", "Baby Face's Blaster", "Shortstop", "Soda Popper", "Pistol and all reskins (Scout)", "Bonk! Atomic Punch, Crit-a-Cola", "Flying Guillotine, Mad Milk, Gas Passer, Jarate", "Pretty Boy's Pocket Pistol, Winger", "Bat and all reskins, Atomizer, Boston Basher, Candy Cane, Fan O'War, Sun-on-a-Stick", "Holy Mackerel", "Sandman", "Wrap Assassin" };
        private static readonly string[] tf2PrimaryScoutWeapons = { "Scattergun, Back Scatter, Force-A-Nature", "Baby Face's Blaster", "Shortstop", "Soda Popper" };
        private static readonly string[] tf2SecondaryScoutWeapons = { "Pistol and all reskins (Scout)", "Bonk! Atomic Punch, Crit-a-Cola", "Flying Guillotine, Mad Milk, Gas Passer, Jarate", "Pretty Boy's Pocket Pistol, Winger" };
        private static readonly string[] tf2MeleeScoutWeapons = { "Bat and all reskins, Atomizer, Boston Basher, Candy Cane, Fan O'War, Sun-on-a-Stick", "Holy Mackerel", "Sandman", "Wrap Assassin" };
        private static readonly string[] tf2MiscScoutWeapons = { };

        // Soldier
        private static readonly string[] tf2SoldierWeapons = { "Rocket Launcher, Black Box, Original, Liberty Launcher, Beggar's Bazooka", "Air Strike", "Cow Mangler 5000", "Direct Hit", "Shotgun, Reserve Shooter, Panic Attack (Soldier)", "Buff Banner, Battalion's Backup, Concheror", "Righteous Bison", "Shovel and all reskins, Equalizer, Pain Train, Disciplinary Action, Market Gardener, Escape Plan", "Half-Zatoichi" };
        private static readonly string[] tf2PrimarySoldierWeapons = { "Rocket Launcher, Black Box, Original, Liberty Launcher, Beggar's Bazooka", "Air Strike", "Cow Mangler 5000", "Direct Hit" };
        private static readonly string[] tf2SecondarySoldierWeapons = { "Shotgun, Reserve Shooter, Panic Attack (Soldier)", "Buff Banner, Battalion's Backup, Concheror", "Righteous Bison" };
        private static readonly string[] tf2MeleeSoldierWeapons = { "Shovel and all reskins, Equalizer, Pain Train, Disciplinary Action, Market Gardener, Escape Plan", "Half-Zatoichi" };
        private static readonly string[] tf2MiscSoldierWeapons = { };

        // Pyro
        private static readonly string[] tf2PyroWeapons = { "Flame Thrower and all reskins, Backburner, Degreaser, Phlogistinator", "Dragon's Fury", "Shotgun, Reserve Shooter, Panic Attack (Pyro)", "Flare Gun, Detonator, Scorch Shot", "Flying Guillotine, Mad Milk, Gas Passer, Jarate", "Manmelter", "Thermal Thruster", "Fire Axe and all reskins, Lollichop, Axtinguisher, Homewrecker, Powerjack, Back Scratcher, Sharpened Volcano Fragment, Third Degree, Neon Annihilator", "Hot Hand" };
        private static readonly string[] tf2PrimaryPyroWeapons = { "Flame Thrower and all reskins, Backburner, Degreaser, Phlogistinator", "Dragon's Fury" };
        private static readonly string[] tf2SecondaryPyroWeapons = { "Shotgun, Reserve Shooter, Panic Attack (Pyro)", "Flare Gun, Detonator, Scorch Shot", "Flying Guillotine, Mad Milk, Gas Passer, Jarate", "Manmelter", "Thermal Thruster" };
        private static readonly string[] tf2MeleePyroWeapons = { "Fire Axe and all reskins, Lollichop, Axtinguisher, Homewrecker, Powerjack, Back Scratcher, Sharpened Volcano Fragment, Third Degree, Neon Annihilator", "Hot Hand" };
        private static readonly string[] tf2MiscPyroWeapons = { };

        // Demoman
        private static readonly string[] tf2DemomanWeapons = { "Grenade Launcher, Loch-n-Load, Iron Bomber", "Loose Cannon", "Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher", "Bottle and all reskins", "Eyelander, Scotsman's Skullcutter, Claidheamh Mòr, Persian Persuader, Pain Train", "Half-Zatoichi", "Ullapool Caber" };
        private static readonly string[] tf2PrimaryDemomanWeapons = { "Grenade Launcher, Loch-n-Load, Iron Bomber", "Loose Cannon" };
        private static readonly string[] tf2SecondaryDemomanWeapons = { "Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher" };
        private static readonly string[] tf2MeleeDemomanWeapons = { "Bottle and all reskins", "Eyelander, Scotsman's Skullcutter, Claidheamh Mòr, Persian Persuader, Pain Train", "Half-Zatoichi", "Ullapool Caber" };
        private static readonly string[] tf2MiscDemomanWeapons = { };

        // Heavy
        private static readonly string[] tf2HeavyWeapons = { "Minigun, Natascha, Brass Beast, Tomislav, Huo-Long Heater", "Shotgun, Family Business, Panic Attack", "Sandvich, Dalokohs Bar, Fishcake, Buffalo Steak Sandvich, Second Banana", "Fists and all reskins, Killing Gloves of Boxing, Gloves of Running Urgently, Warrior's Spirit, Fists of Steel, Eviction Notice, Holiday Punch" };
        private static readonly string[] tf2PrimaryHeavyWeapons = { "Minigun, Natascha, Brass Beast, Tomislav, Huo-Long Heater" };
        private static readonly string[] tf2SecondaryHeavyWeapons = { "Shotgun, Family Business, Panic Attack", "Sandvich, Dalokohs Bar, Fishcake, Buffalo Steak Sandvich, Second Banana" };
        private static readonly string[] tf2MeleeHeavyWeapons = { "Fists and all reskins, Killing Gloves of Boxing, Gloves of Running Urgently, Warrior's Spirit, Fists of Steel, Eviction Notice, Holiday Punch" };
        private static readonly string[] tf2MiscHeavyWeapons = { };

        // Engineer
        private static readonly string[] tf2EngineerWeapons = { "Shotgun, Widowmaker, Panic Attack", "Frontier Justice", "Pomson 6000", "Rescue Ranger", "Pistol and all reskins (Engineer)", "Short Circuit", "Wrangler, Giger Counter", "Wrench, Southern Hospitality, Jag, Eureka Effect", "Gunslinger", "Construction PDA", "Destruction PDA", "Sapper, Red-Tape Recorder, While placing a building" };
        private static readonly string[] tf2PrimaryEngineerWeapons = { "Shotgun, Widowmaker, Panic Attack", "Frontier Justice", "Pomson 6000", "Rescue Ranger" };
        private static readonly string[] tf2SecondaryEngineerWeapons = { "Pistol and all reskins (Engineer)", "Short Circuit", "Wrangler, Giger Counter" };
        private static readonly string[] tf2MeleeEngineerWeapons = { "Wrench, Southern Hospitality, Jag, Eureka Effect", "Gunslinger" };
        private static readonly string[] tf2MiscEngineerWeapons = { "Construction PDA", "Destruction PDA", "Sapper, Red-Tape Recorder, While placing a building" };

        // Medic
        private static readonly string[] tf2MedicWeapons = { "Syringe Gun, Blutsauger, Overdose", "Crusader's Crossbow", "Medi Gun, Kritzkrieg, Quick-Fix, Vaccinator", "Bonesaw and all reskins, Ubersaw, Vita-Saw, Amputator, Solemn Vow" };
        private static readonly string[] tf2PrimaryMedicWeapons = { "Syringe Gun, Blutsauger, Overdose", "Crusader's Crossbow" };
        private static readonly string[] tf2SecondaryMedicWeapons = { "Medi Gun, Kritzkrieg, Quick-Fix, Vaccinator" };
        private static readonly string[] tf2MeleeMedicWeapons = { "Bonesaw and all reskins, Ubersaw, Vita-Saw, Amputator, Solemn Vow" };
        private static readonly string[] tf2MiscMedicWeapons = { };

        // Sniper
        private static readonly string[] tf2SniperWeapons = { "Sniper Rifle, Sydney Sleeper, Hitman's Heatmaker, Machina", "Bazaar Bargain", "Classic", "Huntsman, Fortified Compound", "SMG", "Cleaner's Carbine", "Flying Guillotine, Mad Milk, Gas Passer, Jarate", "Kukri and all reskins, Tribalman's Shiv, Bushwacka, Shahanshah" };
        private static readonly string[] tf2PrimarySniperWeapons = { "Sniper Rifle, Sydney Sleeper, Hitman's Heatmaker, Machina", "Bazaar Bargain", "Classic", "Huntsman, Fortified Compound" };
        private static readonly string[] tf2SecondarySniperWeapons = { "SMG", "Cleaner's Carbine", "Flying Guillotine, Mad Milk, Gas Passer, Jarate" };
        private static readonly string[] tf2MeleeSniperWeapons = { "Kukri and all reskins, Tribalman's Shiv, Bushwacka, Shahanshah" };
        private static readonly string[] tf2MiscSniperWeapons = { };

        // Spy
        private static readonly string[] tf2SpyWeapons = { "Revolver, Ambassador, L'Etranger, Enforcer, Diamondback", "Sapper, Red-Tape Recorder, While placing a building", "Knife and all reskins, Your Eternal Reward, Conniver's Kunai, Big Earner, Spy-cicle", "Disguise kit" };
        private static readonly string[] tf2PrimarySpyWeapons = { "Revolver, Ambassador, L'Etranger, Enforcer, Diamondback" };
        private static readonly string[] tf2SecondarySpyWeapons = { "Sapper, Red-Tape Recorder, While placing a building" };
        private static readonly string[] tf2MeleeSpyWeapons = { "Knife and all reskins, Your Eternal Reward, Conniver's Kunai, Big Earner, Spy-cicle" };
        private static readonly string[] tf2MiscSpyWeapons = { "Sapper, Red-Tape Recorder, While placing a building", "Disguise kit" };

        // Multi-class
        private static readonly string[] tf2MultiClassWeapons = { "Flying Guillotine, Mad Milk, Gas Passer, Jarate", "Half-Zatoichi", "Sapper, Red-Tape Recorder, While placing a building" };
        private static readonly string[] tf2PrimaryMultiClassWeapons = { };
        private static readonly string[] tf2SecondaryMultiClassWeapons = { "Flying Guillotine, Mad Milk, Gas Passer, Jarate", "Sapper, Red-Tape Recorder, While placing a building" };
        private static readonly string[] tf2MeleeMultiClassWeapons = { "Half-Zatoichi" };
        private static readonly string[] tf2MiscMultiClassWeapons = { "Sapper, Red-Tape Recorder, While placing a building" };

        // Explosive
        private static readonly string[] tf2ExplosiveWeapons = { "Rocket Launcher, Black Box, Original, Liberty Launcher, Beggar's Bazooka", "Air Strike", "Direct Hit", "Grenade Launcher, Loch-n-Load, Iron Bomber", "Loose Cannon", "Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher" };
        private static readonly string[] tf2PrimaryExplosiveWeapons = { "Rocket Launcher, Black Box, Original, Liberty Launcher, Beggar's Bazooka", "Air Strike", "Direct Hit", "Grenade Launcher, Loch-n-Load, Iron Bomber", "Loose Cannon" };
        private static readonly string[] tf2SecondaryExplosiveWeapons = { "Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher" };
        private static readonly string[] tf2MeleeExplosiveWeapons = { };
        private static readonly string[] tf2MiscExplosiveWeapons = { };

        // All
        private static readonly string[] tf2AllWeapons = tf2ScoutWeapons.Concat(tf2SoldierWeapons).Concat(tf2PyroWeapons).Concat(tf2DemomanWeapons).Concat(tf2HeavyWeapons).Concat(tf2EngineerWeapons).Concat(tf2MedicWeapons).Concat(tf2SniperWeapons).Concat(tf2SpyWeapons).ToArray();
        private static readonly string[] tf2AllPrimaryWeapons = tf2PrimaryScoutWeapons.Concat(tf2PrimarySoldierWeapons).Concat(tf2PrimaryPyroWeapons).Concat(tf2PrimaryDemomanWeapons).Concat(tf2PrimaryHeavyWeapons).Concat(tf2PrimaryEngineerWeapons).Concat(tf2PrimaryMedicWeapons).Concat(tf2PrimarySniperWeapons).Concat(tf2PrimarySpyWeapons).ToArray();
        private static readonly string[] tf2AllSecondaryWeapons = tf2SecondaryScoutWeapons.Concat(tf2SecondarySoldierWeapons).Concat(tf2SecondaryPyroWeapons).Concat(tf2SecondaryDemomanWeapons).Concat(tf2SecondaryHeavyWeapons).Concat(tf2SecondaryEngineerWeapons).Concat(tf2SecondaryMedicWeapons).Concat(tf2SecondarySniperWeapons).Concat(tf2SecondarySpyWeapons).ToArray();
        private static readonly string[] tf2AllMeleeWeapons = tf2MeleeScoutWeapons.Concat(tf2MeleeSoldierWeapons).Concat(tf2MeleePyroWeapons).Concat(tf2MeleeDemomanWeapons).Concat(tf2MeleeHeavyWeapons).Concat(tf2MeleeEngineerWeapons).Concat(tf2MeleeMedicWeapons).Concat(tf2MeleeSniperWeapons).Concat(tf2MeleeSpyWeapons).ToArray();
        private static readonly string[] tf2AllMiscWeapons = tf2MiscScoutWeapons.Concat(tf2MiscSoldierWeapons).Concat(tf2MiscPyroWeapons).Concat(tf2MiscDemomanWeapons).Concat(tf2MiscHeavyWeapons).Concat(tf2MiscEngineerWeapons).Concat(tf2MiscMedicWeapons).Concat(tf2MiscSniperWeapons).Concat(tf2MiscSpyWeapons).ToArray();

        // All scripts
        private static readonly string[] tf2AllWeaponScripts = { "tf_weapon_scattergun.txt", "tf_weapon_pep_brawler_blaster.txt", "tf_weapon_handgun_scout_primary.txt", "tf_weapon_soda_popper.txt", "tf_weapon_pistol_scout.txt", "tf_weapon_lunchbox_drink.txt", "tf_weapon_handgun_scout_secondary.txt", "tf_weapon_bat.txt", "tf_weapon_bat_fish.txt", "tf_weapon_bat_wood.txt", "tf_weapon_bat_giftwrap.txt", "tf_weapon_rocketlauncher.txt", "tf_weapon_rocketlauncher_airstrike.txt", "tf_weapon_particle_cannon.txt", "tf_weapon_rocketlauncher_directhit.txt", "tf_weapon_shotgun_soldier.txt", "tf_weapon_buff_item.txt", "tf_weapon_raygun.txt", "tf_weapon_shovel.txt", "tf_weapon_flamethrower.txt", "tf_weapon_rocketlauncher_fireball.txt", "tf_weapon_shotgun_pyro.txt", "tf_weapon_flaregun.txt", "tf_weapon_flaregun_revenge.txt", "tf_weapon_rocketpack.txt", "tf_weapon_fireaxe.txt", "tf_weapon_slap.txt", "tf_weapon_grenadelauncher.txt", "tf_weapon_cannon.txt", "tf_weapon_pipebomblauncher.txt", "tf_weapon_bottle.txt", "tf_weapon_sword.txt", "tf_weapon_stickbomb.txt", "tf_weapon_minigun.txt", "tf_weapon_shotgun_hwg.txt", "tf_weapon_lunchbox.txt", "tf_weapon_fists.txt", "tf_weapon_shotgun_primary.txt", "tf_weapon_sentry_revenge.txt", "tf_weapon_drg_pomson.txt", "tf_weapon_shotgun_building_rescue.txt", "tf_weapon_pistol.txt", "tf_weapon_mechanical_arm.txt", "tf_weapon_laser_pointer.txt", "tf_weapon_wrench.txt", "tf_weapon_robot_arm.txt", "tf_weapon_pda_engineer_build.txt", "tf_weapon_pda_engineer_destroy.txt", "tf_weapon_syringegun_medic.txt", "tf_weapon_crossbow.txt", "tf_weapon_medigun.txt", "tf_weapon_bonesaw.txt", "tf_weapon_sniperrifle.txt", "tf_weapon_sniperrifle_classic.txt", "tf_weapon_sniperrifle_decap.txt", "tf_weapon_compound_bow.txt", "tf_weapon_smg.txt", "tf_weapon_charged_smg.txt", "tf_weapon_club.txt", "tf_weapon_revolver.txt", "tf_weapon_knife.txt", "tf_weapon_pda_spy.txt", "tf_weapon_jar.txt", "tf_weapon_katana.txt", "tf_weapon_builder.txt" };

        public static string[] getAllWeapons()
        {
            return tf2AllWeapons;
        }

        public static string[] getAllWeapons(string tf2Class)
        {
            switch (tf2Class)
            {
                case "Scout":
                    return tf2ScoutWeapons;
                case "Soldier":
                    return tf2SoldierWeapons;
                case "Pyro":
                    return tf2PyroWeapons;
                case "Demoman":
                    return tf2DemomanWeapons;
                case "Heavy":
                    return tf2HeavyWeapons;
                case "Engineer":
                    return tf2EngineerWeapons;
                case "Medic":
                    return tf2MedicWeapons;
                case "Sniper":
                    return tf2SniperWeapons;
                case "Spy":
                    return tf2SpyWeapons;
                case "Multi-class":
                    return tf2MultiClassWeapons;
                default:
                    return new string[] { };
            }
        }

        public static string[] getExplosiveWeapons()
        {
            return tf2ExplosiveWeapons;
        }

        public static string[] getPrimaryWeapons()
        {
            return tf2AllPrimaryWeapons;
        }

        public static string[] getPrimaryWeapons(string tf2Class)
        {
            switch (tf2Class)
            {
                case "Scout":
                    return tf2PrimaryScoutWeapons;
                case "Soldier":
                    return tf2PrimarySoldierWeapons;
                case "Pyro":
                    return tf2PrimaryPyroWeapons;
                case "Demoman":
                    return tf2PrimaryDemomanWeapons;
                case "Heavy":
                    return tf2PrimaryHeavyWeapons;
                case "Engineer":
                    return tf2PrimaryEngineerWeapons;
                case "Medic":
                    return tf2PrimaryMedicWeapons;
                case "Sniper":
                    return tf2PrimarySniperWeapons;
                case "Spy":
                    return tf2PrimarySpyWeapons;
                case "Multi-class":
                    return tf2PrimaryMultiClassWeapons;
            }
            throw new ArgumentException($"'{tf2Class}' is not a Team Fortress 2 class!");
        }

        public static string[] getSecondaryWeapons()
        {
            return tf2AllSecondaryWeapons;
        }

        public static string[] getSecondaryWeapons(string tf2Class)
        {
            switch (tf2Class)
            {
                case "Scout":
                    return tf2SecondaryScoutWeapons;
                case "Soldier":
                    return tf2SecondarySoldierWeapons;
                case "Pyro":
                    return tf2SecondaryPyroWeapons;
                case "Demoman":
                    return tf2SecondaryDemomanWeapons;
                case "Heavy":
                    return tf2SecondaryHeavyWeapons;
                case "Engineer":
                    return tf2SecondaryEngineerWeapons;
                case "Medic":
                    return tf2SecondaryMedicWeapons;
                case "Sniper":
                    return tf2SecondarySniperWeapons;
                case "Spy":
                    return tf2SecondarySpyWeapons;
                case "Multi-class":
                    return tf2SecondaryMultiClassWeapons;
            }
            throw new ArgumentException($"'{tf2Class}' is not a Team Fortress 2 class!");
        }

        public static string[] getMeleeWeapons()
        {
            return tf2AllMeleeWeapons;
        }

        public static string[] getMeleeWeapons(string tf2Class)
        {
            switch (tf2Class)
            {
                case "Scout":
                    return tf2MeleeScoutWeapons;
                case "Soldier":
                    return tf2MeleeSoldierWeapons;
                case "Pyro":
                    return tf2MeleePyroWeapons;
                case "Demoman":
                    return tf2MeleeDemomanWeapons;
                case "Heavy":
                    return tf2MeleeHeavyWeapons;
                case "Engineer":
                    return tf2MeleeEngineerWeapons;
                case "Medic":
                    return tf2MeleeMedicWeapons;
                case "Sniper":
                    return tf2MeleeSniperWeapons;
                case "Spy":
                    return tf2MeleeSpyWeapons;
                case "Multi-class":
                    return tf2MeleeMultiClassWeapons;
            }
            throw new ArgumentException($"'{tf2Class}' is not a Team Fortress 2 class!");
        }

        public static string[] getMiscWeapons()
        {
            return tf2AllMiscWeapons;
        }

        public static string[] getMiscWeapons(string tf2Class)
        {
            switch (tf2Class)
            {
                case "Scout":
                    return tf2MiscScoutWeapons;
                case "Soldier":
                    return tf2MiscSoldierWeapons;
                case "Pyro":
                    return tf2MiscPyroWeapons;
                case "Demoman":
                    return tf2MiscDemomanWeapons;
                case "Heavy":
                    return tf2MiscHeavyWeapons;
                case "Engineer":
                    return tf2MiscEngineerWeapons;
                case "Medic":
                    return tf2MiscMedicWeapons;
                case "Sniper":
                    return tf2MiscSniperWeapons;
                case "Spy":
                    return tf2MiscSpyWeapons;
                case "Multi-class":
                    return tf2MiscMultiClassWeapons;
            }
            throw new ArgumentException($"'{tf2Class}' is not a Team Fortress 2 class!");
        }

        public static string[] getWeaponScripts()
        {
            return tf2AllWeaponScripts;
        }

        public static string getWeaponScriptFromWeaponName(string weaponName)
        {
            switch (weaponName)
            {
                // Scout
                case "Scattergun, Back Scatter, Force-A-Nature":
                    return "tf_weapon_scattergun.txt";
                case "Baby Face's Blaster":
                    return "tf_weapon_pep_brawler_blaster.txt";
                case "Shortstop":
                    return "tf_weapon_handgun_scout_primary.txt";
                case "Soda Popper":
                    return "tf_weapon_soda_popper.txt";
                case "Pistol and all reskins (Scout)":
                    return "tf_weapon_pistol_scout.txt";
                case "Bonk! Atomic Punch, Crit-a-Cola":
                    return "tf_weapon_lunchbox_drink.txt";
                case "Pretty Boy's Pocket Pistol, Winger":
                    return "tf_weapon_handgun_scout_secondary.txt";
                case "Bat and all reskins, Atomizer, Boston Basher, Candy Cane, Fan O'War, Sun-on-a-Stick":
                    return "tf_weapon_bat.txt";
                case "Holy Mackerel":
                    return "tf_weapon_bat_fish.txt";
                case "Sandman":
                    return "tf_weapon_bat_wood.txt";
                case "Wrap Assassin":
                    return "tf_weapon_bat_giftwrap.txt";

                // Soldier
                case "Rocket Launcher, Black Box, Original, Liberty Launcher, Beggar's Bazooka":
                    return "tf_weapon_rocketlauncher.txt";
                case "Air Strike":
                    return "tf_weapon_rocketlauncher_airstrike.txt";
                case "Cow Mangler 5000":
                    return "tf_weapon_particle_cannon.txt";
                case "Direct Hit":
                    return "tf_weapon_rocketlauncher_directhit.txt";
                case "Shotgun, Reserve Shooter, Panic Attack (Soldier)":
                    return "tf_weapon_shotgun_soldier.txt";
                case "Buff Banner, Battalion's Backup, Concheror":
                    return "tf_weapon_buff_item.txt";
                case "Righteous Bison":
                    return "tf_weapon_raygun.txt";
                case "Shovel and all reskins, Equalizer, Pain Train, Disciplinary Action, Market Gardener, Escape Plan":
                    return "tf_weapon_shovel.txt";

                // Pyro
                case "Flame Thrower and all reskins, Backburner, Degreaser, Phlogistinator":
                    return "tf_weapon_flamethrower.txt";
                case "Dragon's Fury":
                    return "tf_weapon_rocketlauncher_fireball.txt";
                case "Shotgun, Reserve Shooter, Panic Attack (Pyro)":
                    return "tf_weapon_shotgun_pyro.txt";
                case "Flare Gun, Detonator, Scorch Shot":
                    return "tf_weapon_flaregun.txt";
                case "Manmelter":
                    return "tf_weapon_flaregun_revenge.txt";
                case "Thermal Thruster":
                    return "tf_weapon_rocketpack.txt";
                case "Fire Axe and all reskins, Lollichop, Axtinguisher, Homewrecker, Powerjack, Back Scratcher, Sharpened Volcano Fragment, Third Degree, Neon Annihilator":
                    return "tf_weapon_fireaxe.txt";
                case "Hot Hand":
                    return "tf_weapon_slap.txt";

                // Demoman
                case "Grenade Launcher, Loch-n-Load, Iron Bomber":
                    return "tf_weapon_grenadelauncher.txt";
                case "Loose Cannon":
                    return "tf_weapon_cannon.txt";
                case "Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher":
                    return "tf_weapon_pipebomblauncher.txt";
                case "Bottle and all reskins":
                    return "tf_weapon_bottle.txt";
                case "Eyelander, Scotsman's Skullcutter, Claidheamh Mòr, Persian Persuader, Pain Train":
                    return "tf_weapon_sword.txt";
                case "Ullapool Caber":
                    return "tf_weapon_stickbomb.txt";

                // Heavy
                case "Minigun, Natascha, Brass Beast, Tomislav, Huo-Long Heater":
                    return "tf_weapon_minigun.txt";
                case "Shotgun, Family Business, Panic Attack":
                    return "tf_weapon_shotgun_hwg.txt";
                case "Sandvich, Dalokohs Bar, Fishcake, Buffalo Steak Sandvich, Second Banana":
                    return "tf_weapon_lunchbox.txt";
                case "Fists and all reskins, Killing Gloves of Boxing, Gloves of Running Urgently, Warrior's Spirit, Fists of Steel, Eviction Notice, Holiday Punch":
                    return "tf_weapon_fists.txt";

                // Engineer
                case "Shotgun, Widowmaker, Panic Attack":
                    return "tf_weapon_shotgun_primary.txt";
                case "Frontier Justice":
                    return "tf_weapon_sentry_revenge.txt";
                case "Pomson 6000":
                    return "tf_weapon_drg_pomson.txt";
                case "Rescue Ranger":
                    return "tf_weapon_shotgun_building_rescue.txt";
                case "Pistol and all reskins (Engineer)":
                    return "tf_weapon_pistol.txt";
                case "Short Circuit":
                    return "tf_weapon_mechanical_arm.txt";
                case "Wrangler, Giger Counter":
                    return "tf_weapon_laser_pointer.txt";
                case "Wrench, Southern Hospitality, Jag, Eureka Effect":
                    return "tf_weapon_wrench.txt";
                case "Gunslinger":
                    return "tf_weapon_robot_arm.txt";
                case "Construction PDA":
                    return "tf_weapon_pda_engineer_build.txt";
                case "Destruction PDA":
                    return "tf_weapon_pda_engineer_destroy.txt";

                // Medic
                case "Syringe Gun, Blutsauger, Overdose":
                    return "tf_weapon_syringegun_medic.txt";
                case "Crusader's Crossbow":
                    return "tf_weapon_crossbow.txt";
                case "Medi Gun, Kritzkrieg, Quick-Fix, Vaccinator":
                    return "tf_weapon_medigun.txt";
                case "Bonesaw and all reskins, Ubersaw, Vita-Saw, Amputator, Solemn Vow":
                    return "tf_weapon_bonesaw.txt";

                // Sniper
                case "Sniper Rifle, Sydney Sleeper, Hitman's Heatmaker, Machina":
                    return "tf_weapon_sniperrifle.txt";
                case "Bazaar Bargain":
                    return "tf_weapon_sniperrifle_decap.txt";
                case "Classic":
                    return "tf_weapon_sniperrifle_classic.txt";
                case "Huntsman, Fortified Compound":
                    return "tf_weapon_compound_bow.txt";
                case "SMG":
                    return "tf_weapon_smg.txt";
                case "Cleaner's Carbine":
                    return "tf_weapon_charged_smg.txt";
                case "Kukri and all reskins, Tribalman's Shiv, Bushwacka, Shahanshah":
                    return "tf_weapon_club.txt";

                // Spy
                case "Revolver, Ambassador, L'Etranger, Enforcer, Diamondback":
                    return "tf_weapon_revolver.txt";
                case "Knife and all reskins, Your Eternal Reward, Conniver's Kunai, Big Earner, Spy-cicle":
                    return "tf_weapon_knife.txt";
                case "Disguise kit":
                    return "tf_weapon_pda_spy.txt";

                // Multi-class
                case "Flying Guillotine, Mad Milk, Gas Passer, Jarate":
                    return "tf_weapon_jar.txt"; // Scout/Pyro/Sniper
                case "Half-Zatoichi":
                    return "tf_weapon_katana.txt"; // Soldier/Demoman
                case "Sapper, Red-Tape Recorder, While placing a building":
                    return "tf_weapon_builder.txt"; // Spy/Engineer
            }
            throw new ArgumentException($"'{weaponName}' does not have a weapon script!");
        }

        public static string getWeaponNameFromWeaponScript(string weaponScript)
        {
            switch (weaponScript)
            {
                // Scout
                case "tf_weapon_scattergun.txt":
                    return "Scattergun, Back Scatter, Force-A-Nature";
                case "tf_weapon_pep_brawler_blaster.txt":
                    return "Baby Face's Blaster";
                case "tf_weapon_handgun_scout_primary.txt":
                    return "Shortstop";
                case "tf_weapon_soda_popper.txt":
                    return "Soda Popper";
                case "tf_weapon_pistol_scout.txt":
                    return "Pistol and all reskins (Scout)";
                case "tf_weapon_lunchbox_drink.txt":
                    return "Bonk! Atomic Punch, Crit-a-Cola";
                case "tf_weapon_handgun_scout_secondary.txt":
                    return "Pretty Boy's Pocket Pistol, Winger";
                case "tf_weapon_bat.txt":
                    return "Bat and all reskins, Atomizer, Boston Basher, Candy Cane, Fan O'War, Sun-on-a-Stick";
                case "tf_weapon_bat_fish.txt":
                    return "Holy Mackerel";
                case "tf_weapon_bat_wood.txt":
                    return "Sandman";
                case "tf_weapon_bat_giftwrap.txt":
                    return "Wrap Assassin";

                // Soldier
                case "tf_weapon_rocketlauncher.txt":
                    return "Rocket Launcher, Black Box, Original, Liberty Launcher, Beggar's Bazooka";
                case "tf_weapon_rocketlauncher_airstrike.txt":
                    return "Air Strike";
                case "tf_weapon_particle_cannon.txt":
                    return "Cow Mangler 5000";
                case "tf_weapon_rocketlauncher_directhit.txt":
                    return "Direct Hit";
                case "tf_weapon_shotgun_soldier.txt":
                    return "Shotgun, Reserve Shooter, Panic Attack (Soldier)";
                case "tf_weapon_buff_item.txt":
                    return "Buff Banner, Battalion's Backup, Concheror";
                case "tf_weapon_raygun.txt":
                    return "Righteous Bison";
                case "tf_weapon_shovel.txt":
                    return "Shovel and all reskins, Equalizer, Pain Train, Disciplinary Action, Market Gardener, Escape Plan";

                // Pyro
                case "tf_weapon_flamethrower.txt":
                    return "Flame Thrower and all reskins, Backburner, Degreaser, Phlogistinator";
                case "tf_weapon_rocketlauncher_fireball.txt":
                    return "Dragon's Fury";
                case "tf_weapon_shotgun_pyro.txt":
                    return "Shotgun, Reserve Shooter, Panic Attack (Pyro)";
                case "tf_weapon_flaregun.txt":
                    return "Flare Gun, Detonator, Scorch Shot";
                case "tf_weapon_flaregun_revenge.txt":
                    return "Manmelter";
                case "tf_weapon_rocketpack.txt":
                    return "Thermal Thruster";
                case "tf_weapon_fireaxe.txt":
                    return "Fire Axe and all reskins, Lollichop, Axtinguisher, Homewrecker, Powerjack, Back Scratcher, Sharpened Volcano Fragment, Third Degree, Neon Annihilator";
                case "tf_weapon_slap.txt":
                    return "Hot Hand";

                // Demoman
                case "tf_weapon_grenadelauncher.txt":
                    return "Grenade Launcher, Loch-n-Load, Iron Bomber";
                case "tf_weapon_cannon.txt":
                    return "Loose Cannon";
                case "tf_weapon_pipebomblauncher.txt":
                    return "Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher";
                case "tf_weapon_bottle.txt":
                    return "Bottle and all reskins";
                case "tf_weapon_sword.txt":
                    return "Eyelander, Scotsman's Skullcutter, Claidheamh Mòr, Persian Persuader, Pain Train";
                case "tf_weapon_stickbomb.txt":
                    return "Ullapool Caber";

                // Heavy
                case "tf_weapon_minigun.txt":
                    return "Minigun, Natascha, Brass Beast, Tomislav, Huo-Long Heater";
                case "tf_weapon_shotgun_hwg.txt":
                    return "Shotgun, Family Business, Panic Attack";
                case "tf_weapon_lunchbox.txt":
                    return "Sandvich, Dalokohs Bar, Fishcake, Buffalo Steak Sandvich, Second Banana";
                case "tf_weapon_fists.txt":
                    return "Fists and all reskins, Killing Gloves of Boxing, Gloves of Running Urgently, Warrior's Spirit, Fists of Steel, Eviction Notice, Holiday Punch";

                // Engineer
                case "tf_weapon_shotgun_primary.txt":
                    return "Shotgun, Widowmaker, Panic Attack";
                case "tf_weapon_sentry_revenge.txt":
                    return "Frontier Justice";
                case "tf_weapon_drg_pomson.txt":
                    return "Pomson 6000";
                case "tf_weapon_shotgun_building_rescue.txt":
                    return "Rescue Ranger";
                case "tf_weapon_pistol.txt":
                    return "Pistol and all reskins (Engineer)";
                case "tf_weapon_mechanical_arm.txt":
                    return "Short Circuit";
                case "tf_weapon_laser_pointer.txt":
                    return "Wrangler, Giger Counter";
                case "tf_weapon_wrench.txt":
                    return "Wrench, Southern Hospitality, Jag, Eureka Effect";
                case "tf_weapon_robot_arm.txt":
                    return "Gunslinger";
                case "tf_weapon_pda_engineer_build.txt":
                    return "Construction PDA";
                case "tf_weapon_pda_engineer_destroy.txt":
                    return "Destruction PDA";

                // Medic
                case "tf_weapon_syringegun_medic.txt":
                    return "Syringe Gun, Blutsauger, Overdose";
                case "tf_weapon_crossbow.txt":
                    return "Crusader's Crossbow";
                case "tf_weapon_medigun.txt":
                    return "Medi Gun, Kritzkrieg, Quick-Fix, Vaccinator";
                case "tf_weapon_bonesaw.txt":
                    return "Bonesaw and all reskins, Ubersaw, Vita-Saw, Amputator, Solemn Vow";

                // Sniper
                case "tf_weapon_sniperrifle.txt":
                    return "Sniper Rifle, Sydney Sleeper, Hitman's Heatmaker, Machina";
                case "tf_weapon_sniperrifle_decap.txt":
                    return "Bazaar Bargain";
                case "tf_weapon_sniperrifle_classic.txt":
                    return "Classic";
                case "tf_weapon_compound_bow.txt":
                    return "Huntsman, Fortified Compound";
                case "tf_weapon_smg.txt":
                    return "SMG";
                case "tf_weapon_charged_smg.txt":
                    return "Cleaner's Carbine";
                case "tf_weapon_club.txt":
                    return "Kukri and all reskins, Tribalman's Shiv, Bushwacka, Shahanshah";

                // Spy
                case "tf_weapon_revolver.txt":
                    return "Revolver, Ambassador, L'Etranger, Enforcer, Diamondback";
                case "tf_weapon_knife.txt":
                    return "Knife and all reskins, Your Eternal Reward, Conniver's Kunai, Big Earner, Spy-cicle";
                case "tf_weapon_pda_spy.txt":
                    return "Disguise kit";

                // Multi-class
                case "tf_weapon_jar.txt": // Scout/Pyro/Sniper
                    return "Flying Guillotine, Mad Milk, Gas Passer, Jarate";
                case "tf_weapon_katana.txt": // Soldier/Demoman
                    return "Half-Zatoichi";
                case "tf_weapon_builder.txt": // Spy/Engineer
                    return "Sapper, Red-Tape Recorder, While placing a building";
            }
            throw new ArgumentException($"'{weaponScript}' does not have a weapon associated with it!");
        }

        public static string getClassFromWeaponName(string weaponName)
        {
            switch (weaponName)
            {
                // Scout
                case "Scattergun, Back Scatter, Force-A-Nature":
                case "Baby Face's Blaster":
                case "Shortstop":
                case "Soda Popper":
                case "Pistol and all reskins (Scout)":
                case "Bonk! Atomic Punch, Crit-a-Cola":
                case "Pretty Boy's Pocket Pistol, Winger":
                case "Bat and all reskins, Atomizer, Boston Basher, Candy Cane, Fan O'War, Sun-on-a-Stick":
                case "Holy Mackerel":
                case "Sandman":
                case "Wrap Assassin":
                    return "Scout";

                // Soldier
                case "Rocket Launcher, Black Box, Original, Liberty Launcher, Beggar's Bazooka":
                case "Air Strike":
                case "Cow Mangler 5000":
                case "Direct Hit":
                case "Shotgun, Reserve Shooter, Panic Attack (Soldier)":
                case "Buff Banner, Battalion's Backup, Concheror":
                case "Righteous Bison":
                case "Shovel and all reskins, Equalizer, Pain Train, Disciplinary Action, Market Gardener, Escape Plan":
                    return "Soldier";

                // Pyro
                case "Flame Thrower and all reskins, Backburner, Degreaser, Phlogistinator":
                case "Dragon's Fury":
                case "Shotgun, Reserve Shooter, Panic Attack (Pyro)":
                case "Flare Gun, Detonator, Scorch Shot":
                case "Manmelter":
                case "Thermal Thruster":
                case "Fire Axe and all reskins, Lollichop, Axtinguisher, Homewrecker, Powerjack, Back Scratcher, Sharpened Volcano Fragment, Third Degree, Neon Annihilator":
                case "Hot Hand":
                    return "Pyro";

                // Demoman
                case "Grenade Launcher, Loch-n-Load, Iron Bomber":
                case "Loose Cannon":
                case "Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher":
                case "Bottle and all reskins":
                case "Eyelander, Scotsman's Skullcutter, Claidheamh Mòr, Persian Persuader, Pain Train":
                case "Ullapool Caber":
                    return "Demoman";

                // Heavy
                case "Minigun, Natascha, Brass Beast, Tomislav, Huo-Long Heater":
                case "Shotgun, Family Business, Panic Attack":
                case "Sandvich, Dalokohs Bar, Fishcake, Buffalo Steak Sandvich, Second Banana":
                case "Fists and all reskins, Killing Gloves of Boxing, Gloves of Running Urgently, Warrior's Spirit, Fists of Steel, Eviction Notice, Holiday Punch":
                    return "Heavy";

                // Engineer
                case "Shotgun, Widowmaker, Panic Attack":
                case "Frontier Justice":
                case "Pomson 6000":
                case "Rescue Ranger":
                case "Pistol and all reskins (Engineer)":
                case "Short Circuit":
                case "Wrangler, Giger Counter":
                case "Wrench, Southern Hospitality, Jag, Eureka Effect":
                case "Gunslinger":
                case "Construction PDA":
                case "Destruction PDA":
                    return "Engineer";

                // Medic
                case "Syringe Gun, Blutsauger, Overdose":
                case "Crusader's Crossbow":
                case "Medi Gun, Kritzkrieg, Quick-Fix, Vaccinator":
                case "Bonesaw and all reskins, Ubersaw, Vita-Saw, Amputator, Solemn Vow":
                    return "Medic";

                // Sniper
                case "Sniper Rifle, Sydney Sleeper, Hitman's Heatmaker, Machina":
                case "Bazaar Bargain":
                case "Classic":
                case "Huntsman, Fortified Compound":
                case "SMG":
                case "Cleaner's Carbine":
                case "Kukri and all reskins, Tribalman's Shiv, Bushwacka, Shahanshah":
                    return "Sniper";

                // Spy
                case "Revolver, Ambassador, L'Etranger, Enforcer, Diamondback":
                case "Knife and all reskins, Your Eternal Reward, Conniver's Kunai, Big Earner, Spy-cicle":
                case "Disguise kit":
                    return "Spy";

                // Multi-class
                case "Flying Guillotine, Mad Milk, Gas Passer, Jarate": // Scout/Pyro/Sniper
                case "Half-Zatoichi": // Soldier/Demoman
                case "Sapper, Red-Tape Recorder, While placing a building": // Spy/Engineer
                    return "Multi-class";
            }
            throw new ArgumentException($"'{weaponName}' does not have a class associated with it!");
        }
    }
}
