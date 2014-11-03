using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zsw.Common.Util
{
    public class APPException : ApplicationException
    {
        private string _ErrorCode;
        private APPMessageType _ErrorLevel = APPMessageType.OtherSysInfo;

        public string ErrorCode
        {
            get
            {
                return this._ErrorCode;
            }
        }
        public APPException()
        {
 
        }
        public APPException(string errorMsg)
            : this(errorMsg, APPMessageType.SysErrInfo, "-1")
        { }
        public APPException(string errorMsg,APPMessageType errorLevel,string errorCode):base(errorMsg)
        {
            this._ErrorLevel = errorLevel;
            this._ErrorCode = errorCode;
        }

        public APPException(string errorMsg, APPMessageType errorLevel, Exception innerException)
            : base(errorMsg, innerException)
        {
            this._ErrorLevel = errorLevel;
        }
        
    }
}
