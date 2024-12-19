using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BTS.PolicyCycleAPIDAL
{
    public partial class DA400DataContext : IDisposable
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private OleDbConnection _oledb;
        private string _transactionlibrary = string.Empty;
        private string _tablelibrary = string.Empty;
        private string _environment = string.Empty;
        public OleDbConnection DA400Connection { get; set; }

        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public DA400DataContext(string connectionString, string transactionlibrary, string tablelibrary)
        {
            //create the connection string to the DA400
            _oledb = new OleDbConnection(connectionString);
            _oledb.Open();
            DA400Connection = _oledb;
            _transactionlibrary = transactionlibrary;
            _tablelibrary = tablelibrary;

            //Configure all maps between AS400 and SQL entities
            this.ConfigureMapper();
        }

        public void Dispose()
        {
            if (_oledb != null)
            {
                if (_oledb.State == ConnectionState.Open)
                    _oledb.Close();
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


        internal void ConfigureMapper()
        {
            this.ConfigurePolicyMasterMap();
        }

    }
}
