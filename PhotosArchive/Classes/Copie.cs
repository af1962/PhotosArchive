using PhotosArchive.Evenements;
using PhotosArchive.Interfaces;
using PhotosArchive.Model;
using System;
using System.IO;

namespace PhotosArchive.Classes
{
    public class Copie : ICopie
    {
        public EventHandler<EventDoublons> Even;

        DirectoryInfo directory;
        FileInfo f1, f2;
        Fichier fichier;
        Doublons doublons = new Doublons();
        string date;
        string fileDestination;

        public Copie()
        {
            Even += Copie_even;
        }

      
        /// <summary>
        /// Copie de fichiers
        /// </summary>
        /// <param name="fichier"></param>
        /// <param name="destination"></param>
        public void Copier(object parameter)
        {
            fichier = (Fichier)parameter;
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
                Even(this, new EventDoublons(fileSource, fileDestination));
            }
            try
            {
                File.Copy(fileSource, fileDestination);
            }
            catch { }
        }

        /// <summary>
        /// contrôle doublons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Copie_even(object sender, EventDoublons e)
        {
            f1 = new FileInfo(e.FileSource); 
            f2 = new FileInfo(e.FileDestination);

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
                App.compt++;
                fichier = new Fichier(f1.FullName, date, fileDestination, destination);
                doublons.EcritureFichierDoublons(fichier);
            }
        }    
    }
}
