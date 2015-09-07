using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;
using DevMvcComponent.Error;
using Newtonsoft.Json;
using WereViewApp.Modules.Message;

namespace WereViewApp.Modules.Validations {
    public abstract class Validator {
        protected ErrorCollector ErrorCollector;
        protected delegate bool RunValidation();
        protected List<RunValidation> ValidationCollection;

        private const string SuccessValidationMessage = "Validation successful.";
        private const string FailedValidationMessage = "Validation is not successful. Please check the question-mark icon for more information on validation.";
        private static string ValidationExceedJson;

        /// <summary>
        /// On initialization CollectValidation() is called to collect all the validations.
        /// </summary>
        /// <param name="errorCollector"></param>
        /// <param name="capacity"></param>
        protected Validator(ErrorCollector errorCollector = null, int capacity = 25) {
            if (errorCollector == null) {
                errorCollector = AppConfig.GetNewErrorCollector();
            }
            ErrorCollector = errorCollector;
            ValidationCollection = new List<RunValidation>(capacity);

            CollectValidation();
        }

        protected void AddValidation(RunValidation validation) {
            ValidationCollection.Add(validation);
        }

        protected void ClearValidation() {
            ValidationCollection.Clear();
        }

        /// <summary>
        /// In this method all the  
        /// validation methods 
        /// should be added to the 
        /// collection via AddValidation() method.
        /// Returns true means validation is correct.
        /// </summary>
        public abstract void CollectValidation();
        /// <summary>
        /// Run all the validation methods and then 
        /// set the ErrorCollector for the session.
        /// </summary>
        /// <returns>Returns true if no error exist</returns>
        public bool FinalizeValidation() {
            bool anyValidationErrorExist = false;
            foreach (var action in ValidationCollection) {
                if (!anyValidationErrorExist) {
                    anyValidationErrorExist = !action();
                }
            }
            if (anyValidationErrorExist) {
                AppConfig.SetGlobalError(ErrorCollector);
            }
            return !anyValidationErrorExist;
        }

        public static ValidationMessage GetSuccessMessage(string message = null) {
            if (string.IsNullOrEmpty(message)) {
                message = SuccessValidationMessage;
            }
            return new ValidationMessage() {
                message = message,
                isValid = true,
                isError = false
            };
        }

        public static ValidationMessage GetErrorMessage(string message = null) {
            if (string.IsNullOrEmpty(message)) {
                message = FailedValidationMessage;
            }
            return new ValidationMessage() {
                errorMessage = message,
                isValid = false,
                isError = true
            };
        }

        public static string GetErrorValidationExceedMessage() {
            if (ValidationExceedJson == null) {
                ValidationExceedJson = JsonConvert.SerializeObject(GetErrorMessage(Const.ValidationExceeded));
            }
            return ValidationExceedJson;
        }
    }
}