using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using NewWq.WebUI.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace System.Web.Http
{
    public static class ControllerExtention
    {
        //扩展方法
        public static OkNegotiatedContentResult<ResponseData> ErrorData(this ApiController controller, string error, int code = 500)
        {
            return new OkNegotiatedContentResult<ResponseData>(new ResponseData()
            {
                Code = code,
                ErrorMessage = error
            }, controller);
        }

        public static OkNegotiatedContentResult<ResponseData> SendData(this ApiController controller, object data)
        {
            return new OkNegotiatedContentResult<ResponseData>(new ResponseData()
            {
                Data = data
            }, controller);
        }

    }


    public class JwtTools
    {
        public static string Key { get; set; } = "newwq";
        //加密
        public static string Encoder(Dictionary<string, object> payload, string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = Key;
            }
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            //添加一个jwt时效串
            payload.Add("timeout", DateTime.Now.AddDays(1));

            return encoder.Encode(payload, key);
        }

        //解密
        public static Dictionary<string, object> Decoder(string jwtstr, string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = Key;
            }

            try
            {
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var json = decoder.Decode(jwtstr, key, true);

                //string转化成Dictionary
                //把一个字符串反向生成对应的对象内容
                var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                if ((DateTime)result["timeout"]<DateTime.Now)
                {
                    throw new Exception("jwt已过期，请重新登录");
                }

                result.Remove("tomeout");
                return result;
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
                throw;
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
                throw;
            }
        }
    }
}