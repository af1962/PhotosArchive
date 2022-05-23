namespace PhotosArchive.Model
{
    public class Fichier 
    {
        string nomComplet;
        string nom;
        string date;
        string destination;

        public Fichier(string nom, string date, string nomComplet, string destination)
        {
            Nom = nom;
            Date = date;
            NomComplet = nomComplet;
            Destination = destination;
        }

        public string NomComplet { get => nomComplet; set { nomComplet = value; } }
        public string Nom { get => nom; set { nom = value; } }
        public string Date { get => date; set { date = value; } }
        public string Destination { get => destination; set { destination = value; } }
    }
}
