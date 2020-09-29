using PocSecurityDotNetFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace PocSecurityDotNetFramework.Http
{
    public class SensitiveRijndaelParameterBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            try
            {
                if(!bindingContext.ModelMetadata.IsComplexType)
                {
                    ICipherService cipherService = new RijndaelCipherService();
                    var fieldCryptography = new FieldCryptography(cipherService);

                    var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                    var decrypted = fieldCryptography.Decrypt(value.RawValue.ToString());
                    
                    bindingContext.Model = decrypted;
                }

                return true;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                actionContext.ModelState.AddModelError("SensitiveBinderError", ex);
                return false;
            }
        }
    }
}