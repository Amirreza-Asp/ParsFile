namespace ParsFile.Application
{
    public static class SD
    {
        // files routes
        public const String FileCommonPasswordPath = @"wwwroot/file/identity/commonPassword.txt";
        public const String UserFilesPath = "/file/user files/";
        public const String FileImagesPath = UserFilesPath + "images/";

        // toastr notfication types
        public const String SuccessMessage = "success";
        public const String ErrorMessage = "error";
        public const String WarningMessage = "warning";
        public const String InfoMessage = "info";

        // roles
        public const String Admin = "Admin";
        public const String User = "User";
    }
}
