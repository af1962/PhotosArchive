using PhotosArchive.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PhotosArchive.Classes
{
    public class Change : IChange
    {
        /// <summary>
        /// Renvoi une date valide ou inchangée
        /// </summary>
        BitmapFrame img;
        public string DatePhotos(string fichier, string date)
        {
            string path = fichier;

            try // Dans Try car Bitmap ne prends pas en charge les vidéos => arrêt programme sur une erreur !
            {
                img = BitmapFrame.Create(new Uri(path));
                BitmapMetadata metadata = (BitmapMetadata)img.Metadata;
                date = DateTime.ParseExact(metadata.DateTaken, "dd/MM/yyyy HH:mm:ss", null).ToString();
            }
            catch
            {

            }

            return date;
        }
    }
}
