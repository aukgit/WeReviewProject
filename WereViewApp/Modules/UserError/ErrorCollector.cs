using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WereViewApp.Modules.UserError {
    public class ErrorCollector : IDisposable {
        public enum ErrorType {
            High,
            Medium,
            Low
        }

        public const string SOLUTION_STATE_LINK_CLASS = "rounded-3 label label-info error-solution-link-color";
        public const string SOLUTION_STATE_CLASS = "rounded-3 label label-success";
        private int defaultCapacity = 60;
        private List<BasicError> errors;
        private short orderIncrementer;
        private readonly string highRisk = "rounded-3 label label-danger";
        private readonly string lowRisk = "rounded-3 label label-warning low-error-color";
        private readonly string midRisk = "rounded-3 label label-danger mid-error-color";

        public ErrorCollector(int def = 60) {
            errors = new List<BasicError>(def);
            defaultCapacity = def;
        }

        public void Dispose() {
            errors = null;
            GC.Collect();
        }

        public string GetCSSForError(BasicError e) {
            if (e.Type == ErrorType.High) {
                return highRisk;
            }
            if (e.Type == ErrorType.Medium) {
                return midRisk;
            }
            if (e.Type == ErrorType.Low) {
                return lowRisk;
            }
            return lowRisk;
        }

        /// <summary>
        ///     Is any error exist in the list?
        /// </summary>
        /// <returns>Returns true if any error exist.</returns>
        public bool IsExist() {
            if (errors != null && errors.Count > 0) {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     add error message with low priority
        /// </summary>
        /// <param name="msg">set your message.</param>
        /// <param name="quantityTypeIsNotValidPleaseSelectAValidQuantityType"></param>
        public int Add(string msg, string solution = "", string link = "", string linkTitle = "") {
            var error = new BasicError {
                OrderID = orderIncrementer++,
                ErrorMessage = msg,
                Type = ErrorType.Low
            };
            errors.Add(error);
            return error.OrderID;
        }

        /// <summary>
        ///     add error message with high priority
        /// </summary>
        /// <param name="msg">set your message.</param>
        public int AddHigh(string msg, string solution = "", string link = "", string linkTitle = "") {
            var error = new BasicError {
                Type = ErrorType.High,
                OrderID = orderIncrementer++,
                ErrorMessage = msg,
                Solution = solution,
                Link = link,
                LinkTitle = linkTitle
            };
            errors.Add(error);
            return error.OrderID;
        }

        /// <summary>
        ///     add error message with medium priority
        /// </summary>
        /// <param name="msg">set your message.</param>
        public int AddMedium(string msg, string solution = "", string link = "", string linkTitle = "") {
            var error = new BasicError {
                Type = ErrorType.Medium,
                OrderID = orderIncrementer++,
                ErrorMessage = msg,
                Solution = solution,
                Link = link,
                LinkTitle = linkTitle
            };
            errors.Add(error);
            return error.OrderID;
        }

        /// <summary>
        ///     add error message with given priority
        /// </summary>
        /// <param name="msg">set your message.</param>
        /// <param name="type">Type of your error message.</param>
        public int Add(string msg, ErrorType type, string solution = "", string link = "", string linkTitle = "") {
            var error = new BasicError {
                Type = ErrorType.Low,
                OrderID = orderIncrementer++,
                ErrorMessage = msg,
                Solution = solution,
                Link = link,
                LinkTitle = linkTitle
            };
            errors.Add(error);
            return error.OrderID;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns all error message as string list.</returns>
        public List<string> GetMessages() {
            return errors.Select(n => n.ErrorMessage)
                .ToList();
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns all error message as Error Object.</returns>
        public List<BasicError> GetErrors() {
            if (errors != null && errors.Count > 0) {
                return errors.ToList();
            }
            return null;
        }

        public void Remove(int OrderID) {
            var error = errors.FirstOrDefault(n => n.OrderID == OrderID);
            if (error != null) {
                errors.Remove(error);
            }
        }

        public void Remove(string Message) {
            var error = errors.FirstOrDefault(n => n.ErrorMessage == Message);
            if (error != null) {
                errors.Remove(error);
            }
        }

        /// <summary>
        ///     Clean counter and clean the error list start from 0.
        /// </summary>
        public void Clear() {
            orderIncrementer = 0;
            errors.Clear();
            //errors.Capacity = defaultCapacity;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns high error message as string list.</returns>
        public List<string> GetMessagesHigh() {
            return errors.Where(n => n.Type == ErrorType.High).Select(n => n.ErrorMessage).ToList();
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns low error message as string list.</returns>
        public List<string> GetMessagesLow() {
            if (errors.Count > 0) {
                return errors.Where(n => n.Type == ErrorType.Low).Select(n => n.ErrorMessage).ToList();
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns medium error message as string list.</returns>
        public List<string> GetMessagesMedium() {
            if (errors.Count > 0) {
                return errors.Where(n => n.Type == ErrorType.Medium).Select(n => n.ErrorMessage).ToList();
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns all error message as string list of sorted by order id.</returns>
        public List<string> GetMessagesSorted() {
            if (errors.Count > 0) {
                return errors.OrderBy(n => n.OrderID).Select(n => n.ErrorMessage).ToList();
            }
            return null;
        }

        public class BasicError {
            [Required]
            public short OrderID { get; set; }

            [Required]
            public string ErrorMessage { get; set; }

            public string Solution { get; set; }
            public string Link { get; set; }
            public string LinkTitle { get; set; }

            [Required]
            public ErrorType Type { get; set; }
        }
    }
}