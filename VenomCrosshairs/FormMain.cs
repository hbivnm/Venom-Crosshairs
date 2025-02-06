using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ImageMagick;

namespace VenomCrosshairs
{
    public partial class FormMain : Form
    {
        private static readonly string VC_VERSION = "beta16.0";

        private static readonly string VC_CONFIG_NAME = "_VenomCrosshairsConfig";
        private static readonly string[] PREVIOUS_CONFIG_NAMES = { "VenomCrosshairsConfig", "TF2WeaponSpecificCrosshairs", "VenomCrosshairConfig" };

        private static readonly string PATH_VC = Directory.GetCurrentDirectory();
        private static readonly string PATH_VC_RESOURCES = PATH_VC + @"\resources\";
        private static readonly string PATH_VC_RESOURCES_MATERIALS = PATH_VC + @"\resources\materials\";
        private static readonly string PATH_VC_RESOURCES_PRESETS = PATH_VC + @"\resources\presets\";
        private static readonly string PATH_VC_RESOURCES_PRESETS_AUTOSAVE = PATH_VC + @"\resources\presets\autosave\";
        private static readonly string PATH_VC_RESOURCES_PREVIEWS = PATH_VC + @"\resources\previews\";
        private static readonly string PATH_VC_RESOURCES_SCRIPTS = PATH_VC + @"\resources\scripts\";
        private static readonly string PATH_VC_RESOURCES_VC_USERSETTINGS_CFG_FILE = PATH_VC_RESOURCES + @"\vc_usersettings.cfg";

        private static readonly int MAX_AUTOSAVES = 10;

        private static readonly int FORM_WIDTH_DEFAULT = 876;
        private static readonly int FORM_WIDTH_CONSOLE = 1251;
        private static readonly int FORM_HEIGHT_DEFAULT = 590;

        private static readonly string[] TF2_CLASSES = { "Scout", "Soldier", "Pyro", "Demoman", "Heavy", "Engineer", "Medic", "Sniper", "Spy" };

        private Dictionary<string, string> gPublicCrosshairs = new Dictionary<string, string>();
        private VCUserSettings gUserSettings = null;
        private bool gHasInitialized = false;
        private bool gIsDarkMode = false;
        private bool gShowConsole = true;
        private bool gHasSelectedCrosshair = false;

        public FormMain()
        {
            InitializeComponent();
            initVC();
        }

        /// 
        /// Controls
        ///
        private void btnSteam_Click(object sender, EventArgs e)
        {
            Process.Start(@"https://steamcommunity.com/profiles/76561197996468677");
        }

        private void btnGitHub_Click(object sender, EventArgs e)
        {
            Process.Start(@"https://github.com/hbivnm/Venom-Crosshairs");
        }

        private void btnToggleConsole_Click(object sender, EventArgs e)
        {
            toggleConsole();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (this.panelSettings.Visible)
                this.panelSettings.Visible = false;
            else
                this.panelSettings.Visible = true;
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You are about to reload the crosshair list.\nAre you sure you want to continue?\n\nWARNING: This might take some time!", "Venom Crosshairs - Reload crosshair list", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dialogResult == DialogResult.Yes && performSanityCheck(textBoxTF2Path.Text))
                new Thread(reloadCrosshairList).Start();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You are about to download new/missing crosshairs.\nAre you sure you want to continue?\n\nWARNING: This might take some time!", "Venom Crosshairs - Download new/missing crosshairs", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dialogResult == DialogResult.Yes && performSanityCheck(textBoxTF2Path.Text))
                new Thread(downloadAndGenerateNewCrosshairs).Start();
        }

        private void btnPresetFolder_Click(object sender, EventArgs e)
        {
            Process.Start(PATH_VC_RESOURCES_PRESETS);
        }

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            gIsDarkMode = !gIsDarkMode;
            setDarkModeTheme(gIsDarkMode);
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
                        gUserSettings.UserTF2Path = cofd.FileName;
                        File.WriteAllText(PATH_VC_RESOURCES_VC_USERSETTINGS_CFG_FILE, JsonConvert.SerializeObject(gUserSettings));
                        textBoxTF2Path.Text = gUserSettings.UserTF2Path;
                        renameOldConfig();
                    }));
            }
        }

        private void btnPrevCrosshair_Click(object sender, EventArgs e)
        {
            if (cbCrosshair.Items.Count > 0)
                if (cbCrosshair.SelectedIndex == 0 || cbCrosshair.SelectedIndex == -1)
                    cbCrosshair.SelectedIndex = cbCrosshair.Items.Count - 1;
                else
                    cbCrosshair.SelectedIndex -= 1;
        }

        private void btnNextCrosshair_Click(object sender, EventArgs e)
        {
            if (cbCrosshair.Items.Count > 0)
                if (cbCrosshair.SelectedIndex == cbCrosshair.Items.Count - 1 || cbCrosshair.SelectedIndex == -1)
                    cbCrosshair.SelectedIndex = 0;
                else
                    cbCrosshair.SelectedIndex += 1;
        }

        private void btnAddCrosshair_Click(object sender, EventArgs e)
        {
            bool crosshairAdded = false;

            // Add crosshair if no checkboxes are checked
            if (cbClass.Text.Length > 0
                && cbWeapon.Text.Length > 0
                && cbCrosshair.Text.Length > 0
                && !checkBoxAddPrimaryWeapons.Checked
                && !checkBoxAddSecondaryWeapons.Checked
                && !checkBoxAddMeleeWeapons.Checked
                && !checkBoxAddMiscWeapons.Checked)
            {
                addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, cbWeapon.Text, cbClass.Text }));
                crosshairAdded = true;
            }

            // Add ONLY to _CLASS_
            if (checkBoxAddOnlyClass.Checked)
            {
                if (checkBoxAddPrimaryWeapons.Checked)
                {
                    foreach (var weapon in TF2Weapons.getPrimaryWeapons(cbClass.Text))
                        addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon, cbClass.Text }));
                    crosshairAdded = true;
                }

                if (checkBoxAddSecondaryWeapons.Checked)
                {
                    foreach (var weapon in TF2Weapons.getSecondaryWeapons(cbClass.Text))
                        addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon, cbClass.Text }));
                    crosshairAdded = true;
                }

                if (checkBoxAddMeleeWeapons.Checked)
                {
                    foreach (var weapon in TF2Weapons.getMeleeWeapons(cbClass.Text))
                        addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon, cbClass.Text }));
                    crosshairAdded = true;
                }

                if (checkBoxAddMiscWeapons.Checked)
                {
                    foreach (var weapon in TF2Weapons.getMiscWeapons(cbClass.Text))
                        addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon, cbClass.Text }));
                    crosshairAdded = true;
                }
            }
            // Add to _ALL_
            else if (!checkBoxAddOnlyClass.Checked)
            {
                if (checkBoxAddPrimaryWeapons.Checked)
                {
                    foreach (var tf2Class in TF2_CLASSES)
                        foreach (var weapon in TF2Weapons.getPrimaryWeapons(tf2Class))
                            addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon, tf2Class }));
                    crosshairAdded = true;
                }

                if (checkBoxAddSecondaryWeapons.Checked)
                {
                    foreach (var tf2Class in TF2_CLASSES)
                        foreach (var weapon in TF2Weapons.getSecondaryWeapons(tf2Class))
                            addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon, tf2Class }));
                    crosshairAdded = true;
                }

                if (checkBoxAddMeleeWeapons.Checked)
                {
                    foreach (var tf2Class in TF2_CLASSES)
                        foreach (var weapon in TF2Weapons.getMeleeWeapons(tf2Class))
                            addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon, tf2Class }));
                    crosshairAdded = true;
                }

                if (checkBoxAddMiscWeapons.Checked)
                {
                    foreach (var tf2Class in TF2_CLASSES)
                        foreach (var weapon in TF2Weapons.getMiscWeapons(tf2Class))
                            addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { cbCrosshair.Text, weapon, tf2Class }));
                    crosshairAdded = true;
                }
            }

            if (crosshairAdded)
            {
                btnRemoveSelected.Enabled = true;
                btnPresetExport.Enabled = true;
                btnInstall.Enabled = true;
            }
        }

        private void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            if (gHasSelectedCrosshair)
            {
                for (int i = 0; i < listViewChosenCrosshairs.Items.Count; i++)
                    if (listViewChosenCrosshairs.Items[i].Selected)
                        listViewChosenCrosshairs.Items[i].SubItems.Clear();
            }
            else
            {
                for (int i = 0; i < listViewChosenCrosshairs.Items.Count; i++)
                    listViewChosenCrosshairs.Items[i].SubItems.Clear();
            }

            cleanListViewOfEmptyRows(listViewChosenCrosshairs);

            if (listViewChosenCrosshairs.Items.Count < 1)
            {
                btnRemoveSelected.Enabled = false;
                btnPresetExport.Enabled = false;
                btnInstall.Enabled = false;
            }

            gHasSelectedCrosshair = false;
            btnRemoveSelected.Text = "Remove crosshairs";
            btnRemoveSelected.Width = 119;
        }

        private void btnPresetExport_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.InitialDirectory = PATH_VC_RESOURCES_PRESETS;
                sfd.Title = "Venom Crosshairs - Export preset";
                sfd.Filter = "Venom Crosshairs Preset|*.vnmp";
                sfd.AddExtension = true;

                if (sfd.ShowDialog() == DialogResult.OK && sfd.FileName != "")
                {
                    writeLineToDebugger($"Exporting preset to {Path.GetFileName(sfd.FileName)}...");
                    try
                    {
                        List<ListViewItemCrosshair> crosshairSelectionList = new List<ListViewItemCrosshair>();

                        foreach (ListViewItem item in listViewChosenCrosshairs.Items)
                            crosshairSelectionList.Add(new ListViewItemCrosshair { Crosshair = item.SubItems[0].Text, Weapon = item.SubItems[1].Text, TF2Class = item.SubItems[2].Text });

                        JsonConvert.SerializeObject(crosshairSelectionList);
                        File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(crosshairSelectionList, Formatting.Indented));
                        writeLineToDebugger($"Exported preset as {Path.GetFileName(sfd.FileName)}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to export preset. Are you sure you have permission to save to this location?\n\nIf this problem persists with correct permission and usage, please consider creating a GitHub issue or contacting HbiVnm!", "Venom Crosshairs - Failed to export preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        writeLineToDebugger($"For developer: Exception: {ex.Message}");
                        writeLineToDebugger($"Unable to export preset!");
                    }
                }
            }
        }

        private void btnPresetImport_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = PATH_VC_RESOURCES_PRESETS;
                ofd.Title = "Venom Crosshairs - Import preset";
                ofd.Filter = "Venom Crosshairs Preset|*.vnmp";

                if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName != "")
                {
                    writeLineToDebugger($"Importing {Path.GetFileName(ofd.FileName)}...");
                    try
                    {
                        List<ListViewItemCrosshair> crosshairsFromPresetList = JsonConvert.DeserializeObject<List<ListViewItemCrosshair>>(File.ReadAllText(ofd.FileName));
                        List<string> missingCrosshairsList = new List<string>();

                        if (crosshairsFromPresetList == null)
                            throw new NullReferenceException($"Object reference not set to an instance of an object. Object is 'null' from trying to deserialize {ofd.FileName}");

                        listViewChosenCrosshairs.Items.Clear();
                        foreach (var crosshairFromPreset in crosshairsFromPresetList)
                        {
                            if (!missingCrosshairsList.Contains(crosshairFromPreset.Crosshair) && !File.Exists(PATH_VC_RESOURCES_MATERIALS + $"{crosshairFromPreset.Crosshair}.vtf"))
                            {
                                writeLineToDebugger("Missing crosshair! " + crosshairFromPreset.Crosshair);
                                missingCrosshairsList.Add(crosshairFromPreset.Crosshair);
                            }
                            addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { crosshairFromPreset.Crosshair, crosshairFromPreset.Weapon, crosshairFromPreset.TF2Class }));
                        }

                        if (listViewChosenCrosshairs.Items.Count > 0)
                        {
                            btnRemoveSelected.Enabled = true;
                            btnPresetExport.Enabled = true;
                            btnInstall.Enabled = true;
                        }
                        if (missingCrosshairsList.Count > 0)
                        {
                            string missingCrosshairsListString = "";
                            foreach (var crosshair in missingCrosshairsList)
                            {
                                missingCrosshairsListString += $"\n - {crosshair}";
                            }
                            MessageBox.Show($"WARNING: {Path.GetFileName(ofd.FileName)} contains crosshairs that you do NOT have (and are currently not a part of Venom Crosshairs)!\n\nInstalling a config from this preset will cause some weapons to have missing crosshairs (black square).\n\nYou are missing the following crosshairs from this preset:{missingCrosshairsListString}", "Venom Crosshairs - Missing crosshairs from preset", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        writeLineToDebugger($"Imported preset from {Path.GetFileName(ofd.FileName)}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to read preset. It may have been corrupted, damaged or manually altered in some way or form.\n\nIf this problem persists with correctly exported presets, please consider creating a GitHub issue or contacting HbiVnm!", "Venom Crosshairs - Failed to import preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        writeLineToDebugger($"For developer: Exception: {ex.Message}");
                        writeLineToDebugger($"Unable to import preset from {Path.GetFileName(ofd.FileName)}");
                    }
                }
            }
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            if (listViewChosenCrosshairs.Items.Count > 0 && performSanityCheck(textBoxTF2Path.Text))
                Task.Run(() => performInstallation());
        }

        /// 
        /// Events
        /// 
        private void onCBClassChangeEvent(object sender, EventArgs e)
        {
            if (cbClass.Text != "Multi-class")
            {
                try
                {
                    cbClass.Items.Remove("Multi-class");
                }
                catch { }
            }

            cbWeapon.Items.Clear();
            checkBoxAddOnlyClass.Text = $@"Add ONLY to {cbClass.Text}";
            foreach (var weapon in TF2Weapons.getAllWeapons(cbClass.Text))
                cbWeapon.Items.Add(weapon);

            // Change weapon combo box width depending on class
            switch (cbClass.Text)
            {
                case "Scout":
                    cbWeapon.DropDownWidth = 420;
                    break;
                case "Soldier":
                    cbWeapon.DropDownWidth = 460;
                    break;
                case "Pyro":
                    cbWeapon.DropDownWidth = 735;
                    break;
                case "Demoman":
                    cbWeapon.DropDownWidth = 400;
                    break;
                case "Heavy":
                    cbWeapon.DropDownWidth = 645;
                    break;
                case "Engineer":
                    cbWeapon.DropDownWidth = 270;
                    break;
                case "Medic":
                    cbWeapon.DropDownWidth = 340;
                    break;
                case "Sniper":
                    cbWeapon.DropDownWidth = 315;
                    break;
                case "Spy":
                    cbWeapon.DropDownWidth = 395;
                    break;
                case "Multi-class":
                    cbWeapon.DropDownWidth = 265;
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
            {
                pictureBoxCrosshair.SizeMode = PictureBoxSizeMode.CenterImage;
                pictureBoxCrosshair.ImageLocation = $@"{PATH_VC_RESOURCES_PREVIEWS}\{cbCrosshair.Text}.png";
            }
            else
            {
                pictureBoxCrosshair.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxCrosshair.ImageLocation = $@"{PATH_VC_RESOURCES}\VC.png";
            }

            btnAddCrosshair.Enabled = true;
            checkBoxAddOnlyClass.Enabled = true;
            checkBoxAddPrimaryWeapons.Enabled = true;
            checkBoxAddSecondaryWeapons.Enabled = true;
            checkBoxAddMeleeWeapons.Enabled = true;
            checkBoxAddMiscWeapons.Enabled = true;
        }

        private void onCBZoomCrosshairChangeEvent(object sender, EventArgs e)
        {
            gUserSettings.UserZoomCrosshair = cbZoomCrosshair.Text;
            File.WriteAllText(PATH_VC_RESOURCES_VC_USERSETTINGS_CFG_FILE, JsonConvert.SerializeObject(gUserSettings, Formatting.Indented));
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

        private void onCBExplosionEffectChangeEvent(object sender, EventArgs e)
        {
            gUserSettings.UserExplosionEffectIndex = cbExplosionEffect.SelectedIndex;
            File.WriteAllText(PATH_VC_RESOURCES_VC_USERSETTINGS_CFG_FILE, JsonConvert.SerializeObject(gUserSettings));
        }

        private void onListViewChosenCrosshairSelect(object sender, EventArgs e)
        {
            gHasSelectedCrosshair = true;
            btnRemoveSelected.Text = "Remove selected";
            btnRemoveSelected.Width = 113;

            if (this.listViewChosenCrosshairs.SelectedItems.Count == 1)
            {
                string tf2Class = this.listViewChosenCrosshairs.SelectedItems[0].SubItems[2].Text;
                string weapon = this.listViewChosenCrosshairs.SelectedItems[0].SubItems[1].Text;

                if (tf2Class == "Multi-class")
                    cbClass.Items.Add("Multi-class");
                else
                    cbClass.Items.Remove("Multi-class");

                cbClass.Text = tf2Class;
                cbWeapon.Text = weapon;
            }
        }

        private void onListViewChosenCrosshairColumnSelect(object sender, ColumnClickEventArgs e)
        {
            this.listViewChosenCrosshairs.ListViewItemSorter = new ListViewItemComparer(e.Column);
            this.listViewChosenCrosshairs.Sort();
        }

        private void onFormLoad(object sender, EventArgs e)
        {
            // Fetch public crosshairs, if new crosshairs are available -> prompt user
            isNewCrosshairsAvailable(false);

            // Read current config
            writeToDebugger("Reading current config... ");
            readCurrentConfig();
            writeLineToDebugger("Done!");

            writeLineToDebugger($"Venom Crosshairs version {VC_VERSION}");
        }

        /// 
        /// Functions
        /// 
        private void initVC()
        {
            // Prevent initVC from running more than once
            if (!gHasInitialized)
            {
                pictureBoxLoading.Visible = true;

                // Classes
                foreach (var tf2Class in TF2_CLASSES)
                    cbClass.Items.Add(tf2Class);

                cbClass.SelectedIndexChanged += new EventHandler(onCBClassChangeEvent);
                cbClass.SelectedIndexChanged += new EventHandler(onCheckBoxAddClassWeaponsChangeEvent);

                // Weapons
                cbWeapon.SelectedIndexChanged += new EventHandler(onCBWeaponChangeEvent);

                // Crosshairs
                addCrosshairsToComboBoxFromPath(cbCrosshair, PATH_VC_RESOURCES_PREVIEWS, true);
                cbCrosshair.SelectedIndexChanged += new EventHandler(onCBCrosshairChangeEvent);

                // Zoom crosshair
                addCrosshairsToComboBoxFromPath(cbZoomCrosshair, PATH_VC_RESOURCES_PREVIEWS, new string[] { "NO CHANGE" }, true);
                cbZoomCrosshair.SelectedIndexChanged += new EventHandler(onCBZoomCrosshairChangeEvent);

                // Checkboxes
                checkBoxAddOnlyClass.CheckStateChanged += new EventHandler(onCheckBoxAddClassWeaponsChangeEvent);

                // Explosion effect
                cbExplosionEffect.SelectedIndexChanged += new EventHandler(onCBExplosionEffectChangeEvent);

                // ListView
                listViewChosenCrosshairs.Columns.Add("Crosshair", 220);
                listViewChosenCrosshairs.Columns.Add("Weapon", 499);
                listViewChosenCrosshairs.Columns.Add("Class", 100);
                listViewChosenCrosshairs.SelectedIndexChanged += new EventHandler(onListViewChosenCrosshairSelect);
                listViewChosenCrosshairs.ColumnClick += new ColumnClickEventHandler(onListViewChosenCrosshairColumnSelect);

                // Hide console
                if (gShowConsole)
                {
                    gShowConsole = false;
                    textBoxDebugger.Visible = false;
                    lblStatus.Visible = true;
                    this.Width = FORM_WIDTH_DEFAULT;
                }
                else
                {
                    gShowConsole = true;
                    textBoxDebugger.Visible = true;
                    lblStatus.Visible = false;
                    this.Width = FORM_WIDTH_CONSOLE;
                }

                // Read user settings
                if (File.Exists(PATH_VC_RESOURCES_VC_USERSETTINGS_CFG_FILE))
                {
                    try
                    {
                        gUserSettings = JsonConvert.DeserializeObject<VCUserSettings>(File.ReadAllText(PATH_VC_RESOURCES_VC_USERSETTINGS_CFG_FILE));

                        // Check last used theme
                        gIsDarkMode = gUserSettings.IsDarkMode;
                        setDarkModeTheme(gIsDarkMode);

                        // Explosion effect
                        cbExplosionEffect.SelectedIndex = gUserSettings.UserExplosionEffectIndex;

                        // User path
                        textBoxTF2Path.Text = gUserSettings.UserTF2Path;

                        // Zoom Crosshair
                        if (gUserSettings.UserZoomCrosshair != null)
                            cbZoomCrosshair.Text = gUserSettings.UserZoomCrosshair;
                        else
                            cbZoomCrosshair.Text = "NO CHANGE";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Something went wrong reading the user setting file.", "Venom Crosshairs - Failed to read user settings!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        writeLineToDebugger($"For developer: Exception: {ex.Message}");
                        File.Delete(PATH_VC_RESOURCES_VC_USERSETTINGS_CFG_FILE);
                        gUserSettings = new VCUserSettings
                        {
                            IsDarkMode = false,
                            UserExplosionEffectIndex = 0,
                            UserTF2Path = ""
                        };
                        File.WriteAllText(PATH_VC_RESOURCES_VC_USERSETTINGS_CFG_FILE, JsonConvert.SerializeObject(gUserSettings));

                        cbExplosionEffect.SelectedIndex = 0;
                        textBoxTF2Path.Text = "";
                        gIsDarkMode = false;
                        setDarkModeTheme(gIsDarkMode);
                    }
                }
                else
                {
                    gUserSettings = new VCUserSettings
                    {
                        IsDarkMode = false,
                        UserExplosionEffectIndex = 0,
                        UserTF2Path = "",
                        UserZoomCrosshair = "NO CHANGE"
                    };
                    File.WriteAllText(PATH_VC_RESOURCES_VC_USERSETTINGS_CFG_FILE, JsonConvert.SerializeObject(gUserSettings));
                    cbExplosionEffect.SelectedIndex = 0;
                    cbZoomCrosshair.SelectedIndex = 0;
                    textBoxTF2Path.Text = "";
                    gIsDarkMode = false;
                    setDarkModeTheme(gIsDarkMode);
                }

                // Look for old configs from previous versions
                renameOldConfig();

                pictureBoxLoading.Visible = false;
            }
        }

        private void readCurrentConfig()
        {
            pictureBoxLoading.Visible = true;

            string vcScriptDir = textBoxTF2Path.Text + $@"\tf\custom\{VC_CONFIG_NAME}\scripts";
            List<string> missingCrosshairsList = new List<string>();

            if (Directory.Exists(vcScriptDir))
            {
                foreach (string fullWeaponScriptPath in Directory.GetFiles(vcScriptDir, "*.txt"))
                {
                    try
                    {
                        string crosshair = getCrosshairFromScript(fullWeaponScriptPath);
                        string weapon = TF2Weapons.getWeaponNameFromWeaponScript(Path.GetFileName(fullWeaponScriptPath));
                        string tf2Class = TF2Weapons.getClassFromWeaponName(weapon);
                        addCrosshairToListView(listViewChosenCrosshairs, new ListViewItem(new string[] { crosshair, weapon, tf2Class }));

                        try
                        {
                            if (!File.Exists(PATH_VC_RESOURCES_MATERIALS + $"{crosshair}.vmt"))
                                File.Copy($@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\materials\vgui\replay\thumbnails\{crosshair}.vmt", $@"{PATH_VC_RESOURCES_MATERIALS}\{crosshair}.vmt", true);
                            if (!File.Exists(PATH_VC_RESOURCES_MATERIALS + $"{crosshair}.vtf"))
                                File.Copy($@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\materials\vgui\replay\thumbnails\{crosshair}.vtf", $@"{PATH_VC_RESOURCES_MATERIALS}\{crosshair}.vtf", true);
                        }
                        catch
                        {
                            if (!missingCrosshairsList.Contains(crosshair))
                                missingCrosshairsList.Add(crosshair);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"\"{Path.GetFileName(fullWeaponScriptPath)}\" is unused.\nYou can safely remove this script file.\n\nIf removing this script file causes futher errors, please contact HbiVnm.", "Venom Crosshairs - Could not find weapon from script", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        writeLineToDebugger($"For developer: Exception: {ex.Message}");
                    }
                }

                if (missingCrosshairsList.Count > 0)
                {
                    /* No good solution so far, might re-add later, a warning will be good enough for now...
                    try
                    {
                        generateMissingCrosshairs(missingCrosshairsList);
                    }
                    catch (Exception ex) { }
                    */

                    string missingCrosshairsListString = "";
                    foreach (var crosshair in missingCrosshairsList)
                    {
                        missingCrosshairsListString += $"\n - {crosshair}";
                    }

                    MessageBox.Show($"WARNING: The currently installed Venom Crosshairs config contains scripts with crosshairs that you do NOT have!\n\nUsing this config will cause some weapons to have missing crosshairs (black square).\n\nYou are missing the following crosshairs:{missingCrosshairsListString}", "Venom Crosshairs - Missing crosshairs found within installed config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


                if (listViewChosenCrosshairs.Items.Count > 0)
                {
                    // Scroll to top
                    listViewChosenCrosshairs.EnsureVisible(0);

                    btnRemoveSelected.Enabled = true;
                    btnPresetExport.Enabled = true;
                    btnInstall.Enabled = true;
                }
            }
            else
            {
                btnRemoveSelected.Enabled = false;
                btnPresetExport.Enabled = false;
                btnInstall.Enabled = false;
            }

            cbClass.Enabled = true;
            cbWeapon.Enabled = false;
            cbCrosshair.Enabled = false;
            pictureBoxLoading.Visible = false;
        }

        private void renameOldConfig()
        {
            if (gUserSettings.UserTF2Path != "")
                foreach (var prevConfigName in PREVIOUS_CONFIG_NAMES)
                {
                    string prevConfigPath = $@"{textBoxTF2Path.Text}\tf\custom\{prevConfigName}";
                    if (Directory.Exists(prevConfigPath))
                    {
                        Directory.Move(prevConfigPath, $@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}");
                        MessageBox.Show($"An old config was found and has now been renamed!\n\n\"{prevConfigName}\" -> \"{VC_CONFIG_NAME}\"", "Venom Crosshairs - Old config folder renamed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                }
        }

        private void doVPKScriptCheck()
        {
            List<string> conflictingFilesList = new List<string>();

            writeToDebugger("Preparing VPK check... ");
            Process vpkProcess = new Process();
            vpkProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            vpkProcess.StartInfo.UseShellExecute = false;
            vpkProcess.StartInfo.RedirectStandardOutput = true;
            vpkProcess.StartInfo.CreateNoWindow = true;
            vpkProcess.StartInfo.FileName = textBoxTF2Path.Text + @"\bin\vpk.exe";
            writeLineToDebugger("Done!");

            // Check VPK
            writeToDebugger($"Performing VPK check... ");
            foreach (string vpkFile in Directory.GetFiles(textBoxTF2Path.Text + @"\tf\custom", "*.vpk", SearchOption.AllDirectories))
            {
                StreamReader sw;
                string vpkOutput;
                vpkProcess.StartInfo.Arguments = $" l \"{vpkFile}\"";
                vpkProcess.Start();
                sw = vpkProcess.StandardOutput;
                vpkOutput = sw.ReadToEnd();
                vpkProcess.WaitForExit();

                foreach (string weaponFile in TF2Weapons.getWeaponScripts())
                    if (vpkOutput.IndexOf(weaponFile) > -1)
                    {
                        conflictingFilesList.Add(Path.GetFileName(vpkFile));
                        break;
                    }
            }
            writeLineToDebugger("Done!");

            // Check scripts
            writeToDebugger($"Performing script check... ");
            List<string> scriptFilesTF2 = new List<string>(TF2Weapons.getWeaponScripts());
            List<string> scriptFilePathsCustom = new List<string>(Directory.GetFiles(textBoxTF2Path.Text + @"\tf\custom", "*.txt", SearchOption.AllDirectories));
            foreach (string scriptFilePath in scriptFilePathsCustom)
            {
                if (scriptFilePath.IndexOf(VC_CONFIG_NAME) == -1) // Ignore script if inside Venom Crosshairs config
                    if (scriptFilesTF2.Contains(Path.GetFileName(scriptFilePath)))
                    {
                        var scriptFilePathArr = scriptFilePath.Split(Path.DirectorySeparatorChar);
                        int customIndex = -1;
                        for (int i = 0; i < scriptFilePathArr.Length; i++)
                            if (scriptFilePathArr[i] == "custom")
                            {
                                customIndex = i + 1;
                                break;
                            }

                        string susFileWithPartialPath = "";
                        for (int i = customIndex; i < scriptFilePathArr.Length; i++)
                            susFileWithPartialPath += $"\\{scriptFilePathArr[i]}";
                        conflictingFilesList.Add(susFileWithPartialPath);
                    }
            }
            writeLineToDebugger("Done!");

            string conflictingFilesListString = "";
            int conflictingFilesListStringSize = 50; // Only show X entries to prevent overflowing MessageBox

            for (int i = 0; i < conflictingFilesList.Count; i++)
            {
                if (i > conflictingFilesListStringSize)
                    break;

                conflictingFilesListString += $"\n- {conflictingFilesList[i]}";
            }

            if (conflictingFilesList.Count > conflictingFilesListStringSize)
                conflictingFilesListString += $"\n... and {conflictingFilesList.Count - conflictingFilesListStringSize} more...";

            if (conflictingFilesList.Count > 0)
                MessageBox.Show($"WARNING: VPK/script file(s) were found in \\custom altering weapon scripts and will most likely not work in combination with Venom Crosshairs configs!\n\nIt is recommended to remove these files if you wish configs generated by Venom Crosshairs to work without issues. For more info see the FAQ on the repository wiki.\n\nVPK/script file(s) potentially causing issues: {conflictingFilesListString}", "Venom Crosshairs - Weapon script altering VPK file!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private bool isNewCrosshairsAvailable(bool suppressNotification)
        {
            writeLineToDebugger("Attempting to fetch public crosshair list... ");
            _ = fetchCrosshairsFromPublicRepo();

            foreach (KeyValuePair<string, string> crosshair in gPublicCrosshairs)
                if (!File.Exists($@"{PATH_VC_RESOURCES_MATERIALS}\{crosshair.Key}"))
                {
                    writeLineToDebugger("New crosshairs available!");
                    if (!suppressNotification)
                        MessageBox.Show("There are new crosshairs available for download!\n\nYou can download them from the settings panel.", "Venom Crosshairs - New crosshairs available!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }

            return false;
        }

        private Task fetchCrosshairsFromPublicRepo()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("VenomCrosshairs", VC_VERSION));
            string contentsUrl = $"https://api.github.com/repos/hbivnm/Venom-Crosshairs-List/contents";
            try
            {
                string response = httpClient.GetStringAsync(contentsUrl).Result;

                JArray contents = (JArray)JsonConvert.DeserializeObject(response);
                gPublicCrosshairs.Clear();
                foreach (JToken file in contents)
                    gPublicCrosshairs.Add((string)file["name"], (string)file["download_url"]);
            }
            catch (Exception ex)
            {
                writeLineToDebugger("\nFailed to fetch crosshairs from public repo!");
                writeLineToDebugger(ex.ToString());
            }

            return null;
        }

        private List<string> downloadMissingCrosshairs()
        {
            var downloadedCrosshairsList = new List<string>();
            var taskList = new List<Task>();
            int newCrosshairsFileCount = 0;

            foreach (KeyValuePair<string, string> crosshair in gPublicCrosshairs)
                if (!File.Exists($@"{PATH_VC_RESOURCES_MATERIALS}\{crosshair.Key}"))
                {
                    using (WebClient webClient = new WebClient())
                    {
                        taskList.Add(webClient.DownloadFileTaskAsync(new Uri(crosshair.Value), $@"{PATH_VC_RESOURCES_MATERIALS}\{crosshair.Key}"));
                    }
                    downloadedCrosshairsList.Add(crosshair.Key);
                    newCrosshairsFileCount++;
                }

            bool tasksFinished = false;
            if (taskList.Count > 0)
            {
                while (!tasksFinished)
                {
                    float completedTasks = 0;
                    foreach (Task task in taskList)
                        if (task.IsCompleted)
                            completedTasks++;

                    if (completedTasks == taskList.Count)
                        tasksFinished = true;
                    else
                        writeLineToDebugger($"Downloading crosshairs... ({Math.Round(((float)completedTasks / (float)taskList.Count) * 100)}%)");

                    Thread.Sleep(250);
                }
                writeLineToDebugger("Downloading crosshairs... (100%)");
            }

            // Keeping this as a backup if something goes wrong
            Task.WaitAll(taskList.ToArray());

            writeLineToDebugger($"Downloaded {newCrosshairsFileCount / 2} crosshair(s)!");
            return downloadedCrosshairsList;
        }

        private void toggleConsole()
        {
            if (gShowConsole)
            {
                gShowConsole = false;
                lblStatus.Visible = true;
                textBoxDebugger.Visible = false;
                this.MinimumSize = new Size(FORM_WIDTH_DEFAULT, FORM_HEIGHT_DEFAULT);
                ActiveForm.Width = FORM_WIDTH_DEFAULT;
            }
            else
            {
                gShowConsole = true;
                lblStatus.Visible = false;
                textBoxDebugger.Visible = true;
                this.MinimumSize = new Size(FORM_WIDTH_CONSOLE, FORM_HEIGHT_DEFAULT);
                ActiveForm.Width = FORM_WIDTH_CONSOLE;
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
            {
                listView.Items.Add(crosshairListViewItem);

                // Make sure newly added crosshair is visible in ListView
                crosshairListViewItem.EnsureVisible();
            }
            else
                foreach (ListViewItem item in listView.Items)
                    if (item.SubItems[0].Text != crosshairListViewItem.SubItems[0].Text && item.SubItems[1].Text == crosshairListViewItem.SubItems[1].Text)
                    {
                        item.SubItems[0].Text = cbCrosshair.Text;
                        item.EnsureVisible();
                        break;
                    }
        }

        private string getExplosionEffectParticleName(string name)
        {
            switch (name)
            {
                case "Default":
                    return "ExplosionCore_wall";
                case "Duck Trail (Invisible)":
                    return "duck_collect_trail_special_red";
                case "Electric shock":
                    return "electrocuted_red_flash";
                case "Muzzle flash":
                    return "muzzle_minigun_starflash01";
                case "Pyro pool":
                    return "eotl_pyro_pool_explosion_flash";
                case "Spy sapper":
                    return "ExplosionCore_sapperdestroyed";
            }
            throw new ArgumentException($"Could not find ExplosionEffect particle name for '{name}'!");
        }

        private string getExplosionPlayerEffectParticleName(string name)
        {
            switch (name)
            {
                case "Default":
                    return "ExplosionCore_MidAir";
                case "Duck Trail (Invisible)":
                    return "duck_collect_trail_special_blue";
                case "Electric shock":
                    return "electrocuted_blue_flash";
                case "Muzzle flash":
                    return "muzzle_minigun_starflash01";
                case "Pyro pool":
                    return "eotl_pyro_pool_explosion_flash";
                case "Spy sapper":
                    return "ExplosionCore_sapperdestroyed";
            }
            throw new ArgumentException($"Could not find ExplosionPlayerEffect particle name for '{name}'!");
        }

        private string getExplosionWaterEffectParticleName(string name)
        {
            switch (name)
            {
                case "Default":
                    return "ExplosionCore_MidAir_underwater";
                case "Duck Trail (Invisible)":
                    return "duck_collect_trail_special_red";
                case "Electric shock":
                    return "electrocuted_red_flash";
                case "Muzzle flash":
                    return "muzzle_minigun_starflash01";
                case "Pyro pool":
                    return "eotl_pyro_pool_explosion_flash";
                case "Spy sapper":
                    return "ExplosionCore_sapperdestroyed";
            }
            throw new ArgumentException($"Could not find ExplosionWaterEffect particle name for '{name}'!");
        }

        // Should probably re-write so it overwrites, not sure of the thoughtprocess of moving or deleting (same filename =/= same file content)
        private void moveFilesByExtensionOrDelete(string sourceDirectory, string targetDirectory, string extension)
        {
            foreach (string file in Directory.GetFiles(sourceDirectory, "*." + extension))
                if (!File.Exists(targetDirectory + Path.GetFileName(file)))
                    File.Move(file, targetDirectory + Path.GetFileName(file));
                else
                    File.Delete(file);
        }

        // private void performInstallation(bool removeOldConfig)
        private void performInstallation()
        {
            Invoke(new MethodInvoker(delegate ()
            {
                pictureBoxLoading.Visible = true;
                textBoxTF2Path.Enabled = false;
                btnReload.Enabled = false;
                btnDownload.Enabled = false;
                cbClass.Enabled = false;
                cbCrosshair.Enabled = false;
                cbZoomCrosshair.Enabled = false;
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
                btnPresetImport.Enabled = false;
                btnPresetExport.Enabled = false;
                btnInstall.Enabled = false;
            }));

            bool isUpdate = false;
            if (Directory.Exists($@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}"))
            {
                writeLineToDebugger("Updating current Venom Crosshairs config!");
                isUpdate = true;
            }

            writeToDebugger("Removing old Venom Crosshairs config... ");
            if (Directory.Exists($@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}"))
                Directory.Delete($@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}", true);
            writeLineToDebugger("Done!");

            // Installation process
            writeToDebugger("Creating config structure... ");
            if (!Directory.Exists($@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\materials\vgui\replay\thumbnails"))
                Directory.CreateDirectory($@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\materials\vgui\replay\thumbnails");
            if (!Directory.Exists($@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\scripts"))
                Directory.CreateDirectory($@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\scripts");
            writeLineToDebugger("Done!");

            writeToDebugger("Copying materials... ");
            Invoke(new MethodInvoker(delegate ()
            {
                if (cbZoomCrosshair.Text != "NO CHANGE")
                {
                    File.Copy($@"{PATH_VC_RESOURCES_MATERIALS}\{cbZoomCrosshair.Text}.vmt", $@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\materials\vgui\replay\thumbnails\{cbZoomCrosshair.Text}.vmt", true);
                    File.Copy($@"{PATH_VC_RESOURCES_MATERIALS}\{cbZoomCrosshair.Text}.vtf", $@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\materials\vgui\replay\thumbnails\{cbZoomCrosshair.Text}.vtf", true);
                }

                foreach (ListViewItem item in listViewChosenCrosshairs.Items)
                {
                    try
                    {
                        string crosshair = item.SubItems[0].Text;
                        File.Copy($@"{PATH_VC_RESOURCES_MATERIALS}\{crosshair}.vmt", $@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\materials\vgui\replay\thumbnails\{crosshair}.vmt", true);
                        File.Copy($@"{PATH_VC_RESOURCES_MATERIALS}\{crosshair}.vtf", $@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\materials\vgui\replay\thumbnails\{crosshair}.vtf", true);
                    }
                    catch (Exception ex)
                    {
                        writeLineToDebugger(ex.ToString());
                    }
                }
            }));
            writeLineToDebugger("Done!");

            writeToDebugger("Adding scripts... ");
            Invoke(new MethodInvoker(delegate ()
            {
                // Loop through user selection
                foreach (ListViewItem item in listViewChosenCrosshairs.Items)
                {
                    // Get selected crosshair
                    string crosshair = item.SubItems[0].Text;

                    // Get and set Zoom crosshair
                    string zoomCrosshair = cbZoomCrosshair.Text;
                    if (cbZoomCrosshair.Text == "NO CHANGE")
                        zoomCrosshair = crosshair;

                    // Get width and height, assume default (64x64)
                    int crosshairWidth = 64;
                    int crosshairHeight = 64;
                    int zoomCrosshairWidth = 64;
                    int zoomCrosshairHeight = 64;

                    using (Bitmap crosshairBitmap = new Bitmap($@"{PATH_VC_RESOURCES_PREVIEWS}\{crosshair}.png"))
                    {
                        crosshairWidth = crosshairBitmap.Width;
                        crosshairHeight = crosshairBitmap.Height;
                    }

                    using (Bitmap zoomCrosshairBitmap = new Bitmap($@"{PATH_VC_RESOURCES_PREVIEWS}\{zoomCrosshair}.png"))
                    {
                        zoomCrosshairWidth = zoomCrosshairBitmap.Width;
                        zoomCrosshairHeight = zoomCrosshairBitmap.Height;
                    }

                    // Get weapon script
                    string weaponName = item.SubItems[1].Text;
                    string weaponScriptName = TF2Weapons.getWeaponScriptFromWeaponName(weaponName);

                    string fullInstallScriptPath = $@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\scripts\{weaponScriptName}";

                    if (File.Exists(fullInstallScriptPath))
                        File.Delete(fullInstallScriptPath);

                    // Write script file
                    try
                    {
                        File.WriteAllText(fullInstallScriptPath,
                            File.ReadAllText($@"{PATH_VC_RESOURCES_SCRIPTS}\{weaponScriptName}")
                                .Replace("VC_PLACEHOLDER_EXPLOSION_EFFECT", getExplosionEffectParticleName(cbExplosionEffect.Text))
                                .Replace("VC_PLACEHOLDER_EXPLOSION_PLAYER_EFFECT", getExplosionPlayerEffectParticleName(cbExplosionEffect.Text))
                                .Replace("VC_PLACEHOLDER_EXPLOSION_WATER_EFFECT", getExplosionWaterEffectParticleName(cbExplosionEffect.Text))
                                .Replace("VC_PLACEHOLDER", crosshair)
                                .Replace("VC_WIDTH_PLACEHOLDER", crosshairWidth.ToString())
                                .Replace("VC_HEIGHT_PLACEHOLDER", crosshairHeight.ToString())
                                .Replace("VC_ZOOM_PLACEHOLDER", zoomCrosshair)
                                .Replace("VC_ZOOM_WIDTH_PLACEHOLDER", zoomCrosshairWidth.ToString())
                                .Replace("VC_ZOOM_HEIGHT_PLACEHOLDER", zoomCrosshairHeight.ToString())
                        );
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Something went terribly wrong! Please tell HbiVnm by adding him on Steam or creating a GitHub issue!", "Venom Crosshairs - Failed to write script file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        writeLineToDebugger($"For developer: Exception: {ex.Message}");
                    }
                }

                // Add explosion effects for unselected explosion weapons, crosshair is set to TF2 default ("quarteredcircle")
                foreach (string explosiveWeapon in TF2Weapons.getExplosiveWeapons())
                {
                    bool copiedMaterial = false;
                    string crosshair = "quarteredcircle";
                    string weaponScriptName = TF2Weapons.getWeaponScriptFromWeaponName(explosiveWeapon);
                    string fullScriptPath = $@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\scripts\{weaponScriptName}";
                    int crosshairWidth = 64;
                    int crosshairHeight = 64;

                    if (!File.Exists(fullScriptPath))
                    {
                        try
                        {
                            File.WriteAllText(fullScriptPath,
                                File.ReadAllText($@"{PATH_VC_RESOURCES_SCRIPTS}\{weaponScriptName}")
                                    .Replace("VC_PLACEHOLDER_EXPLOSION_EFFECT", getExplosionEffectParticleName(cbExplosionEffect.Text))
                                    .Replace("VC_PLACEHOLDER_EXPLOSION_PLAYER_EFFECT", getExplosionPlayerEffectParticleName(cbExplosionEffect.Text))
                                    .Replace("VC_PLACEHOLDER_EXPLOSION_WATER_EFFECT", getExplosionWaterEffectParticleName(cbExplosionEffect.Text))
                                    .Replace("VC_PLACEHOLDER", crosshair)
                                    .Replace("VC_WIDTH_PLACEHOLDER", crosshairWidth.ToString())
                                    .Replace("VC_HEIGHT_PLACEHOLDER", crosshairHeight.ToString())
                            );

                            if (!copiedMaterial)
                            {
                                File.Copy($@"{PATH_VC_RESOURCES_MATERIALS}\{crosshair}.vmt", $@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\materials\vgui\replay\thumbnails\{crosshair}.vmt", true);
                                File.Copy($@"{PATH_VC_RESOURCES_MATERIALS}\{crosshair}.vtf", $@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\materials\vgui\replay\thumbnails\{crosshair}.vtf", true);
                                copiedMaterial = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Something went terribly wrong! Please tell HbiVnm by adding him on Steam or creating a GitHub issue!", "Venom Crosshairs - Failed to write default script file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            writeLineToDebugger($"For developer: Exception: {ex.Message}");
                        }
                    }
                }
            }));
            writeLineToDebugger("Done!");

            doVPKScriptCheck();

            File.WriteAllText($@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\README.txt", $"This crosshair config was generated using Venom Crosshairs ({VC_VERSION})\n\nhttps://github.com/hbivnm/Venom-Crosshairs\n\nNeed to report a bug? Check out https://github.com/hbivnm/Venom-Crosshairs/issues\nNeed help? Check out https://github.com/hbivnm/Venom-Crosshairs/wiki");

            // Create backup as preset
            string presetTimestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string autosavePresetName = $"{presetTimestamp}.vnmp";
            Invoke(new MethodInvoker(delegate ()
            {
                writeLineToDebugger($"Exporting preset to {autosavePresetName}...");
                try
                {
                    List<ListViewItemCrosshair> crosshairSelectionList = new List<ListViewItemCrosshair>();

                    foreach (ListViewItem item in listViewChosenCrosshairs.Items)
                        crosshairSelectionList.Add(new ListViewItemCrosshair { Crosshair = item.SubItems[0].Text, Weapon = item.SubItems[1].Text, TF2Class = item.SubItems[2].Text });

                    JsonConvert.SerializeObject(crosshairSelectionList);
                    File.WriteAllText(PATH_VC_RESOURCES_PRESETS_AUTOSAVE + autosavePresetName, JsonConvert.SerializeObject(crosshairSelectionList, Formatting.Indented));
                    writeLineToDebugger($"Exported preset as {autosavePresetName}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to export preset. Are you sure you have permission to save to this location?\n\nIf this problem persists with correct permission and usage, please consider creating a GitHub issue or contacting HbiVnm!", "Venom Crosshairs - Failed to export preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    writeLineToDebugger($"For developer: Exception: {ex.Message}");
                    writeLineToDebugger($"Unable to export preset!");
                }
            }));

            // Find oldest autosaved preset and delete it if there are more than 10 autosaves
            DirectoryInfo autosaveDirInfo = new DirectoryInfo(PATH_VC_RESOURCES_PRESETS_AUTOSAVE);
            FileInfo[] autosaveFiles = autosaveDirInfo.GetFiles();
            if (autosaveFiles.Length > MAX_AUTOSAVES)
            {
                FileInfo oldestAutosaveFile = autosaveFiles.OrderBy(file => file.CreationTime).First();
                oldestAutosaveFile.Delete();
            }

            if (!isUpdate)
                writeLineToDebugger("Venom Crosshairs config successfully installed!");
            else
                writeLineToDebugger("Venom Crosshairs config successfully updated!");

            Invoke(new MethodInvoker(delegate ()
            {
                pictureBoxLoading.Visible = false;
                textBoxTF2Path.Enabled = true;
                btnReload.Enabled = true;
                btnDownload.Enabled = true;
                cbClass.Enabled = true;
                cbCrosshair.Enabled = true;
                cbZoomCrosshair.Enabled = true;
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
                btnPresetImport.Enabled = true;
                btnPresetExport.Enabled = true;
                btnInstall.Enabled = true;
            }));
        }

        private void reloadCrosshairList()
        {
            Invoke(new MethodInvoker(delegate ()
            {
                pictureBoxLoading.Visible = true;
                pictureBoxCrosshair.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxCrosshair.ImageLocation = $@"{PATH_VC_RESOURCES}\VC.png";

                listViewChosenCrosshairs.Items.Clear();

                textBoxTF2Path.Enabled = false;
                btnReload.Enabled = false;
                btnDownload.Enabled = false;
                cbClass.Enabled = false;
                cbCrosshair.Enabled = false;
                cbZoomCrosshair.Enabled = false;
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
                btnPresetImport.Enabled = false;
                btnPresetExport.Enabled = false;
                btnInstall.Enabled = false;
            }));

            writeToDebugger("Deleting old previews... ");
            foreach (string previewFile in Directory.GetFiles(PATH_VC_RESOURCES_PREVIEWS))
                File.Delete(previewFile);
            writeLineToDebugger("Done!");

            // Generate previews
            writeToDebugger("Preparing vtf2tga process... ");
            Process vtf2tgaProcess = new Process();
            vtf2tgaProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            vtf2tgaProcess.StartInfo.FileName = textBoxTF2Path.Text + @"\bin\vtf2tga.exe";
            writeLineToDebugger("Done!");

            writeToDebugger("Running vtf2tga.exe... ");
            foreach (string vtfFile in Directory.GetFiles(PATH_VC_RESOURCES_MATERIALS, "*.vtf"))
            {
                vtf2tgaProcess.StartInfo.Arguments = @"/C -i " + "\"" + vtfFile + "\"";
                vtf2tgaProcess.Start();
            }
            try
            {
                vtf2tgaProcess.WaitForExit();
            }
            catch { }
            writeLineToDebugger("Done!");

            moveFilesByExtensionOrDelete(PATH_VC_RESOURCES_MATERIALS, PATH_VC_RESOURCES_PREVIEWS, "tga");

            string[] tgaFiles = Directory.GetFiles(PATH_VC_RESOURCES_PREVIEWS, "*.tga");
            int tgaFileCount = tgaFiles.Length;
            int processedTgaFiles = 0;

            Parallel.ForEach(tgaFiles, tgaFile =>
            {
                using (MagickImage tgaImage = new MagickImage(tgaFile))
                {
                    tgaImage.Write(Path.Combine(PATH_VC_RESOURCES_PREVIEWS, Path.GetFileNameWithoutExtension(tgaFile) + ".png"));
                }

                writeLineToDebugger($"Generating previews... ({Math.Round(((float)Interlocked.Increment(ref processedTgaFiles) / (float)tgaFileCount) * 100)}%)");
            });

            writeToDebugger("Performing cleanup... ");
            // Function no longer needed due to cleanup steps, remove/refactor?
            moveFilesByExtensionOrDelete(PATH_VC, PATH_VC_RESOURCES_PREVIEWS, "png");
            foreach (string tgaFile in Directory.GetFiles(PATH_VC_RESOURCES_PREVIEWS, "*.tga"))
                File.Delete(tgaFile);
            writeLineToDebugger("Done!");

            // Add to ComboBox
            Invoke(new MethodInvoker(delegate ()
            {
                addCrosshairsToComboBoxFromPath(cbCrosshair, PATH_VC_RESOURCES_PREVIEWS, true);
                addCrosshairsToComboBoxFromPath(cbZoomCrosshair, PATH_VC_RESOURCES_PREVIEWS, new string[] { "NO CHANGE" }, true);
            }));

            writeLineToDebugger("Finished reloading crosshair list!");


            Invoke(new MethodInvoker(delegate ()
            {
                pictureBoxLoading.Visible = false;
                textBoxTF2Path.Enabled = true;
                cbClass.Enabled = true;
                cbClass.SelectedIndex = -1;
                cbWeapon.Enabled = false;
                cbWeapon.SelectedIndex = -1;
                cbCrosshair.Enabled = false;
                cbCrosshair.SelectedIndex = -1;
                cbZoomCrosshair.Enabled = true;
                cbZoomCrosshair.Text = gUserSettings.UserZoomCrosshair;
                btnReload.Enabled = true;
                btnDownload.Enabled = true;
                btnPresetImport.Enabled = true;
                readCurrentConfig();
            }));
        }

        private void downloadAndGenerateNewCrosshairs()
        {
            Invoke(new MethodInvoker(delegate ()
            {
                pictureBoxLoading.Visible = true;
                pictureBoxCrosshair.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxCrosshair.ImageLocation = PATH_VC_RESOURCES + @"VC.png";

                listViewChosenCrosshairs.Items.Clear();

                textBoxTF2Path.Enabled = false;
                btnReload.Enabled = false;
                btnDownload.Enabled = false;
                cbClass.Enabled = false;
                cbCrosshair.Enabled = false;
                cbZoomCrosshair.Enabled = false;
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
                btnPresetImport.Enabled = false;
                btnPresetExport.Enabled = false;
                btnInstall.Enabled = false;
            }));

            // Download publicly available crosshairs
            List<string> downloadedCrosshairsList = downloadMissingCrosshairs();

            if (downloadedCrosshairsList.Count > 0)
            {
                // Generate previews
                writeToDebugger("Preparing vtf2tga process... ");
                Process vtf2tgaProcess = new Process();
                vtf2tgaProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                vtf2tgaProcess.StartInfo.FileName = textBoxTF2Path.Text + @"\bin\vtf2tga.exe";
                writeLineToDebugger("Done!");

                writeToDebugger("Running vtf2tga.exe... ");
                foreach (string vtfFile in Directory.GetFiles(PATH_VC_RESOURCES_MATERIALS, "*.vtf"))
                {
                    string filename = Path.GetFileNameWithoutExtension(vtfFile) + ".vtf";
                    if (downloadedCrosshairsList.Contains(filename))
                    {
                        vtf2tgaProcess.StartInfo.Arguments = @"/C -i " + "\"" + vtfFile + "\"";
                        vtf2tgaProcess.Start();
                    }
                }
                try
                {
                    vtf2tgaProcess.WaitForExit();
                }
                catch { }
                writeLineToDebugger("Done!");

                moveFilesByExtensionOrDelete(PATH_VC_RESOURCES_MATERIALS, PATH_VC_RESOURCES_PREVIEWS, "tga");

                string[] tgaFiles = Directory.GetFiles(PATH_VC_RESOURCES_PREVIEWS, "*.tga");
                int tgaFileCount = tgaFiles.Length;
                int processedTgaFiles = 0;

                Parallel.ForEach(tgaFiles, tgaFile =>
                {
                    using (MagickImage tgaImage = new MagickImage(tgaFile))
                    {
                        tgaImage.Write(Path.Combine(PATH_VC_RESOURCES_PREVIEWS, Path.GetFileNameWithoutExtension(tgaFile) + ".png"));
                    }

                    writeLineToDebugger($"Generating previews... ({Math.Round(((float)Interlocked.Increment(ref processedTgaFiles) / (float)tgaFileCount) * 100)}%)");
                });

                writeToDebugger("Performing cleanup... ");
                // Function no longer needed due to cleanup steps, remove/refactor?
                moveFilesByExtensionOrDelete(PATH_VC, PATH_VC_RESOURCES_PREVIEWS, "png");
                foreach (string tgaFile in Directory.GetFiles(PATH_VC_RESOURCES_PREVIEWS, "*.tga"))
                    File.Delete(tgaFile);
                writeLineToDebugger("Done!");

                // Add to ComboBox
                Invoke(new MethodInvoker(delegate ()
                {
                    addCrosshairsToComboBoxFromPath(cbCrosshair, PATH_VC_RESOURCES_PREVIEWS, true);
                    addCrosshairsToComboBoxFromPath(cbZoomCrosshair, PATH_VC_RESOURCES_PREVIEWS, new string[] { "NO CHANGE" }, true);
                }));

                _ = isNewCrosshairsAvailable(true);
                writeLineToDebugger("Updated crosshair list with new crosshairs!");
            }
            else
            {
                writeLineToDebugger("No new crosshairs available!");
            }


            Invoke(new MethodInvoker(delegate ()
            {
                pictureBoxLoading.Visible = false;
                textBoxTF2Path.Enabled = true;
                cbClass.Enabled = true;
                cbClass.SelectedIndex = -1;
                cbWeapon.Enabled = false;
                cbWeapon.SelectedIndex = -1;
                cbCrosshair.Enabled = false;
                cbCrosshair.SelectedIndex = -1;
                cbZoomCrosshair.Enabled = true;
                cbZoomCrosshair.Text = gUserSettings.UserZoomCrosshair;
                btnReload.Enabled = true;
                btnDownload.Enabled = true;
                btnPresetImport.Enabled = true;
                readCurrentConfig();
            }));
        }

        private void generateMissingCrosshairs(List<string> missingCrosshairNames)
        {
            // Copy from config to VC
            writeToDebugger("Copying missing crosshairs... ");
            foreach (var crosshair in missingCrosshairNames)
            {
                File.Copy($@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\materials\vgui\replay\thumbnails\{crosshair}.vtf", PATH_VC_RESOURCES_MATERIALS + $"{crosshair}.vtf", true);
                File.Copy($@"{textBoxTF2Path.Text}\tf\custom\{VC_CONFIG_NAME}\materials\vgui\replay\thumbnails\{crosshair}.vmt", PATH_VC_RESOURCES_MATERIALS + $"{crosshair}.vmt", true);
            }
            writeLineToDebugger("Done!");

            // Generate missing
            writeToDebugger("Preparing vtf2tga process... ");
            Process vtf2tgaProcess = new Process();
            vtf2tgaProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            vtf2tgaProcess.StartInfo.FileName = textBoxTF2Path.Text + @"\bin\vtf2tga.exe";
            writeLineToDebugger("Done!");

            writeToDebugger("Running vtf2tga.exe... ");
            foreach (string vtfFile in Directory.GetFiles(PATH_VC_RESOURCES_MATERIALS, "*.vtf"))
            {
                if (missingCrosshairNames.Contains(Path.GetFileNameWithoutExtension(vtfFile)))
                {
                    vtf2tgaProcess.StartInfo.Arguments = @"/C -i " + "\"" + vtfFile + "\"";
                    vtf2tgaProcess.Start();
                }
            }
            try
            {
                vtf2tgaProcess.WaitForExit();
            }
            catch { }
            writeLineToDebugger("Done!");

            moveFilesByExtensionOrDelete(PATH_VC_RESOURCES_MATERIALS, PATH_VC_RESOURCES_PREVIEWS, "tga");

            string[] tgaFiles = Directory.GetFiles(PATH_VC_RESOURCES_PREVIEWS, "*.tga");
            int tgaFileCount = tgaFiles.Length;
            int processedTgaFiles = 0;

            Parallel.ForEach(tgaFiles, tgaFile =>
            {
                using (MagickImage tgaImage = new MagickImage(tgaFile))
                {
                    tgaImage.Write(Path.Combine(PATH_VC_RESOURCES_PREVIEWS, Path.GetFileNameWithoutExtension(tgaFile) + ".png"));
                }

                writeLineToDebugger($"Generating previews... ({Math.Round(((float)Interlocked.Increment(ref processedTgaFiles) / (float)tgaFileCount) * 100)}%)");
            });

            writeToDebugger("Performing cleanup... ");
            // Function no longer needed due to cleanup steps, remove/refactor?
            moveFilesByExtensionOrDelete(PATH_VC, PATH_VC_RESOURCES_PREVIEWS, "png");
            foreach (string tgaFile in Directory.GetFiles(PATH_VC_RESOURCES_PREVIEWS, "*.tga"))
                File.Delete(tgaFile);
            writeLineToDebugger("Done!");

            writeLineToDebugger($"Read and generated {missingCrosshairNames.Count} new crosshair(s)!");

            // Add to ComboBox
            Invoke(new MethodInvoker(delegate ()
            {
                addCrosshairsToComboBoxFromPath(cbCrosshair, PATH_VC_RESOURCES_PREVIEWS, true);
                addCrosshairsToComboBoxFromPath(cbZoomCrosshair, PATH_VC_RESOURCES_PREVIEWS, new string[] { "NO CHANGE" }, true);
            }));
        }

        private void addCrosshairsToComboBoxFromPath(ComboBox cb, string path, string[] prependItems, bool clearComboBoxItems)
        {
            if (clearComboBoxItems)
                cb.Items.Clear();

            foreach (var item in prependItems)
                cb.Items.Add(item);

            foreach (var crosshair in Directory.GetFiles(path, "*.png"))
            {
                string crosshairName = Path.GetFileNameWithoutExtension(crosshair);
                cb.Items.Add(crosshairName);
            }

            setComboBoxDropDownWidthToLongestStringPixelWidth(cb);
        }

        private void addCrosshairsToComboBoxFromPath(ComboBox cb, string path, bool clearComboBoxItems)
        {
            if (clearComboBoxItems)
                cb.Items.Clear();

            foreach (var crosshair in Directory.GetFiles(path, "*.png"))
            {
                string crosshairName = Path.GetFileNameWithoutExtension(crosshair);
                cb.Items.Add(crosshairName);
            }

            setComboBoxDropDownWidthToLongestStringPixelWidth(cb);
        }

        private List<string> getCurrentCrosshairNames()
        {
            List<string> missingCrosshairs = new List<string>();

            string[] currentCrosshairs = Directory.GetFiles(PATH_VC_RESOURCES_PREVIEWS);

            foreach (string crosshair in currentCrosshairs)
            {
                string crosshairName = Path.GetFileNameWithoutExtension(crosshair);
                missingCrosshairs.Add(crosshairName);
            }

            return missingCrosshairs;
        }

        private void setComboBoxDropDownWidthToLongestStringPixelWidth(ComboBox cb)
        {
            int longestPixelWidth = 0;
            foreach (var item in cb.Items)
            {
                SizeF xhairSize = new SizeF();

                using (Bitmap bitmap = new Bitmap(1, 1))
                using (Graphics g = Graphics.FromImage(bitmap))
                    xhairSize = g.MeasureString(item.ToString(), cb.Font);

                if (xhairSize.Width > longestPixelWidth)
                    longestPixelWidth = (int)xhairSize.Width;
            }
            cb.DropDownWidth = longestPixelWidth + 20;
        }

        private string getCrosshairFromScript(string pathToScript)
        {
            string[] scriptLines = File.ReadAllLines(pathToScript);
            foreach (string scriptLine in scriptLines)
            {
                if (scriptLine.Contains("vgui/replay/thumbnails/"))
                {
                    string[] crosshairLine = scriptLine.Split('/');
                    string crosshair = crosshairLine[crosshairLine.Length - 1];
                    return crosshair.Substring(0, crosshair.Length - 1);
                }
            }

            throw new Exception($"Could not find crosshair in script '{pathToScript}'");
        }

        private void setDarkModeTheme(bool darkMode)
        {
            // Form
            if (darkMode)
            {
                this.BackColor = Color.FromArgb(20, 20, 20);
            }
            else
            {
                this.BackColor = Color.FromArgb(240, 240, 240);
            }

            // Controllers
            foreach (Control controlComponent in this.Controls)
            {
                if (controlComponent is Button)
                {
                    Button btnComponent = (Button)controlComponent;
                    if (darkMode)
                    {
                        btnComponent.FlatStyle = FlatStyle.Popup;
                        btnComponent.BackColor = Color.FromArgb(45, 45, 45);
                        btnComponent.ForeColor = Color.FromArgb(225, 225, 225);
                    }
                    else
                    {
                        btnComponent.FlatStyle = FlatStyle.Standard;
                        btnComponent.BackColor = SystemColors.ControlLight;
                        btnComponent.ForeColor = SystemColors.ControlText;
                    }
                }
                else if (controlComponent is CheckBox)
                {
                    if (darkMode)
                    {
                        controlComponent.ForeColor = Color.FromArgb(225, 225, 225);
                    }
                    else
                    {
                        controlComponent.ForeColor = SystemColors.ControlText;
                    }
                }
                else if (controlComponent is ComboBox)
                {
                    ComboBox comboBoxComponent = (ComboBox)controlComponent;
                    if (darkMode)
                    {
                        comboBoxComponent.BackColor = Color.FromArgb(45, 45, 45);
                        comboBoxComponent.ForeColor = Color.FromArgb(225, 225, 225);
                    }
                    else
                    {
                        comboBoxComponent.BackColor = SystemColors.Window;
                        comboBoxComponent.ForeColor = SystemColors.WindowText;
                    }
                }
                else if (controlComponent is Label)
                {
                    if (darkMode)
                    {
                        controlComponent.ForeColor = Color.FromArgb(225, 225, 225);
                    }
                    else
                    {
                        controlComponent.ForeColor = SystemColors.ControlText;
                    }
                }
                else if (controlComponent is ListView)
                {
                    ListView listViewComponent = (ListView)controlComponent;
                    if (darkMode)
                    {
                        listViewComponent.BorderStyle = BorderStyle.FixedSingle;
                        listViewComponent.GridLines = false;
                        listViewComponent.BackColor = Color.FromArgb(45, 45, 45);
                        listViewComponent.ForeColor = Color.FromArgb(225, 225, 225);
                    }
                    else
                    {
                        listViewComponent.BorderStyle = BorderStyle.Fixed3D;
                        listViewComponent.GridLines = true;
                        listViewComponent.BackColor = SystemColors.Window;
                        listViewComponent.ForeColor = SystemColors.WindowText;
                    }
                }
                else if (controlComponent is Panel)
                {
                    Panel panelComponent = (Panel)controlComponent;
                    if (darkMode)
                    {
                        panelComponent.BorderStyle = BorderStyle.FixedSingle;
                        panelComponent.BackColor = Color.FromArgb(80, 80, 80);
                    }
                    else
                    {
                        panelComponent.BorderStyle = BorderStyle.Fixed3D;
                        panelComponent.BackColor = SystemColors.ControlLight;
                    }
                }
                else if (controlComponent is PictureBox)
                {
                    // No changes between light/dark mode right now..
                }
                else if (controlComponent is TextBox)
                {
                    if (controlComponent.Name != "textBoxDebugger")
                    {
                        TextBox txtBoxComponent = (TextBox)controlComponent;
                        if (darkMode)
                        {
                            txtBoxComponent.BorderStyle = BorderStyle.FixedSingle;
                            txtBoxComponent.BackColor = Color.FromArgb(45, 45, 45);
                            txtBoxComponent.ForeColor = Color.FromArgb(225, 225, 225);
                        }
                        else
                        {
                            txtBoxComponent.BorderStyle = BorderStyle.Fixed3D;
                            txtBoxComponent.BackColor = SystemColors.Window;
                            txtBoxComponent.ForeColor = SystemColors.ControlText;
                        }
                    }
                }
            }
            gUserSettings.IsDarkMode = darkMode;
            File.WriteAllText(PATH_VC_RESOURCES_VC_USERSETTINGS_CFG_FILE, JsonConvert.SerializeObject(gUserSettings));
        }

        private bool performSanityCheck(string path)
        {
            writeToDebugger("Sanity check... ");
            // Check if specified directory exist (1)
            if (!Directory.Exists(path))
            {
                writeLineToDebugger("Failed! Error code: 1");
                MessageBox.Show("The specified path does not seem to exist.\nDid you set the correct path?", "Venom Crosshairs - TF2 Path does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if specified TF2 Path contains tf.exe (2)
            if (!File.Exists($@"{path}\tf.exe") || !File.Exists($@"{path}\tf_win64.exe"))
            {
                writeLineToDebugger("Failed! Error code: 2");
                MessageBox.Show("The specified TF2 path does not contain \"tf.exe\" or \"tf_win64.exe\".\nDid you set the correct path?\n\nHint: Select the \"Team Fortress 2\" directory.", "Venom Crosshairs - Invalid TF2 path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if vtf2tga.exe exists (3)
            if (!File.Exists($@"{path}\bin\vtf2tga.exe"))
            {
                writeLineToDebugger("Failed! Error code: 3");
                MessageBox.Show("Could not find \"vtf2tga.exe\".\nPlease verify game files.", "Venom Crosshairs - vtf2tga.exe does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if vtf2tga.exe exists (4)
            if (!File.Exists($@"{path}\bin\tier0.dll"))
            {
                writeLineToDebugger("Failed! Error code: 4");
                MessageBox.Show("Could not find \"tier0.dll\".\nPlease verify game files.", "Venom Crosshairs - tier0.dll does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if vpk.exe exists (6)
            if (!File.Exists($@"{path}\bin\vpk.exe"))
            {
                writeLineToDebugger("Failed! Error code: 6");
                MessageBox.Show("Could not find \"vpk.exe\".\nPlease verify game files.", "Venom Crosshairs - vpk.exe does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if Venom Crosshairs executable exists in \tf\custom (7)
            if (Directory.Exists(path + @"\tf\custom"))
            {
                foreach (string executable in Directory.GetFiles(path + @"\tf\custom", "*.exe", SearchOption.AllDirectories))
                {
                    if (Path.GetFileName(executable) == "VenomCrosshairs.exe")
                    {
                        writeLineToDebugger("Failed! Error code: 7");
                        MessageBox.Show("Venom Crosshairs found within TF2 custom folder!\n\nIn order for Venom Crosshairs to work properly, make sure to extract the release anywhere except your custom TF2 folder.", "Venom Crosshairs - Release extracted to custom folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            writeLineToDebugger("Done!");
            return true;
        }
    }
}
