using Newtonsoft.Json;
using PocSecurityDotNetFramework.Services;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace PocSecurityDotNetFramework.Http
{
    public class SensitiveClassBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            try
            {
                if(bindingContext.ModelMetadata.IsComplexType)
                {
                    var sensitiveHelper = new SensitiveAnnotationHelper();
                    var isSensitiveClass = sensitiveHelper.IsSensitiveType(bindingContext.ModelType);
                    if (isSensitiveClass)
                    {
                        var properties = sensitiveHelper.GetSensitiveProperties(bindingContext.ModelType);

                        foreach(var parameter in bindingContext.ModelMetadata.Properties)
                        {
                        }
                    }
                    //var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                    //var sensitiveHelper = new SensitiveAnnotationHelper();

                    //var isSensitiveClass = sensitiveHelper.IsSensitiveClass(value);

                    //ICipherService cipherService = new RijndaelCipherService();

                    //if (isSensitiveClass)
                    //{
                    //    if (sensitiveHelper.IsList(value))
                    //    {
                    //        var listCryptography = new ListCryptography(cipherService);
                    //        value = listCryptography.Decrypt(value);
                    //    }
                    //    else
                    //    {
                    //        var classCryptography = new ClassCryptography(cipherService);
                    //        value = classCryptography.Decrypt(value);
                    //    }
                    //}
                }
                

                return true;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                var objMessage = new
                {
                    Message = "Ocorreu um erro no parametro enviado."
                };
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(objMessage), Encoding.UTF8, "application/json")
                };
                return false;
            }
        }
    }
}