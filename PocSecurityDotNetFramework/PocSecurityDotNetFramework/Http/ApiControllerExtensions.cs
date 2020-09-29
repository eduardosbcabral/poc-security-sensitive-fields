using PocSecurityDotNetFramework.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace PocSecurityDotNetFramework.Http
{
    public static class ApiControllerExtensions
    {
        public static IHttpActionResult OkSensitive<T>(this ApiController apiController, T value)
        {
            try
            {
                var sensitiveHelper = new SensitiveAnnotationHelper();
                ICipherService cipherService = new RijndaelCipherService();

                var isSensitiveClass = sensitiveHelper.IsSensitiveClass(value);

                if(isSensitiveClass)
                {
                    if(sensitiveHelper.IsList(value))
                    {
                        var listCryptography = new ListCryptography(cipherService);
                        value = listCryptography.Encrypt(value);
                    }
                    else
                    {
                        var classCryptography = new ClassCryptography(cipherService);
                        value = classCryptography.Encrypt(value);
                    }
                }

                return new ResponseMessageResult(
                    apiController.Request.CreateResponse(HttpStatusCode.OK, value)
                );
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch(SensitiveClassNullException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                var objMessage = new
                {
                    Message = "Ocorreu um erro no atributo da resposta."
                };
                return new ResponseMessageResult(
                    apiController.Request.CreateResponse(HttpStatusCode.BadRequest, objMessage)
                );
            }
        }
    }
}