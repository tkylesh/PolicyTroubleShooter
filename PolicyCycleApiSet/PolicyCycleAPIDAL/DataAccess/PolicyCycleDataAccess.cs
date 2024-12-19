using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using BTS.PolicyCycleAPICommon.Dto;
using BTS.PolicyCycleAPICommon;

namespace BTS.PolicyCycleAPIDAL.DataAccess
{
    internal partial class PolicyCycleDataAccess
    {
        //build the AS400 connection parms
        static AS400Parms parms = new AS400Parms
        {
            Server = ConfigurationManager.AppSettings["AS400Server"].ToString(),
            Database = ConfigurationManager.AppSettings["Database"].ToString(),
            UserID = ConfigurationManager.AppSettings["UserID"].ToString(),
            //Password = ConfigurationManager.AppSettings["Password"].ToString(),
            Password = BTS.PolicyCycleAPICommon.Extensions.Decrypt(ConfigurationManager.AppSettings["Password"].ToString()),
            DataQueueLibrary = ConfigurationManager.AppSettings["ObjectLibrary"].ToString(),
            DataQueueIn = ConfigurationManager.AppSettings["DataQueueIn"].ToString(),
            DataQueueOut = ConfigurationManager.AppSettings["DataQueueOut"].ToString()
            //con = BTS.PolicyCycleAPICommon.Extensions.Decrypt(con);
            //return string.Format(con, server, librarylist, transactionLibrary);
    };

        static string DA400ConnectionString
        {
            get
            {
                var server = ConfigurationManager.AppSettings["AS400Server"].ToString();
                var initialcatalog = ConfigurationManager.AppSettings["Database"].ToString();
                var librarylist = ConfigurationManager.AppSettings["LibraryList"].ToString();
                var con = ConfigurationManager.ConnectionStrings["DA400ConnectionString"].ConnectionString;
                con = BTS.PolicyCycleAPICommon.Extensions.Decrypt(con);
                return string.Format(con, server, initialcatalog, librarylist);
            }
        }

        static string AS400ConnectionString
        {
            get
            {
                var server = ConfigurationManager.AppSettings["AS400Server"].ToString();
                var librarylist = ConfigurationManager.AppSettings["LibraryList"].ToString();
                var transactionLibrary = ConfigurationManager.AppSettings["TransactionLibrary"].ToString();
                var con = ConfigurationManager.ConnectionStrings["DA400ConnectionString"].ConnectionString;
                con = BTS.PolicyCycleAPICommon.Extensions.Decrypt(con);
                return string.Format(con, server, librarylist, transactionLibrary);
            }
        }

        static string LibraryList
        {
            get
            {
                return ConfigurationManager.AppSettings["LibraryList"].ToString();

            }
        }

        static string TransactionLibrary
        {
            get
            {
                return ConfigurationManager.AppSettings["TransactionLibrary"].ToString();

            }
        }

        static string TableLibrary
        {
            get
            {
                return ConfigurationManager.AppSettings["TableLibrary"].ToString();

            }
        }

        static string ObjectLibrary
        {
            get
            {
                return ConfigurationManager.AppSettings["ObjectLibrary"].ToString();

            }
        }

        static string DataQueueIn
        {
            get
            {
                return ConfigurationManager.AppSettings["DataQueueIn"].ToString();

            }
        }



        static string DataQueueOut
        {
            get
            {
                return ConfigurationManager.AppSettings["DataQueueOut"].ToString();

            }
        }
        static string DMSUser
        {
            get
            {
                return ConfigurationManager.AppSettings["DMSUser"].ToString();
            }
        }
        static string DMSDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["DMSDomain"].ToString();
            }
        }
        static string DMSPassword
        {
            get
            {
                return BTS.PolicyCycleAPICommon.Extensions.Decrypt(ConfigurationManager.AppSettings["DMSPassword"].ToString());
            }
        }
        static string DMSFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["DMSFilePath"].ToString();
            }
        }


        public static ResponseDto GetResponseInformation(string quoteID, string companyNumber, string policyPrefix, int policyNumber, string entryDate)
        {
            ResponseDto response = new ResponseDto();
            /*  using (var context = new AS400DataContext(parms))
            {
                response = context.GetResponseInformation(quoteID, companyNumber, policyPrefix, policyNumber, entryDate);
                context.AS400Connection.Disconnect(cwbx.cwbcoServiceEnum.cwbcoServiceDataQueues);
                context.AS400Connection.Disconnect(cwbx.cwbcoServiceEnum.cwbcoServiceSecurity);
                context.AS400Connection.Disconnect(cwbx.cwbcoServiceEnum.cwbcoServiceRemoteCmd);
            }  */
            using (var context = new AS400DataContext(parms))
            {
                response = context.GetResponseInformation(quoteID, companyNumber, policyPrefix, policyNumber, entryDate);
                context.AS400Connection.Close();
            }
            return response;
        }

        public static PolicyMasterDto GetPolicyMasterInformation(int policyNumber)
        {
            PolicyMasterDto response = new PolicyMasterDto();
            using (var context = new DA400DataContext(DA400ConnectionString, TransactionLibrary, TableLibrary))
            {
                response = context.GetPolicyMasterInformation(policyNumber);
                context.DA400Connection.Close();
            }
            return response;
        }

        public static DMSResponseDto GetDMSResponseInformation(int policyNumber, string quoteID, string activity)
        {
            DMSResponseDto response = new DMSResponseDto();
            using (var context = new DMSDataContext(DMSUser, DMSDomain,DMSPassword))
            {
                response = context.GetDMSResponseInformation(policyNumber, quoteID, parms.Server, parms.Database, parms.DataQueueLibrary, activity);
            }
            return response;
        }

    }
}