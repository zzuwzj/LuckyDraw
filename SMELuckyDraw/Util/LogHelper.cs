using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SMELuckyDraw.Util
{
    public class LogHelper
    {
        private static ILog log = LogManager.GetLogger("SMELuckyDraw.LogHelper");
        public static void INFO(string msg, Exception e = null)
        {
            log.Info(msg, e);
        }

        public static void WARN(string msg, Exception e = null)
        {
            log.Warn(msg, e);
        }

        public static void DEBUG(string msg, Exception e = null)
        {
            log.Debug(msg, e);
        }

        public static void ERROR(string msg, Exception e = null)
        {
            log.Error(msg, e);
        }

        public static void FATAL(string msg, Exception e = null)
        {
            log.Fatal(msg, e);
        }
    }
}
