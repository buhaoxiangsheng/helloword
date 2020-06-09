using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ufida.T.EAP.Net;

namespace zySoft
{
  public class insertvouchar
    {
        string appkey = "6f9c6db8-dd7c-4b2a-a520-24c2b64704ef";
        string appsecret = "ukqfda";


        //获取用户token
        public string  getusertokens(string userNames, string passs, string caccNums, string cmServerURLs, string privateKeyPaths )
        {
            // v2 获取token 用户名 密码
       

            string userName = userNames;
            string pass = passs;
            string caccNum = caccNums;
            string privateKeyPath = privateKeyPaths;
            string cmServerURL = cmServerURLs;
            if (string.IsNullOrEmpty(privateKeyPath) || !File.Exists(privateKeyPath))
            {
                MessageBox.Show("请指定私钥路径！");
            }
            var header = new Dictionary<string, object>
            {
                {"appkey", appkey},
                {"orgid",string.Empty},//90009444367
                {"appsecret", appsecret}
            };
            RestSharp.Serializers.JsonSerializer jsonSerializer = new RestSharp.Serializers.JsonSerializer();
            string datas = jsonSerializer.Serialize(header);
            Ufida.T.EAP.Net.security.TokenManage tokenManage = new Ufida.T.EAP.Net.security.TokenManage();

            string signvalue = tokenManage.CreateSignedToken(datas, privateKeyPath);
            string authStr = @"{""appKey"":""" + appkey + @""",""authInfo"":""" + signvalue + @""",""orgId"":""""}";
            string encode = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(authStr));
            string serverUrl = cmServerURL.Trim(); ;
            if (serverUrl.Contains("api/v1"))
            {
                serverUrl = serverUrl.Replace("api/v1", "api/v2");
                cmServerURL  = serverUrl;
            }
            string host = serverUrl.Substring(0, serverUrl.IndexOf('/', serverUrl.IndexOf("//") + 2) + 1);
            TRestClient restclient = new TRestClient(host);
            ITRestRequest restquest = new TRestRequest();
            restquest.Resource = serverUrl.Replace(host, "") + "collaborationapp/GetRealNameTPlusToken?IsFree=1";
            restquest.AddParameter("Authorization", encode, TParameterType.HttpHeader);
            pass = this.EncodeMD5(pass);
            string args = string.Format("{{userName:\"{0}\",password:\"{1}\",accNum:\"{2}\"}}", userName, pass, caccNum);
            restquest.AddParameter("_args", args);
            restquest.Method = TMethod.POST;
            string responsedata = restclient.Execute(restquest);
            Newtonsoft.Json.Linq.JObject token = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(responsedata);
            string token_v2="";

            if (token["access_token"] != null)
            {
                token_v2 = token["access_token"].ToString();
                //txtLog.AppendText("\r\n Call:GetTokenV2 \r\n result:" + token_v2);
                //txtLog.AppendText("\r\n Call:Signed Authorization \r\n result:" + encode);
               // fromOrgId = false;
            }
            else
            {
                MessageBox.Show(responsedata);
                //txtLog.AppendText("\r\n Call:GetTokenV2 \r\n result:" + responsedata);
             
            }
            return token_v2;

        }

        //密码加密与数据库对比
        public string EncodeMD5(string str)
        {
            UTF8Encoding encode = new UTF8Encoding();
            Byte[] hashByte = new byte[] { };

            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            hashByte = md5.ComputeHash(encode.GetBytes(str));

            return Ufida.T.EAP.Aop.Util.SpereTool.ConvertBytesToHex(hashByte, false);
        }
        //插入数据
        public void insertdocument(string usertoken, string privateKeyPaths, string cmServerURLs,string typevouchar,string arg,string typename) 
        {
            string mess = "";
            //v2

            
                string orgid = "";
                //业务请求的Authorization
                var customParas = new Dictionary<string, object>
                {
                    {"access_token", usertoken},

                };
                //默认规则是当前参数+appsecret，组成签名的原值
                var bizheader = new Dictionary<string, object>
                {
                    {"appkey", appkey},
                    {"orgid",string.Empty},
                    {"appsecret", appsecret}
                };
                string privateKeyPath = "J:\\T+OpenApi智赢密钥\\cjet_pri.pem";


                RestSharp.Serializers.JsonSerializer jsonSerializer = new RestSharp.Serializers.JsonSerializer();
                string bizdatas = jsonSerializer.Serialize(bizheader);

                Ufida.T.EAP.Net.security.TokenManage tokenManage = new Ufida.T.EAP.Net.security.TokenManage();
                string bizAuthorization = tokenManage.CreateSignedToken(bizdatas, privateKeyPath, customParas);

                ITRestRequest restquest1 = new TRestRequest();
                restquest1.Method = TMethod.POST;
                string serverUrl = cmServerURLs;
                string host = serverUrl.Substring(0, serverUrl.IndexOf('/', serverUrl.IndexOf("//") + 2) + 1);
                restquest1.Resource = serverUrl.Replace(host, "") + typevouchar;
                string authStr1 = @"{""appKey"":""" + appkey + @""",""authInfo"":""" + bizAuthorization + @""",""orgId"":" + @"""""" + @"}";
                string encode1 = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(authStr1));
                restquest1.AddParameter("Authorization", encode1, TParameterType.HttpHeader);
                string args = arg;

                restquest1.AddParameter("_args", args);
                TRestClient restclient = new TRestClient(host);
                string responsedata1 = restclient.Execute(restquest1);
                mess = responsedata1;
            //txtLog.AppendText("\r\n\r\n call:" + txtResourceName.Text);
            //txtLog.AppendText("\r\n result:" + responsedata1);
            //MessageBox.Show(responsedata1, "生成"+ typename, MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (responsedata1 != ""&& responsedata1!=null && responsedata1 != "null" && !string.IsNullOrEmpty(responsedata1) )
            {
                throw new MyException(responsedata1);
            }
       
        }
    }
    
}
