using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.ComponentModel;
using Path = System.IO.Path;
using GTSavesManager.Properties;

namespace GTSavesManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        List<GlobalSave> globalSaves = new List<GlobalSave>();
        List<Save> saves = new List<Save>();
        BindingList<GlobalSave> bGlobalSaves;
        BindingList<Save> bSaves;
        GlobalSave currGlobalSave;

        public MainWindow()
        {
            InitializeComponent();

            //check if the settings are populated
            if(! Directory.Exists(Settings.Default.savesFolder) || ! File.Exists(Settings.Default.launcherPath))
            {
                //create the settings if none exist
                SettingsForm sForm = new SettingsForm();
                sForm.ShowDialog();
            }

            bGlobalSaves = new BindingList<GlobalSave>(globalSaves);
            bSaves = new BindingList<Save>(saves);

            //check if the first global save exists
            if(! LoadGlobalSaves())
            {
                NewGlobalSave(1, true);
                Settings.Default.lastGlobalSaveName = "gs01";
                MessageBox.Show("Global save backup gs01 created.");
            }

            LoadAllSaves();

            globalSaveLabel.Content = $"Current global save: {currGlobalSave}";

            saveListBox.DisplayMemberPath = "displayName";
            saveListBox.ItemsSource = bSaves;

            globalSaveBox.DisplayMemberPath = "name";
            globalSaveBox.ItemsSource = bGlobalSaves;

            CheckEnableButtons();

            bSaves.ListChanged += (s, e) => CheckEnableButtons();

            globalSaveBox.SelectionChanged += (s, e) => LoadSaves((GlobalSave)globalSaveBox.SelectedItem);

            saveListBox.SelectionChanged += (s, e) => CheckEnableButtons();
            saveListBox.MouseDoubleClick += (s, e) =>
            {
                if (saveListBox.SelectedIndex >= 0) 
                    ChangeSaveName(saveListBox.SelectedIndex);
            };

            changeGlobalSaveButton.Click += (s, e) => ChangeGlobalSave((GlobalSave)globalSaveBox.SelectedItem);

            newGlobalSaveButton.Click += (s, e) => NewGlobalSave();

            newSaveButton.Click += (s, e) => NewSave();

            overwriteSaveButton.Click += (s, e) => ReplaceSave((Save)saveListBox.SelectedItem);

            restoreSaveButton.Click += (s, e) =>
            {
                if (currGlobalSave.Equals(globalSaveBox.SelectedItem))
                    RestoreSave((Save)saveListBox.SelectedItem);
                else 
                    ChangeGlobalSave((GlobalSave)globalSaveBox.SelectedItem, (Save)saveListBox.SelectedItem);
            };

            importSavePointButton.Click += (s, e) => ImportExternalSave();

            settingsButton.Click += (s, e) => ChangeSettings();

            Closing += (sender, e) =>
            {
                //save settings (used to update the last global save)
                Settings.Default.lastGlobalSaveName = currGlobalSave.ToString();
                Settings.Default.Save();
            };
        }

        string GetSaveDirPath(GlobalSave? gs = null)
        {
            if (gs == null) gs = currGlobalSave;
            return Path.Combine(Settings.Default.savesFolder, gs.ToString());
        }
        string GetSaveFilePath(Save save, GlobalSave? gs = null)
        {
            return Path.Combine(GetSaveDirPath(gs), save.ToString());
        }
        string GetGlobalSaveFilePath(GlobalSave? gs = null)
        {
            return Path.Combine(GetSaveDirPath(gs), "globalSave.gt");
        }

        bool LoadGlobalSaves()
        {
            globalSaves.Clear();
            //populate list of global saves from folder names
            var dirs = Directory.EnumerateDirectories(Settings.Default.savesFolder);
            foreach(string dir in dirs)
            {
                string dirName = Path.GetFileName(dir);
                if(GlobalSave.IsValidFolder(dirName))
                {
                    globalSaves.Add(new GlobalSave(dirName));
                }
            }
            if (globalSaves.Count == 0) return false;
            globalSaves.Sort();
            bGlobalSaves.ResetBindings();

            //globalSaveBox.BindingGroup
            //select the right one from the settings
            currGlobalSave = globalSaves.Find(gs => gs.name.Equals(Settings.Default.lastGlobalSaveName));
            globalSaveBox.SelectedItem = currGlobalSave;

            return true;
        }
        void LoadSaves(GlobalSave gs)
        {
            saves.Clear();
            var gsFolder = GetSaveDirPath(gs);
            var files = Directory.EnumerateFiles(gsFolder);
            foreach(string filePath in files)
            {
                string file = Path.GetFileName(filePath);
                if(Save.IsValid(file))
                {
                    saves.Add(new Save(file));
                }
            }
            saves.Sort();
            bSaves.ResetBindings();
        }
        void LoadAllSaves()
        {
            //populate global saves
            LoadGlobalSaves();

            currGlobalSave = globalSaves.Find(gs => gs.name.Equals(Settings.Default.lastGlobalSaveName));
            if (currGlobalSave.name == null) { currGlobalSave = globalSaves.Find(gs => gs.num.Equals(globalSaves.Min(gs1 => gs1.num))); }

            //populate local saves with current global save
            globalSaveBox.SelectedItem = currGlobalSave;
            LoadSaves(currGlobalSave);
        }
        void NewSave(string name = null)
        {
            string dir = GetSaveDirPath();
            //take the current save from the base folder and copy it over to the relevant one
            string source = Utils.getOrigSavePath();
            string newName = name;
            if(string.IsNullOrWhiteSpace(newName)) newName = Microsoft.VisualBasic.Interaction.InputBox("What is the name of the new save?", "Save Name");
            if(string.IsNullOrWhiteSpace(newName)) newName = "savedGame";
            Save newSave = new Save(newName, DateTime.Now);
            string dest = Path.Combine(dir, newSave.ToString());
            File.Copy(source, dest);
            LoadSaves(currGlobalSave);
        }
        void ReplaceSave(Save oldSave)
        {
            string oldName = oldSave.name;
            //delete old save from disk
            File.Delete(GetSaveFilePath(oldSave));
            //delete save from list
            saves.Remove(oldSave);
            //make new save from current time and old name
            NewSave(oldName);
            LoadSaves(currGlobalSave);
        }
        void RestoreSave(Save save)
        {
            if (!Utils.isGTRunning())
            {
                string source = GetSaveFilePath(save);
                string dest = Utils.getOrigSavePath();
                File.Copy(source, dest, true);
                var result = MessageBox.Show("Save point successfully loaded. Launch Golden Treasure now?", "Load successful", MessageBoxButton.YesNo);
                if(result == MessageBoxResult.Yes) { _ = System.Diagnostics.Process.Start(Settings.Default.launcherPath); }
            }
            else MessageBox.Show("Cannot restore a save while Golden Treasure is running! Close Golden Treasure before trying again.", "Error");
        }
        void NewGlobalSave(int? num = null, bool first = false)
        {
            //set the right number and create the gsave struct
            int theNum = num ?? globalSaves.Max(s => s.num) + 1;
            GlobalSave gs = new GlobalSave(theNum);
            //create the folder and file
            Directory.CreateDirectory(GetSaveDirPath(gs));
            File.Create(GetGlobalSaveFilePath(gs)).Dispose();

            if(first)
            {
                //automatically backup the global save on first open
                File.Copy(Utils.getOrigGlobalSavePath(), GetGlobalSaveFilePath(gs), true);
                currGlobalSave = gs;
                globalSaveBox.SelectedItem = gs;
            }
            else ChangeGlobalSave(gs);
        }
        bool ChangeGlobalSave(GlobalSave gs, Save? s = null)
        {
            if (!Utils.isGTRunning())
            {
                if (currGlobalSave.Equals(gs) && s == null) return true;
                var prompt = MessageBox.Show("Do you want to backup the current story point?", "Global save swap", MessageBoxButton.YesNoCancel);
                if (prompt.Equals(MessageBoxResult.Yes)) { NewSave(); }
                else if (prompt.Equals(MessageBoxResult.Cancel)) { return false; }

                //back up the current global save
                string gssource = Utils.getOrigGlobalSavePath();
                string gsdest = GetGlobalSaveFilePath();
                File.Copy(gssource, gsdest, true);

                //swap the new global save
                gssource = GetGlobalSaveFilePath(gs);
                gsdest = Utils.getOrigGlobalSavePath();
                File.Copy(gssource, gsdest, true);

                //swap the corresponding save
                string sdest = Utils.getOrigSavePath();
                File.Delete(sdest);
                if (s != null)
                {
                    RestoreSave(s.Value);
                }
                else
                {
                    File.Create(sdest).Dispose();
                }
                currGlobalSave = gs;
                globalSaveLabel.Content = $"Current global save: {currGlobalSave}";
                LoadSaves(gs);
                return true;
            }
            else
            {
                MessageBox.Show("Cannot swap a global save file while Golden Treasure is running! Close Golden Treasure before trying again.", "Error");
                globalSaveBox.SelectedItem = currGlobalSave;
                return false;
            }
        }
        void ImportExternalSave()
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Multiselect = true;
            dialog.InitialDirectory = Settings.Default.savesFolder;
            dialog.Title = "Select a savedGame.gt file or corresponding backup(s)...";
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && File.Exists(dialog.FileName))
            {
                foreach(string inPath in dialog.FileNames)
                {
                    var date = File.GetLastWriteTime(inPath);
                    var name = Microsoft.VisualBasic.Interaction.InputBox("What is the name of the new save?", "Save Name", Path.GetFileNameWithoutExtension(inPath));
                    if (string.IsNullOrWhiteSpace(name)) name = "savedGame";
                    var save = new Save(name, date);
                    var outPath = GetSaveFilePath(save);
                    File.Copy(inPath, outPath, true);
                    saves.Add(save);
                }
                saves.Sort();
                bSaves.ResetBindings();
                MessageBox.Show("Save(s) successfully imported.", "Success");
            }
        }
        void ChangeSaveName(int i)
        {
            var oldSave = saves[i];
            var newName = Microsoft.VisualBasic.Interaction.InputBox("What is the new name of the save?", "Change Save Name", oldSave.name);
            if(! string.IsNullOrEmpty(newName))
            {
                Save newSave = new Save(newName, oldSave.dt);
                File.Copy(GetSaveFilePath(oldSave), GetSaveFilePath(newSave), true);
                File.Delete(GetSaveFilePath(oldSave));
                saves[i] = newSave;
                bSaves.ResetBindings();
            }
        }
        void ChangeSettings()
        {
            //open up the other form basically
            SettingsForm sf = new SettingsForm();

            string oldSaveFolder = Settings.Default.savesFolder;
            sf.Show();

            //wait for closing
            if (!oldSaveFolder.Equals(Settings.Default.savesFolder))
            {
                LoadAllSaves();
            }
        }
        void CheckEnableButtons()
        {
            bool a = currGlobalSave.Equals(globalSaveBox.SelectedItem);
            bool b = saves.Count != 0 && saveListBox.SelectedIndex != -1;

            newSaveButton.IsEnabled = a;
            overwriteSaveButton.IsEnabled = a && b;
            restoreSaveButton.IsEnabled = b;
        }
    }

    struct GlobalSave : IComparable
    {
        static readonly Regex r = new Regex("gs([0-9]+)");
        public static bool IsValidFolder(string dir)
        {
            var m = r.Match(dir);
            return m.Success;
        }

        public GlobalSave(string name, int num)
        {
            this.name = name;
            this.num = num;
        }

        public GlobalSave(string name)
        {
            Match m = r.Match(name);
            if (m.Success)
            {
                this.name = name;
                this.num = int.Parse(m.Groups[1].Value);
            }
            else throw new Exception("couldn't parse global save name");
        }

        public GlobalSave(int num)
        {
            this.num = num;
            this.name = $"gs{num.ToString("00")}";
        }

        public override string ToString()
        {
            return name;
        }

        public int CompareTo(object obj)
        {
            GlobalSave gs2 = (GlobalSave)obj;
            return this.num.CompareTo(gs2.num);
        }

        public string name { get; }
        public int num { get; }
    }

    struct Save : IComparable
    {
        static readonly Regex r = new Regex(@"([0-9]+) - (.*)\.gt");
        public static bool IsValid(string fullName)
        {
            var m = r.Match(fullName);
            return m.Success;
        }
        public Save(string name, DateTime dt)
        {
            this.name = name;
            this.dt = dt;
        }
        public Save(string fullName)
        {
            var m = r.Match(fullName);
            if (m.Success)
            {
                this.name = m.Groups[2].Value.Trim();
                this.dt = DateTime.ParseExact(m.Groups[1].Value, Settings.Default.dateTimeFilePattern, System.Globalization.CultureInfo.InvariantCulture);
            }
            else throw new Exception("failed save name decoding");
        }

        override public string ToString()
        {
            return $"{dt.ToString(Settings.Default.dateTimeFilePattern)} - {name}.gt";
        }

        public int CompareTo(object obj)
        {
            var save2 = (Save)obj;
            return - this.dt.CompareTo(save2.dt);
        }

        public string name;
        public DateTime dt;
        public string displayName { get { return $"{name} - {dt.ToString(Settings.Default.dateTimeDisplayPattern)}"; } }
    }
}
