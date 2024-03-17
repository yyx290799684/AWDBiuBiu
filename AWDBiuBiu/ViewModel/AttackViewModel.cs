using AWDBiuBiu.Resource;
using AWDBiuBiu.Util;
using AWDBiuBiu.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace AWDBiuBiu.ViewModel
{
    public class AttackViewModel : BaseViewModel
    {
        int attackcount = 0;

        private string log { get; set; } = string.Empty;
        private bool isattack { get; set; } = false;
        private bool isthread { get; set; } = false;
        private int attackprogress { get; set; } = 0;

        public string Log { get { return log; } set { log = value; OnPropertyChanged("Log"); } }
        public bool IsAttack { get { return isattack; } set { isattack = value; OnPropertyChanged("IsAttack"); } }
        public bool IsThread { get { return isthread; } set { isthread = value; OnPropertyChanged("IsThread"); } }
        public int AttackProgress { get { return attackprogress; } set { attackprogress = value; OnPropertyChanged("AttackProgress"); } }
        public int AttackCount { get { return _requestViewModel.UrlList.Count; } }

        public RequestViewModel _requestViewModel;
        public CommitViewModel _commitViewModel;


        private ICommand clearLogCommand;

        public ICommand ClearLogCommand
        {
            get
            {
                return clearLogCommand ?? (clearLogCommand = new CommandHandler(() => ClearLog()));
            }
        }

        private void ClearLog()
        {
            Log = string.Empty;
        }

        private ICommand copyLogCommand;

        public ICommand CopyLogCommand
        {
            get
            {
                return copyLogCommand ?? (copyLogCommand = new CommandHandler(() => CopyLog()));
            }
        }

        private void CopyLog()
        {
            System.Windows.Clipboard.SetText(Log);
        }


        private ICommand startCommand;

        public ICommand StartCommand
        {
            get
            {
                return startCommand ?? (startCommand = new CommandHandler(() => Start()));
            }
        }

        private void Start()
        {
            attackcount++;
            StartAttackAsync();
        }

        private ICommand stopCommand;

        public ICommand StopCommand
        {
            get
            {
                return stopCommand ?? (stopCommand = new CommandHandler(() => Stop()));
            }
        }

        private void Stop()
        {
            attackcount++;
            IsAttack = false;
            AttackProgress = 0;
        }



        private async Task StartAttackAsync()
        {
            if (IsAttack)
            {
                return;
            }
            IsAttack = true;
            int oldattackcount = attackcount;
            AttackProgress = 0;

            foreach (var item in _requestViewModel.UrlList)
            {
                if (oldattackcount != attackcount)
                {
                    AttackProgress = 0;
                    return;
                }

                if (IsThread)
                {
                    SendAttack(item);
                    await Task.Delay(1);
                }
                else
                {
                    await SendAttack(item);
                }
                AttackProgress++;
            }
            IsAttack = false;

        }

        private async Task SendAttack(string Url)
        {
            var ret = await NetWork.getHttpWebRequest(Url,
                                        paramList: _requestViewModel.ParamList,
                                        jsonParam: _requestViewModel.ParamJson,
                                        headerList: _requestViewModel.HeaderList,
                                        _HttpMode: _requestViewModel._HttpMode,
                                        _PostMode: _requestViewModel._PostMode,
                                        filePath: _requestViewModel.FilePath,
                                        fileParam: _requestViewModel.FileParam);
            if (!string.IsNullOrEmpty(ret))
            {
                if (!string.IsNullOrEmpty(_requestViewModel.FlagReg))
                {
                    string flag = Utils.DoRegex(ret, _requestViewModel.FlagReg);
                    if (!string.IsNullOrEmpty(_commitViewModel.Url))
                    {
                        try
                        {
                            SubFlag(flag);
                            PutLog("发现FLAG", new Uri(Url).Host, flag);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        private async void SubFlag(string flag)
        {

            if (string.IsNullOrEmpty(_commitViewModel.FlagIdRange))
            {
                var ret = await NetWork.getHttpWebRequest(_commitViewModel.Url.Replace("{flag}", flag),
                                                                            paramList: _commitViewModel.ParamList,
                                                                            jsonParam: _commitViewModel.ParamJson.Replace("{flag}", flag),
                                                                            headerList: _commitViewModel.HeaderList,
                                                                            _HttpMode: _commitViewModel._HttpMode,
                                                                            _PostMode: (PostMode)Enum.Parse(typeof(PostMode), _commitViewModel._FlagPostMode.ToString()));
                PutLog("提交FLAG", _commitViewModel.FlagCommitName, flag, ret);
            }
            else
            {
                var ids = Utils.BuildStartAndEnd(_commitViewModel.FlagIdRange);
                var idstart = ids.Key;
                var ipend = ids.Value;

                for (int id = idstart; id <= ipend; id++)
                {
                    var ret = await NetWork.getHttpWebRequest(_commitViewModel.Url.Replace("{flag}", flag).Replace("{flagid}", id.ToString()),
                                                                            paramList: _commitViewModel.ParamList,
                                                                            jsonParam: _commitViewModel.ParamJson.Replace("{flag}", flag),
                                                                            headerList: _commitViewModel.HeaderList,
                                                                            _HttpMode: _commitViewModel._HttpMode,
                                                                            _PostMode: (PostMode)Enum.Parse(typeof(PostMode), _commitViewModel._FlagPostMode.ToString()));
                    PutLog("提交FLAG", _commitViewModel.FlagCommitName, flag, ret);
                }
            }

        }

        private void PutLog(string content, string host = "", string flag = "", string ret = "")
        {
            Log += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] [{content}] [{host}] [{flag}] [{ret}]\r\n";
        }











    }
}
