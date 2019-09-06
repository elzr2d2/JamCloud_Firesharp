using System;
using Google.Cloud.Storage.V1;
using System.Diagnostics;

namespace JamCloud_Firesharp
{
    class StorageQuickstart
    {
        public StorageQuickstart()
        {
            // Your Google Cloud Platform project ID.
            string projectId = "jamcloud-db-2aea9";

            // [END storage_quickstart]
            Debug.Assert("YOUR-PROJECT-" + "ID" != projectId,
                "Edit Program.cs and replace YOUR-PROJECT-ID with your Google Project Id.");
            // [START storage_quickstart]

            // Instantiates a client.
            StorageClient storageClient = StorageClient.Create();

            // The name for the new bucket.
            string bucketName = projectId + ".appspot.com";
            try
            {
                // Creates the new bucket.
                storageClient.CreateBucket(projectId, bucketName);
                Console.WriteLine($"Bucket {bucketName} created.");
            }
            catch (Google.GoogleApiException e)
            when (e.Error.Code == 409)
            {
                // The bucket already exists.  That's fine.
                Console.WriteLine(e.Error.Message);
            }
        }
    }
}

