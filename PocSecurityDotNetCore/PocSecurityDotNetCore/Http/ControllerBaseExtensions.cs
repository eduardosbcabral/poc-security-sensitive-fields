using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PocSecurityDotNetCore.Services;
using System;
using System.Net;
using System.Net.Http;

namespace PocSecurityDotNetCore.Http
{
    public static class ControllerBaseExtensions
    {
        public static IActionResult OkSensitive<T>(this ControllerBase controllerBase, T value)
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

                return controllerBase.Ok(value);
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
            catch (Exception ex)
            {
                var objMessage = new
                {
                    Message = "Ocorreu um erro no atributo da resposta.",
                    Exception = ex
                };

                return controllerBase.BadRequest(objMessage);
            }
        }
    }
}