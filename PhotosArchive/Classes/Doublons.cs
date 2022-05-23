using PhotosArchive.Interfaces;
using PhotosArchive.Model;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace PhotosArchive.Classes
{
    public class Doublons : IDoublons
    {
        public static StreamWriter fichierDoublons { get; set; }
        public static string dateNow;

        /// <summary>
        /// Nom du fichier des doublons
        /// </summary>
        /// <param name="destination"></param>
        public void NomDoublons(string destination)
        {
            dateNow = DateTime.Now.ToShortDateString().ToString() + " à " + DateTime.Now.ToShortTimeString();
            string suffixeFichiersDoublons = "";

            // Conserve les chiffres d'une chaine
            MatchCollection chiffres = Regex.Matches(dateNow, "[0-9]");
            foreach (Match match in chiffres)
            {
                suffixeFichiersDoublons += match.Value;
            }

            App.chemin = destination + "\\Doublons_" + suffixeFichiersDoublons + ".txt";
        }

        /// <summary>
        /// Ecriture des doublons
        /// </summary>
        /// <param name="nomComplet"></param>
        /// <param name="date"></param>
        /// <param name="nouveauNom"></param>
        public void EcritureFichierDoublons(object parameter)
        {
            Fichier f = (Fichier)parameter;

            if (App.compt == 1)
            {
                // NomDoublons(f.Destination);

                fichierDoublons = new StreamWriter(App.chemin);
                {
                    // En-tête du fichier
                    fichierDoublons.WriteLine("Liste des fichiers renommés le : " + dateNow);
                    fichierDoublons.WriteLine("===================================================");
                    fichierDoublons.WriteLine();
                    fichierDoublons.WriteLine();
                }
            }

            if (App.compt != 0)
            {
                fichierDoublons.WriteLine(f.Date + ": " + f.Nom + " --> " + f.NomComplet);
                fichierDoublons.WriteLine();
            }
        }

        /// <summary>
        /// Fermeture fichier doublons
        /// </summary>
        public void FermetureFichierDoublons()
        {
            if (File.Exists(App.chemin))
            {
                fichierDoublons.Close();
            }
        }
    }
}
