namespace Core.Messages
{
    public static class Message
    {
        public const string NullError = "The field '{PropertyName}' cannot be null.";
        public const string LengthError = "The length of the field '{PropertyName}' must be between '{MinLength}' and '{MaxLength}' characters. Found '{TotalLength}' characters.";
        public const string EmptyError = "The field '{PropertyName}' cannot be empty.";
        public const string MatchError = "The format of the field '{PropertyName}' does not match.";
        public const string ValueError = "Invalid value for '{PropertyName}'.";
        public const string ValueWithValueError = "Invalid value for '{PropertyName}'. Value: '{PropertyValue}'.";
        public const string CountError = "There must be at least one '{PropertyName}'.";
        public const string PasswordError = "Passwords do not match.";
        public const string TelError = "'{PropertyName}' must have a maximum of 10 digits.";
        public const string DecimalError = "'{PropertyName}' must not have more than '{ExpectedPrecision}' total digits, allowing up to '{ExpectedScale}' decimal places. Found '{Digits}' digits and '{ActualScale}' decimals in '{PropertyValue}'.";
        public const string MaxLengthError = "The length of '{PropertyName}' must be '{MaxLength}' characters or less. Total entered: '{TotalLength}'.";
        public const string UpdateDateError = "The update date must be the current date.";
        public const string NotFoundEntity = "The entity '{entity}' has no records.";
        public const string NotFoundEntityById = "The entity '{entity}' has no records for the ID '{id}'.";
        public const string EmailError = "The field '{PropertyName}' must be a valid email format.";
        public const string UniqueLengthError = "The length of '{PropertyName}' must be '{MaxLength}' characters. Total entered: '{TotalLength}'.";
        public const string DateError = "The date format is invalid, it must be (YYYYMMDD).";
        public const string GreaterThanZero = "The field '{PropertyName}' must be greater than zero.";
        public static string NotFoundInSystem(string entityName) => $"There are no {entityName} in the system.";
        public static string CreatedSuccessfully(string entityName) => $"The {entityName} was created successfully.";
        public static string CreatedLogSuccessfully(string entityName) => $"The {entityName} log was inserted successfully.";
        public static string AlreadyExists(string entityName, string data) => $"The {entityName} {data} already exists.";
        public static string NotBeRegistered(string entityName) => $"The {entityName} could not be registered in the system.";
        public static string NotFoundExists(string entityName, string data) => $"The {entityName} {data} does not exist.";
        public static string NotBeLinked(string entityNameOne, string entityNameTwo) => $"The {entityNameOne} could not be linked to the {entityNameTwo}.";
        public static string ErrorLogin => "Invalid username or password.";
        public static string ErrorProcessing(Exception ex) => $"Error processing the request. {ex}";
        public const string Order = "Order must be 'asc' or 'desc' if provided.";
        public const string Sort = "Sort must be a valid field of the request.";
        public const string EventNotExists = "The propertie 'evento' does not exist in message.";
        public static string EventTypeNotExists(string eventType) => $"The handler for event {eventType} does not exist.";
        public const string FileSize = "The file must not exceed 10 MB.";
        public const string FileFormat = "Unsupported file format. Only PDF, JPG, and PNG are allowed.";
        public static string DeleteFile(string fileName) => $"The file {fileName} was deleted successfully.";
        public static string MissinDocument(string documents) => $"Missing mandatory documents: {documents}";
        public static string UpdateSuccessfully(string entityName) => $"The {entityName} was update successfully.";

    }
}