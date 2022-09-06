namespace TF2WeaponSpecificCrosshairs
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.textBoxTF2Path = new System.Windows.Forms.TextBox();
            this.lblTF2Path = new System.Windows.Forms.Label();
            this.textBoxDebugger = new System.Windows.Forms.TextBox();
            this.cbClass = new System.Windows.Forms.ComboBox();
            this.cbWeapon = new System.Windows.Forms.ComboBox();
            this.cbCrosshair = new System.Windows.Forms.ComboBox();
            this.lblClass = new System.Windows.Forms.Label();
            this.lblWeapon = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listViewChosenCrosshairs = new System.Windows.Forms.ListView();
            this.btnBrowseTF2Path = new System.Windows.Forms.Button();
            this.checkBoxAddClassWeapons = new System.Windows.Forms.CheckBox();
            this.lblSettings = new System.Windows.Forms.Label();
            this.checkBoxAddPrimaryWeapons = new System.Windows.Forms.CheckBox();
            this.checkBoxAddSecondaryWeapons = new System.Windows.Forms.CheckBox();
            this.checkBoxAddMeleeWeapons = new System.Windows.Forms.CheckBox();
            this.checkBoxAddMiscWeapons = new System.Windows.Forms.CheckBox();
            this.lblAdditionalSettings = new System.Windows.Forms.Label();
            this.comboBoxExplosionEffect = new System.Windows.Forms.ComboBox();
            this.lblNoExplosion = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBoxLoading = new System.Windows.Forms.PictureBox();
            this.btnGitHub = new System.Windows.Forms.Button();
            this.btnSteam = new System.Windows.Forms.Button();
            this.btnNextCrosshair = new System.Windows.Forms.Button();
            this.btnPrevCrosshair = new System.Windows.Forms.Button();
            this.btnInstallClean = new System.Windows.Forms.Button();
            this.btnInstall = new System.Windows.Forms.Button();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.btnAddCrosshair = new System.Windows.Forms.Button();
            this.pictureBoxCrosshair = new System.Windows.Forms.PictureBox();
            this.btnReload = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCrosshair)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxTF2Path
            // 
            this.textBoxTF2Path.Location = new System.Drawing.Point(12, 24);
            this.textBoxTF2Path.Name = "textBoxTF2Path";
            this.textBoxTF2Path.Size = new System.Drawing.Size(364, 20);
            this.textBoxTF2Path.TabIndex = 3;
            // 
            // lblTF2Path
            // 
            this.lblTF2Path.AutoSize = true;
            this.lblTF2Path.Location = new System.Drawing.Point(9, 9);
            this.lblTF2Path.Name = "lblTF2Path";
            this.lblTF2Path.Size = new System.Drawing.Size(54, 13);
            this.lblTF2Path.TabIndex = 2;
            this.lblTF2Path.Text = "TF2 Path:";
            // 
            // textBoxDebugger
            // 
            this.textBoxDebugger.BackColor = System.Drawing.Color.Black;
            this.textBoxDebugger.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxDebugger.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDebugger.ForeColor = System.Drawing.SystemColors.Control;
            this.textBoxDebugger.Location = new System.Drawing.Point(858, 9);
            this.textBoxDebugger.Multiline = true;
            this.textBoxDebugger.Name = "textBoxDebugger";
            this.textBoxDebugger.ReadOnly = true;
            this.textBoxDebugger.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDebugger.Size = new System.Drawing.Size(297, 530);
            this.textBoxDebugger.TabIndex = 4;
            // 
            // cbClass
            // 
            this.cbClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClass.FormattingEnabled = true;
            this.cbClass.Location = new System.Drawing.Point(71, 50);
            this.cbClass.Name = "cbClass";
            this.cbClass.Size = new System.Drawing.Size(182, 21);
            this.cbClass.TabIndex = 5;
            // 
            // cbWeapon
            // 
            this.cbWeapon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWeapon.DropDownWidth = 420;
            this.cbWeapon.Enabled = false;
            this.cbWeapon.FormattingEnabled = true;
            this.cbWeapon.Location = new System.Drawing.Point(71, 77);
            this.cbWeapon.Name = "cbWeapon";
            this.cbWeapon.Size = new System.Drawing.Size(182, 21);
            this.cbWeapon.TabIndex = 6;
            // 
            // cbCrosshair
            // 
            this.cbCrosshair.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCrosshair.DropDownWidth = 140;
            this.cbCrosshair.Enabled = false;
            this.cbCrosshair.FormattingEnabled = true;
            this.cbCrosshair.Location = new System.Drawing.Point(71, 104);
            this.cbCrosshair.Name = "cbCrosshair";
            this.cbCrosshair.Size = new System.Drawing.Size(182, 21);
            this.cbCrosshair.TabIndex = 7;
            // 
            // lblClass
            // 
            this.lblClass.AutoSize = true;
            this.lblClass.Location = new System.Drawing.Point(30, 53);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(35, 13);
            this.lblClass.TabIndex = 8;
            this.lblClass.Text = "Class:";
            // 
            // lblWeapon
            // 
            this.lblWeapon.AutoSize = true;
            this.lblWeapon.Location = new System.Drawing.Point(14, 80);
            this.lblWeapon.Name = "lblWeapon";
            this.lblWeapon.Size = new System.Drawing.Size(51, 13);
            this.lblWeapon.TabIndex = 9;
            this.lblWeapon.Text = "Weapon:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Crosshair:";
            // 
            // listViewChosenCrosshairs
            // 
            this.listViewChosenCrosshairs.FullRowSelect = true;
            this.listViewChosenCrosshairs.GridLines = true;
            this.listViewChosenCrosshairs.HideSelection = false;
            this.listViewChosenCrosshairs.Location = new System.Drawing.Point(12, 343);
            this.listViewChosenCrosshairs.Name = "listViewChosenCrosshairs";
            this.listViewChosenCrosshairs.Size = new System.Drawing.Size(840, 196);
            this.listViewChosenCrosshairs.TabIndex = 12;
            this.listViewChosenCrosshairs.UseCompatibleStateImageBehavior = false;
            this.listViewChosenCrosshairs.View = System.Windows.Forms.View.Details;
            // 
            // btnBrowseTF2Path
            // 
            this.btnBrowseTF2Path.Location = new System.Drawing.Point(379, 23);
            this.btnBrowseTF2Path.Name = "btnBrowseTF2Path";
            this.btnBrowseTF2Path.Size = new System.Drawing.Size(63, 22);
            this.btnBrowseTF2Path.TabIndex = 22;
            this.btnBrowseTF2Path.Text = "Browse...";
            this.btnBrowseTF2Path.UseVisualStyleBackColor = true;
            this.btnBrowseTF2Path.Click += new System.EventHandler(this.btnBrowseTF2Path_Click);
            // 
            // checkBoxAddClassWeapons
            // 
            this.checkBoxAddClassWeapons.AutoSize = true;
            this.checkBoxAddClassWeapons.Enabled = false;
            this.checkBoxAddClassWeapons.Location = new System.Drawing.Point(71, 156);
            this.checkBoxAddClassWeapons.Name = "checkBoxAddClassWeapons";
            this.checkBoxAddClassWeapons.Size = new System.Drawing.Size(175, 17);
            this.checkBoxAddClassWeapons.TabIndex = 23;
            this.checkBoxAddClassWeapons.Text = "Add to EVERY class weapons?";
            this.checkBoxAddClassWeapons.UseVisualStyleBackColor = true;
            // 
            // lblSettings
            // 
            this.lblSettings.AutoSize = true;
            this.lblSettings.Location = new System.Drawing.Point(28, 157);
            this.lblSettings.Name = "lblSettings";
            this.lblSettings.Size = new System.Drawing.Size(35, 13);
            this.lblSettings.TabIndex = 24;
            this.lblSettings.Text = "And...";
            // 
            // checkBoxAddPrimaryWeapons
            // 
            this.checkBoxAddPrimaryWeapons.AutoSize = true;
            this.checkBoxAddPrimaryWeapons.Enabled = false;
            this.checkBoxAddPrimaryWeapons.Location = new System.Drawing.Point(71, 183);
            this.checkBoxAddPrimaryWeapons.Name = "checkBoxAddPrimaryWeapons";
            this.checkBoxAddPrimaryWeapons.Size = new System.Drawing.Size(167, 17);
            this.checkBoxAddPrimaryWeapons.TabIndex = 25;
            this.checkBoxAddPrimaryWeapons.Text = "Add to ALL primary weapons?";
            this.checkBoxAddPrimaryWeapons.UseVisualStyleBackColor = true;
            // 
            // checkBoxAddSecondaryWeapons
            // 
            this.checkBoxAddSecondaryWeapons.AutoSize = true;
            this.checkBoxAddSecondaryWeapons.Enabled = false;
            this.checkBoxAddSecondaryWeapons.Location = new System.Drawing.Point(71, 202);
            this.checkBoxAddSecondaryWeapons.Name = "checkBoxAddSecondaryWeapons";
            this.checkBoxAddSecondaryWeapons.Size = new System.Drawing.Size(183, 17);
            this.checkBoxAddSecondaryWeapons.TabIndex = 26;
            this.checkBoxAddSecondaryWeapons.Text = "Add to ALL secondary weapons?";
            this.checkBoxAddSecondaryWeapons.UseVisualStyleBackColor = true;
            // 
            // checkBoxAddMeleeWeapons
            // 
            this.checkBoxAddMeleeWeapons.AutoSize = true;
            this.checkBoxAddMeleeWeapons.Enabled = false;
            this.checkBoxAddMeleeWeapons.Location = new System.Drawing.Point(71, 221);
            this.checkBoxAddMeleeWeapons.Name = "checkBoxAddMeleeWeapons";
            this.checkBoxAddMeleeWeapons.Size = new System.Drawing.Size(162, 17);
            this.checkBoxAddMeleeWeapons.TabIndex = 27;
            this.checkBoxAddMeleeWeapons.Text = "Add to ALL melee weapons?";
            this.checkBoxAddMeleeWeapons.UseVisualStyleBackColor = true;
            // 
            // checkBoxAddMiscWeapons
            // 
            this.checkBoxAddMiscWeapons.AutoSize = true;
            this.checkBoxAddMiscWeapons.Enabled = false;
            this.checkBoxAddMiscWeapons.Location = new System.Drawing.Point(71, 248);
            this.checkBoxAddMiscWeapons.Name = "checkBoxAddMiscWeapons";
            this.checkBoxAddMiscWeapons.Size = new System.Drawing.Size(158, 17);
            this.checkBoxAddMiscWeapons.TabIndex = 28;
            this.checkBoxAddMiscWeapons.Text = "Add to ALL misc. weapons?";
            this.checkBoxAddMiscWeapons.UseVisualStyleBackColor = true;
            // 
            // lblAdditionalSettings
            // 
            this.lblAdditionalSettings.AutoSize = true;
            this.lblAdditionalSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdditionalSettings.Location = new System.Drawing.Point(340, 51);
            this.lblAdditionalSettings.Name = "lblAdditionalSettings";
            this.lblAdditionalSettings.Size = new System.Drawing.Size(106, 15);
            this.lblAdditionalSettings.TabIndex = 29;
            this.lblAdditionalSettings.Text = "Additional settings";
            // 
            // comboBoxExplosionEffect
            // 
            this.comboBoxExplosionEffect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExplosionEffect.FormattingEnabled = true;
            this.comboBoxExplosionEffect.Items.AddRange(new object[] {
            "Default",
            "Electric shock",
            "Muzzle flash",
            "Spy sapper",
            "Pyro pool"});
            this.comboBoxExplosionEffect.Location = new System.Drawing.Point(96, 3);
            this.comboBoxExplosionEffect.Name = "comboBoxExplosionEffect";
            this.comboBoxExplosionEffect.Size = new System.Drawing.Size(95, 21);
            this.comboBoxExplosionEffect.TabIndex = 31;
            // 
            // lblNoExplosion
            // 
            this.lblNoExplosion.AutoSize = true;
            this.lblNoExplosion.Location = new System.Drawing.Point(5, 6);
            this.lblNoExplosion.Name = "lblNoExplosion";
            this.lblNoExplosion.Size = new System.Drawing.Size(85, 13);
            this.lblNoExplosion.TabIndex = 32;
            this.lblNoExplosion.Text = "Explosion effect:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.comboBoxExplosionEffect);
            this.panel1.Controls.Add(this.lblNoExplosion);
            this.panel1.Location = new System.Drawing.Point(340, 69);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(206, 231);
            this.panel1.TabIndex = 33;
            // 
            // pictureBoxLoading
            // 
            this.pictureBoxLoading.Image = global::TF2WeaponSpecificCrosshairs.Properties.Resources.loading;
            this.pictureBoxLoading.Location = new System.Drawing.Point(820, 9);
            this.pictureBoxLoading.Name = "pictureBoxLoading";
            this.pictureBoxLoading.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLoading.TabIndex = 34;
            this.pictureBoxLoading.TabStop = false;
            this.pictureBoxLoading.Visible = false;
            // 
            // btnGitHub
            // 
            this.btnGitHub.Image = global::TF2WeaponSpecificCrosshairs.Properties.Resources.github_logo_24px;
            this.btnGitHub.Location = new System.Drawing.Point(782, 9);
            this.btnGitHub.Name = "btnGitHub";
            this.btnGitHub.Size = new System.Drawing.Size(32, 32);
            this.btnGitHub.TabIndex = 21;
            this.btnGitHub.UseVisualStyleBackColor = true;
            this.btnGitHub.Click += new System.EventHandler(this.btnGitHub_Click);
            // 
            // btnSteam
            // 
            this.btnSteam.Image = global::TF2WeaponSpecificCrosshairs.Properties.Resources.steam_logo_black_transparent_24px;
            this.btnSteam.Location = new System.Drawing.Point(744, 9);
            this.btnSteam.Name = "btnSteam";
            this.btnSteam.Size = new System.Drawing.Size(32, 32);
            this.btnSteam.TabIndex = 20;
            this.btnSteam.UseVisualStyleBackColor = true;
            this.btnSteam.Click += new System.EventHandler(this.btnSteam_Click);
            // 
            // btnNextCrosshair
            // 
            this.btnNextCrosshair.Enabled = false;
            this.btnNextCrosshair.Image = global::TF2WeaponSpecificCrosshairs.Properties.Resources.arrow;
            this.btnNextCrosshair.Location = new System.Drawing.Point(299, 127);
            this.btnNextCrosshair.Name = "btnNextCrosshair";
            this.btnNextCrosshair.Size = new System.Drawing.Size(36, 22);
            this.btnNextCrosshair.TabIndex = 18;
            this.btnNextCrosshair.UseVisualStyleBackColor = true;
            this.btnNextCrosshair.Click += new System.EventHandler(this.btnNextCrosshair_Click);
            // 
            // btnPrevCrosshair
            // 
            this.btnPrevCrosshair.Enabled = false;
            this.btnPrevCrosshair.Image = global::TF2WeaponSpecificCrosshairs.Properties.Resources.arrow_1801;
            this.btnPrevCrosshair.Location = new System.Drawing.Point(258, 127);
            this.btnPrevCrosshair.Name = "btnPrevCrosshair";
            this.btnPrevCrosshair.Size = new System.Drawing.Size(36, 22);
            this.btnPrevCrosshair.TabIndex = 17;
            this.btnPrevCrosshair.UseVisualStyleBackColor = true;
            this.btnPrevCrosshair.Click += new System.EventHandler(this.btnPrevCrosshair_Click);
            // 
            // btnInstallClean
            // 
            this.btnInstallClean.Enabled = false;
            this.btnInstallClean.Image = global::TF2WeaponSpecificCrosshairs.Properties.Resources.compile_warning;
            this.btnInstallClean.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnInstallClean.Location = new System.Drawing.Point(655, 315);
            this.btnInstallClean.Name = "btnInstallClean";
            this.btnInstallClean.Size = new System.Drawing.Size(94, 22);
            this.btnInstallClean.TabIndex = 16;
            this.btnInstallClean.Text = "Install (clean)";
            this.btnInstallClean.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInstallClean.UseVisualStyleBackColor = true;
            this.btnInstallClean.Click += new System.EventHandler(this.btnInstallClean_Click);
            // 
            // btnInstall
            // 
            this.btnInstall.Enabled = false;
            this.btnInstall.Image = global::TF2WeaponSpecificCrosshairs.Properties.Resources.compile;
            this.btnInstall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnInstall.Location = new System.Drawing.Point(755, 315);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(98, 22);
            this.btnInstall.TabIndex = 15;
            this.btnInstall.Text = "Install/Update";
            this.btnInstall.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Enabled = false;
            this.btnRemoveSelected.Image = global::TF2WeaponSpecificCrosshairs.Properties.Resources.cross;
            this.btnRemoveSelected.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRemoveSelected.Location = new System.Drawing.Point(11, 315);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(113, 22);
            this.btnRemoveSelected.TabIndex = 14;
            this.btnRemoveSelected.Text = "Remove selected";
            this.btnRemoveSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveSelected.UseVisualStyleBackColor = true;
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // btnAddCrosshair
            // 
            this.btnAddCrosshair.Enabled = false;
            this.btnAddCrosshair.Image = global::TF2WeaponSpecificCrosshairs.Properties.Resources.tick1;
            this.btnAddCrosshair.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddCrosshair.Location = new System.Drawing.Point(71, 271);
            this.btnAddCrosshair.Name = "btnAddCrosshair";
            this.btnAddCrosshair.Size = new System.Drawing.Size(120, 29);
            this.btnAddCrosshair.TabIndex = 13;
            this.btnAddCrosshair.Text = "   Add crosshair";
            this.btnAddCrosshair.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddCrosshair.UseVisualStyleBackColor = true;
            this.btnAddCrosshair.Click += new System.EventHandler(this.btnAddCrosshair_Click);
            // 
            // pictureBoxCrosshair
            // 
            this.pictureBoxCrosshair.BackgroundImage = global::TF2WeaponSpecificCrosshairs.Properties.Resources.cp_badlands;
            this.pictureBoxCrosshair.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxCrosshair.Image = global::TF2WeaponSpecificCrosshairs.Properties.Resources.TF2WSC;
            this.pictureBoxCrosshair.Location = new System.Drawing.Point(259, 50);
            this.pictureBoxCrosshair.Name = "pictureBoxCrosshair";
            this.pictureBoxCrosshair.Size = new System.Drawing.Size(75, 75);
            this.pictureBoxCrosshair.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxCrosshair.TabIndex = 11;
            this.pictureBoxCrosshair.TabStop = false;
            // 
            // btnReload
            // 
            this.btnReload.Image = global::TF2WeaponSpecificCrosshairs.Properties.Resources.arrow_circle_double_135;
            this.btnReload.Location = new System.Drawing.Point(820, 9);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(32, 32);
            this.btnReload.TabIndex = 0;
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 551);
            this.Controls.Add(this.pictureBoxLoading);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblAdditionalSettings);
            this.Controls.Add(this.checkBoxAddMiscWeapons);
            this.Controls.Add(this.checkBoxAddMeleeWeapons);
            this.Controls.Add(this.checkBoxAddSecondaryWeapons);
            this.Controls.Add(this.checkBoxAddPrimaryWeapons);
            this.Controls.Add(this.lblSettings);
            this.Controls.Add(this.checkBoxAddClassWeapons);
            this.Controls.Add(this.btnBrowseTF2Path);
            this.Controls.Add(this.btnGitHub);
            this.Controls.Add(this.btnSteam);
            this.Controls.Add(this.btnNextCrosshair);
            this.Controls.Add(this.btnPrevCrosshair);
            this.Controls.Add(this.btnInstallClean);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.btnRemoveSelected);
            this.Controls.Add(this.btnAddCrosshair);
            this.Controls.Add(this.listViewChosenCrosshairs);
            this.Controls.Add(this.pictureBoxCrosshair);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblWeapon);
            this.Controls.Add(this.lblClass);
            this.Controls.Add(this.cbCrosshair);
            this.Controls.Add(this.cbWeapon);
            this.Controls.Add(this.cbClass);
            this.Controls.Add(this.textBoxDebugger);
            this.Controls.Add(this.textBoxTF2Path);
            this.Controls.Add(this.lblTF2Path);
            this.Controls.Add(this.btnReload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "TF2 Weapon Specific Crosshairs";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCrosshair)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.TextBox textBoxTF2Path;
        private System.Windows.Forms.Label lblTF2Path;
        private System.Windows.Forms.TextBox textBoxDebugger;
        private System.Windows.Forms.ComboBox cbClass;
        private System.Windows.Forms.ComboBox cbWeapon;
        private System.Windows.Forms.ComboBox cbCrosshair;
        private System.Windows.Forms.Label lblClass;
        private System.Windows.Forms.Label lblWeapon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBoxCrosshair;
        private System.Windows.Forms.ListView listViewChosenCrosshairs;
        private System.Windows.Forms.Button btnAddCrosshair;
        private System.Windows.Forms.Button btnRemoveSelected;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Button btnInstallClean;
        private System.Windows.Forms.Button btnPrevCrosshair;
        private System.Windows.Forms.Button btnNextCrosshair;
        private System.Windows.Forms.Button btnSteam;
        private System.Windows.Forms.Button btnGitHub;
        private System.Windows.Forms.Button btnBrowseTF2Path;
        private System.Windows.Forms.CheckBox checkBoxAddClassWeapons;
        private System.Windows.Forms.Label lblSettings;
        private System.Windows.Forms.CheckBox checkBoxAddPrimaryWeapons;
        private System.Windows.Forms.CheckBox checkBoxAddSecondaryWeapons;
        private System.Windows.Forms.CheckBox checkBoxAddMeleeWeapons;
        private System.Windows.Forms.CheckBox checkBoxAddMiscWeapons;
        private System.Windows.Forms.Label lblAdditionalSettings;
        private System.Windows.Forms.ComboBox comboBoxExplosionEffect;
        private System.Windows.Forms.Label lblNoExplosion;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBoxLoading;
    }
}

