using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PaintWithMe
{
    class FileLogger : ILogger
    {
        private bool _useDebugger = false;

        public void Log(Exception e)
        {
            Debug.WriteIf(_useDebugger, e);
        }

        public void Log(string s)
        {
            Debug.WriteIf(_useDebugger, s);
        }

        public void Log(Exception e, string s)
        {
            Debug.WriteIf(_useDebugger, e);
            Debug.WriteIf(_useDebugger, s);
        }
    }
}