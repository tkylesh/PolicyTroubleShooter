using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using BTS.PolicyCycleAPICommon.Dto;
using BTS.PolicyCycleAPIDAL.PolicyCycleTableAdapters;
using IBM.Data.DB2.iSeries;
using cwbx;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;
using System.IO;
using Newtonsoft.Json;
using log4net;


namespace BTS.PolicyCycleAPIDAL
{
    public partial class AS400DataContext
    {

        static string AS400ConnectionString
        {
            get
            {
                var server = ConfigurationManager.AppSettings["AS400Server"].ToString();
                var initialcatalog = ConfigurationManager.AppSettings["Database"].ToString();
                var librarylist = ConfigurationManager.AppSettings["LibraryList"].ToString();
                var transactionLibrary = ConfigurationManager.AppSettings["TransactionLibrary"].ToString();
                var userID = ConfigurationManager.AppSettings["UserID"].ToString();
                var password = BTS.PolicyCycleAPICommon.Extensions.Decrypt(ConfigurationManager.AppSettings["Password"].ToString());
                var con = ConfigurationManager.ConnectionStrings["AS400ConnectionString"].ConnectionString;
                con = BTS.PolicyCycleAPICommon.Extensions.Decrypt(con);
                return string.Format(con, server, librarylist, transactionLibrary);
            }
        }


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
        };



        static string UserID
        {
            get
            {
                return ConfigurationManager.AppSettings["UserID"].ToString();

            }
        } 


        static string Password
        {
            get
            {
                return BTS.PolicyCycleAPICommon.Extensions.Decrypt(ConfigurationManager.AppSettings["Password"].ToString());
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


        static int TimeOutInSeconds
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["TimeOutInSeconds"].ToString());

            }
        }

        public ResponseDto GetResponseInformation(string quoteID, string companyNumber, string policyPrefix, int policyNumber, string entryDate)
        {
            ResponseDto response = new ResponseDto();

            String policyCycleSql = " Call Sp_Policy_Processor(@queueData, @errorCode, @errorMessage)";

            StringBuilder sb = new StringBuilder();
            sb.Append(' ', 189);
            sb.Append(String.Format("{0,-2}", companyNumber));
            sb.Append(String.Format("{0,-4}", policyPrefix));
            //sb.Append(String.Format("{0,-9}", policyNumber.ToString()));
            sb.Append(policyNumber.ToString("000000000"));
            String trandate = DateTime.Parse(entryDate).ToString("yyyy-MM-dd");
            sb.Append(trandate);
            sb.Append(String.Format("{0,-11}", quoteID));
            Guid g = Guid.NewGuid();
            sb.Append(String.Format("{0,-36}", g.ToString()));

            try
            {
                using (var iSeriesCon = new iDB2Connection(AS400ConnectionString))
                {
                    if (iSeriesCon != null && iSeriesCon.State == ConnectionState.Closed)
                    {
                        iSeriesCon.Open();
                    }

                    using (var cmd = new iDB2Command(policyCycleSql, iSeriesCon))
                    {
                        cmd.Parameters.Add("@queueData", iDB2DbType.iDB2VarChar);
                        cmd.Parameters["@queueData"].Value = String.Format("{0,-271}", sb);
                        cmd.Parameters["@queueData"].Direction = ParameterDirection.Input;

                        cmd.Parameters.Add("@errorCode", iDB2DbType.iDB2VarChar);
                        cmd.Parameters["@errorCode"].Value = "";
                        cmd.Parameters["@errorCode"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("@errorMessage", iDB2DbType.iDB2VarChar);
                        cmd.Parameters["@errorMessage"].Value = "";
                        cmd.Parameters["@errorMessage"].Direction = ParameterDirection.Output;


                        cmd.ExecuteNonQuery();
                        response.PolicyIssuanceStatus = cmd.Parameters["@errorCode"].Value.ToString();
                        response.PolicyIssuanceFailureMessage = cmd.Parameters["@errorMessage"].Value.ToString();

                        cmd.Connection.Close();
                        cmd.Dispose();

                    }

                    iSeriesCon.Close();
                    iSeriesCon.Dispose();

                }
            }
            catch (Exception ex)
            {
                log.Error($"PolicyCycleAPI FAILED on: PolicyNumber[{policyNumber}]", ex);
            }

            return response;
        }
    }


 /*   public ResponseDto GetResponseInformation(string quoteID, string companyNumber, string policyPrefix, int policyNumber, string entryDate)
    {
        ResponseDto response = new ResponseDto();
        StringConverter sc = new StringConverter();
        string dqrecord;
        StringBuilder sb = new StringBuilder();
        sb.Append(' ', 189);
        sb.Append(String.Format("{0,-2}", companyNumber.ToString()));
        sb.Append(String.Format("{0,-4}", policyPrefix.ToString()));
        //sb.Append(String.Format("{0,-9}", policyNumber.ToString()));
        sb.Append(policyNumber.ToString("000000000"));
        String trandate = DateTime.Parse(entryDate).ToString("yyyy-MM-dd");
        sb.Append(trandate);
        sb.Append(String.Format("{0,-11}", quoteID.ToString()));
        Guid g = Guid.NewGuid();
        sb.Append(String.Format("{0,-36}", g.ToString()));
        dqrecord = sb.ToString();

        var dq = new DataQueue();
        dq.LibraryName = parms.DataQueueLibrary;
        dq.QueueName = parms.DataQueueIn;
        dq.system = AS400Connection;
        KeyedDataQueue dqk = new KeyedDataQueue();
        dqk.LibraryName = parms.DataQueueLibrary;
        dqk.QueueName = parms.DataQueueOut;
        dqk.system = AS400Connection;

        if (dq.system.HasSignedOn)
        {
            object dqdata = sc.ToBytes(dqrecord);

            //Write data queue record
            try
            {
                dq.Write(dqdata);
                //a4.Disconnect(cwbcoServiceEnum.cwbcoServiceAll);

                // Receive data back from keyed data queue
                object queuekey = sc.ToBytes(g.ToString());
                int timeOut = TimeOutInSeconds;

                object senderInfo;

                dynamic qdata = dqk.Read(ref queuekey, cwbdqSearchOrderEnum.cwbdqEqual, timeOut, out senderInfo);

                if (sc.FromBytes(qdata).Contains("Success")) response.PolicyIssuanceStatus = "Success";
                else
                {
                    response.PolicyIssuanceStatus = "Failure";
                    response.PolicyIssuanceFailureMessage = sc.FromBytes(qdata);
                }
            }
            catch
            {
                response.PolicyIssuanceStatus = "Failure";
                foreach (Error cwbxError in dqk.Errors)
                {
                    string txt = cwbxError.Text;
                    string err = cwbxError.ToString();
                    response.PolicyIssuanceFailureMessage = txt;
                }
            }
        }

        return response;
    }
}   */

public partial class DA400DataContext
    {
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

        public PolicyMasterDto GetPolicyMasterInformation(int policyNumber)
        {
            PolicyMasterDto result = new PolicyMasterDto();

            //log.Debug("In GetPolicyMasterInformation");

            using (var context = new DA400DataContext(DA400ConnectionString, TransactionLibrary, TableLibrary))
            {
                using (var ta = new PolicyMasterTableAdapter())
                {
                    try
                    {
                        ta.Connection = context.DA400Connection;
                        ta.SetLibrary(TransactionLibrary);

                        var rows = ta.GetPolicyMasterData(policyNumber).ToArray();
                        if (rows.Count() > 0)
                        {
                            result = AutoMapper.Mapper.Map<PolicyMasterDto>(rows[rows.Count() - 1]);
                        }

                        ta.Connection.Close();
                    }
                    catch (Exception ex)
                    {
                        //log.Error(ex.Message, ex);
                        throw;
                    }
                }

                context.DA400Connection.Close();
            }

            return result;
        }




        internal void ConfigurePolicyMasterMap()
        {
            AutoMapper.Mapper.CreateMap<PolicyCycle.PolicyMasterRow, PolicyMasterDto>();
        }

}



public class AutoActivate
    {

        static string AutoActivationURL
        {
            get
            {
                return ConfigurationManager.AppSettings["AutoActivationURL"].ToString();

            }
        }
        static string IgnoreCertificateError
        {
            get
            {
                return ConfigurationManager.AppSettings["IgnoreCertificateError"].ToString();

            }
        }

        static int TimeOutInSeconds
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["TimeOutInSeconds"].ToString());

            }
        }


        public AutoActivation GetAutoActivationResponse(int policyNumber)
        {
            WebClient webClient;
            AutoActivation activate = new AutoActivation();
            Uri address = new Uri(AutoActivationURL + policyNumber.ToString("000000000"));

            using (webClient = new WebClientWithTimeout())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (IgnoreCertificateError.Equals("true"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                }
                var response = webClient.DownloadString(address);
                activate = JsonConvert.DeserializeObject<AutoActivation>(response.ToString());
            }


            return activate;
        }
    }


    public class WebClientWithTimeout : WebClient
    {
        static int TimeOutInMilliSeconds
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["TimeOutInSeconds"].ToString()) * 1000;

            }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest wr = base.GetWebRequest(address);
            wr.Timeout = TimeOutInMilliSeconds;
            return wr;
        }
    }


    public partial class DMSDataContext
    {
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

        public DMSResponseDto GetDMSResponseInformation(int policyNumber, string quoteID, string server, string database, string library, string activity)
        {

            DMSResponseDto response = new DMSResponseDto();
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Guid fileName = Guid.NewGuid();
            string Transaction;
            string first3 = "";
            string environment = "";
            int len = 0;

            try
            {
                    len = library.Length;
            }
            catch (Exception)
            {
            }


            if(len >= 3)
            {
                first3 = library.Substring(0, 3);
            }

            if (len >= 7) environment = library.Substring(6, len - 6);

            if (!first3.Equals("TST")) environment = "PROD";
            switch(activity)
            {
                case "N":
                    Transaction = "New Business";
                    break;
                case "E":
                    Transaction = "Endorsement";
                    break;
                case "C":
                    Transaction = "Pending Cancel";
                    break;
                case "R":
                    Transaction = "Reinstate";
                    break;
                case "P":
                    Transaction = "Pending Renewal";
                    break;
                default:
                    Transaction = "";
                    break;
            }

            try
            {

            StreamWriter sw;
                FileIOPermission myPerm = new FileIOPermission(FileIOPermissionAccess.AllAccess, DMSFilePath + fileName);
                myPerm.Assert();
                sw = File.CreateText(DMSFilePath + fileName);
                sw.Close();
                sw = File.AppendText(DMSFilePath + fileName);
                sw.WriteLine(policyNumber.ToString() + "," + quoteID + "," + server + "," + database + "," + environment + "," + Transaction);
                sw.Close();
            }
            catch (Exception ex)
            {
                log.Error($"Error writing to DMS document file for policy {policyNumber.ToString()}: {ex.Message}  with PolicyCycleAPI", ex);
            }

            return response;
        }
    }

}
