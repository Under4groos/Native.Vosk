using System.ComponentModel;

namespace Native.Vosk.Helper
{
    public class HttpDownloadProgressChangedEventArgs : ProgressChangedEventArgs
    {
        internal HttpDownloadProgressChangedEventArgs(int progressPercentage, object? userToken, long bytesReceived, long totalBytesToReceive) :
            base(progressPercentage, userToken)
        {
            BytesReceived = bytesReceived;
            TotalBytesToReceive = totalBytesToReceive;
        }

        public long BytesReceived { get; }
        public long TotalBytesToReceive { get; }
    }
    public delegate void HttpDownloadProgressChangedEventHandler(object sender, HttpDownloadProgressChangedEventArgs e);



    public class HttpClientDownloader : HttpClient
    {
        public event HttpDownloadProgressChangedEventHandler? DownloadProgressChanged;
        public async Task DwinloadAsyncFile(string fileUrl, string destinationPath)
        {
            try
            {
                // Send a GET request to the specified URL
                using (HttpResponseMessage response = await GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode(); // Throw if not a success code.

                    // Get the total size of the file
                    long totalBytes = response.Content.Headers.ContentLength ?? -1;
                    using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                                   fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        byte[] buffer = new byte[8192]; // Buffer size
                        long totalReadBytes = 0; // Total bytes read so far
                        int readBytes;

                        // Read the content stream in chunks
                        while ((readBytes = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, readBytes); // Write to file
                            totalReadBytes += readBytes; // Update total bytes read

                            // Calculate and display progress
                            if (totalBytes != -1)
                            {
                                int progressPercentage = (int)((totalReadBytes * 100) / totalBytes);
                                Console.WriteLine($"Download progress: {progressPercentage}%");
                                DownloadProgressChanged?.Invoke(this, new HttpDownloadProgressChangedEventArgs(progressPercentage, null, totalBytes, totalReadBytes));
                            }

                        }
                    }
                }
                Console.WriteLine("File downloaded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
