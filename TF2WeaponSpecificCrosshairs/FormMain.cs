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

namespace TF2WeaponSpecificCrosshairs
{
    public partial class FormMain : Form
    {
        private string PATH_TF2WSC = Directory.GetCurrentDirectory();
        private string PATH_TF2WSC_RESOURCES = Directory.GetCurrentDirectory() + @"\resources\";
        private string PATH_TF2WSC_RESOURCES_MATERIALS = Directory.GetCurrentDirectory() + @"\resources\materials\";
        private string PATH_TF2WSC_RESOURCES_PREVIEWS = Directory.GetCurrentDirectory() + @"\resources\previews\";
        private string PATH_TF2WSC_RESOURCES_SCRIPTS = Directory.GetCurrentDirectory() + @"\resources\scripts\";

        private string FILEPATH_GENERATEPREVIEWSBAT = Directory.GetCurrentDirectory() + @"\resources\previews\generatepreviews.bat";

        private bool hasInitialized = false;

        private readonly string[] tf2Classes = { "Scout", "Soldier", "Pyro", "Demoman", "Heavy", "Engineer", "Medic", "Sniper", "Spy" };

        private readonly string[] tf2ScoutWeapons = { "Scattergun, Back Scatter, Force-A-Nature", "Baby Face's Blaster", "Shortstop", "Soda Popper", "Pistol and all reskins (Scout)", "Bonk! Atomic Punch, Crit-a-Cola", "Flying Guillotine", "Mad Milk, Gas Passer, Jarate", "Pretty Boy's Pocket Pistol, Winger", "Bat and all reskins, Atomizer, Boston Basher, Candy Cane, Fan O'War, Sun-on-a-Stick", "Holy Mackerel", "Sandman", "Wrap Assassin" };
        private readonly string[] tf2SoldierWeapons = { "Rocket Launcher, Black Box, Original, Liberty Launcher, Beggar's Bazooka", "Air Strike", "Cow Mangler 5000", "Direct Hit", "Shotgun, Reserve Shooter, Panic Attack", "Buff Banner, Battalion's Backup, Concheror", "Righteous Bison", "Shovel and all reskins, Equalizer, Pain Train, Disciplinary Action, Market Gardener, Escape Plan", "Half-Zatoichi" };
        private readonly string[] tf2PyroWeapons = { "Flame Thrower and all reskins, Backburner, Degreaser, Phlogistinator", "Dragon's Fury", "Shotgun, Reserve Shooter, Panic Attack", "Flare Gun, Detonator, Scorch Shot", "Mad Milk, Gas Passer, Jarate", "Manmelter", "Thermal Thruster", "Fire Axe and all reskins, Lollichop, Axtinguisher, Homewrecker, Powerjack, Back Scratcher, Sharpened Volcano Fragment, Third Degree, Neon Annihilator", "Hot Hand" };
        private readonly string[] tf2DemomanWeapons = { "Grenade Launcher, Loch-n-Load, Iron Bomber", "Loose Cannon", "Stickybomb Launcher, Scottish Resistance, Sticky Jumper, Quickiebomb Launcher", "Bottle and all reskins", "Eyelander, Scotsman's Skullcutter, Claidheamh Mòr, Persian Persuader, Pain Train", "Half-Zatoichi", "Ullapool Caber" };
        private readonly string[] tf2HeavyWeapons = { "Minigun, Natascha, Brass Beast, Tomislav, Huo-Long Heater", "Shotgun, Family Business, Panic Attack", "Sandvich, Dalokohs Bar, Fishcake, Buffalo Steak Sandvich, Second Banana", "Fists and all reskins, Killing Gloves of Boxing, Gloves of Running Urgently, Warrior's Spirit, Fists of Steel, Eviction Notice, Holiday Punch" };
        private readonly string[] tf2EngineerWeapons = { "Shotgun, Widowmaker, Panic Attack", "Frontier Justice", "Pomson 6000", "Rescue Ranger", "Pistol and all reskins (Engineer)", "Short Circuit", "Wrangler, Giger Counter", "Wrench, Southern Hospitality, Jag, Eureka Effect", "Gunslinger", "Construction PDA", "Destruction PDA", "While placing a building" };
        private readonly string[] tf2MedicWeapons = { "Syringe Gun, Blutsauger, Overdose", "Crusader's Crossbow", "Medi Gun, Kritzkrieg, Quick-Fix, Vaccinator", "Bonesaw and all reskins, Ubersaw, Vita-Saw, Amputator, Solemn Vow" };
        private readonly string[] tf2SniperWeapons = { "Sniper Rifle, Sydney Sleeper, Bazaar Bargain, Machina", "Classic", "Hitman's Heatmaker", "Huntsman, Fortified Compound", "SMG", "Cleaner's Carbine", "Mad Milk, Gas Passer, Jarate", "Kukri and all reskins, Tribalman's Shiv, Bushwacka, Shahanshah" };
        private readonly string[] tf2SpyWeapons = { "Revolver, Ambassador, L'Etranger, Enforcer, Diamondback", "Sapper, Red-Tape Recorder", "Knife and all reskins, Your Eternal Reward, Conniver's Kunai, Big Earner, Spy-cicle", "Disguise kit" };


        public FormMain()
        {
            InitializeComponent();
            initTF2WSC();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("This will clear the currently selected crosshairs.\nAre you sure you want to continue?", "Reload crosshairs", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dialogResult == DialogResult.Yes && performSanityCheck())
                new Thread(generateCrosshairs).Start();
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
            if (cbClass.Text.Length > 0 && cbWeapon.Text.Length > 0 && cbCrosshair.Text.Length > 0)
            {
                ListViewItem crosshairWeaponItem = new ListViewItem(new string[] { cbCrosshair.Text, cbWeapon.Text });

                if (!listViewItemExists(listViewChosenCrosshairs, crosshairWeaponItem))
                    listViewChosenCrosshairs.Items.Add(crosshairWeaponItem);

                btnRemoveSelected.Enabled = true;
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
            MessageBox.Show("Feature not implemented yet.", "Feature not implemented!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnInstallClean_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Feature not implemented yet.", "Feature not implemented!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //MessageBox.Show("This will reset any installed TF2WSC config.\nAre you sure you want to continue?", "Feature not implemented!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /*
            Functions
         */
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

                // ListView
                listViewChosenCrosshairs.Columns.Add("Crosshair", 220);
                listViewChosenCrosshairs.Columns.Add("Weapon", 420);
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

        private void generateCrosshairs()
        {
            Invoke(new MethodInvoker(delegate ()
            {
                pictureBoxCrosshair.ImageLocation = PATH_TF2WSC_RESOURCES + @"TF2WSC.png";

                listViewChosenCrosshairs.Items.Clear();

                textBoxTF2Path.Enabled = false;
                btnReload.Enabled = false;
                cbClass.Enabled = false;
                cbCrosshair.Enabled = false;
                cbWeapon.Enabled = false;
                btnAddCrosshair.Enabled = false;
                btnRemoveSelected.Enabled = false;
            }));

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
            // Create or clear generatepreviews.bat
            File.Create(FILEPATH_GENERATEPREVIEWSBAT).Close();

            using (StreamWriter sw = new StreamWriter(FILEPATH_GENERATEPREVIEWSBAT))
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
            generatepreviewsProcess.StartInfo.FileName = FILEPATH_GENERATEPREVIEWSBAT;
            writeLineToDebugger("Done!");

            writeToDebugger("Running generatepreviews.bat... ");
            generatepreviewsProcess.Start();
            generatepreviewsProcess.WaitForExit();
            writeLineToDebugger("Done!");

            moveFilesByExtensionOrDelete(PATH_TF2WSC, PATH_TF2WSC_RESOURCES_PREVIEWS, "png");

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
                cbClass.SelectedIndex = -1;
                cbWeapon.SelectedIndex = -1;
                cbWeapon.Enabled = false;

                textBoxTF2Path.Enabled = true;
                btnReload.Enabled = true;
                cbClass.Enabled = true;
            }));

            writeLineToDebugger("Finished reloading crosshair list!");
        }

        private void moveFilesByExtensionOrDelete(string sourceDirectory, string targetDirectory, string extension)
        {
            foreach (string file in Directory.GetFiles(sourceDirectory, "*." + extension))
                if (!File.Exists(targetDirectory + Path.GetFileName(file)))
                    File.Move(file, targetDirectory + Path.GetFileName(file));
                else
                    File.Delete(file);
        }

        private bool performSanityCheck()
        {
            string path = textBoxTF2Path.Text;

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

        // Events
        private void onCBClassChangeEvent(object sender, EventArgs e)
        {
            cbWeapon.Items.Clear();
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
        }
    }
}
