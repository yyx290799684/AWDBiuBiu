using AWDBiuBiu.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWDBiuBiu.Resource
{
    public class FilePath
    {
        public static string baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\AWDBiuBiu");
        public static string defaultConfigPath = Path.Combine(baseDir, "default.config");
    }

    public class DrawerMenu
    {
        public static ObservableCollection<DrawerListViewModel> drawerMenuList = new ObservableCollection<DrawerListViewModel>()
        {
            new DrawerListViewModel(){title="首页",type=DrawerItemType.Home},
            new DrawerListViewModel(){title="提交管理",type=DrawerItemType.CommitManage },
            new DrawerListViewModel(){title="请求管理",type=DrawerItemType.RequestManage},
            //new DrawerListViewModel(){title="攻击管理",type=DrawerItemType.AttackManage },
        };
    }

    public enum DrawerItemType
    {
        Home,
        RequestManage,
        CommitManage,
        AttackManage,
    }

    public enum HttpMode
    {
        GET,
        POST,
    }

    public enum UrlSource
    {
        拼接,
        来自列表,
    }

    public enum PostMode
    {
        键值对,
        JSON,
        FormData,
    }

    public enum FlagPostMode
    {
        键值对,
        JSON
    }
}
