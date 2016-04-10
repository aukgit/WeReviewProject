namespace WeReviewApp.Modules.Validations {
    public class ValidationMessage {
        /// <summary>
        /// Message if the validation is successful.
        /// jquery-server-side-validation framework message set.
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// If validation success or not.
        /// </summary>
        public bool isValid { get; set; }
        public bool isError { get; set; }
        public int errorCode { get; set; }
        public string errorMessage { get; set; }

    }

}