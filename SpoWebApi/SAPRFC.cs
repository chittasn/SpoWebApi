using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAP.Middleware.Connector;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;
using System.Security;
using Microsoft.SharePoint.Client;
using System.IO;
using System.Configuration;
using OfficeDevPnP.Core;
using Microsoft.SharePoint;
using AuthenticationManager = OfficeDevPnP.Core.AuthenticationManager;
using System.Threading.Tasks;


namespace SpoWebApi
{

    public class ConnectToSharedFolder : IDisposable
    {
        readonly string _networkName;

        public  ConnectToSharedFolder(string networkName, NetworkCredential credentials)
        {
            _networkName = networkName;

            var netResource = new NetResource
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = networkName
            };

            var userName = string.IsNullOrEmpty(credentials.Domain)
                ? credentials.UserName
                : string.Format(@"{0}\{1}", credentials.Domain, credentials.UserName);

            var result = WNetAddConnection2(
                netResource,
                credentials.Password,
                userName,
                0);

            if (result != 0)
            {
                throw new Win32Exception(result, "Error connecting to remote share");
            }
        }

        ~ConnectToSharedFolder()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            WNetCancelConnection2(_networkName, 0, true);
        }

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NetResource netResource,
            string password, string username, int flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags,
            bool force);

        [StructLayout(LayoutKind.Sequential)]
        public class NetResource
        {
            public ResourceScope Scope;
            public ResourceType ResourceType;
            public ResourceDisplaytype DisplayType;
            public int Usage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }

        public enum ResourceScope : int
        {
            Connected = 1,
            GlobalNetwork,
            Remembered,
            Recent,
            Context
        };

        public enum ResourceType : int
        {
            Any = 0,
            Disk = 1,
            Print = 2,
            Reserved = 8,
        }

        public enum ResourceDisplaytype : int
        {
            Generic = 0x0,
            Domain = 0x01,
            Server = 0x02,
            Share = 0x03,
            File = 0x04,
            Group = 0x05,
            Network = 0x06,
            Root = 0x07,
            Shareadmin = 0x08,
            Directory = 0x09,
            Tree = 0x0a,
            Ndscontainer = 0x0b
        }
    }
    public class SAPRFC
    {
       public  string FilePath { get; set; }
        public string objRTFTOPDF { get; set; }
        /// <summary>
        /// below function get regions.
        /// </summary>
        /// <returns></returns>
        public List<SIISAPRegionDTO> GetRegions()
        {
            List<SIISAPRegionDTO> objRegionsList = new List<SIISAPRegionDTO>();
            var rfcDestination = RfcDestinationManager.GetDestination("SIISAP");

            try
            {
                if (rfcDestination != null)
                {
                    var getARNRfc = rfcDestination.Repository.CreateFunction("ZEHS_MSDS_PRTL_F4_LANG_REG");
                    RfcSessionManager.BeginContext(rfcDestination);
                    getARNRfc.Invoke(rfcDestination);
                    var GetLanDetailsTable = getARNRfc.GetTable("REGIONS");
                    foreach (var row in GetLanDetailsTable)
                    {
                        objRegionsList.Add(new SIISAPRegionDTO
                        {
                            RegionID = row[0].ToString().Substring(row[0].ToString().IndexOf("=") + 1),
                            RegionName = row[1].ToString().Substring(row[1].ToString().IndexOf("=") + 1)
                        });
                        var listinfo = objRegionsList;
                    }
                    RfcSessionManager.EndContext(rfcDestination);
                    rfcDestination = null;
                }
            }
            catch (Exception ex)
            {
                FilePath = ConfigurationManager.AppSettings["siteUrl"];
                WriteLog(FilePath, ex.Message);

            }

            return objRegionsList;
        }

        /// <summary>
        /// Get the Languages table from ZEHS_MSDS_PRTL_F4_LANG_REG
        /// </summary>
        /// <returns></returns>
        public List<SIISAPLanguageDTO> GetLanguages()
        {
            List<SIISAPLanguageDTO> objLanguagesList = new List<SIISAPLanguageDTO>();
            var rfcDestination = RfcDestinationManager.GetDestination("SIISAP");

            try
            {
                if (rfcDestination != null)
                {
                    var getARNRfc = rfcDestination.Repository.CreateFunction("ZEHS_MSDS_PRTL_F4_LANG_REG");
                    RfcSessionManager.BeginContext(rfcDestination);
                    getARNRfc.Invoke(rfcDestination);
                    var GetLanDetailsTable = getARNRfc.GetTable("LANGUAGES");
                    foreach (var row in GetLanDetailsTable)
                    {
                        objLanguagesList.Add(new SIISAPLanguageDTO
                        {
                            LanguageID = row[0].ToString().Substring(row[0].ToString().IndexOf("=") + 1),
                            LanguageName = row[1].ToString().Substring(row[1].ToString().IndexOf("=") + 1)
                        });
                        var listinfo = objLanguagesList;
                    }
                    RfcSessionManager.EndContext(rfcDestination);
                    rfcDestination = null;
                }
            }
            catch (Exception ex)
            {
                FilePath = ConfigurationManager.AppSettings["siteUrl"];
                WriteLog(FilePath, ex.Message);
            }

            return objLanguagesList;
        }

        /// <summary>
        ///  To get the list of Products 
        /// </summary>
        /// 
        public List<SIISAPProdDTO> GetProducts()
        {
            List<SIISAPProdDTO> objProdutsList = new List<SIISAPProdDTO>();
            var rfcDestination = RfcDestinationManager.GetDestination("SIISAP");
            try
            {
                if (rfcDestination != null)
                {
                    var getARNRfc = rfcDestination.Repository.CreateFunction("ZEHS_MSDS_PRTL_F4_PROD");
                    RfcSessionManager.BeginContext(rfcDestination);
                    getARNRfc.Invoke(rfcDestination);
                    var GetProductsTable = getARNRfc.GetTable("PRODS");



                    foreach (var row in GetProductsTable)
                    {
                        objProdutsList.Add(new SIISAPProdDTO { Product = row[0].ToString().Substring(row[0].ToString().IndexOf("=") + 1) });
                        var listinfo = objProdutsList;
                    }
                    RfcSessionManager.EndContext(rfcDestination);
                    rfcDestination = null;
                }
            }
            catch (Exception ex)
            {
                FilePath = ConfigurationManager.AppSettings["siteUrl"];
                WriteLog(FilePath, ex.Message);
            }

            return objProdutsList;
        }

        /// <summary>
        /// Below function gets products information.
        /// </summary>
        /// <param name="Productname"></param>
        /// <param name="RegionID"></param>
        /// <param name="RegionName"></param>
        /// <param name="LanguageId"></param>
        /// <param name="LanguageName"></param>
        /// <param name="Materialno"></param>
        /// <param name="MaxHitcount"></param>
        /// <param name="ViewType"></param>
        /// <returns></returns>
        public List<SIISAPMSDSDTO> GetProductsInformation(string RegionID, string RegionName, string LanguageId, string LanguageName, string Productname, string Materialno, int MaxHitcount, string ViewType)
        {
           
            List<SIISAPMSDSDTO> objMDSDSList = new List<SIISAPMSDSDTO>();
            var rfcDestination = RfcDestinationManager.GetDestination("SIISAP");
            try
            {
                if (rfcDestination != null)
                {

                    var getGateEntryRfc = rfcDestination.Repository.CreateFunction("ZEHS_MSDS_PRTL_F4_LIST_OF_MSDS");
                    /// PRODUCT
                    RfcStructureMetadata metaData = rfcDestination.Repository.GetStructureMetadata("ZEHS_GEN_PRODS");
                    IRfcStructure structProduct = metaData.CreateStructure();
                    structProduct.SetValue("BRAND2", Productname);//, "ALKANOX® 240");
                    getGateEntryRfc.SetValue("I_PROD", structProduct);
                    /// REGION
                    RfcStructureMetadata metaDataCountry = rfcDestination.Repository.GetStructureMetadata("ZEHS_GEN_CNTRY");
                    IRfcStructure structRegions = metaDataCountry.CreateStructure();
                    structRegions.SetValue("LDEPID", RegionID);// "SDS_US");
                    structRegions.SetValue("LDEPNAM", RegionName);// "");
                    getGateEntryRfc.SetValue("I_REGION", structRegions);
                    // LANGUAGE
                    RfcStructureMetadata metaDataLanguage = rfcDestination.Repository.GetStructureMetadata("ZEHS_GEN_LNGS");
                    IRfcStructure strucLanguage = metaDataLanguage.CreateStructure();
                    strucLanguage.SetValue("SPRAS", LanguageId);// "E");
                    strucLanguage.SetValue("SPTXT", LanguageName);// "");
                    getGateEntryRfc.SetValue("I_LANGUAGE", strucLanguage);
                    /// MATERIAL NUMBER
                    getGateEntryRfc.SetValue("I_MATNR", Materialno);// "*");
                    /// MAX HIT COUNT
                    getGateEntryRfc.SetValue("I_MAX_HIT_COUNT", MaxHitcount);// 300);
                    /// I VIEW TYPE
                    getGateEntryRfc.SetValue("I_VIEWTYPE", ViewType);// "");
                    // RfcSessionManager.BeginContext(rfcDestination);
                    getGateEntryRfc.Invoke(rfcDestination);
                    /// get the table values 
                    IRfcTable Report = getGateEntryRfc.GetTable("E_REPORT_TAB");
                    IRfcTable ObjectReport = getGateEntryRfc.GetTable("E_REPORT_OBJECT_TAB");
                    //Getting exported values from SAP
                    var COUNT = getGateEntryRfc.GetValue("E_COUNT");
                    var MaxHITCount = getGateEntryRfc.GetValue("E_FLG_MAX_HIT_REACHED");
                    var objRTFT = getGateEntryRfc.GetValue("E_BRAND2").ToString().Substring(getGateEntryRfc.GetValue("E_BRAND2").ToString().LastIndexOf("=") + 1).Replace("}", "");
                   
                    RfcSessionManager.EndContext(rfcDestination);
                    foreach (var row in Report)
                    {

                        objMDSDSList.Add(new SIISAPMSDSDTO
                        {
                            ID = row[0].ToString().Substring(row[0].ToString().IndexOf("=") + 1),
                            RECN = row[1].ToString().Substring(row[1].ToString().IndexOf("=") + 1),
                            LANGU = row[2].ToString().Substring(row[2].ToString().IndexOf("=") + 1),
                            LANGUTXT = row[3].ToString().Substring(row[3].ToString().IndexOf("=") + 1),
                            LanguageID = row[4].ToString().Substring(row[4].ToString().IndexOf("=") + 1),
                            LanguageText = row[5].ToString().Substring(row[5].ToString().IndexOf("=") + 1),
                            Version = row[6].ToString().Substring(row[6].ToString().IndexOf("=") + 1),
                            REPTYPE = row[7].ToString().Substring(row[7].ToString().IndexOf("=") + 1),
                            REPTYPETEXT = row[8].ToString().Substring(row[8].ToString().IndexOf("=") + 1),
                            RVLID = row[9].ToString().Substring(row[9].ToString().IndexOf("=") + 1),
                            RVLIDTXT = row[10].ToString().Substring(row[10].ToString().IndexOf("=") + 1),
                            STATUS = row[11].ToString().Substring(row[11].ToString().IndexOf("=") + 1),
                            STATUSTXT = row[12].ToString().Substring(row[12].ToString().IndexOf("=") + 1),
                            GENDAT = row[13].ToString().Substring(row[13].ToString().IndexOf("=") + 1),
                            VALDAT = row[14].ToString().Substring(row[14].ToString().IndexOf("=") + 1),
                            REMARK = row[15].ToString().Substring(row[15].ToString().IndexOf("=") + 1),
                            PrdFileName= objRTFT
                        });
                        var listinfo = objMDSDSList;
                    }

                    rfcDestination = null;
                }
            }
            catch (Exception ex)
            {
                RfcSessionManager.EndContext(rfcDestination);
                rfcDestination = null;
                FilePath = ConfigurationManager.AppSettings["siteUrl"];
                WriteLog(FilePath, ex.Message);
            }

            return objMDSDSList;
        }
        public static string networkId = ConfigurationManager.AppSettings["NetworkPathUerID"];
        public static string netWorkPwd = ConfigurationManager.AppSettings["NetworkPathPassword"];
        public string networkPath = ConfigurationManager.AppSettings["NetworkPath"];// @"\\52.73.108.38\msds_docs\ECQ";
        NetworkCredential credentials = new NetworkCredential(networkId, netWorkPwd);
        public string myNetworkPath = string.Empty;

        /// <summary>
        /// DownloadFileByte
        /// </summary>
        /// <param name="DownloadURL"></param>
        /// <returns></returns>
        public byte[] DownloadFileByte(string DownloadURL)
        {
            byte[] fileBytes = null;

            
            using (new ConnectToSharedFolder(networkPath, credentials))
            {
                var fileList = Directory.GetDirectories(networkPath);

                foreach (var item in fileList) { if (item.Contains("ClientDocuments")) { myNetworkPath = item; } }

                myNetworkPath = myNetworkPath + DownloadURL;

                try
                {
                    fileBytes = System.IO.File.ReadAllBytes(myNetworkPath);
                }
                catch (Exception ex)
                {
                    string Message = ex.Message.ToString();
                    WriteLog(FilePath, ex.Message);
                }
            }

            return fileBytes;
        }
        /// <summary>
        /// SaveBytesToFile
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="bytesToWrite"></param>
        public string SaveBytesToFile(string filename, byte[] bytesToWrite)
        {
            try
            { 
            if (filename != null && filename.Length > 0 && bytesToWrite != null)
            {
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));

                FileStream file = System.IO.File.Create(filename);

                file.Write(bytesToWrite, 0, bytesToWrite.Length);

                file.Close();
            }
        }
            catch(Exception ex)
            {
                FilePath = ConfigurationManager.AppSettings["ErrorFilePath"];
                WriteLog(FilePath, ex.Message);

            }
            return filename;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaterialNo"></param>
        /// <param name="ProductName"></param>
        /// <returns></returns>
        public string RTFtoPDFFile(string MaterialNo, string ProductName, string strRegion, string strLanguageName,  string strVersion, string rtfFileName)
        {
           
            var rfcRTFtoPDFFile = RfcDestinationManager.GetDestination("SIISAP");
            string filePath = string.Empty;
            string strFilePathURL = string.Empty;
            var GetFile = string.Empty;
            try
            {
                if (rfcRTFtoPDFFile != null)
                {
                    var getGateEntryRfc = rfcRTFtoPDFFile.Repository.CreateFunction("ZEHS_MSDS_PRTL_TRAN_RTF_FILE");
                    /// PRODUCT
                    getGateEntryRfc.SetValue("I_RECN", MaterialNo);// 10415919);
                    getGateEntryRfc.SetValue("I_BRAND2", ProductName); // "ALKANOX® 240");
                    getGateEntryRfc.Invoke(rfcRTFtoPDFFile);
                     GetFile = getGateEntryRfc.GetValue("E_FILE").ToString();
                    IRfcTable tblValueFile = getGateEntryRfc.GetTable("E_VALUEFILE_TAB");
                    IRfcTable ObjectRTFDocument = getGateEntryRfc.GetTable("E_DOCUMENT_TAB");
                    RfcSessionManager.EndContext(rfcRTFtoPDFFile);
                    var SiteUrl = ConfigurationManager.AppSettings["siteUrl"];
                    var fileDirectory = ConfigurationManager.AppSettings["fileDirectory"] + GetFile;
                    var libraryName = ConfigurationManager.AppSettings["strlibraryName"];
                   
                    var strDestinationPath=string.Empty;

                    string strFileForMaterialNo = rtfFileName.Replace(" ", "_");
                    strDestinationPath = HttpContext.Current.Server.MapPath("~/SharePoint") + "\\" + strFileForMaterialNo.Replace("/", "_") + "_" + strRegion + "_" + strLanguageName + "_" + strVersion + ".rtf";

                    //// Below function "DownloadFileByte" to download file from shared location and convert them bytes.
                    byte[] readFile = DownloadFileByte(fileDirectory);

                    ////Below function to "SaveBytesToFile" to convert the bytes to file with product name 
                    string strfielName = SaveBytesToFile(strDestinationPath, readFile);
                    strFilePathURL = strfielName;

                    rfcRTFtoPDFFile = null;
                }
            }
            catch (Exception ex)
            {
                RfcSessionManager.EndContext(rfcRTFtoPDFFile);
                rfcRTFtoPDFFile = null;
                FilePath = ConfigurationManager.AppSettings["ErrorFilePath"];
                WriteLog(FilePath, ex.Message);
            }
            return strFilePathURL;
        }

        /// <summary>
        ///  below method log error information.
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>

        public  bool WriteLog(string strFileName, string strMessage)
        {
            try
            {
                FileStream objFilestream = new FileStream(strFileName, FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(strMessage);
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public  class SIIErrorMessage
    { 
    public string ErrorMessage { get; set; }
    public string FilePath { get; set; }
    }
    public class SIISAPMSDSDTO
    {
        public string ID { get; set; }
        //    RECN
        public string RECN { get; set; }
        //    LANGU
        public string LANGU { get; set; }
        //  LANGUTXT
        public string LANGUTXT { get; set; }
        //  SBGVID
        public string LanguageID { get; set; }
        //  SBGVTXT
        public string LanguageText { get; set; }
        //  VERSION
        public string Version { get; set; }
        //  REPTYPE
        public string REPTYPE { get; set; }
        //  REPTYPETXT
        public string REPTYPETEXT { get; set; }
        //  RVLID
        public string RVLID { get; set; }
        //  RVLIDTXT
        public string RVLIDTXT { get; set; }
        //  STATUS
        public string STATUS { get; set; }
        //  STATUSTXT
        public string STATUSTXT { get; set; }
        //  GENDAT
        public string GENDAT { get; set; }
        //  VALDAT
        public string VALDAT { get; set; }
        //  REMARK
        public string REMARK { get; set; }
        //ProductFileName 
        public string PrdFileName { get; set; }

    }

    public class SIISAPRegionDTO
    {

        // regionID --> LDEPID
        // RegionName --> LDEPNAM

        public string RegionID { get; set; }

        public string RegionName { get; set; }

    }

    public class SIISAPLanguageDTO
    {
        // languageID--> SPRAS
        //LanguageName --> SPTXT

        public string LanguageID { get; set; }

        public string LanguageName { get; set; }

    }
    public class SIISAPProdDTO
    {
        // Product --> BRAND2
        public string Product { get; set; }
    }
    public class SIISAPRTFToPDFDTO
    {
        public string RTFFileName { get; set; }
    }
}