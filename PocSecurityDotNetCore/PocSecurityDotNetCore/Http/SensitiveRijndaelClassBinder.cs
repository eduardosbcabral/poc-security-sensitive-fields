using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using PocSecurityDotNetCore.Services;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PocSecurityDotNetCore.Http
{
    public class SensitiveRijndaelClassBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                if(bindingContext.ModelMetadata.IsComplexType)
                {
                    var culture = Thread.CurrentThread.CurrentCulture;
                    using var reader = new StreamReader(bindingContext.HttpContext.Request.Body);
                    var bodyString = await reader.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false);
                    var body = JsonConvert.DeserializeObject(bodyString, bindingContext.ModelType, new JsonSerializerSettings()
                    {
                        Culture = culture
                    });

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
   
                        bindingContext.Result = ModelBindingResult.Success(body);
                    }
                }

                return;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError("SensitiveBinderError", ex.Message);
                bindingContext.Result = ModelBindingResult.Failed();
                return;
            }
        }
    }
}