using Newtonsoft.Json;
using PocSecurityDotNetFramework.Services;
using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace PocSecurityDotNetFramework.Http
{
    public class SensitiveRijndaelClassBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            try
            {
                if(bindingContext.ModelMetadata.IsComplexType)
                {
                    var bodyString = actionContext.Request.Content.ReadAsStringAsync().Result;
                    var body = JsonConvert.DeserializeObject(bodyString, bindingContext.ModelType);

                    var sensitiveHelper = new SensitiveAnnotationHelper();
                    var isSensitiveClass = sensitiveHelper.IsSensitiveClass(body);
                    if (isSensitiveClass)
                    {
                        ICipherService cipherService = new RijndaelCipherService();

                        var isList = sensitiveHelper.IsList(body);

                        if (isList)
                        {
                            var listCryptography = new ListCryptography(cipherService);
                            body = listCryptography.Decrypt(body);
                        }
                        else
                        {
                            var classCryptography = new ClassCryptography(cipherService);
                            body = classCryptography.Decrypt(body);
                        }
   
                        bindingContext.Model = body;
                    }
                }

                return true;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                actionContext.ModelState.AddModelError("SensitiveBinderError", "");
                return false;
            }
        }
    }
}