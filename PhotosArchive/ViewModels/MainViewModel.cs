using DialogueService.Interfaces;
using PhotosArchive.Interfaces;
using PhotosArchive.Model;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Command;

namespace PhotosArchive.ViewModels
{
    public class MainViewModel : ModelBase
    {
        #region Champs
        BackgroundWorker worker;
        private FileInfo[] fichiers;

        #endregion

        #region Propriétés
        // Pourcentage barre de progression
        private int pourcent;
        public int Pourcent { get => pourcent; set { pourcent = value; RaisePropertyChanged(); } }

        // Copie en cours
        private string indexencours;
        public string IndexEnCours { get => indexencours; set { indexencours = value; RaisePropertyChanged(); } }

        // Variable fichier
        private Fichier f;
        public Fichier F { get => f; set { f = value; RaisePropertyChanged(); } }

        // Source des fichiers avec contrôle non vide
        private string source;
        public string Source { get => source; set { source = value; RaisePropertyChanged(); } }

        // Destination des fichiers avec contrôle non vide
        private string destination;
        public string Destination { get => destination; set { destination = value; RaisePropertyChanged(); } }

        // Titre de la fenêtre
        public string Titre { get; set; }

        // Copyright
        public string Copyright { get; set; }

        // Case à cocher affichage image
        private bool chkImage;
        public bool ChkImage { get => chkImage; set { chkImage = value; RaisePropertyChanged(); } }
        #endregion

        #region Services
        readonly ICopie copie;
        readonly IDialogue dialogue;
        readonly IDoublons d;
        readonly IChange pd;
        #endregion

        #region Boutons
        // Commande des boutons simples
        private RelayCommand cmdCopier;
        public RelayCommand CmdCopier
        {
            get
            {   // Bouton fonctionne uniquement si "Source" et "Destination" ne sont pas vides
                return cmdCopier ?? (cmdCopier = new RelayCommand(CopierMethode, () => !string.IsNullOrEmpty(Source) & !string.IsNullOrEmpty(Destination)));
            }
        }

        // Regroupement des commandes de boutons et identification de l'appel
        private RelayCommand<string> buttonCommand;
        public RelayCommand<string> ButtonCommand
        {
            get
            {
                return buttonCommand ?? (buttonCommand = new RelayCommand<string>(ButtonClick));
            }
        }

        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        public MainViewModel(ICopie copieservice, IDialogue dialogueService, IDoublons doublonService, IChange change)
        {
            Initialisation();
            Copyright = "Archive de photos V1.4 - © 2021";
            Titre = "Archivage de photos";
            copie = copieservice;
            dialogue = dialogueService;
            d = doublonService;
            pd = change;
        }

        /// <summary>
        /// Boutons simple sans Canexecutechanged
        /// </summary>
        /// <param name="btnNom"></param>
        private void ButtonClick(object btnNom)
        {
            switch (btnNom)
            {
                case "btnSource":
                    SourceMethode();
                    break;
                case "btnDestination":
                    DestinationMethode();
                    break;
                case "btnAnnuler":
                    AnnulerMethode();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Initialisation BackGroudWorker
        /// </summary>
        public void BackGround()
        {
            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChangedAsync;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
        }

        /// <summary>
        /// Initialisation
        /// </summary>
        public void Initialisation()
        {
            Thread t = new Thread(BackGround);
            t.Start();
        }

        #region Chemin

        /// <summary>
        /// Dossier source
        /// </summary>
        public void SourceMethode()
        {
            Source = null;
            Source = dialogue.Dossier();
            if (Source != null)
            {
                ListeFichierSource();
            }
        }

        /// <summary>
        /// Liste des fichiers sources
        /// </summary>
        public async void ListeFichierSource()
        {
            //int maxBits = 0;
            await Task.Run(() =>
            {
                DirectoryInfo repertoire = new DirectoryInfo(Source);
                fichiers = repertoire.GetFiles();
                //for (int i = 0; i < fichiers.Length; i++)
                //{
                //    maxBits += (int)fichiers[i].Length;
                //}
                //foreach(FileInfo file in repertoire.GetFiles())
                //{
                //    maxBits += (int)file.Length;    // Taille des données
                //}
            });
        }

        /// <summary>
        /// Dossier de destination
        /// </summary>
        public void DestinationMethode()
        {
            Destination = null;
            Destination = dialogue.Dossier();
            d.NomDoublons(Destination);
        }

        #endregion

        /// <summary>
        /// Copie des fichiers
        /// </summary>
        public void CopierMethode()
        {
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }
        }

        #region BackGroundWorker

        /// <summary>
        /// Dowork
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int nbFichiers = fichiers.Length;
            string date;

            Parallel.For(0, nbFichiers, i =>
            {
                date = pd.DatePhotos(fichiers[i].FullName, fichiers[i].LastWriteTime.ToShortDateString()).Substring(6, 4);

                // Création d'une instance de fichier
                F = new Fichier(fichiers[i].Name, date, fichiers[i].FullName, Destination);

                // Copie de fichiers                     
                copie.Copier(F);

                // Progression
                //worker.ReportProgress((i + 1) * 100 / nbFichiers, i + 1);

            });              


        }

        /// <summary>
        /// Progressbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_ProgressChangedAsync(object sender, ProgressChangedEventArgs e)
        {
            Pourcent = e.ProgressPercentage;
            IndexEnCours = e.UserState.ToString() + "/" + fichiers.Length.ToString();
        }

        /// <summary>
        /// Completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                dialogue.ShowStop("Archivage", "Archivage stoppé");
            }

            else
            {
                // Message de fichiers en doubles
                if (App.compt > 0)
                {
                    string message = $"Il existe {App.compt} fichier(s) renommés car portant le même nom dans la déstination. Voulez-vous consulter la liste ?";
                    if (dialogue.YesNo("! Doublons détectés", message))
                    {
                        Process.Start(App.chemin);
                    }
                }
                else
                {
                    dialogue.ShowMessage("Opération de copie", "Travail terminé");
                }
            }

            Raz();
        }

        /// <summary>
        /// Annulation
        /// </summary>
        public void AnnulerMethode()
        {
            worker.CancelAsync();
        }
        #endregion

        /// <summary>
        /// Réinitialisation
        /// </summary>
        public void Raz()
        {
            d.FermetureFichierDoublons();
            App.compt = 0;
            Pourcent = 0;
            Destination = null; Source = null;
            F = null;
            IndexEnCours = null;
            ChkImage = false;
        }
    }
}
