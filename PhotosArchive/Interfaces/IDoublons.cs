using PhotosArchive.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotosArchive.Interfaces
{
    public interface IDoublons
    {
        void NomDoublons(string destination);
        void EcritureFichierDoublons(object f);
        void FermetureFichierDoublons();

    }
}
