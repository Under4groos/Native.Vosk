using Native.Vosk.Helper;

using (AIModelDownloader downloader = new AIModelDownloader())
{
    // big
    // big-lgraph
    // small
    // spk
    // tts
    var listModels = await downloader.FindVoskModels("ru", "small");
    foreach (var item in listModels)
    {
        Console.WriteLine(item.GetInfo());
    }

    await downloader.DownloadVoskModel(listModels.First(), (o, e) =>
    {
        Console.WriteLine(e.ProgressPercentage);
    });




    #region Types of models
    //var listModels = await downloader.FindVoskModels("ru");

    //foreach (var item in downloader.Tyes)
    //{
    //    Console.WriteLine(item);
    //}
    #endregion




    #region GetVoskModelsList
    //var listModels = await downloader.GetVoskModelsList();
    //foreach (var item in listModels)
    //{
    //    Console.WriteLine(item);
    //} 
    #endregion
}

void Client_DownloadProgressChanged(object sender, HttpDownloadProgressChangedEventArgs e)
{
    Console.WriteLine(e.ProgressPercentage);
}