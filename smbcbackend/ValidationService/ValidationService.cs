namespace smbcbackend.ValidationService
{
    public interface IValidationService
    {
        bool Validate(string dataType, string value, string constraintExpression, out string validationMessage);
    }

    public class ValidationService : IValidationService
    {
        public bool Validate(string dataType, string value, string constraintExpression, out string validationMessage)
        {
            validationMessage=string.Empty;

            try
            {
                switch (dataType.ToLower())
                {
                    case "int":
                        if (!int.TryParse(value, out int intValue))
                        {
                            validationMessage = "Invalid integer value.";
                            return false;
                        }
                        return EvaluateConstraint(intValue, constraintExpression, out validationMessage);

                    case "nvarchar":
                    case "nvarchar(255)":    
                    case "string":

                        return EvaluateConstraint(value, constraintExpression, out validationMessage);

                    case "datetime":
                        if (!DateTime.TryParse(value, out DateTime dateTimeValue))
                        {
                            validationMessage = "Invalid date/time value.";
                            return false;
                        }
                        return EvaluateConstraint(dateTimeValue, constraintExpression, out validationMessage);

                    case "bit":
                        if (!bool.TryParse(value, out bool boolValue))
                        {
                            validationMessage = "Invalid boolean value.";
                            return false;
                        }
                        return EvaluateConstraint(boolValue, constraintExpression, out validationMessage);

                    default:
                        validationMessage = "Unsupported data type.";
                        return false;
                }
            }
            catch (Exception ex)
            {
                validationMessage = $"Validation error: {ex.Message}";
                return false;
            }


            throw new NotImplementedException();
        }

        private bool EvaluateConstraint<T>(T value, string constraintExpression, out string validationMessage)
        {
            // For simplicity, assume constraintExpression is a simple expression like "value > 0"
            // You might need to implement a more robust expression evaluation for complex constraints
            validationMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(constraintExpression))
                return true;

            var expr = constraintExpression.Replace("value", value.ToString());
            var result = (bool)new System.Data.DataTable().Compute(expr, string.Empty);

            if (!result)
            {
                validationMessage = "Constraint violation.";
                return false;
            }

            return true;
        }

    }
}
