using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.SharePoint.Client;
using System.IO;

namespace SpoWebApi.Controllers
{
    public class TestController : ApiController
    {
        /// <summary>
        /// Got Get userInformation
        /// </summary>
        /// <param name="sharePointUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/test/UserInfo")]
        public async Task<IEnumerable<string>> GetUserInfo(string sharePointUrl)
        {
            var results = new List<string>();

            try
            {
                var userToken = this.Request.Headers.Authorization.Parameter;
                var newToken = await GetSharePointAccessToken(sharePointUrl, userToken);

                using (var context = new ClientContext(sharePointUrl))
                {
                    context.ExecutingWebRequest +=
                        (s, e) => e.WebRequestExecutor.WebRequest.Headers.Add(
                        "Authorization", "Bearer " + newToken);

                    var web = context.Web;
                    var user = web.CurrentUser;
                    context.Load(user);
                    context.ExecuteQuery();

                    results.Add(user.Title);
                    results.Add(user.LoginName);
                    results.Add(user.Email);
                }
            }
            catch (Exception ex)
            {
                results.Add(ex.ToString());
                string strErroFilePath = ConfigurationManager.AppSettings["ErrorFilePath"];
                WriteLog(strErroFilePath, ex.Message);
            }

            return results;
        }
        /// <summary>
        ///  SAPLanguageInformation
        /// </summary>
        /// <param name="sharePointUrl"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("api/test/SAPLanguages")]
        public async Task<List<SIISAPLanguageDTO>> GetSAPLanguages(string sharePointUrl)
        {
            SAPRFC objSAPRFCLanguage = new SAPRFC();
            var results = new List<SIISAPLanguageDTO>();

            try
            {

                var userToken = this.Request.Headers.Authorization.Parameter;
                var newToken = await GetSharePointAccessToken(sharePointUrl, userToken);

                using (var context = new ClientContext(sharePointUrl))
                {
                    results = objSAPRFCLanguage.GetLanguages();
                    context.ExecutingWebRequest +=
                        (s, e) => e.WebRequestExecutor.WebRequest.Headers.Add(
                        "Authorization", "Bearer " + newToken);

                    var web = context.Web;
                    var user = web.CurrentUser;
                    context.Load(user);
                    context.ExecuteQuery();


                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
                string strErroFilePath = ConfigurationManager.AppSettings["ErrorFilePath"];
                WriteLog(strErroFilePath, ex.Message);
            }

            return results;
        }
        /// <summary>
        /// SAP Regiionsinformation.
        /// </summary>
        /// <param name="sharePointUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/test/SAPRegions")]
        public async Task<List<SIISAPRegionDTO>> GetSAPRegions(string sharePointUrl)
        {
            SAPRFC objSAPRFCRegions = new SAPRFC();
            var results = new List<SIISAPRegionDTO>();

            try
            {

                var userToken = this.Request.Headers.Authorization.Parameter;
                var newToken = await GetSharePointAccessToken(sharePointUrl, userToken);

                using (var context = new ClientContext(sharePointUrl))
                {
                    results = objSAPRFCRegions.GetRegions();
                    context.ExecutingWebRequest +=
                        (s, e) => e.WebRequestExecutor.WebRequest.Headers.Add(
                        "Authorization", "Bearer " + newToken);

                    var web = context.Web;
                    var user = web.CurrentUser;
                    context.Load(user);
                    context.ExecuteQuery();


                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                string strErroFilePath = ConfigurationManager.AppSettings["ErrorFilePath"];
                WriteLog(strErroFilePath, ex.Message);
            }

            return results;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sharePointUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/test/SAPProducts")]
        public async Task<List<SIISAPProdDTO>> GetSAPProducts(string sharePointUrl)
        {
            SAPRFC objSAPRFCProducts = new SAPRFC();
            var results = new List<SIISAPProdDTO>();

            try
            {

                var userToken = this.Request.Headers.Authorization.Parameter;
                var newToken = await GetSharePointAccessToken(sharePointUrl, userToken);

                using (var context = new ClientContext(sharePointUrl))
                {
                    results = objSAPRFCProducts.GetProducts();
                    context.ExecutingWebRequest +=
                        (s, e) => e.WebRequestExecutor.WebRequest.Headers.Add(
                        "Authorization", "Bearer " + newToken);

                    var web = context.Web;
                    var user = web.CurrentUser;
                    context.Load(user);
                    context.ExecuteQuery();


                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
                string strErroFilePath = ConfigurationManager.AppSettings["ErrorFilePath"];
                WriteLog(strErroFilePath, ex.Message);
            }

            return results;
        }
        /// <summary>
        /// below function used to get production information 
        /// </summary>
        /// <param name="sharePointUrl"></param>
        /// <param name="RegionID"></param>
        /// <param name="RegionName"></param>
        /// <param name="LanguageId"></param>
        /// <param name="LanguageName"></param>
        /// <param name="Productname"></param>
        /// <param name="Materialno"></param>
        /// <param name="MaxHitcount"></param>
        /// <param name="View"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/test/SAPProductsInfo")]
        public async Task<List<SIISAPMSDSDTO>> GetSAPProductsInfo(string sharePointUrl, string RegionID, string RegionName, string LanguageId, string LanguageName, string Productname, string Materialno, int MaxHitcount, string View)
        {
            SAPRFC SISAPMSDS = new SAPRFC();
            var results = new List<SIISAPMSDSDTO>();

            try
            {
                var userToken = this.Request.Headers.Authorization.Parameter;
                var newToken = await GetSharePointAccessToken(sharePointUrl, userToken);

                using (var context = new ClientContext(sharePointUrl))
                {
                    results = SISAPMSDS.GetProductsInformation(RegionID, RegionName, LanguageId, LanguageName, Productname, Materialno, MaxHitcount, View);
                    context.ExecutingWebRequest +=
                        (s, e) => e.WebRequestExecutor.WebRequest.Headers.Add(
                        "Authorization", "Bearer " + newToken);

                    var web = context.Web;
                    var user = web.CurrentUser;
                    context.Load(user);
                    context.ExecuteQuery();
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
                string strErroFilePath = ConfigurationManager.AppSettings["ErrorFilePath"];
                WriteLog(strErroFilePath, ex.Message);
            }
            return results;
        }

        [HttpGet]
        [Route("api/test/SAPRTFFile")]
        public async Task<string> GetSAPRTFFile(string sharePointUrl, string MaterialNo, string ProductName, string strRegion, string strLanguageName, string strVersion, string rtfFileName)
        {
            SAPRFC SISAPMSDSRTFFile = new SAPRFC();
            var results = string.Empty;

            try
            {
                var userToken = this.Request.Headers.Authorization.Parameter;
                var newToken = await GetSharePointAccessToken(sharePointUrl, userToken);

                using (var context = new ClientContext(sharePointUrl))
                {
                    results = SISAPMSDSRTFFile.RTFtoPDFFile(MaterialNo, ProductName, strRegion, strLanguageName, strVersion, rtfFileName);
                    context.ExecutingWebRequest +=
                        (s, e) => e.WebRequestExecutor.WebRequest.Headers.Add(
                        "Authorization", "Bearer " + newToken);

                    var web = context.Web;
                    var user = web.CurrentUser;

                    context.Load(user);
                    context.ExecuteQuery();

                    // Below code to save file in document library
                    using (
                        FileStream fileStream = new FileStream(results, FileMode.Open))
                    {
                        FileCreationInformation createFile = new FileCreationInformation();
                        createFile.Url = results.Substring(results.LastIndexOf("\\") + 1).ToString();

                        createFile.ContentStream = fileStream;
                        createFile.Overwrite = true;

                        List spList = context.Web.Lists.GetByTitle("Documents");
                        context.Load(spList.RootFolder);
                        context.ExecuteQuery();
                        Microsoft.SharePoint.Client.File addedFile = spList.RootFolder.Files.Add(createFile);
                        context.Load(addedFile);
                        context.ExecuteQuery();
                        // Below code update the RequestorName and Requestor fields
                        ListItem item = addedFile.ListItemAllFields;
                        item["Requestor"] = user.Title;
                        var assignedToValue = new FieldUserValue() { LookupId = user.Id };
                        var assignedToValues = new[] { assignedToValue };
                        item["RequestorName"] = assignedToValues;
                        item.Update();
                        context.Load(item);
                        context.ExecuteQuery();
                    }
                    System.IO.File.Delete(results);
                    var tskFile = Task.Run(async delegate
                    {
                        await Task.Delay(10000);
                        results = ConfigurationManager.AppSettings["documentLibraryPath"];
                        return results;
                    });
                    tskFile.Wait();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                string strErroFilePath = ConfigurationManager.AppSettings["ErrorFilePath"];
                WriteLog(strErroFilePath, ex.Message);
            }
            return results;
        }

        /// <summary>
        ///  below functionto Get sharepoint access token from Azure AD--> AppRegistration.
        /// </summary>
        /// <param name="sharePointUrl"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        internal async static Task<string> GetSharePointAccessToken(string sharePointUrl, string accessToken)
        {
            var clientID = ConfigurationManager.AppSettings["ClientID"];
            var clientSecret = ConfigurationManager.AppSettings["ClientSecret"];
            var tenant = ConfigurationManager.AppSettings["Tenant"];

            var appCred = new ClientCredential(clientID, clientSecret);
            var authContext = new AuthenticationContext(
                "https://login.microsoftonline.com/" + tenant);

            var resource = new Uri(sharePointUrl).GetLeftPart(UriPartial.Authority);
            var authResult = await authContext.AcquireTokenAsync(resource, appCred,
                  new UserAssertion(accessToken));
            return authResult.AccessToken;
        }

        [HttpGet]
        [Route("api/test/GetSAPGetRegionsCopy")]
        public IHttpActionResult GetSAPGetRegionsCopy()
        {
            SAPRFC objSAPRFC1 = new SAPRFC();
            try
            { 
                return Json(objSAPRFC1.GetRegions());
            }
            catch(Exception ex)
            {
               
                string strErroFilePath = ConfigurationManager.AppSettings["ErrorFilePath"];
                WriteLog(strErroFilePath, ex.Message);
                return Json(objSAPRFC1.GetRegions());
            }
        }

        [HttpGet]
        [Route("api/test/GetTestHello")]
        public IHttpActionResult TestHello(string strhello)
        {
            return Ok(strhello);
           
        }

        public bool WriteLog(string strFileName, string strMessage)
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
}
