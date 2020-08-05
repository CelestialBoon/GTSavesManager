using GTSavesManager.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTSavesManager
{
    public partial class SettingsForm : Form
    {

        public SettingsForm()
        {
            InitializeComponent();

            var saveFolder = Utils.EmptyToNull(Settings.Default.savesFolder) ?? Path.Combine($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}Low", @"Dreaming Door Studios\Golden Treasure - The Great Green\Saves");
            var launcherPath = Utils.EmptyToNull(Settings.Default.launcherPath) ?? @"C:\Program Files (x86)\Steam\steamapps\common\Golden Treasure The Great Green\Golden Treasure - The Great Green.exe";

            if (! Directory.Exists(saveFolder)) saveFolder = null;
            if (! File.Exists(launcherPath)) launcherPath = null;

            saveTextBox.Text = saveFolder;
            launcherTextBox.Text = launcherPath;

            saveButton.Click += (s, e) => Save();
            browseLauncherButton.Click += (s, e) => selectGTLauncherPath();
            browseSaveButton.Click += (s, e) => selectSaveFolder();
        }

        void Save()
        {
            Settings.Default.launcherPath = launcherTextBox.Text;
            Settings.Default.savesFolder = saveTextBox.Text;
            if(File.Exists(Settings.Default.launcherPath) && Directory.Exists(Settings.Default.savesFolder))
            {
                Settings.Default.Save();
                this.DialogResult = DialogResult.OK;
                this.Dispose();
            } else {
                MessageBox.Show("Invalid settings. Please select the correct location for the save folder and the Golden Treasure executable.");
            }
        }

        void selectGTLauncherPath()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                launcherTextBox.Text = dialog.FileName;
        }

        void selectSaveFolder()
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if(result == DialogResult.OK)
                saveTextBox.Text = dialog.SelectedPath;
        }
    }
}
