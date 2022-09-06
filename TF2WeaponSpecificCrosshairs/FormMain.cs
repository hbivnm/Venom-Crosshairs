using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Dialogs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace TF2WeaponSpecificCrosshairs
{
    public partial class FormMain : Form
    {
        private static readonly string PATH_TF2WSC = Directory.GetCurrentDirectory();
        private static readonly string PATH_TF2WSC_RESOURCES = Directory.GetCurrentDirectory() + @"\resources\";
        private static readonly string PATH_TF2WSC_RESOURCES_TF2WSC_EXPLOSION_EFFECT_CFG_FILE = PATH_TF2WSC_RESOURCES + @"\tf2wsc_expeff.cfg";
        private static readonly string PATH_TF2WSC_RESOURCES_TF2WSC_USERPATH_CFG_FILE = PATH_TF2WSC_RESOURCES + @"\tf2wsc_userpath.cfg";
        private static readonly string PATH_TF2WSC_RESOURCES_MATERIALS = Directory.GetCurrentDirectory() + @"\resources\materials\";
        private static readonly string PATH_TF2WSC_RESOURCES_PREVIEWS = Directory.GetCurrentDirectory() + @"\resources\previews\";
        private static readonly string PATH_TF2WSC_RESOURCES_SCRIPTS = Directory.GetCurrentDirectory() + @"\resources\scripts\";

        private static readonly string PATH_TF2WSC_RESOURCES_PREVIEWS_GENERATEPREVIEWSBAT = Directory.GetCurrentDirectory() + @"\resources\previews\generatepreviews.bat";

        private bool hasInitialized = false;

        private readonly string[] tf2Classes = { "Scout", "Soldier", "Pyro", "Demoman", "Heavy", "Engineer", "Medic", "Sniper", "Spy" };

        private static readonly string[] tf2ScoutWeapons = { "Scattergun, Back Scatter, Force-A-Nature", "Baby Face's Blaster", "Shortstop", "Soda Popper", "Pistol and all reskins (Scout)", "Bonk! Atomic Punch, Crit-a-Cola", "Flying Guillotine", "Mad Milk, Gas Passer, Jarate", "Pretty Boy's Pocket Pistol, Winger", "Bat and all reskins, Atomizer, Boston Basher, Candy Cane, Fan O'War, Sun-on-a-Stick", "Holy Mackerel", "Sandman", "Wrap Assassin" };
        private static readonly string[] tf2SoldierWeapons = { "Rocket Launcher, Black Box, Original, Liberty Launcher, Beggar's Bazooka", "Air Strike", "Cow Mangler 5000", "Direct Hit", "Shotgun, Reserve Shooter, Panic Attack (Soldier)", "Buff Banner, Battalion's Backup, Concheror", "Righteous Bison", "Shovel and all reskins, Equalizer, Pain Train, Disciplinary Action, Market Gardener, Escape Plan", "Half-Zatoichi" };
        private static readonly string[] tf2PyroWeapons = { "Flame Thrower and all reskins, Backburner, Degreaser, Phlogistinator", "Dragon's Fury", "Shotgun, Reserve Shooter, Panic Attack (Pyro)", "Flare Gun, Detonator, Scorch Shot", "Mad Milk, Gas Passer, Jarate", "Manmelter", "Thermal Thruster", "Fire Axe and all reskins, Lollichop, Axtinguisher, Homewrecker, Powerjack, Back Scratcher, Sharpened Volcano Fragment, Third Degree, Neon Annihilator", "Hot Hand" };
        private static readonly string[] tf2DemomanWeapons = { "Grenade Launcher, Loch-n-Load, Iron Bomber", "Loose Cannon", "Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher", "Bottle and all reskins", "Eyelander, Scotsman's Skullcutter, Claidheamh Mòr, Persian Persuader, Pain Train", "Half-Zatoichi", "Ullapool Caber" };
        private static readonly string[] tf2HeavyWeapons = { "Minigun, Natascha, Brass Beast, Tomislav, Huo-Long Heater", "Shotgun, Family Business, Panic Attack", "Sandvich, Dalokohs Bar, Fishcake, Buffalo Steak Sandvich, Second Banana", "Fists and all reskins, Killing Gloves of Boxing, Gloves of Running Urgently, Warrior's Spirit, Fists of Steel, Eviction Notice, Holiday Punch" };
        private static readonly string[] tf2EngineerWeapons = { "Shotgun, Widowmaker, Panic Attack", "Frontier Justice", "Pomson 6000", "Rescue Ranger", "Pistol and all reskins (Engineer)", "Short Circuit", "Wrangler, Giger Counter", "Wrench, Southern Hospitality, Jag, Eureka Effect", "Gunslinger", "Construction PDA", "Destruction PDA", "While placing a building" };
        private static readonly string[] tf2MedicWeapons = { "Syringe Gun, Blutsauger, Overdose", "Crusader's Crossbow", "Medi Gun, Kritzkrieg, Quick-Fix, Vaccinator", "Bonesaw and all reskins, Ubersaw, Vita-Saw, Amputator, Solemn Vow" };
        private static readonly string[] tf2SniperWeapons = { "Sniper Rifle, Sydney Sleeper, Bazaar Bargain, Machina", "Classic", "Hitman's Heatmaker", "Huntsman, Fortified Compound", "SMG", "Cleaner's Carbine", "Mad Milk, Gas Passer, Jarate", "Kukri and all reskins, Tribalman's Shiv, Bushwacka, Shahanshah" };
        private static readonly string[] tf2SpyWeapons = { "Revolver, Ambassador, L'Etranger, Enforcer, Diamondback", "Sapper, Red-Tape Recorder", "Knife and all reskins, Your Eternal Reward, Conniver's Kunai, Big Earner, Spy-cicle", "Disguise kit" };
        private static readonly string[] tf2AllWeapons = tf2ScoutWeapons.Concat(tf2SoldierWeapons).Concat(tf2PyroWeapons).Concat(tf2DemomanWeapons).Concat(tf2HeavyWeapons).Concat(tf2EngineerWeapons).Concat(tf2MedicWeapons).Concat(tf2SniperWeapons).Concat(tf2SpyWeapons).ToArray();

        private static readonly string[] tf2PrimaryWeapons = { "Scattergun, Back Scatter, Force-A-Nature", "Baby Face's Blaster", "Shortstop", "Soda Popper", "Rocket Launcher, Black Box, Original, Liberty Launcher, Beggar's Bazooka", "Air Strike", "Cow Mangler 5000", "Direct Hit", "Flame Thrower and all reskins, Backburner, Degreaser, Phlogistinator", "Dragon's Fury", "Grenade Launcher, Loch-n-Load, Iron Bomber", "Loose Cannon", "Minigun, Natascha, Brass Beast, Tomislav, Huo-Long Heater", "Shotgun, Widowmaker, Panic Attack", "Frontier Justice", "Pomson 6000", "Rescue Ranger", "Syringe Gun, Blutsauger, Overdose", "Crusader's Crossbow", "Sniper Rifle, Sydney Sleeper, Bazaar Bargain, Machina", "Classic", "Hitman's Heatmaker", "Huntsman, Fortified Compound", "Revolver, Ambassador, L'Etranger, Enforcer, Diamondback" };
        private static readonly string[] tf2SecondaryWeapons = { "Pistol and all reskins (Scout)", "Bonk! Atomic Punch, Crit-a-Cola", "Flying Guillotine", "Mad Milk, Gas Passer, Jarate", "Pretty Boy's Pocket Pistol, Winger", "Shotgun, Reserve Shooter, Panic Attack (Soldier)", "Buff Banner, Battalion's Backup, Concheror", "Righteous Bison", "Shotgun, Reserve Shooter, Panic Attack (Pyro)", "Flare Gun, Detonator, Scorch Shot", "Manmelter", "Thermal Thruster", "Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher", "Shotgun, Family Business, Panic Attack", "Sandvich, Dalokohs Bar, Fishcake, Buffalo Steak Sandvich, Second Banana", "Pistol and all reskins (Engineer)", "Short Circuit", "Wrangler, Giger Counter", "Medi Gun, Kritzkrieg, Quick-Fix, Vaccinator", "SMG", "Cleaner's Carbine", "Sapper, Red-Tape Recorder" };
        private static readonly string[] tf2MeleeWeapons = { "Bat and all reskins, Atomizer, Boston Basher, Candy Cane, Fan O'War, Sun-on-a-Stick", "Holy Mackerel", "Sandman", "Wrap Assassin", "Shovel and all reskins, Equalizer, Pain Train, Disciplinary Action, Market Gardener, Escape Plan", "Half-Zatoichi", "Fire Axe and all reskins, Lollichop, Axtinguisher, Homewrecker, Powerjack, Back Scratcher, Sharpened Volcano Fragment, Third Degree, Neon Annihilator", "Hot Hand", "Bottle and all reskins", "Eyelander, Scotsman's Skullcutter, Claidheamh Mòr, Persian Persuader, Pain Train", "Ullapool Caber", "Fists and all reskins, Killing Gloves of Boxing, Gloves of Running Urgently, Warrior's Spirit, Fists of Steel, Eviction Notice, Holiday Punch", "Wrench, Southern Hospitality, Jag, Eureka Effect", "Gunslinger", "Bonesaw and all reskins, Ubersaw, Vita-Saw, Amputator, Solemn Vow", "Kukri and all reskins, Tribalman's Shiv, Bushwacka, Shahanshah", "Knife and all reskins, Your Eternal Reward, Conniver's Kunai, Big Earner, Spy-cicle" };
        private static readonly string[] tf2MiscWeapons = { "Construction PDA", "Destruction PDA", "While placing a building", "Disguise kit" };


        public FormMain()
        {
            InitializeComponent();
            initTF2WSC();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You are about to reload the crosshair list. This will clear the currently selected crosshairs.\nAre you sure you want to continue?\n\nWARNING: This might take a long time!", "Reload crosshairs", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dialogResult == DialogResult.Yes && performSanityCheck(textBoxTF2Path.Text))
                new Thread(generateCrosshairs).Start();
        }

        private void btnGitHub_Click(object sender, EventArgs e)
        {
            Process.Start(@"https://github.com/hbivnm/TF2WeaponSpecificCrosshairs");
        }

        private void btnSteam_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://steamcommunity.com/profiles/76561197996468677");
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
                        File.WriteAllText(PATH_TF2WSC_RESOURCES_TF2WSC_USERPATH_CFG_FILE, cofd.FileName);
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

        private void btnAddCrosshair_Click(object sender, EventArgs e)
        {
            bool crosshairAdded = false;

            if (cbClass.Text.Length > 0 && cbWeapon.Text.Length > 0 && cbCrosshair.Text.Length > 0)
            {
                addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, cbWeapon.Text }));
                crosshairAdded = true;
            }

            if (checkBoxAddClassWeapons.Checked)
            {
                foreach (var weapon in getWeaponsFromClassName(cbClass.Text))
                    addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon }));
                crosshairAdded = true;
            }

            if (checkBoxAddPrimaryWeapons.Checked)
            {
                foreach (var weapon in tf2PrimaryWeapons)
                    addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon }));
                crosshairAdded = true;
            }

            if (checkBoxAddSecondaryWeapons.Checked)
            {
                foreach (var weapon in tf2SecondaryWeapons)
                    addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon }));
                crosshairAdded = true;
            }

            if (checkBoxAddMeleeWeapons.Checked)
            {
                foreach (var weapon in tf2MeleeWeapons)
                    addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon }));
                crosshairAdded = true;
            }

            if (checkBoxAddMiscWeapons.Checked)
            {
                foreach (var weapon in tf2MiscWeapons)
                    addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon }));
                crosshairAdded = true;
            }

            if (crosshairAdded) // This could be changed to an event that triggers when addCrosshairToListView is called
            {
                btnRemoveSelected.Enabled = true;
                btnInstall.Enabled = true;
                btnInstallClean.Enabled = true;
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

        private void btnInstall_Click(object sender, EventArgs e)
        {
            if (listViewChosenCrosshairs.Items.Count > 0 && performSanityCheck(textBoxTF2Path.Text))
                Task.Run(() => performInstallation(false));
        }

        private void btnInstallClean_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("This will reset any installed TF2WSC config.\nAre you sure you want to continue?", "Clean installation. Are you sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes && listViewChosenCrosshairs.Items.Count > 0 && performSanityCheck(textBoxTF2Path.Text))
                Task.Run(() => performInstallation(true));

        }

        /// 
        /// Events
        /// 
        private void onCBClassChangeEvent(object sender, EventArgs e)
        {
            cbWeapon.Items.Clear();
            checkBoxAddClassWeapons.Text = $@"Add to EVERY {cbClass.Text} weapon?";
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
                    cbWeapon.DropDownWidth = 240;
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
                pictureBoxCrosshair.ImageLocation = PATH_TF2WSC_RESOURCES_PREVIEWS + cbCrosshair.Text + ".png";
            else
                pictureBoxCrosshair.ImageLocation = PATH_TF2WSC_RESOURCES + @"TF2WSC.png";

            btnAddCrosshair.Enabled = true;
            checkBoxAddClassWeapons.Enabled = true;
            checkBoxAddPrimaryWeapons.Enabled = true;
            checkBoxAddSecondaryWeapons.Enabled = true;
            checkBoxAddMeleeWeapons.Enabled = true;
            checkBoxAddMiscWeapons.Enabled = true;
        }

        private void onCBExplosionEffectChangeEvent(object server, EventArgs e)
        {
            File.WriteAllText(PATH_TF2WSC_RESOURCES_TF2WSC_EXPLOSION_EFFECT_CFG_FILE, Convert.ToString(comboBoxExplosionEffect.SelectedIndex));
        }

        /// 
        /// Functions
        /// 
        private void initTF2WSC()
        {
            // Prevent initTF2WSC from running more than once
            if (!hasInitialized)
            {
                // Classes
                foreach (var tf2Class in tf2Classes)
                    cbClass.Items.Add(tf2Class);
                cbClass.SelectedIndexChanged += new EventHandler(onCBClassChangeEvent);

                // Weapons
                cbWeapon.SelectedIndexChanged += new EventHandler(onCBWeaponChangeEvent);

                // Crosshairs
                cbCrosshair.Items.Clear();
                foreach (var crosshair in Directory.GetFiles(PATH_TF2WSC_RESOURCES_PREVIEWS, "*.png"))
                {
                    string crosshairName = Path.GetFileNameWithoutExtension(crosshair);
                    cbCrosshair.Items.Add(crosshairName);
                }
                cbCrosshair.SelectedIndexChanged += new EventHandler(onCBCrosshairChangeEvent);

                // No explosion
                if (File.Exists(PATH_TF2WSC_RESOURCES_TF2WSC_EXPLOSION_EFFECT_CFG_FILE))
                    try
                    {
                        comboBoxExplosionEffect.SelectedIndex = Convert.ToInt32(File.ReadAllText(PATH_TF2WSC_RESOURCES_TF2WSC_EXPLOSION_EFFECT_CFG_FILE));
                    }
                    catch (Exception)
                    {
                        throw new FormatException($@"The contents of {PATH_TF2WSC_RESOURCES_TF2WSC_EXPLOSION_EFFECT_CFG_FILE} could not be parsed to an Integer.");
                    }
                else
                    comboBoxExplosionEffect.SelectedIndex = 0;

                comboBoxExplosionEffect.SelectedIndexChanged += new EventHandler(onCBExplosionEffectChangeEvent);

                // ListView
                listViewChosenCrosshairs.Columns.Add("Crosshair", 220);
                listViewChosenCrosshairs.Columns.Add("Weapon", 420);

                // Read settings
                if (File.Exists(PATH_TF2WSC_RESOURCES_TF2WSC_USERPATH_CFG_FILE))
                    textBoxTF2Path.Text = File.ReadAllText(PATH_TF2WSC_RESOURCES_TF2WSC_USERPATH_CFG_FILE);
            }
        }

        private void writeLineToDebugger(string text)
        {
            Invoke(new MethodInvoker(delegate ()
            {
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
                checkBoxAddClassWeapons.Enabled = false;
                checkBoxAddPrimaryWeapons.Enabled = false;
                checkBoxAddSecondaryWeapons.Enabled = false;
                checkBoxAddMeleeWeapons.Enabled = false;
                checkBoxAddMiscWeapons.Enabled = false;
                btnRemoveSelected.Enabled = false;
                btnPrevCrosshair.Enabled = false;
                btnNextCrosshair.Enabled = false;
                btnInstall.Enabled = false;
                btnInstallClean.Enabled = false;

            }));

            writeLineToDebugger("");
            bool isUpdate = false;
            if (removeOldConfig)
            {
                writeLineToDebugger("Clean installation of TF2WSC started...");
                writeToDebugger("Removing old TF2WSC configs... ");
                if (Directory.Exists($@"{textBoxTF2Path.Text}\tf\custom\TF2WeaponSpecificCrosshairs"))
                    Directory.Delete($@"{textBoxTF2Path.Text}\tf\custom\TF2WeaponSpecificCrosshairs", true);
                writeLineToDebugger("Done!");
            }
            else
                if (Directory.Exists($@"{textBoxTF2Path.Text}\tf\custom\TF2WeaponSpecificCrosshairs"))
            {
                writeLineToDebugger("Updating current TF2WSC config...");
                isUpdate = true;
            }
            else
                writeLineToDebugger("Installation of TF2WSC started...");

            // Installation process
            if (!Directory.Exists($@"{textBoxTF2Path.Text}\tf\custom\TF2WeaponSpecificCrosshairs\materials\vgui\replay\thumbnails"))
                Directory.CreateDirectory($@"{textBoxTF2Path.Text}\tf\custom\TF2WeaponSpecificCrosshairs\materials\vgui\replay\thumbnails");
            if (!Directory.Exists($@"{textBoxTF2Path.Text}\tf\custom\TF2WeaponSpecificCrosshairs\scripts"))
                Directory.CreateDirectory($@"{textBoxTF2Path.Text}\tf\custom\TF2WeaponSpecificCrosshairs\scripts");

            writeToDebugger("Copying materials... ");
            foreach (var crosshairVMT in Directory.GetFiles(PATH_TF2WSC_RESOURCES_MATERIALS, "*.vmt"))
                File.Copy(crosshairVMT, $@"{textBoxTF2Path.Text}\tf\custom\TF2WeaponSpecificCrosshairs\materials\vgui\replay\thumbnails\{Path.GetFileName(crosshairVMT)}", true);
            foreach (var crosshairVTF in Directory.GetFiles(PATH_TF2WSC_RESOURCES_MATERIALS, "*.vtf"))
                File.Copy(crosshairVTF, $@"{textBoxTF2Path.Text}\tf\custom\TF2WeaponSpecificCrosshairs\materials\vgui\replay\thumbnails\{Path.GetFileName(crosshairVTF)}", true);
            writeLineToDebugger("Done!");

            writeToDebugger("Adding scripts... ");
            Invoke(new MethodInvoker(delegate ()
            {
                foreach (ListViewItem item in listViewChosenCrosshairs.Items)
                {
                    string crosshair = item.SubItems[0].Text;
                    string weapon = item.SubItems[1].Text;
                    string weaponFilename = getWeaponFilenameFromWeaponName(weapon);
                    string fullScriptPath = $@"{textBoxTF2Path.Text}\tf\custom\TF2WeaponSpecificCrosshairs\scripts\{weaponFilename}";

                    if (File.Exists(fullScriptPath))
                        File.Delete(fullScriptPath);

                    File.WriteAllText(
                        $@"{textBoxTF2Path.Text}\tf\custom\TF2WeaponSpecificCrosshairs\scripts\{weaponFilename}",
                        File.ReadAllText($@"{PATH_TF2WSC_RESOURCES_SCRIPTS}\{weaponFilename}")
                            .Replace("TF2WSC_PLACEHOLDER_EXPLOSION_EFFECT", getExplosionEffectParticleName(comboBoxExplosionEffect.Text))
                            .Replace("TF2WSC_PLACEHOLDER_EXPLOSION_PLAYER_EFFECT", getExplosionPlayerEffectParticleName(comboBoxExplosionEffect.Text))
                            .Replace("TF2WSC_PLACEHOLDER_EXPLOSION_WATER_EFFECT", getExplosionWaterEffectParticleName(comboBoxExplosionEffect.Text))
                            .Replace("TF2WSC_PLACEHOLDER", crosshair)
                    );
                }
            }));
            writeLineToDebugger("Done!");

            if (!isUpdate)
            {
                writeLineToDebugger("==============================");
                writeLineToDebugger("TF2WSC successfully installed!");
            }
            else
            {
                writeLineToDebugger("======================");
                writeLineToDebugger("TF2WSC config updated!");
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
                checkBoxAddClassWeapons.Enabled = true;
                checkBoxAddPrimaryWeapons.Enabled = true;
                checkBoxAddSecondaryWeapons.Enabled = true;
                checkBoxAddMeleeWeapons.Enabled = true;
                checkBoxAddMiscWeapons.Enabled = true;
                btnRemoveSelected.Enabled = true;
                btnPrevCrosshair.Enabled = true;
                btnNextCrosshair.Enabled = true;
                btnInstall.Enabled = true;
                btnInstallClean.Enabled = true;
            }));
        }

        private void generateCrosshairs()
        {
            Invoke(new MethodInvoker(delegate ()
            {
                pictureBoxLoading.Visible = true;
                pictureBoxCrosshair.ImageLocation = PATH_TF2WSC_RESOURCES + @"TF2WSC.png";

                listViewChosenCrosshairs.Items.Clear();

                textBoxTF2Path.Enabled = false;
                btnReload.Enabled = false;
                cbClass.Enabled = false;
                cbCrosshair.Enabled = false;
                cbWeapon.Enabled = false;
                btnAddCrosshair.Enabled = false;
                checkBoxAddClassWeapons.Enabled = false;
                checkBoxAddPrimaryWeapons.Enabled = false;
                checkBoxAddSecondaryWeapons.Enabled = false;
                checkBoxAddMeleeWeapons.Enabled = false;
                checkBoxAddMiscWeapons.Enabled = false;
                btnRemoveSelected.Enabled = false;
                btnPrevCrosshair.Enabled = false;
                btnNextCrosshair.Enabled = false;
                btnInstall.Enabled = false;
                btnInstallClean.Enabled = false;
            }));

            writeToDebugger("Deleting old previews... ");
            foreach (string previewFile in Directory.GetFiles(PATH_TF2WSC_RESOURCES_PREVIEWS))
                File.Delete(previewFile);
            writeLineToDebugger("Done!");

            writeToDebugger("Preparing vtf2tga process... ");
            Process vtf2tgaProcess = new Process();
            vtf2tgaProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            vtf2tgaProcess.StartInfo.FileName = textBoxTF2Path.Text + @"\bin\vtf2tga.exe";
            writeLineToDebugger("Done!");

            writeToDebugger("Running vtf2tga.exe... ");
            // Generate previews
            foreach (string vtfFile in Directory.GetFiles(PATH_TF2WSC_RESOURCES_MATERIALS, "*.vtf"))
            {
                //writeLineToDebugger("vtf2tga.exe -i " + Path.GetFileName(vtfFile));
                vtf2tgaProcess.StartInfo.Arguments = @"/C -i " + vtfFile;
                vtf2tgaProcess.Start();
            }
            vtf2tgaProcess.WaitForExit();
            writeLineToDebugger("Done!");

            moveFilesByExtensionOrDelete(PATH_TF2WSC_RESOURCES_MATERIALS, PATH_TF2WSC_RESOURCES_PREVIEWS, "tga");

            writeToDebugger("Generating generatepreviews.bat... ");
            File.Create(PATH_TF2WSC_RESOURCES_PREVIEWS_GENERATEPREVIEWSBAT).Close();
            using (StreamWriter sw = new StreamWriter(PATH_TF2WSC_RESOURCES_PREVIEWS_GENERATEPREVIEWSBAT))
            {
                foreach (string tgaFile in Directory.GetFiles(PATH_TF2WSC_RESOURCES_PREVIEWS, "*.tga"))
                {
                    string filename = Path.GetFileNameWithoutExtension(tgaFile);
                    sw.WriteLine(PATH_TF2WSC_RESOURCES + "ffmpeg.exe -i " + tgaFile + " " + filename + ".png");
                }
                sw.WriteLine("exit");
            }
            writeLineToDebugger("Done!");

            writeToDebugger("Preparing generatepreviews process... ");
            Process generatepreviewsProcess = new Process();
            generatepreviewsProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            generatepreviewsProcess.StartInfo.FileName = PATH_TF2WSC_RESOURCES_PREVIEWS_GENERATEPREVIEWSBAT;
            writeLineToDebugger("Done!");

            writeToDebugger("Running generatepreviews.bat... ");
            generatepreviewsProcess.Start();
            generatepreviewsProcess.WaitForExit();
            writeLineToDebugger("Done!");

            writeToDebugger("Performing cleanup... ");
            moveFilesByExtensionOrDelete(PATH_TF2WSC, PATH_TF2WSC_RESOURCES_PREVIEWS, "png"); // Function no longer needed due to cleanup steps, remove/refactor?
            File.Delete(PATH_TF2WSC_RESOURCES_PREVIEWS_GENERATEPREVIEWSBAT);
            foreach (string tgaFile in Directory.GetFiles(PATH_TF2WSC_RESOURCES_PREVIEWS, "*.tga"))
                File.Delete(tgaFile);
            writeLineToDebugger("Done!");


            // Add to ComboBox
            Invoke(new MethodInvoker(delegate ()
            {
                cbCrosshair.Items.Clear();
                foreach (var crosshair in Directory.GetFiles(PATH_TF2WSC_RESOURCES_PREVIEWS, "*.png"))
                {
                    string crosshairName = Path.GetFileNameWithoutExtension(crosshair);
                    cbCrosshair.Items.Add(crosshairName);
                }
            }));

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

            writeLineToDebugger("==================================");
            writeLineToDebugger("Finished reloading crosshair list!");
        }

        private string getWeaponFilenameFromWeaponName(string weapon)
        {
            switch (weapon)
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
                case "Flying Guillotine":
                    return "tf_weapon_cleaver.txt";
                case "Mad Milk, Gas Passer, Jarate": // Scout/Pyro/Sniper
                    return "tf_weapon_jar_milk.txt";
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
                case "Half-Zatoichi": // Soldier/Demoman
                    return "tf_weapon_katana.txt";
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
                case "While placing a building":
                    return "tf_weapon_builder.txt";
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
                case "Sniper Rifle, Sydney Sleeper, Bazaar Bargain, Machina":
                    return "tf_weapon_sniperrifle.txt";
                case "Classic":
                    return "tf_weapon_sniperrifle_classic.txt";
                case "Hitman's Heatmaker":
                    return "tf_weapon_sniperrifle_decap.txt";
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
                case "Sapper, Red-Tape Recorder":
                    return "tf_weapon_sapper.txt";
                case "Knife and all reskins, Your Eternal Reward, Conniver's Kunai, Big Earner, Spy-cicle":
                    return "tf_weapon_knife.txt";
                case "Disguise kit":
                    return "tf_weapon_pda_spy.txt";

                default:
                    throw new ArgumentException($"There is no script associated with '{weapon}'!");
            }
        }

        private bool performSanityCheck(string path)
        {
            writeLineToDebugger("");
            // Check if specified directory exist
            writeToDebugger("Does given path exist? ");
            if (!Directory.Exists(path))
            {
                writeLineToDebugger("No!");
                MessageBox.Show("The specified TF2 path does not seem to exist.\nDid you set the correct path?", "TF2 Path does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            writeLineToDebugger("Yes.");

            // Check if specified TF2 Path contains hl2.exe
            writeToDebugger("Does \"hl2.exe\" exist? ");
            if (!File.Exists(path + @"\hl2.exe"))
            {
                writeLineToDebugger("No!");
                MessageBox.Show("The specified TF2 path does not contain \"hl2.exe\".\nDid you set the correct path?", "TF2 Path invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            writeLineToDebugger("Yes.");

            // Check if vtf2tga.exe exists
            writeToDebugger("Does \"vtf2tga.exe\" exist? ");
            if (!File.Exists(path + @"\bin\vtf2tga.exe"))
            {
                writeLineToDebugger("No!");
                MessageBox.Show("Could not find \"vta2tga.exe\".\nPlease verify game files.", "vta2tga.exe does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            writeLineToDebugger("Yes.");

            // Check if vtf2tga.exe exists
            writeToDebugger("Does \"tier0.dll\" exist? ");
            if (!File.Exists(path + @"\bin\tier0.dll"))
            {
                writeLineToDebugger("No!");
                MessageBox.Show("Could not find \"tier0.dll\".\nPlease verify game files.", "tier0.dll does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            writeLineToDebugger("Yes.");

            // Check if project contains ffmpeg.exe
            writeToDebugger("Does \"ffmpeg.exe\" exist? ");
            if (!File.Exists(PATH_TF2WSC + @"\resources\ffmpeg.exe"))
            {
                writeLineToDebugger("No!");
                MessageBox.Show("Could not find \"ffmpeg.exe\".\nPlease download the latest release of TF2WSC.\n(If that doesn't work create GitHub issue.)", "ffmpeg.exe missing from project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            writeLineToDebugger("Yes.");

            return true;
        }


    }
}
