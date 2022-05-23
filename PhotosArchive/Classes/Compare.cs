using System;
using System.IO;

namespace PhotosArchive.Classes

{
    class Compare
    {
        const int nbBits = sizeof(Int64);

        /// <summary>
        /// Comparaison de 2 fichiers
        /// </summary>
        /// <param name="premier"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool Comparer(ref FileInfo premier, ref FileInfo second)
        {
            if (premier.Length != second.Length)
                return false;

            int iterations = (int)Math.Ceiling((double)premier.Length / nbBits);

            using (FileStream f1 = premier.OpenRead())
            using (FileStream f2 = second.OpenRead())
            {
                byte[] t1 = new byte[nbBits];
                byte[] t2 = new byte[nbBits];

                for (int i = 0; i < iterations; i++)
                {
                    f1.Read(t1, 0, nbBits);
                    f2.Read(t2, 0, nbBits);

                    if (BitConverter.ToInt64(t1, 0) != BitConverter.ToInt64(t2, 0))

                        return false;
                }
            }

            return true;
        }
    }
}
