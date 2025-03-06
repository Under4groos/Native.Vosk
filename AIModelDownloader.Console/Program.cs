using Native.Vosk.Helper;

using (AIModelDownloader downloader = new AIModelDownloader())
{

    var listModels = await downloader.FindVoskModels("ru");

    foreach (var item in downloader.Tyes)
    {
        Console.WriteLine(item);
    }

    foreach (var item in listModels)
    {
        Console.WriteLine(item.GetInfo());
    }











    #region GetVoskModelsList
    //var listModels = await downloader.GetVoskModelsList();
    //foreach (var item in listModels)
    //{
    //    Console.WriteLine(item);
    //} 
    #endregion
}