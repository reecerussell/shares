namespace Shares.Core
{
    public class Constants
    {
        public const string PasswordIterationCountKey = "Password:IterationCount";
        public const string PasswordSaltSizeKey = "Password:SaltSize";
        public const string PasswordKeySizeKey = "Password:KeySize";

        public const string PasswordRequiredLengthKey = "Password:Validation:RequiredLength";
        public const string PasswordRequireUppercaseKey = "Password:Validation:RequireUppercase";
        public const string PasswordRequireLowercaseKey = "Password:Validation:RequireLowercase";
        public const string PasswordRequireNonAlphanumericKey = "Password:Validation:RequireNonAlphanumeric";
        public const string PasswordRequireDigitKey = "Password:Validation:RequireDigit";
        public const string PasswordRequiredUniqueCharsKey = "Password:Validation:RequiredUniqueChars";

        public const string ConnectionStringCacheKey = "Cache:ConnectionString";

        public const string HasherKeyIdKey = "Hasher:KeyId";
    }
}
