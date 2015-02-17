using System;

namespace PortalWebAPI.Domain.Test
{
    public static class ProjectRepositoryTestConfig
    {
        public static long MaxStorageCapacity;
        public static long MaxUserCapacity;
        public static long MaxVideoFileLength;
        public static long MaxStorage;
        public static string DataFileName;
        public static string LocalDataUri;
        public static string LocalVideoUri;
        public static string ProjectName;
        public static string UserName;
        public static string VideoFileName;
        public static string ProjectId;
        public static Version StorageVersion;

        static ProjectRepositoryTestConfig()
        {
            MaxStorageCapacity = (long) (60e+6*500*1024*1024);
            MaxUserCapacity = (long) 60e+6;
            MaxVideoFileLength = 500*1024*1024;

            MaxStorage = 1024*1024*1024;

            DataFileName = @"UPLOAD_JOB_1.avsx";
            LocalDataUri = @"C:\Users\makarov.t\Videos\Sample Videos\UPLOAD_JOB_1.avsx";
            LocalVideoUri = @"C:\Users\makarov.t\Videos\Sample Videos\audi.flv";
            ProjectName = @"UPLOAD_JOB_1";
            UserName = @"vasya";
            VideoFileName = @"audi.flv";

            ProjectId = @"9450b1fa40a3535abc40380450cf1fac";

            StorageVersion = new Version(2, 0, 0, 0);
        }
    }
}