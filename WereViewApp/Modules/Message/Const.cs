namespace WeReviewApp.Modules.Message {
    public struct Const {
        #region Error List

        /// <summary>
        ///     Sorry! you have exceed the limit of validation. Please try some time later.
        /// </summary>
        public const string ValidationExceeded =
            "Sorry! you have exceed the limit of validation. Please try some time later.";

        public const string JunkMessageResult =
            "Please make sure you have filled up the subject and message and it is valid. Junk messages will result your account to be blocked.";

        #endregion
    }
}