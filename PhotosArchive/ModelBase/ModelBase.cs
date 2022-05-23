using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PhotosArchive
{
    public class ModelBase : INotifyPropertyChanged
    {
        /*
         * [CallerMemberName]
         * Cet attribut peut être utilisé sur les paramètres et se trouve dans l’espace de noms 'System.Runtime.CompilerServices'.
         * Il s’agit d’un attribut utilisé pour injecter le nom de la méthode qui appelle une autre méthode.
         * Cela est généralement utilisé comme moyen d’éliminer les « chaînes » lors de l’implémentation de INotifyPropertyChanged
        */

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            // if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

}
}
