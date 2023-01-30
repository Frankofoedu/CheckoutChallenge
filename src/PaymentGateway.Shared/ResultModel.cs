using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Shared
{
    public class ResultModel<T>
    {
        public List<ValidationResult> ValidationErrors { get; set; } = new List<ValidationResult>();

        public List<string> ErrorMessages => ValidationErrors.Select((ValidationResult c) => c.ErrorMessage).ToList();

        public string Message { get; set; } = string.Empty;

        public T Data { get; set; }

        public string this[string columnName]
        {
            get
            {
                string columnName2 = columnName;
                ValidationResult validationResult = ValidationErrors.FirstOrDefault((ValidationResult r) => r.MemberNames.FirstOrDefault() == columnName2);
                if (validationResult != null)
                {
                    return validationResult.ErrorMessage;
                }

                return string.Empty;
            }
        }

        public string ErrorMessageString => ErrorMessages.JoinToStringBy("\n");

        public bool HasError
        {
            get
            {
                if (ValidationErrors.Count > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public ResultModel()
        {
        }

        public ResultModel(T data, string message = "")
        {
            Data = data;
            Message = message;
        }

        public ResultModel(string errorMessage)
        {
            AddError(errorMessage);
        }

        public ResultModel(List<string> errorMessage)
        {
            errorMessage.ForEach(delegate (string x)
            {
                AddError(x);
            });
        }

        public void AddError(string error)
        {
            ValidationErrors.Add(new ValidationResult(error));
        }

        public void AddError(ValidationResult validationResult)
        {
            ValidationErrors.Add(validationResult);
        }

        public void AddError(IEnumerable<ValidationResult> validationResults)
        {
            ValidationErrors.AddRange(validationResults);
        }
    }
}