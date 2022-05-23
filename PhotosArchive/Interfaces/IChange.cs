using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotosArchive.Interfaces
{
    public interface IChange
    {
        string DatePhotos(string fichier, string date);
    }
}
