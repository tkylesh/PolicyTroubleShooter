using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using cwbx;
using BTS.PolicyCycleAPICommon.Dto;
using System.Threading;

namespace BTS.PolicyCycleAPIDAL
{
  public partial class AS400DataContext : IDisposable
  {

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IBM.Data.DB2.iSeries.iDB2Connection _as400Connection;

        //private AS400System _as400Connection;

        private string _librarylist = string.Empty;
        public IBM.Data.DB2.iSeries.iDB2Connection AS400Connection { get; set; }
        //public AS400System AS400Connection { get; set; }

        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public AS400DataContext(AS400Parms parms)
        {
            int tryCount = 10;

            retry:
            if (tryCount > 0)
            {
                try
                {
                    //create the connection string to the AS400
                    _as400Connection = new IBM.Data.DB2.iSeries.iDB2Connection(AS400ConnectionString);
                    //_as400Connection = new AS400System();
                    //_as400Connection.Define(parms.Server);
                    //_as400Connection.UserID = parms.UserID;
                    //_as400Connection.Password = parms.Password;
                    //_as400Connection.PromptMode = cwbcoPromptModeEnum.cwbcoPromptNever;
                    //_as400Connection.Connect(cwbcoServiceEnum.cwbcoServiceDataQueues);
                    //_as400Connection.Connect(cwbcoServiceEnum.cwbcoServiceSecurity);
                    //_as400Connection.Connect(cwbcoServiceEnum.cwbcoServiceRemoteCmd);

                    Thread.Sleep(50);
                }
                catch
                {
                    tryCount--;
                    goto retry;
                }
            }

            //_as400Connection.Open();
            AS400Connection = _as400Connection;
            //_librarylist = librarylist;
           // _commonlibrary = commonlibrary;
        }

        public void Dispose()
        {
            if (_as400Connection != null)
            {
                _as400Connection.Dispose();
                //_as400Connection.Disconnect(cwbcoServiceEnum.cwbcoServiceAll);
            }

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }

            disposed = true;
        }
    }
}