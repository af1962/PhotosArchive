namespace DialogueService.Interfaces
{
    public interface IDialogue
    {
        bool YesNo(string title, string message);
        void ShowMessage(string title, string message);
        void ShowError(string title, string message);
        void ShowStop(string title, string message);
        string Dossier();
        string Fichier();

    }
}
