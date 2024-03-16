using AWDBiuBiu.Resource;
using AWDBiuBiu.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AWDBiuBiu
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (!Directory.Exists(FilePath.baseDir))
            {
                Directory.CreateDirectory(FilePath.baseDir);//创建该文件夹
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
