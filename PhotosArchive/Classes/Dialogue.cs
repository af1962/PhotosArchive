using DialogueService.Interfaces;
using System.Windows;
using System.Windows.Forms;

namespace DialogueService.Classes
{
    public class Dialogue : IDialogue
    {
        /// <summary>
        /// Repertoire
        /// </summary>
        /// <returns></returns>
        public string Dossier()
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            DialogResult result = folderDialog.ShowDialog();
            return result.ToString() == "OK" ? folderDialog.SelectedPath.ToString() : null;
        }

        /// <summary>
        /// Fichier
        /// </summary>
        /// <returns></returns>
        public string Fichier()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.ShowDialog();
            return openFileDialog.FileName != null ? openFileDialog.FileName : null;
        }

        /// <summary>
        /// Message d'erreur
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public void ShowError(string title, string message)
        {
            System.Windows.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No);
        }

        /// <summary>
        /// Message standard
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public void ShowMessage(string title, string message)
        {
            System.Windows.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Message Stop
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public void ShowStop(string title, string message)
        {
            System.Windows.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Stop);

        }

        /// <summary>
        /// Messsage Yes/No
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool YesNo(string title, string message)
        {
            return MessageBoxResult.Yes == System.Windows.MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

        }
    }
}
