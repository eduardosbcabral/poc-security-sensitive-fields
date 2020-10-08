using Microsoft.AspNetCore.Mvc.ModelBinding;
using PocSecurityDotNetCore.Services;
using System;
using System.Threading.Tasks;

namespace PocSecurityDotNetCore.Http
{
    public class SensitiveRijndaelParameterBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                if (!bindingContext.ModelMetadata.IsComplexType)
                {
                    ICipherService cipherService = new RijndaelCipherService();
                    var fieldCryptography = new FieldCryptography(cipherService);

                    var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                    var decrypted = fieldCryptography.Decrypt(value.ToString());

                    bindingContext.Result = ModelBindingResult.Success(decrypted);
                }

                return Task.CompletedTask;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch(FormatException)
            {
                throw new FormatException("O formato do parâmetro deve ser base64.");
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError("SensitiveBinderError", ex.Message);
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
        }
    }
}