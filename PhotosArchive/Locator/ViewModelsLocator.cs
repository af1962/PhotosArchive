using DialogueService.Classes;
using DialogueService.Interfaces;
using PhotosArchive.Classes;
using PhotosArchive.Interfaces;
using PhotosArchive.ViewModels;

namespace PhotosArchive.Locator
{
    public class ViewModelsLocator
    {
        /// <summary>
        /// Sans contenur Ioc
        /// </summary>
        ICopie copieService = new Copie();
        IDialogue dialogueService = new Dialogue();
        IDoublons doublons = new Doublons();
        IChange change = new Change();
      

        public MainViewModel MainViewModelPrimaire => new MainViewModel(copieService, dialogueService, doublons, change);

    }
}
