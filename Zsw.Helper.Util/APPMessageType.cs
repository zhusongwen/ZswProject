using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zsw.Common.Util
{
    // 摘要:
    //     出错提示信息的等级
    public enum APPMessageType
    {
        [Description("系统异常")]
        SysErrInfo = 0,
        [Description("数据库操作异常")]
        SysDatabaseInfo = 1,
        [Description("硬盘文件操作异常")]
        SysFileInfo = 2,
        [Description("显示给用户的异常信息")]
        DisplayToUser = 3,
        [Description("代码运行的信息")]
        CodeRunInfo = 4,
        [Description("系统警告")]
        SysWarning = 5,
        [Description("其它系统异常")]
        OtherSysInfo = 6,
        [Description("数据验证")]
        DataInvalid = 7,
    }
}
