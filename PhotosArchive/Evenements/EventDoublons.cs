using System;

namespace PhotosArchive.Evenements
{
    public class EventDoublons : EventArgs
    {
        public string FileSource;
        public string FileDestination;

        public EventDoublons(string source, string destination)
        {
            FileSource = source;
            FileDestination = destination;
        }

    }
}
