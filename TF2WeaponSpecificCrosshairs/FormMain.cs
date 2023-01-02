using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace VenomCrosshairs
{
    public partial class FormMain : Form
    {
        private static readonly string VC_VERSION = "beta8.0";

        private static readonly string PATH_VC = Directory.GetCurrentDirectory();
        private static readonly string PATH_VC_RESOURCES = Directory.GetCurrentDirectory() + @"\resources\";
        private static readonly string PATH_VC_RESOURCES_MATERIALS = Directory.GetCurrentDirectory() + @"\resources\materials\";
        private static readonly string PATH_VC_RESOURCES_PREVIEWS = Directory.GetCurrentDirectory() + @"\resources\previews\";
        private static readonly string PATH_VC_RESOURCES_PREVIEWS_GENERATEPREVIEWSBAT = Directory.GetCurrentDirectory() + @"\resources\previews\generatepreviews.bat";
        private static readonly string PATH_VC_RESOURCES_SCRIPTS = Directory.GetCurrentDirectory() + @"\resources\scripts\";
        private static readonly string PATH_VC_RESOURCES_VC_EXPLOSION_EFFECT_CFG_FILE = PATH_VC_RESOURCES + @"\vc_expeff.cfg";
        private static readonly string PATH_VC_RESOURCES_VC_USERPATH_CFG_FILE = PATH_VC_RESOURCES + @"\vc_userpath.cfg";

        private Dictionary<string, string[]> classPrimaryMap = new Dictionary<string, string[]>();
        private Dictionary<string, string[]> classSecondaryMap = new Dictionary<string, string[]>();
        private Dictionary<string, string[]> classMeleeMap = new Dictionary<string, string[]>();
        private Dictionary<string, string[]> classMiscMap = new Dictionary<string, string[]>();
        private Dictionary<string, string> publicCrosshairs = new Dictionary<string, string>();
        private Dictionary<string, string> weaponScriptPairs = new Dictionary<string, string>();

        private bool hasInitialized = false;
        private bool showConsole = true;

        private static readonly string[] tf2Classes = { "Scout", "Soldier", "Pyro", "Demoman", "Heavy", "Engineer", "Medic", "Sniper", "Spy" };

        // TODO: Re-write, make "TF2Class" class..
        private static readonly string[] tf2ScoutWeapons = { "Scattergun, Back Scatter, Force-A-Nature", "Baby Face's Blaster", "Shortstop", "Soda Popper", "Pistol and all reskins (Scout)", "Bonk! Atomic Punch, Crit-a-Cola", "Flying Guillotine, Mad Milk, Gas Passer, Jarate", "Pretty Boy's Pocket Pistol, Winger", "Bat and all reskins, Atomizer, Boston Basher, Candy Cane, Fan O'War, Sun-on-a-Stick", "Holy Mackerel", "Sandman", "Wrap Assassin" };
        private static readonly string[] tf2PrimaryScoutWeapons = { "Scattergun, Back Scatter, Force-A-Nature", "Baby Face's Blaster", "Shortstop", "Soda Popper" };
        private static readonly string[] tf2SecondaryScoutWeapons = { "Pistol and all reskins (Scout)", "Bonk! Atomic Punch, Crit-a-Cola", "Flying Guillotine, Mad Milk, Gas Passer, Jarate", "Pretty Boy's Pocket Pistol, Winger" };
        private static readonly string[] tf2MeleeScoutWeapons = { "Bat and all reskins, Atomizer, Boston Basher, Candy Cane, Fan O'War, Sun-on-a-Stick", "Holy Mackerel", "Sandman", "Wrap Assassin" };
        private static readonly string[] tf2MiscScoutWeapons = { };

        private static readonly string[] tf2SoldierWeapons = { "Rocket Launcher, Black Box, Original, Liberty Launcher, Beggar's Bazooka", "Air Strike", "Cow Mangler 5000", "Direct Hit", "Shotgun, Reserve Shooter, Panic Attack (Soldier)", "Buff Banner, Battalion's Backup, Concheror", "Righteous Bison", "Shovel and all reskins, Equalizer, Pain Train, Disciplinary Action, Market Gardener, Escape Plan", "Half-Zatoichi" };
        private static readonly string[] tf2PrimarySoldierWeapons = { "Rocket Launcher, Black Box, Original, Liberty Launcher, Beggar's Bazooka", "Air Strike", "Cow Mangler 5000", "Direct Hit" };
        private static readonly string[] tf2SecondarySoldierWeapons = { "Shotgun, Reserve Shooter, Panic Attack (Soldier)", "Buff Banner, Battalion's Backup, Concheror", "Righteous Bison" };
        private static readonly string[] tf2MeleeSoldierWeapons = { "Shovel and all reskins, Equalizer, Pain Train, Disciplinary Action, Market Gardener, Escape Plan", "Half-Zatoichi" };
        private static readonly string[] tf2MiscSoldierWeapons = { };

        private static readonly string[] tf2PyroWeapons = { "Flame Thrower and all reskins, Backburner, Degreaser, Phlogistinator", "Dragon's Fury", "Shotgun, Reserve Shooter, Panic Attack (Pyro)", "Flare Gun, Detonator, Scorch Shot", "Flying Guillotine, Mad Milk, Gas Passer, Jarate", "Manmelter", "Thermal Thruster", "Fire Axe and all reskins, Lollichop, Axtinguisher, Homewrecker, Powerjack, Back Scratcher, Sharpened Volcano Fragment, Third Degree, Neon Annihilator", "Hot Hand" };
        private static readonly string[] tf2PrimaryPyroWeapons = { "Flame Thrower and all reskins, Backburner, Degreaser, Phlogistinator", "Dragon's Fury" };
        private static readonly string[] tf2SecondaryPyroWeapons = { "Shotgun, Reserve Shooter, Panic Attack (Pyro)", "Flare Gun, Detonator, Scorch Shot", "Flying Guillotine, Mad Milk, Gas Passer, Jarate", "Manmelter", "Thermal Thruster" };
        private static readonly string[] tf2MeleePyroWeapons = { "Fire Axe and all reskins, Lollichop, Axtinguisher, Homewrecker, Powerjack, Back Scratcher, Sharpened Volcano Fragment, Third Degree, Neon Annihilator", "Hot Hand" };
        private static readonly string[] tf2MiscPyroWeapons = { };

        private static readonly string[] tf2DemomanWeapons = { "Grenade Launcher, Loch-n-Load, Iron Bomber", "Loose Cannon", "Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher", "Bottle and all reskins", "Eyelander, Scotsman's Skullcutter, Claidheamh Mòr, Persian Persuader, Pain Train", "Half-Zatoichi", "Ullapool Caber" };
        private static readonly string[] tf2PrimaryDemomanWeapons = { "Grenade Launcher, Loch-n-Load, Iron Bomber", "Loose Cannon", "Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher", "Bottle and all reskins", "Eyelander, Scotsman's Skullcutter, Claidheamh Mòr, Persian Persuader, Pain Train", "Half-Zatoichi", "Ullapool Caber" };
        private static readonly string[] tf2SecondaryDemomanWeapons = { "Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher" };
        private static readonly string[] tf2MeleeDemomanWeapons = { "Bottle and all reskins", "Eyelander, Scotsman's Skullcutter, Claidheamh Mòr, Persian Persuader, Pain Train", "Half-Zatoichi", "Ullapool Caber" };
        private static readonly string[] tf2MiscDemomanWeapons = { };

        private static readonly string[] tf2HeavyWeapons = { "Minigun, Natascha, Brass Beast, Tomislav, Huo-Long Heater", "Shotgun, Family Business, Panic Attack", "Sandvich, Dalokohs Bar, Fishcake, Buffalo Steak Sandvich, Second Banana", "Fists and all reskins, Killing Gloves of Boxing, Gloves of Running Urgently, Warrior's Spirit, Fists of Steel, Eviction Notice, Holiday Punch" };
        private static readonly string[] tf2PrimaryHeavyWeapons = { "Minigun, Natascha, Brass Beast, Tomislav, Huo-Long Heater" };
        private static readonly string[] tf2SecondaryHeavyWeapons = { "Shotgun, Family Business, Panic Attack", "Sandvich, Dalokohs Bar, Fishcake, Buffalo Steak Sandvich, Second Banana" };
        private static readonly string[] tf2MeleeHeavyWeapons = { "Fists and all reskins, Killing Gloves of Boxing, Gloves of Running Urgently, Warrior's Spirit, Fists of Steel, Eviction Notice, Holiday Punch" };
        private static readonly string[] tf2MiscHeavyWeapons = { };

        private static readonly string[] tf2EngineerWeapons = { "Shotgun, Widowmaker, Panic Attack", "Frontier Justice", "Pomson 6000", "Rescue Ranger", "Pistol and all reskins (Engineer)", "Short Circuit", "Wrangler, Giger Counter", "Wrench, Southern Hospitality, Jag, Eureka Effect", "Gunslinger", "Construction PDA", "Destruction PDA", "Sapper, Red-Tape Recorder, While placing a building" };
        private static readonly string[] tf2PrimaryEngineerWeapons = { "Shotgun, Widowmaker, Panic Attack", "Frontier Justice", "Pomson 6000", "Rescue Ranger" };
        private static readonly string[] tf2SecondaryEngineerWeapons = { "Pistol and all reskins (Engineer)", "Short Circuit", "Wrangler, Giger Counter" };
        private static readonly string[] tf2MeleeEngineerWeapons = { "Wrench, Southern Hospitality, Jag, Eureka Effect", "Gunslinger" };
        private static readonly string[] tf2MiscEngineerWeapons = { "Construction PDA", "Destruction PDA", "Sapper, Red-Tape Recorder, While placing a building" };

        private static readonly string[] tf2MedicWeapons = { "Syringe Gun, Blutsauger, Overdose", "Crusader's Crossbow", "Medi Gun, Kritzkrieg, Quick-Fix, Vaccinator", "Bonesaw and all reskins, Ubersaw, Vita-Saw, Amputator, Solemn Vow" };
        private static readonly string[] tf2PrimaryMedicWeapons = { "Syringe Gun, Blutsauger, Overdose", "Crusader's Crossbow" };
        private static readonly string[] tf2SecondaryMedicWeapons = { "Medi Gun, Kritzkrieg, Quick-Fix, Vaccinator" };
        private static readonly string[] tf2MeleeMedicWeapons = { "Bonesaw and all reskins, Ubersaw, Vita-Saw, Amputator, Solemn Vow" };
        private static readonly string[] tf2MiscMedicWeapons = { };

        private static readonly string[] tf2SniperWeapons = { "Sniper Rifle, Sydney Sleeper, Bazaar Bargain, Machina", "Classic", "Hitman's Heatmaker", "Huntsman, Fortified Compound", "SMG", "Cleaner's Carbine", "Flying Guillotine, Mad Milk, Gas Passer, Jarate", "Kukri and all reskins, Tribalman's Shiv, Bushwacka, Shahanshah" };
        private static readonly string[] tf2PrimarySniperWeapons = { "Sniper Rifle, Sydney Sleeper, Bazaar Bargain, Machina", "Classic", "Hitman's Heatmaker", "Huntsman, Fortified Compound" };
        private static readonly string[] tf2SecondarySniperWeapons = { "SMG", "Cleaner's Carbine", "Flying Guillotine, Mad Milk, Gas Passer, Jarate" };
        private static readonly string[] tf2MeleeSniperWeapons = { "Kukri and all reskins, Tribalman's Shiv, Bushwacka, Shahanshah" };
        private static readonly string[] tf2MiscSniperWeapons = { };

        private static readonly string[] tf2SpyWeapons = { "Revolver, Ambassador, L'Etranger, Enforcer, Diamondback", "Sapper, Red-Tape Recorder, While placing a building", "Knife and all reskins, Your Eternal Reward, Conniver's Kunai, Big Earner, Spy-cicle", "Disguise kit" };
        private static readonly string[] tf2PrimarySpyWeapons = { "Revolver, Ambassador, L'Etranger, Enforcer, Diamondback" };
        private static readonly string[] tf2SecondarySpyWeapons = { "Sapper, Red-Tape Recorder, While placing a building" };
        private static readonly string[] tf2MeleeSpyWeapons = { "Knife and all reskins, Your Eternal Reward, Conniver's Kunai, Big Earner, Spy-cicle" };
        private static readonly string[] tf2MiscSpyWeapons = { "Sapper, Red-Tape Recorder, While placing a building", "Disguise kit" };

        private static readonly string[] tf2AllWeapons = tf2ScoutWeapons.Concat(tf2SoldierWeapons).Concat(tf2PyroWeapons).Concat(tf2DemomanWeapons).Concat(tf2HeavyWeapons).Concat(tf2EngineerWeapons).Concat(tf2MedicWeapons).Concat(tf2SniperWeapons).Concat(tf2SpyWeapons).ToArray();
        private static readonly string[] tf2AllPrimaryWeapons = { "Scattergun, Back Scatter, Force-A-Nature", "Baby Face's Blaster", "Shortstop", "Soda Popper", "Rocket Launcher, Black Box, Original, Liberty Launcher, Beggar's Bazooka", "Air Strike", "Cow Mangler 5000", "Direct Hit", "Flame Thrower and all reskins, Backburner, Degreaser, Phlogistinator", "Dragon's Fury", "Grenade Launcher, Loch-n-Load, Iron Bomber", "Loose Cannon", "Minigun, Natascha, Brass Beast, Tomislav, Huo-Long Heater", "Shotgun, Widowmaker, Panic Attack", "Frontier Justice", "Pomson 6000", "Rescue Ranger", "Syringe Gun, Blutsauger, Overdose", "Crusader's Crossbow", "Sniper Rifle, Sydney Sleeper, Bazaar Bargain, Machina", "Classic", "Hitman's Heatmaker", "Huntsman, Fortified Compound", "Revolver, Ambassador, L'Etranger, Enforcer, Diamondback" };
        private static readonly string[] tf2AllSecondaryWeapons = { "Pistol and all reskins (Scout)", "Bonk! Atomic Punch, Crit-a-Cola", "Flying Guillotine, Mad Milk, Gas Passer, Jarate", "Pretty Boy's Pocket Pistol, Winger", "Shotgun, Reserve Shooter, Panic Attack (Soldier)", "Buff Banner, Battalion's Backup, Concheror", "Righteous Bison", "Shotgun, Reserve Shooter, Panic Attack (Pyro)", "Flare Gun, Detonator, Scorch Shot", "Manmelter", "Thermal Thruster", "Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher", "Shotgun, Family Business, Panic Attack", "Sandvich, Dalokohs Bar, Fishcake, Buffalo Steak Sandvich, Second Banana", "Pistol and all reskins (Engineer)", "Short Circuit", "Wrangler, Giger Counter", "Medi Gun, Kritzkrieg, Quick-Fix, Vaccinator", "SMG", "Cleaner's Carbine" };
        private static readonly string[] tf2AllMeleeWeapons = { "Bat and all reskins, Atomizer, Boston Basher, Candy Cane, Fan O'War, Sun-on-a-Stick", "Holy Mackerel", "Sandman", "Wrap Assassin", "Shovel and all reskins, Equalizer, Pain Train, Disciplinary Action, Market Gardener, Escape Plan", "Half-Zatoichi", "Fire Axe and all reskins, Lollichop, Axtinguisher, Homewrecker, Powerjack, Back Scratcher, Sharpened Volcano Fragment, Third Degree, Neon Annihilator", "Hot Hand", "Bottle and all reskins", "Eyelander, Scotsman's Skullcutter, Claidheamh Mòr, Persian Persuader, Pain Train", "Ullapool Caber", "Fists and all reskins, Killing Gloves of Boxing, Gloves of Running Urgently, Warrior's Spirit, Fists of Steel, Eviction Notice, Holiday Punch", "Wrench, Southern Hospitality, Jag, Eureka Effect", "Gunslinger", "Bonesaw and all reskins, Ubersaw, Vita-Saw, Amputator, Solemn Vow", "Kukri and all reskins, Tribalman's Shiv, Bushwacka, Shahanshah", "Knife and all reskins, Your Eternal Reward, Conniver's Kunai, Big Earner, Spy-cicle" };
        private static readonly string[] tf2AllMiscWeapons = { "Construction PDA", "Destruction PDA", "Sapper, Red-Tape Recorder, While placing a building", "Disguise kit" };

        public FormMain()
        {
            InitializeComponent();
            initVC();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You are about to download missing/new crosshairs and reload the crosshair list. This will clear currently selected crosshairs.\nAre you sure you want to continue?\n\nWARNING: This might take some time!", "Update crosshair list", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dialogResult == DialogResult.Yes && performSanityCheck(textBoxTF2Path.Text))
                new Thread(downloadAndGenerateCrosshairs).Start();
        }

        private void btnGitHub_Click(object sender, EventArgs e)
        {
            Process.Start(@"https://github.com/hbivnm/Venom-Crosshairs");
        }

        private void btnSteam_Click(object sender, EventArgs e)
        {
            Process.Start(@"https://steamcommunity.com/profiles/76561197996468677");
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Process.Start(@"https://github.com/hbivnm/Venom-Crosshairs/wiki");
        }

        private void btnBrowseTF2Path_Click(object sender, EventArgs e)
        {
            using (var cofd = new CommonOpenFileDialog())
            {
                cofd.InitialDirectory = @"C:\";
                cofd.IsFolderPicker = true;

                if (cofd.ShowDialog() == CommonFileDialogResult.Ok && performSanityCheck(cofd.FileName))
                    Invoke(new MethodInvoker(delegate ()
                    {
                        textBoxTF2Path.Text = cofd.FileName;
                        File.WriteAllText(PATH_VC_RESOURCES_VC_USERPATH_CFG_FILE, cofd.FileName);
                    }));
            }
        }

        private void btnPrevCrosshair_Click(object sender, EventArgs e)
        {
            if (cbCrosshair.SelectedIndex == 0 || cbCrosshair.SelectedIndex == -1)
                cbCrosshair.SelectedIndex = cbCrosshair.Items.Count - 1;
            else
                cbCrosshair.SelectedIndex -= 1;
        }

        private void btnNextCrosshair_Click(object sender, EventArgs e)
        {
            if (cbCrosshair.SelectedIndex == cbCrosshair.Items.Count - 1 || cbCrosshair.SelectedIndex == -1)
                cbCrosshair.SelectedIndex = 0;
            else
                cbCrosshair.SelectedIndex += 1;
        }

        private void btnToggleConsole_Click(object sender, EventArgs e)
        {
            toggleConsole();
        }

        private void btnAddCrosshair_Click(object sender, EventArgs e)
        {
            bool crosshairAdded = false;

            if (cbClass.Text.Length > 0 && cbWeapon.Text.Length > 0 && cbCrosshair.Text.Length > 0)
            {
                addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, cbWeapon.Text }));
                crosshairAdded = true;
            }

            if (checkBoxAddOnlyClass.Checked)
            {
                if (checkBoxAddPrimaryWeapons.Checked)
                {
                    foreach (var weapon in classPrimaryMap[cbClass.Text])
                        addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon }));
                    crosshairAdded = true;
                }

                if (checkBoxAddSecondaryWeapons.Checked)
                {
                    foreach (var weapon in classSecondaryMap[cbClass.Text])
                        addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon }));
                    crosshairAdded = true;
                }

                if (checkBoxAddMeleeWeapons.Checked)
                {
                    foreach (var weapon in classMeleeMap[cbClass.Text])
                        addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon }));
                    crosshairAdded = true;
                }

                if (checkBoxAddMiscWeapons.Checked)
                {
                    foreach (var weapon in classMiscMap[cbClass.Text])
                        addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon }));
                    crosshairAdded = true;
                }
            }
            else if (!checkBoxAddOnlyClass.Checked)
            {
                if (checkBoxAddPrimaryWeapons.Checked)
                {
                    foreach (var weapon in tf2AllPrimaryWeapons)
                        addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon }));
                    crosshairAdded = true;
                }

                if (checkBoxAddSecondaryWeapons.Checked)
                {
                    foreach (var weapon in tf2AllSecondaryWeapons)
                        addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon }));
                    crosshairAdded = true;
                }

                if (checkBoxAddMeleeWeapons.Checked)
                {
                    foreach (var weapon in tf2AllMeleeWeapons)
                        addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon }));
                    crosshairAdded = true;
                }

                if (checkBoxAddMiscWeapons.Checked)
                {
                    foreach (var weapon in tf2AllMiscWeapons)
                        addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon }));
                    crosshairAdded = true;
                }
            }

            // This could be changed to an event that triggers when addCrosshairToListView is called...
            if (crosshairAdded)
            {
                btnRemoveSelected.Enabled = true;
                btnInstall.Enabled = true;
                btnInstallClean.Enabled = true;
                btnReadConfig.Enabled = true;
            }
        }

        private void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listViewChosenCrosshairs.Items.Count; i++)
                if (listViewChosenCrosshairs.Items[i].Selected)
                    listViewChosenCrosshairs.Items[i].SubItems.Clear();

            cleanListViewOfEmptyRows(listViewChosenCrosshairs);

            if (listViewChosenCrosshairs.Items.Count < 1)
                btnRemoveSelected.Enabled = false;
        }

        private void btnReadConfig_Click(object sender, EventArgs e)
        {
            writeToDebugger("Reading current config... ");
            string vcScripDir = textBoxTF2Path.Text + @"\tf\custom\VenomCrosshairConfig\scripts";
            if (Directory.Exists(vcScripDir))
            {
                foreach (string script in Directory.GetFiles(vcScripDir, "*.txt"))
                {
                    try
                    {
                        string crosshair = getCrosshairFromScript(script);
                        string weapon = getWeaponFromScriptName(script);
                        addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new String[] { crosshair, weapon }));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"\"{Path.GetFileName(script)}\" is no longer used.\nYou can safely remove this script file or do \"Install (clean)\" once the config has been read.\n\nFor developer: Exception: {ex.Message}", "Could not find weapon from script", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                btnInstall.Enabled = true;
                btnInstallClean.Enabled = true;
                btnRemoveSelected.Enabled = true;
                writeLineToDebugger("Done!");
            }
            else
            {
                MessageBox.Show("No Venom Crosshairs config folder found!\n\nMake sure your custom crosshair folder is named \"VenomCrosshairConfig\".", "Failed to read current config", MessageBoxButtons.OK, MessageBoxIcon.Error);
                writeLineToDebugger("Failed!");
            }
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            if (listViewChosenCrosshairs.Items.Count > 0 && performSanityCheck(textBoxTF2Path.Text))
                Task.Run(() => performInstallation(false));
        }

        private void btnInstallClean_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("This will REMOVE any installed config by Venom Crosshairs and create a new one.\nAre you sure you want to continue?", "Clean installation. Are you sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes && listViewChosenCrosshairs.Items.Count > 0 && performSanityCheck(textBoxTF2Path.Text))
                Task.Run(() => performInstallation(true));
        }

        /// 
        /// Events
        /// 

        private void onCBClassChangeEvent(object sender, EventArgs e)
        {
            cbWeapon.Items.Clear();
            checkBoxAddOnlyClass.Text = $@"Add to ONLY to {cbClass.Text}";
            switch (cbClass.Text)
            {
                case "Scout":
                    foreach (var weapon in tf2ScoutWeapons)
                        cbWeapon.Items.Add(weapon);
                    cbWeapon.DropDownWidth = 420;
                    break;
                case "Soldier":
                    foreach (var weapon in tf2SoldierWeapons)
                        cbWeapon.Items.Add(weapon);
                    cbWeapon.DropDownWidth = 460;
                    break;
                case "Pyro":
                    foreach (var weapon in tf2PyroWeapons)
                        cbWeapon.Items.Add(weapon);
                    cbWeapon.DropDownWidth = 735;
                    break;
                case "Demoman":
                    foreach (var weapon in tf2DemomanWeapons)
                        cbWeapon.Items.Add(weapon);
                    cbWeapon.DropDownWidth = 400;
                    break;
                case "Heavy":
                    foreach (var weapon in tf2HeavyWeapons)
                        cbWeapon.Items.Add(weapon);
                    cbWeapon.DropDownWidth = 645;
                    break;
                case "Engineer":
                    foreach (var weapon in tf2EngineerWeapons)
                        cbWeapon.Items.Add(weapon);
                    cbWeapon.DropDownWidth = 270;
                    break;
                case "Medic":
                    foreach (var weapon in tf2MedicWeapons)
                        cbWeapon.Items.Add(weapon);
                    cbWeapon.DropDownWidth = 340;
                    break;
                case "Sniper":
                    foreach (var weapon in tf2SniperWeapons)
                        cbWeapon.Items.Add(weapon);
                    cbWeapon.DropDownWidth = 315;
                    break;
                case "Spy":
                    foreach (var weapon in tf2SpyWeapons)
                        cbWeapon.Items.Add(weapon);
                    cbWeapon.DropDownWidth = 395;
                    break;
            }
            cbWeapon.Enabled = true;
            cbCrosshair.Enabled = true;
        }

        private void onCBWeaponChangeEvent(object sender, EventArgs e)
        {
            btnPrevCrosshair.Enabled = true;
            btnNextCrosshair.Enabled = true;
            cbCrosshair.Enabled = true;
        }

        private void onCBCrosshairChangeEvent(object sender, EventArgs e)
        {
            if (cbCrosshair.Text.Length > 0)
                pictureBoxCrosshair.ImageLocation = PATH_VC_RESOURCES_PREVIEWS + cbCrosshair.Text + ".png";
            else
                pictureBoxCrosshair.ImageLocation = PATH_VC_RESOURCES + @"VC.png";

            btnAddCrosshair.Enabled = true;
            checkBoxAddOnlyClass.Enabled = true;
            checkBoxAddPrimaryWeapons.Enabled = true;
            checkBoxAddSecondaryWeapons.Enabled = true;
            checkBoxAddMeleeWeapons.Enabled = true;
            checkBoxAddMiscWeapons.Enabled = true;
        }

        private void onCheckBoxAddClassWeaponsChangeEvent(object sender, EventArgs e)
        {
            if (checkBoxAddOnlyClass.Checked)
            {
                checkBoxAddPrimaryWeapons.Text = $"Add to ALL {cbClass.Text} primary weapons";
                checkBoxAddSecondaryWeapons.Text = $"Add to ALL {cbClass.Text} secondary weapons";
                checkBoxAddMeleeWeapons.Text = $"Add to ALL {cbClass.Text} melee weapons";
                checkBoxAddMiscWeapons.Text = $"Add to ALL {cbClass.Text} misc. weapons";
            }
            else
            {
                checkBoxAddPrimaryWeapons.Text = "Add to ALL primary weapons";
                checkBoxAddSecondaryWeapons.Text = "Add to ALL secondary weapons";
                checkBoxAddMeleeWeapons.Text = "Add to ALL melee weapons";
                checkBoxAddMiscWeapons.Text = "Add to ALL misc. weapons";
            }
        }

        private void onCBExplosionEffectChangeEvent(object server, EventArgs e)
        {
            File.WriteAllText(PATH_VC_RESOURCES_VC_EXPLOSION_EFFECT_CFG_FILE, Convert.ToString(cbExplosionEffect.SelectedIndex));
        }

        private void onFormLoad(object sender, EventArgs e)
        {
            // Fetch public crosshairs, if new crosshairs are available -> prompt user
            writeToDebugger("Fetching public crosshair list... ");

            bool newCrosshairsAvailable = isNewCrosshairsAvailable(false);

            writeLineToDebugger("Done!");

            if (newCrosshairsAvailable)
                writeLineToDebugger("New crosshairs available!");

            writeLineToDebugger($"Venom Crosshairs version {VC_VERSION}");
        }

        /// 
        /// Functions
        /// 
        private void initVC()
        {
            // Prevent initVC from running more than once
            if (!hasInitialized)
            {
                pictureBoxLoading.Visible = true;
                initDicts();

                // Classes
                foreach (var tf2Class in tf2Classes)
                    cbClass.Items.Add(tf2Class);
                cbClass.SelectedIndexChanged += new EventHandler(onCBClassChangeEvent);
                cbClass.SelectedIndexChanged += new EventHandler(onCheckBoxAddClassWeaponsChangeEvent);

                // Weapons
                cbWeapon.SelectedIndexChanged += new EventHandler(onCBWeaponChangeEvent);

                // Crosshairs
                cbCrosshair.Items.Clear();
                foreach (var crosshair in Directory.GetFiles(PATH_VC_RESOURCES_PREVIEWS, "*.png"))
                {
                    string crosshairName = Path.GetFileNameWithoutExtension(crosshair);
                    cbCrosshair.Items.Add(crosshairName);
                }
                cbCrosshair.SelectedIndexChanged += new EventHandler(onCBCrosshairChangeEvent);

                // Checkboxes
                checkBoxAddOnlyClass.CheckStateChanged += new EventHandler(onCheckBoxAddClassWeaponsChangeEvent);

                // ListView
                listViewChosenCrosshairs.Columns.Add("Crosshair", 220);
                listViewChosenCrosshairs.Columns.Add("Weapon", 599);

                // Hide console
                if (showConsole)
                {
                    showConsole = false;
                    textBoxDebugger.Visible = false;
                    lblStatus.Visible = true;
                    this.Width = 876;
                }
                else
                {
                    showConsole = true;
                    textBoxDebugger.Visible = true;
                    lblStatus.Visible = false;
                    this.Width = 1183;
                }

                // Read user settings
                // Explosion effect
                if (File.Exists(PATH_VC_RESOURCES_VC_EXPLOSION_EFFECT_CFG_FILE))
                    try
                    {
                        cbExplosionEffect.SelectedIndex = Convert.ToInt32(File.ReadAllText(PATH_VC_RESOURCES_VC_EXPLOSION_EFFECT_CFG_FILE));
                    }
                    catch (Exception)
                    {
                        throw new FormatException($@"The contents of {PATH_VC_RESOURCES_VC_EXPLOSION_EFFECT_CFG_FILE} could not be parsed to an Integer.");
                    }
                else
                    cbExplosionEffect.SelectedIndex = 0;
                cbExplosionEffect.SelectedIndexChanged += new EventHandler(onCBExplosionEffectChangeEvent);

                // User TF2 path
                if (File.Exists(PATH_VC_RESOURCES_VC_USERPATH_CFG_FILE))
                    textBoxTF2Path.Text = File.ReadAllText(PATH_VC_RESOURCES_VC_USERPATH_CFG_FILE);

                // TODO: Search for existing config with different name

                pictureBoxLoading.Visible = false;
            }
        }

        private bool isNewCrosshairsAvailable(bool suppressNotification)
        {
            _ = fetchCrosshairsFromPublicRepo();

            foreach (KeyValuePair<string, string> crosshair in publicCrosshairs)
                if (!File.Exists($@"{PATH_VC_RESOURCES_MATERIALS}\{crosshair.Key}"))
                {
                    if (!suppressNotification)
                    {
                        MessageBox.Show("There are new crosshairs available for download!\n\nDownload them by hitting the double arrow button in the top right.", "TF2 Weapon Specific Crosshairs - New crosshairs available!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return true;
                }

            return false;
        }

        private Task fetchCrosshairsFromPublicRepo()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApplication", "1"));
            var contentsUrl = $"https://api.github.com/repos/hbivnm/Venom-Crosshairs-List/contents";
            var response = httpClient.GetStringAsync(contentsUrl).Result;

            var contents = (JArray)JsonConvert.DeserializeObject(response);

            publicCrosshairs.Clear();
            foreach (var file in contents)
                publicCrosshairs.Add((string)file["name"], (string)file["download_url"]);

            return null;
        }

        private Task downloadMissingCrosshairs()
        {
            var tasks = new List<Task>();
            int newCrosshairsFileCount = 0;
            writeToDebugger($"Downloading crosshairs... ");

            foreach (KeyValuePair<string, string> crosshair in publicCrosshairs)
                if (!File.Exists($@"{PATH_VC_RESOURCES_MATERIALS}\{crosshair.Key}"))
                {
                    using (WebClient webClient = new WebClient())
                    {
                        tasks.Add(webClient.DownloadFileTaskAsync(new Uri(crosshair.Value), $@"{PATH_VC_RESOURCES_MATERIALS}\{crosshair.Key}"));
                    }
                    newCrosshairsFileCount++;
                }

            Task.WaitAll(tasks.ToArray());
            writeLineToDebugger("Done!");
            writeLineToDebugger($"Downloaded {newCrosshairsFileCount / 2} crosshair(s)!");
            return null;
        }

        private void toggleConsole()
        {
            if (showConsole)
            {
                showConsole = false;
                lblStatus.Visible = true;
                textBoxDebugger.Visible = false;
                ActiveForm.Width = 880;
            }
            else
            {
                showConsole = true;
                lblStatus.Visible = false;
                textBoxDebugger.Visible = true;
                ActiveForm.Width = 1183;
            }
        }

        private void writeLineToDebugger(string text)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                lblStatus.Text = text;
                textBoxDebugger.Text += text + Environment.NewLine;

                // Scroll to bottom
                textBoxDebugger.SelectionStart = textBoxDebugger.Text.Length;
                textBoxDebugger.ScrollToCaret();
            }));
        }

        private void writeToDebugger(string text)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                lblStatus.Text = text;
                textBoxDebugger.Text += text;

                // Scroll to bottom
                textBoxDebugger.SelectionStart = textBoxDebugger.Text.Length;
                textBoxDebugger.ScrollToCaret();
            }));
        }

        private void cleanListViewOfEmptyRows(ListView listView)
        {
            List<ListViewItem> oldListViewItems = new List<ListViewItem>();

            // Sort out ListViewItems that are empty
            foreach (ListViewItem listViewItem in listView.Items)
                if (listViewItem.SubItems[0].Text.Length > 0)
                    oldListViewItems.Add(listViewItem);
                else
                    continue;

            listView.Items.Clear();

            foreach (ListViewItem listViewItem in oldListViewItems)
                listView.Items.Add(listViewItem);
        }

        private bool listViewItemExists(ListView listView, ListViewItem listViewItem)
        {
            foreach (ListViewItem item in listView.Items)
                if (item.SubItems[1].Text == listViewItem.SubItems[1].Text)
                    return true;

            return false;
        }

        private void addCrosshairToListView(ListView listView, ListViewItem crosshairListViewItem)
        {
            if (!listViewItemExists(listView, crosshairListViewItem))
                listView.Items.Add(crosshairListViewItem);
            else
                foreach (ListViewItem item in listView.Items)
                    if (item.SubItems[0].Text != crosshairListViewItem.SubItems[0].Text && item.SubItems[1].Text == crosshairListViewItem.SubItems[1].Text)
                    {
                        item.SubItems[0].Text = cbCrosshair.Text;
                        break;
                    }
        }

        private string[] getWeaponsFromClassName(string className)
        {
            switch (className)
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
                default:
                    return null;
            }
        }

        private string getExplosionEffectParticleName(string name)
        {
            switch (name)
            {
                case "Default":
                    return "ExplosionCore_wall";
                case "Electric shock":
                    return "electrocuted_red_flash";
                case "Muzzle flash":
                    return "muzzle_minigun_starflash01";
                case "Spy sapper":
                    return "ExplosionCore_sapperdestroyed";
                case "Pyro pool":
                    return "eotl_pyro_pool_explosion_flash";
            }
            throw new ArgumentException($"Could not find ExplosionEffect particle name for '{name}'!");
        }

        private string getExplosionPlayerEffectParticleName(string name)
        {
            switch (name)
            {
                case "Default":
                    return "ExplosionCore_MidAir";
                case "Electric shock":
                    return "electrocuted_blue_flash";
                case "Muzzle flash":
                    return "muzzle_minigun_starflash01";
                case "Spy sapper":
                    return "ExplosionCore_sapperdestroyed";
                case "Pyro pool":
                    return "eotl_pyro_pool_explosion_flash";
            }
            throw new ArgumentException($"Could not find ExplosionPlayerEffect particle name for '{name}'!");
        }

        private string getExplosionWaterEffectParticleName(string name)
        {
            switch (name)
            {
                case "Default":
                    return "ExplosionCore_MidAir_underwater";
                case "Electric shock":
                    return "electrocuted_red_flash";
                case "Muzzle flash":
                    return "muzzle_minigun_starflash01";
                case "Spy sapper":
                    return "ExplosionCore_sapperdestroyed";
                case "Pyro pool":
                    return "eotl_pyro_pool_explosion_flash";
            }
            throw new ArgumentException($"Could not find ExplosionWaterEffect particle name for '{name}'!");
        }

        private void moveFilesByExtensionOrDelete(string sourceDirectory, string targetDirectory, string extension) // Should probably re-write so it overwrites, not sure of the thoughtprocess of moving or deleting (same filename =/= same file content)
        {
            foreach (string file in Directory.GetFiles(sourceDirectory, "*." + extension))
                if (!File.Exists(targetDirectory + Path.GetFileName(file)))
                    File.Move(file, targetDirectory + Path.GetFileName(file));
                else
                    File.Delete(file);
        }

        private void performInstallation(bool removeOldConfig)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                pictureBoxLoading.Visible = true;
                textBoxTF2Path.Enabled = false;
                btnReload.Enabled = false;
                cbClass.Enabled = false;
                cbCrosshair.Enabled = false;
                cbWeapon.Enabled = false;
                btnAddCrosshair.Enabled = false;
                checkBoxAddOnlyClass.Enabled = false;
                checkBoxAddPrimaryWeapons.Enabled = false;
                checkBoxAddSecondaryWeapons.Enabled = false;
                checkBoxAddMeleeWeapons.Enabled = false;
                checkBoxAddMiscWeapons.Enabled = false;
                btnRemoveSelected.Enabled = false;
                btnPrevCrosshair.Enabled = false;
                btnNextCrosshair.Enabled = false;
                btnInstall.Enabled = false;
                btnInstallClean.Enabled = false;
                btnReadConfig.Enabled = false;
            }));

            bool isUpdate = false;
            if (removeOldConfig)
            {
                writeLineToDebugger("Clean installation of Venom Crosshairs config started!");
                writeToDebugger("Removing old Venom Crosshairs config... ");
                if (Directory.Exists($@"{textBoxTF2Path.Text}\tf\custom\VenomCrosshairConfig"))
                    Directory.Delete($@"{textBoxTF2Path.Text}\tf\custom\VenomCrosshairConfig", true);
                writeLineToDebugger("Done!");
            }
            else if (Directory.Exists($@"{textBoxTF2Path.Text}\tf\custom\VenomCrosshairConfig"))
            {
                writeLineToDebugger("Updating current Venom Crosshairs config!");
                isUpdate = true;
            }
            else
                writeLineToDebugger("Installation of Venom Crosshairs config started!");

            // Installation process
            if (!Directory.Exists($@"{textBoxTF2Path.Text}\tf\custom\VenomCrosshairConfig\materials\vgui\replay\thumbnails"))
                Directory.CreateDirectory($@"{textBoxTF2Path.Text}\tf\custom\VenomCrosshairConfig\materials\vgui\replay\thumbnails");
            if (!Directory.Exists($@"{textBoxTF2Path.Text}\tf\custom\VenomCrosshairConfig\scripts"))
                Directory.CreateDirectory($@"{textBoxTF2Path.Text}\tf\custom\VenomCrosshairConfig\scripts");

            writeToDebugger("Copying materials... ");
            foreach (var crosshairVMT in Directory.GetFiles(PATH_VC_RESOURCES_MATERIALS, "*.vmt"))
                File.Copy(crosshairVMT, $@"{textBoxTF2Path.Text}\tf\custom\VenomCrosshairConfig\materials\vgui\replay\thumbnails\{Path.GetFileName(crosshairVMT)}", true);
            foreach (var crosshairVTF in Directory.GetFiles(PATH_VC_RESOURCES_MATERIALS, "*.vtf"))
                File.Copy(crosshairVTF, $@"{textBoxTF2Path.Text}\tf\custom\VenomCrosshairConfig\materials\vgui\replay\thumbnails\{Path.GetFileName(crosshairVTF)}", true);
            writeLineToDebugger("Done!");

            writeToDebugger("Adding scripts... ");
            Invoke(new MethodInvoker(delegate ()
            {
                foreach (ListViewItem item in listViewChosenCrosshairs.Items)
                {
                    string crosshair = item.SubItems[0].Text;
                    string weaponName = item.SubItems[1].Text;
                    string weaponScriptName = weaponScriptPairs[weaponName];
                    string fullScriptPath = $@"{textBoxTF2Path.Text}\tf\custom\VenomCrosshairConfig\scripts\{weaponScriptName}";

                    if (File.Exists(fullScriptPath))
                        File.Delete(fullScriptPath);

                    File.WriteAllText(
                        $@"{textBoxTF2Path.Text}\tf\custom\VenomCrosshairConfig\scripts\{weaponScriptName}",
                        File.ReadAllText($@"{PATH_VC_RESOURCES_SCRIPTS}\{weaponScriptName}")
                            .Replace("VC_PLACEHOLDER_EXPLOSION_EFFECT", getExplosionEffectParticleName(cbExplosionEffect.Text))
                            .Replace("VC_PLACEHOLDER_EXPLOSION_PLAYER_EFFECT", getExplosionPlayerEffectParticleName(cbExplosionEffect.Text))
                            .Replace("VC_PLACEHOLDER_EXPLOSION_WATER_EFFECT", getExplosionWaterEffectParticleName(cbExplosionEffect.Text))
                            .Replace("VC_PLACEHOLDER", crosshair)
                    );
                }
            }));
            writeLineToDebugger("Done!");

            if (!isUpdate)
            {
                writeLineToDebugger("Venom Crosshairs config config successfully installed!");
            }
            else
            {
                writeLineToDebugger("Venom Crosshairs config successfully updated!");
            }

            Invoke(new MethodInvoker(delegate ()
            {
                pictureBoxLoading.Visible = false;
                textBoxTF2Path.Enabled = true;
                btnReload.Enabled = true;
                cbClass.Enabled = true;
                cbCrosshair.Enabled = true;
                cbWeapon.Enabled = true;
                btnAddCrosshair.Enabled = true;
                checkBoxAddOnlyClass.Enabled = true;
                checkBoxAddPrimaryWeapons.Enabled = true;
                checkBoxAddSecondaryWeapons.Enabled = true;
                checkBoxAddMeleeWeapons.Enabled = true;
                checkBoxAddMiscWeapons.Enabled = true;
                btnRemoveSelected.Enabled = true;
                btnPrevCrosshair.Enabled = true;
                btnNextCrosshair.Enabled = true;
                btnInstall.Enabled = true;
                btnInstallClean.Enabled = true;
                btnReadConfig.Enabled = true;
            }));
        }

        private void downloadAndGenerateCrosshairs()
        {
            Invoke(new MethodInvoker(delegate ()
            {
                pictureBoxLoading.Visible = true;
                pictureBoxCrosshair.ImageLocation = PATH_VC_RESOURCES + @"VenomCrosshairs.png";

                listViewChosenCrosshairs.Items.Clear();

                textBoxTF2Path.Enabled = false;
                btnReload.Enabled = false;
                cbClass.Enabled = false;
                cbCrosshair.Enabled = false;
                cbWeapon.Enabled = false;
                btnAddCrosshair.Enabled = false;
                checkBoxAddOnlyClass.Enabled = false;
                checkBoxAddPrimaryWeapons.Enabled = false;
                checkBoxAddSecondaryWeapons.Enabled = false;
                checkBoxAddMeleeWeapons.Enabled = false;
                checkBoxAddMiscWeapons.Enabled = false;
                btnRemoveSelected.Enabled = false;
                btnPrevCrosshair.Enabled = false;
                btnNextCrosshair.Enabled = false;
                btnInstall.Enabled = false;
                btnInstallClean.Enabled = false;
                btnReadConfig.Enabled = false;
            }));

            // Download publicly available crosshairs
            // code
            _ = downloadMissingCrosshairs();

            writeToDebugger("Deleting old previews... ");
            foreach (string previewFile in Directory.GetFiles(PATH_VC_RESOURCES_PREVIEWS))
                File.Delete(previewFile);
            writeLineToDebugger("Done!");

            writeToDebugger("Preparing vtf2tga process... ");
            Process vtf2tgaProcess = new Process();
            vtf2tgaProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            vtf2tgaProcess.StartInfo.FileName = textBoxTF2Path.Text + @"\bin\vtf2tga.exe";
            writeLineToDebugger("Done!");

            writeToDebugger("Running vtf2tga.exe... ");
            // Generate previews
            foreach (string vtfFile in Directory.GetFiles(PATH_VC_RESOURCES_MATERIALS, "*.vtf"))
            {
                vtf2tgaProcess.StartInfo.Arguments = @"/C -i " + "\"" + vtfFile + "\"";
                vtf2tgaProcess.Start();
            }
            vtf2tgaProcess.WaitForExit();
            writeLineToDebugger("Done!");

            moveFilesByExtensionOrDelete(PATH_VC_RESOURCES_MATERIALS, PATH_VC_RESOURCES_PREVIEWS, "tga");

            writeToDebugger("Generating generatepreviews.bat... ");
            File.Create(PATH_VC_RESOURCES_PREVIEWS_GENERATEPREVIEWSBAT).Close();
            using (StreamWriter sw = new StreamWriter(PATH_VC_RESOURCES_PREVIEWS_GENERATEPREVIEWSBAT))
            {
                foreach (string tgaFile in Directory.GetFiles(PATH_VC_RESOURCES_PREVIEWS, "*.tga"))
                {
                    string filename = Path.GetFileNameWithoutExtension(tgaFile);
                    sw.WriteLine("\"" + PATH_VC_RESOURCES + "ffmpeg.exe\" -y -i \"" + tgaFile + "\" " + filename + ".png");
                }
                sw.WriteLine("exit");
            }
            writeLineToDebugger("Done!");

            writeToDebugger("Preparing generatepreviews process... ");
            Process generatepreviewsProcess = new Process();
            generatepreviewsProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            generatepreviewsProcess.StartInfo.FileName = PATH_VC_RESOURCES_PREVIEWS_GENERATEPREVIEWSBAT;
            writeLineToDebugger("Done!");

            writeToDebugger("Running generatepreviews.bat... ");
            generatepreviewsProcess.Start();
            generatepreviewsProcess.WaitForExit();
            writeLineToDebugger("Done!");

            writeToDebugger("Performing cleanup... ");
            moveFilesByExtensionOrDelete(PATH_VC, PATH_VC_RESOURCES_PREVIEWS, "png"); // Function no longer needed due to cleanup steps, remove/refactor?
            File.Delete(PATH_VC_RESOURCES_PREVIEWS_GENERATEPREVIEWSBAT);
            foreach (string tgaFile in Directory.GetFiles(PATH_VC_RESOURCES_PREVIEWS, "*.tga"))
                File.Delete(tgaFile);
            writeLineToDebugger("Done!");

            // Add to ComboBox
            Invoke(new MethodInvoker(delegate ()
            {
                cbCrosshair.Items.Clear();
                foreach (var crosshair in Directory.GetFiles(PATH_VC_RESOURCES_PREVIEWS, "*.png"))
                {
                    string crosshairName = Path.GetFileNameWithoutExtension(crosshair);
                    cbCrosshair.Items.Add(crosshairName);
                }
            }));

            writeToDebugger("Fetching public crosshair list... ");
            _ = isNewCrosshairsAvailable(true);
            writeLineToDebugger("Done!");

            Invoke(new MethodInvoker(delegate ()
            {
                pictureBoxLoading.Visible = false;
                cbClass.SelectedIndex = -1;
                cbWeapon.SelectedIndex = -1;
                cbWeapon.Enabled = false;
                textBoxTF2Path.Enabled = true;
                btnReload.Enabled = true;
                cbClass.Enabled = true;
            }));
            writeLineToDebugger("Finished reloading crosshair list!");
        }

        private void initDicts()
        {
            // Scout
            classPrimaryMap.Add("Scout", tf2PrimaryScoutWeapons);
            classSecondaryMap.Add("Scout", tf2SecondaryScoutWeapons);
            classMeleeMap.Add("Scout", tf2MeleeScoutWeapons);
            classMiscMap.Add("Scout", tf2MiscScoutWeapons);
            weaponScriptPairs.Add("Scattergun, Back Scatter, Force-A-Nature", "tf_weapon_scattergun.txt");
            weaponScriptPairs.Add("Baby Face's Blaster", "tf_weapon_pep_brawler_blaster.txt");
            weaponScriptPairs.Add("Shortstop", "tf_weapon_handgun_scout_primary.txt");
            weaponScriptPairs.Add("Soda Popper", "tf_weapon_soda_popper.txt");
            weaponScriptPairs.Add("Pistol and all reskins (Scout)", "tf_weapon_pistol_scout.txt");
            weaponScriptPairs.Add("Bonk! Atomic Punch, Crit-a-Cola", "tf_weapon_lunchbox_drink.txt");
            weaponScriptPairs.Add("Pretty Boy's Pocket Pistol, Winger", "tf_weapon_handgun_scout_secondary.txt");
            weaponScriptPairs.Add("Bat and all reskins, Atomizer, Boston Basher, Candy Cane, Fan O'War, Sun-on-a-Stick", "tf_weapon_bat.txt");
            weaponScriptPairs.Add("Holy Mackerel", "tf_weapon_bat_fish.txt");
            weaponScriptPairs.Add("Sandman", "tf_weapon_bat_wood.txt");
            weaponScriptPairs.Add("Wrap Assassin", "tf_weapon_bat_giftwrap.txt");

            // Soldier
            classPrimaryMap.Add("Soldier", tf2PrimarySoldierWeapons);
            classSecondaryMap.Add("Soldier", tf2SecondarySoldierWeapons);
            classMeleeMap.Add("Soldier", tf2MeleeSoldierWeapons);
            classMiscMap.Add("Soldier", tf2MiscSoldierWeapons);
            weaponScriptPairs.Add("Rocket Launcher, Black Box, Original, Liberty Launcher, Beggar's Bazooka", "tf_weapon_rocketlauncher.txt");
            weaponScriptPairs.Add("Air Strike", "tf_weapon_rocketlauncher_airstrike.txt");
            weaponScriptPairs.Add("Cow Mangler 5000", "tf_weapon_particle_cannon.txt");
            weaponScriptPairs.Add("Direct Hit", "tf_weapon_rocketlauncher_directhit.txt");
            weaponScriptPairs.Add("Shotgun, Reserve Shooter, Panic Attack (Soldier)", "tf_weapon_shotgun_soldier.txt");
            weaponScriptPairs.Add("Buff Banner, Battalion's Backup, Concheror", "tf_weapon_buff_item.txt");
            weaponScriptPairs.Add("Righteous Bison", "tf_weapon_raygun.txt");
            weaponScriptPairs.Add("Shovel and all reskins, Equalizer, Pain Train, Disciplinary Action, Market Gardener, Escape Plan", "tf_weapon_shovel.txt");

            // Pyro
            classPrimaryMap.Add("Pyro", tf2PrimaryPyroWeapons);
            classSecondaryMap.Add("Pyro", tf2SecondaryPyroWeapons);
            classMeleeMap.Add("Pyro", tf2MeleePyroWeapons);
            classMiscMap.Add("Pyro", tf2MiscPyroWeapons);
            weaponScriptPairs.Add("Flame Thrower and all reskins, Backburner, Degreaser, Phlogistinator", "tf_weapon_flamethrower.txt");
            weaponScriptPairs.Add("Dragon's Fury", "tf_weapon_rocketlauncher_fireball.txt");
            weaponScriptPairs.Add("Shotgun, Reserve Shooter, Panic Attack (Pyro)", "tf_weapon_shotgun_pyro.txt");
            weaponScriptPairs.Add("Flare Gun, Detonator, Scorch Shot", "tf_weapon_flaregun.txt");
            weaponScriptPairs.Add("Manmelter", "tf_weapon_flaregun_revenge.txt");
            weaponScriptPairs.Add("Thermal Thruster", "tf_weapon_rocketpack.txt");
            weaponScriptPairs.Add("Fire Axe and all reskins, Lollichop, Axtinguisher, Homewrecker, Powerjack, Back Scratcher, Sharpened Volcano Fragment, Third Degree, Neon Annihilator", "tf_weapon_fireaxe.txt");
            weaponScriptPairs.Add("Hot Hand", "tf_weapon_slap.txt");

            // Demoman
            classPrimaryMap.Add("Demoman", tf2PrimaryDemomanWeapons);
            classSecondaryMap.Add("Demoman", tf2SecondaryDemomanWeapons);
            classMeleeMap.Add("Demoman", tf2MeleeDemomanWeapons);
            classMiscMap.Add("Demoman", tf2MiscDemomanWeapons);
            weaponScriptPairs.Add("Grenade Launcher, Loch-n-Load, Iron Bomber", "tf_weapon_grenadelauncher.txt");
            weaponScriptPairs.Add("Loose Cannon", "tf_weapon_cannon.txt");
            weaponScriptPairs.Add("Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher", "tf_weapon_pipebomblauncher.txt");
            weaponScriptPairs.Add("Bottle and all reskins", "tf_weapon_bottle.txt");
            weaponScriptPairs.Add("Eyelander, Scotsman's Skullcutter, Claidheamh Mòr, Persian Persuader, Pain Train", "tf_weapon_sword.txt");
            weaponScriptPairs.Add("Ullapool Caber", "tf_weapon_stickbomb.txt");


            // Heavy
            classPrimaryMap.Add("Heavy", tf2PrimaryHeavyWeapons);
            classSecondaryMap.Add("Heavy", tf2SecondaryHeavyWeapons);
            classMeleeMap.Add("Heavy", tf2MeleeHeavyWeapons);
            classMiscMap.Add("Heavy", tf2MiscHeavyWeapons);
            weaponScriptPairs.Add("Minigun, Natascha, Brass Beast, Tomislav, Huo-Long Heater", "tf_weapon_minigun.txt");
            weaponScriptPairs.Add("Shotgun, Family Business, Panic Attack", "tf_weapon_shotgun_hwg.txt");
            weaponScriptPairs.Add("Sandvich, Dalokohs Bar, Fishcake, Buffalo Steak Sandvich, Second Banana", "tf_weapon_lunchbox.txt");
            weaponScriptPairs.Add("Fists and all reskins, Killing Gloves of Boxing, Gloves of Running Urgently, Warrior's Spirit, Fists of Steel, Eviction Notice, Holiday Punch", "tf_weapon_fists.txt");

            // Engineer
            classPrimaryMap.Add("Engineer", tf2PrimaryEngineerWeapons);
            classSecondaryMap.Add("Engineer", tf2SecondaryEngineerWeapons);
            classMeleeMap.Add("Engineer", tf2MeleeEngineerWeapons);
            classMiscMap.Add("Engineer", tf2MiscEngineerWeapons);
            weaponScriptPairs.Add("Shotgun, Widowmaker, Panic Attack", "tf_weapon_shotgun_primary.txt");
            weaponScriptPairs.Add("Frontier Justice", "tf_weapon_sentry_revenge.txt");
            weaponScriptPairs.Add("Pomson 6000", "tf_weapon_drg_pomson.txt");
            weaponScriptPairs.Add("Rescue Ranger", "tf_weapon_shotgun_building_rescue.txt");
            weaponScriptPairs.Add("Pistol and all reskins (Engineer)", "tf_weapon_pistol.txt");
            weaponScriptPairs.Add("Short Circuit", "tf_weapon_mechanical_arm.txt");
            weaponScriptPairs.Add("Wrangler, Giger Counter", "tf_weapon_laser_pointer.txt");
            weaponScriptPairs.Add("Wrench, Southern Hospitality, Jag, Eureka Effect", "tf_weapon_wrench.txt");
            weaponScriptPairs.Add("Gunslinger", "tf_weapon_robot_arm.txt");
            weaponScriptPairs.Add("Construction PDA", "tf_weapon_pda_engineer_build.txt");
            weaponScriptPairs.Add("Destruction PDA", "tf_weapon_pda_engineer_destroy.txt");

            // Medic
            classPrimaryMap.Add("Medic", tf2PrimaryMedicWeapons);
            classSecondaryMap.Add("Medic", tf2SecondaryMedicWeapons);
            classMeleeMap.Add("Medic", tf2MeleeMedicWeapons);
            classMiscMap.Add("Medic", tf2MiscMedicWeapons);
            weaponScriptPairs.Add("Syringe Gun, Blutsauger, Overdose", "tf_weapon_syringegun_medic.txt");
            weaponScriptPairs.Add("Crusader's Crossbow", "tf_weapon_crossbow.txt");
            weaponScriptPairs.Add("Medi Gun, Kritzkrieg, Quick-Fix, Vaccinator", "tf_weapon_medigun.txt");
            weaponScriptPairs.Add("Bonesaw and all reskins, Ubersaw, Vita-Saw, Amputator, Solemn Vow", "tf_weapon_bonesaw.txt");

            // Sniper
            classPrimaryMap.Add("Sniper", tf2PrimarySniperWeapons);
            classSecondaryMap.Add("Sniper", tf2SecondarySniperWeapons);
            classMeleeMap.Add("Sniper", tf2MeleeSniperWeapons);
            classMiscMap.Add("Sniper", tf2MiscSniperWeapons);
            weaponScriptPairs.Add("Sniper Rifle, Sydney Sleeper, Bazaar Bargain, Machina", "tf_weapon_sniperrifle.txt");
            weaponScriptPairs.Add("Classic", "tf_weapon_sniperrifle_classic.txt");
            weaponScriptPairs.Add("Hitman's Heatmaker", "tf_weapon_sniperrifle_decap.txt");
            weaponScriptPairs.Add("Huntsman, Fortified Compound", "tf_weapon_compound_bow.txt");
            weaponScriptPairs.Add("SMG", "tf_weapon_smg.txt");
            weaponScriptPairs.Add("Cleaner's Carbine", "tf_weapon_charged_smg.txt");
            weaponScriptPairs.Add("Kukri and all reskins, Tribalman's Shiv, Bushwacka, Shahanshah", "tf_weapon_club.txt");

            // Spy
            classPrimaryMap.Add("Spy", tf2PrimarySpyWeapons);
            classSecondaryMap.Add("Spy", tf2SecondarySpyWeapons);
            classMeleeMap.Add("Spy", tf2MeleeSpyWeapons);
            classMiscMap.Add("Spy", tf2MiscSpyWeapons);
            weaponScriptPairs.Add("Revolver, Ambassador, L'Etranger, Enforcer, Diamondback", "tf_weapon_revolver.txt");
            weaponScriptPairs.Add("Knife and all reskins, Your Eternal Reward, Conniver's Kunai, Big Earner, Spy-cicle", "tf_weapon_knife.txt");
            weaponScriptPairs.Add("Disguise kit", "tf_weapon_pda_spy.txt");

            // Multi-class
            weaponScriptPairs.Add("Flying Guillotine, Mad Milk, Gas Passer, Jarate", "tf_weapon_jar.txt"); // Scout/Pyro/Sniper
            weaponScriptPairs.Add("Half-Zatoichi", "tf_weapon_katana.txt"); // Soldier/Demoman
            weaponScriptPairs.Add("Sapper, Red-Tape Recorder, While placing a building", "tf_weapon_builder.txt"); // Spy/Engineer
        }

        private string getCrosshairFromScript(string script)
        {
            string[] scriptLines = File.ReadAllLines(script);
            foreach (string scriptLine in scriptLines) // TODO: Fix this lazy implementation
                if (scriptLine.Contains("vgui/replay/thumbnails/"))
                {
                    string[] crosshairLine = scriptLine.Split('/');
                    string crosshair = crosshairLine[crosshairLine.Length - 1];
                    return crosshair.Substring(0, crosshair.Length - 1);
                }

            throw new Exception($"Could not find crosshair in script {script}");
        }

        private string getWeaponFromScriptName(string script)
        {
            foreach (KeyValuePair<string, string> weaponScriptEntry in weaponScriptPairs)
            {
                if (weaponScriptEntry.Value == Path.GetFileName(script))
                    return weaponScriptEntry.Key;
            }

            throw new Exception($"Could not find weapon from script {script}");
        }

        private bool performSanityCheck(string path)
        {
            writeLineToDebugger("Sanity check started!");
            // Check if specified directory exist (sanity1)
            if (!Directory.Exists(path))
            {
                writeLineToDebugger("Sanity check failed! Error: sanity1");
                MessageBox.Show("The specified TF2 path does not seem to exist.\nDid you set the correct path?", "TF2 Path does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if specified TF2 Path contains hl2.exe (sanity2)
            if (!File.Exists(path + @"\hl2.exe"))
            {
                writeLineToDebugger("Sanity check failed! Error: sanity2");
                MessageBox.Show("The specified TF2 path does not contain \"hl2.exe\".\nDid you set the correct path?", "TF2 Path invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if vtf2tga.exe exists (sanity3)
            if (!File.Exists(path + @"\bin\vtf2tga.exe"))
            {
                writeLineToDebugger("Sanity check failed! Error: sanity3");
                MessageBox.Show("Could not find \"vtf2tga.exe\".\nPlease verify game files.", "vtf2tga.exe does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if vtf2tga.exe exists (sanity4)
            if (!File.Exists(path + @"\bin\tier0.dll"))
            {
                writeLineToDebugger("Sanity check failed! Error: sanity4");
                MessageBox.Show("Could not find \"tier0.dll\".\nPlease verify game files.", "tier0.dll does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if project contains ffmpeg.exe (sanity5)
            if (!File.Exists(PATH_VC + @"\resources\ffmpeg.exe"))
            {
                writeLineToDebugger("Sanity check failed! Error: sanity5");
                MessageBox.Show("Could not find \"ffmpeg.exe\".\nPlease download the latest release of Venom Crosshairs.\n(If the issue persist please create a GitHub issue.)", "ffmpeg.exe could not be found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            writeLineToDebugger("Sanity check finished!");
            return true;
        }
    }
}
