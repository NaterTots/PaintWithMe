using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaintWithMe
{
    public interface ILogger
    {
        void Log(Exception e);
        void Log(string s);
        void Log(Exception e, string s);
    }

    public class LogManager : List<ILogger>, IService, ILogger
    {

        public void Initialize()
        {
            this.Add(new FileLogger());
        }

        public ServiceType GetServiceType()
        {
            return ServiceType.Logger;
        }

        public void Log(Exception e)
        {
            foreach (ILogger logger in this)
            {
                logger.Log(e);
            }
        }

        public void Log(string s)
        {
            foreach (ILogger logger in this)
            {
                Log(s);
            }
        }

        public void Log(Exception e, string s)
        {
            foreach (ILogger logger in this)
            {
                Log(e, s);
            }
        }
    }
}
