﻿using AWDBiuBiu.Resource;
using AWDBiuBiu.View;
using AWDBiuBiu.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace AWDBiuBiu.Model
{
    public class ConfigModel
    {
        public RequestConfigModel RequestConfigModel { get; set; } = new RequestConfigModel();
        public CommitConfigModel CommitConfigModel { get; set; } = new CommitConfigModel();

        private ICommand exportConfigCommand;

        [JsonIgnore]
        public ICommand ExportConfigCommand
        {
            get
            {
                return exportConfigCommand ?? (exportConfigCommand = new CommandHandler(() => ExportConfig()));
            }
        }

        private void ExportConfig()
        {
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.InitialDirectory = FilePath.baseDir;
            sfd.Filter = "配置文件|*.json;*.txt;*.config;";
            sfd.FileName = "config";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Debug.WriteLine(sfd.FileName);
                SaveConfig(BuildConfig(), sfd.FileName);
            }
        }

        public string BuildConfig()
        {
            var configString = JsonConvert.SerializeObject(this, Formatting.Indented);
            return configString;
        }


        public bool SaveConfig(string configModel, string path)
        {

            File.Create(path).Close();
            StreamWriter stream = new StreamWriter(path);
            stream.WriteLine(configModel);
            stream.Dispose();//流关闭

            return true;
        }

    }

    public class RequestConfigModel
    {
        public ObservableCollection<RequestViewModel> RequestList { get; set; } = new ObservableCollection<RequestViewModel>();

        private ICommand addRequestCommand;

        [JsonIgnore]
        public ICommand AddRequestCommand
        {
            get
            {
                return addRequestCommand ?? (addRequestCommand = new CommandHandler(() => AddRequest()));
            }
        }

        private void AddRequest()
        {
            if (MainWindow.configModel.CommitConfigModel.CommitList.Count == 0)
            {
                MessageBox.Show("请先前往提交管理增加FLAG提交项");
            }
            else
            {
                AddRequestDialog addRequestDialog = new AddRequestDialog();
                if (addRequestDialog.ShowDialog() == true)
                {
                    var request = addRequestDialog._requestViewModel;
                    RequestList.Add(request);
                }
            }


        }
    }

    public class CommitConfigModel
    {
        public ObservableCollection<CommitViewModel> CommitList { get; set; } = new ObservableCollection<CommitViewModel>();

        private ICommand addCommitCommand;

        public ICommand AddCommitCommand
        {
            get
            {
                return addCommitCommand ?? (addCommitCommand = new CommandHandler(() => AddCommit()));
            }
        }

        private void AddCommit()
        {
            AddCommitDialog addCommitDialog = new AddCommitDialog();
            if (addCommitDialog.ShowDialog() == true)
            {
                var commit = addCommitDialog._commitViewModel;
                CommitList.Add(commit);
            }
        }
    }
}

