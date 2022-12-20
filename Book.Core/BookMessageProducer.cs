//using Azure.Identity;
using Azure.Storage.Blobs;
using System;
using System.IO;

namespace Books.Core
{
    public class BookMessageProducer
    {
        // declaring an event using built-in EventHandler
        protected event EventHandler<BookEventArgs> BookAdded;

        // event handler
        protected async void BookMessageProducer_BookAdded(object sender, BookEventArgs e)
        {
            // Add record to the queue and lister will process that in DB or wherever.
            Console.WriteLine($"Book added sucessfully !");

            //if using this then you need to provide ROLE "Storage Blob Data Contributor" to registered APP whose details are below.

            string tenantId = "fd5f0964-f948-4bc0-a591-069e02e468ee";
            string clientId = "e867ced8-0812-4ec3-ac00-beb40122ee3b";
            string clientSecret = "fAQ8Q~VOoGcgEy_5UV2~Q0HlO2_hYekafze2IbYi";

            //ClientSecretCredential secretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            FileStream fs = e.File as FileStream;
            string fileName = Path.GetFileName(fs.Name);
            
            string blobUri = $"https://{"bookblob"}.blob.core.windows.net/{"bookcontainer"}/"+ fileName;
            BlobClient blobClient = new BlobClient(new Uri(blobUri));

            await blobClient.UploadAsync(e.File, true);
        }

        public void AddBookToQueue(BookEventArgs book)
        {
            // Registering event handler
            this.BookAdded += BookMessageProducer_BookAdded;

            OnBookAdded(book);
        }

        // Invoking event 
        protected virtual void OnBookAdded(BookEventArgs e)
        {
            BookAdded?.Invoke(this, e);
        }
    }
}
