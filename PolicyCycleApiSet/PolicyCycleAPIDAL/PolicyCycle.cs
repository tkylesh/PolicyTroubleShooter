using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.PolicyCycleAPIDAL
{
    public partial class PolicyCycle
    {
    }
}

namespace BTS.PolicyCycleAPIDAL.PolicyCycleTableAdapters
{
    public partial class PolicyMasterTableAdapter
    {
        public void SetLibrary(string libraryName)
        {
            int count = this.CommandCollection.Length;
            for (int i = 0; i < count; i++)
            {
                this.CommandCollection[i].CommandText = string.Format(this.CommandCollection[i].CommandText, libraryName);
            }
        }
    }
}