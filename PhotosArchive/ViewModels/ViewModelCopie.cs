using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using PhotosArchive.Classes;
using PhotosArchive.Model;
using System.IO;

namespace PhotosArchive.ViewModels
{
    public class ViewModelCopie : ObservableObject
    {
        public static int compt { get; set; }

        private string date;
        public string Date { get => date; set { SetProperty(ref date, value); OnPropertyChanged(nameof(Date)); } }

        private string fileDestination;
        public string FileDestination { get => fileDestination; set { SetProperty(ref fileDestination, value); OnPropertyChanged(nameof(FileDestination)); } }

        DirectoryInfo directory;
        FileInfo f1, f2;
        Fichier fichier;
        Doublons doublons = new Doublons();

        public ViewModelCopie()
        {
            WeakReferenceMessenger.Default.Register<Fichier>(this, Copier);
        }

        /// <summary>
        /// Copie de fichiers
        /// </summary>
        /// <param name="fichier"></param>
        /// <param name="destination"></param>
        public void Copier(object o, Fichier f)
        {
            fichier = f;
            directory = new DirectoryInfo(fichier.Destination + "\\" + fichier.Date);

            if (!directory.Exists)
            {
                directory.Create();
            }

            string fileSource = fichier.NomComplet;
            fileDestination = fichier.Destination + "\\" + fichier.Date + "\\" + fichier.Nom;

            // Teste si le nom du fichier existe
            if (File.Exists(fileDestination))
            {
                Renomer(fileSource, fileDestination);
            }
            try
            {
                File.Copy(fileSource, fileDestination);    // si false : Exception fichier existe
                                                           // File.Copy(fileSource, fileDestination, chkCopy);    // si false : Exception fichier existe

            }
            catch { }
        }

        /// <summary>
        /// contrôle doublons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //public void Renommer(object sender, EventDoublons e)
        public void Renomer(string s, string d)
        {
            f1 = new FileInfo(s);// new FileInfo(e.FileSource);
            f2 = new FileInfo(d); // new FileInfo(e.FileDestination);

            string dossierDestination = fichier.Destination + "\\" + fichier.Date + "\\";
            date = fichier.Date;
            string destination = fichier.Destination;

            // Nombre de fichier dans le repertore de destination
            int nbFichiersSD = Directory.GetFiles(dossierDestination, "*.*", SearchOption.TopDirectoryOnly).Length;

            // Séparation du nom et de l'extension
            string[] tab = fichier.Nom.Split('.');

            // Si la taille des fichiers est différente le fichier de destination est renommé
            if (Compare.Comparer(ref f1, ref f2) == false)
            {
                nbFichiersSD++;
                fileDestination = dossierDestination + tab[0] + "_" + nbFichiersSD.ToString() + "." + tab[1];
                compt++;
                fichier = new Fichier(f1.FullName, date, fileDestination, destination);
                doublons.EcritureFichierDoublons(fichier);
            }
        }
    }
}
