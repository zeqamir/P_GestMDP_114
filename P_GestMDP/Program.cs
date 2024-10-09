using System;
using System.IO;

namespace P_GestMDP
{
    class Program
    {
        // Chemin du fichier contenant le master password
        static string masterPasswordFilePath = @"C:\Users\amizeqiri\Desktop\GitHub\P_GestMDP_114\P_GestMDP\MasterPassword\masterpassword";

        static void Main(string[] args)
        {
            // Efface l'écran
            Console.Clear();

            // Vérifie et demande le master password
            if (!CheckMasterPassword())
            {
                return; // Si le master password est incorrect ou l'utilisateur annule, on quitte le programme
            }

            // Variable pour stocker la réponse de l'utilisateur s'il veut recommencer
            char Reponse;

            // Boucle principale du programme qui permet de recommencer après chaque action
            do
            {
                // Efface l'écran
                Console.Clear();

                // Menu
                Console.Write("*******************************" +
                    "\nSélectionnez une action" +
                    "\n1. Consulter les mots de passe" +
                    "\n2. Ajouter un mot de passe" +
                    "\n3. Supprimer un mot de passe" +
                    "\n4. Quitter le programme" +
                    "\n*******************************" +
                    "\n\nFaites votre choix : ");

                // Lecture du choix de l'utilisateur
                string Choix = Console.ReadLine();

                // Utilisation d'un switch pour exécuter une action en fonction du choix
                switch (Choix)
                {
                    case "1":
                        // Consultation des mots de passe
                        ConsulterMotDePasse();
                        break;

                    case "2":
                        // Ajouter un mot de passe
                        AjouterMotDePasse();
                        break;

                    case "3":
                        // Supprimer un mot de passe
                        SupprimerMotDePasse();
                        break;

                    case "4":
                        // Quitter le programme
                        Environment.Exit(0);
                        break;

                    default:
                        // Cas où l'utilisateur entre une option invalide
                        Console.WriteLine("Choix invalide");
                        break;
                }

                // Demande à l'utilisateur s'il veut relancer le programme
                Console.Write("\nVoulez-vous relancer le programme (o / n): ");
                Reponse = Convert.ToChar(Console.ReadLine());
            }
            // Boucle tant que l'utilisateur entre 'o' ou '0' pour continuer
            while (Reponse == 'o' || Reponse == '0');
        }

        // Fonction pour vérifier ou créer le master password
        static bool CheckMasterPassword()
        {
            if (File.Exists(masterPasswordFilePath))
            {
                // Si le fichier existe, on demande le master password
                Console.Write("Veuillez entrer le master password : ");
                string inputMasterPassword = Console.ReadLine();

                // Lecture du master password dans le fichier
                string savedMasterPassword = File.ReadAllText(masterPasswordFilePath);

                // Vérification du mot de passe
                if (inputMasterPassword == savedMasterPassword)
                {
                    Console.WriteLine("Accès accordé.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Mot de passe incorrect.");
                    return false;
                }
            }
            else
            {
                // Si le fichier n'existe pas, on demande de créer un master password
                Console.WriteLine("Aucun master password trouvé.");
                Console.Write("Veuillez créer un master password : ");
                string newMasterPassword = Console.ReadLine();

                // Écriture du nouveau master password dans le fichier
                File.WriteAllText(masterPasswordFilePath, newMasterPassword);

                Console.WriteLine("Master password enregistré.");
                return true;
            }
        }

        static void ConsulterMotDePasse()
        {
            Console.Clear();
            string directoryPath = @"C:\Users\amizeqiri\Desktop\GitHub\P_GestMDP_114\P_GestMDP\password";

            try
            {
                if (Directory.Exists(directoryPath))
                {
                    string[] files = Directory.GetFiles(directoryPath, "*");
                    if (files.Length == 0)
                    {
                        Console.WriteLine("Aucun mot de passe trouvé.");
                    }
                    else
                    {
                        Console.WriteLine("Vos mots de passe :\n");
                        for (int i = 0; i < files.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                        }

                        Console.Write("\nQuel mot de passe voulez-vous consulter et éventuellement modifier ? (entrez le numéro) : ");
                        string choix = Console.ReadLine();

                        if (int.TryParse(choix, out int index) && index > 0 && index <= files.Length)
                        {
                            string filePath = files[index - 1];
                            string fileContent = File.ReadAllText(filePath);
                            Console.WriteLine("\nContenu actuel :\n" + fileContent);

                            Console.Write("Veuillez entrer la nouvelle URL (laisser vide pour conserver l'ancienne) : ");
                            string newUrl = Console.ReadLine();
                            Console.Write("Veuillez entrer le nouvel identifiant (laisser vide pour conserver l'ancien) : ");
                            string newIdentifiant = Console.ReadLine();
                            Console.Write("Veuillez entrer le nouveau mot de passe (laisser vide pour conserver l'ancien) : ");
                            string newMotDePasse = Console.ReadLine();

                            string[] lines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                            if (!string.IsNullOrWhiteSpace(newUrl))
                            {
                                lines[0] = "URL : " + newUrl;
                                string newFileName = newUrl;
                                string newFilePath = Path.Combine(directoryPath, newFileName);
                                File.Delete(filePath);
                                File.WriteAllLines(newFilePath, lines);
                                Console.WriteLine("Les modifications ont été enregistrées avec succès sous le nouveau nom.");
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(newIdentifiant))
                                {
                                    lines[1] = "Identifiant : " + newIdentifiant;
                                }
                                if (!string.IsNullOrWhiteSpace(newMotDePasse))
                                {
                                    lines[2] = "Mot de passe : " + newMotDePasse;
                                }
                                File.WriteAllLines(filePath, lines);
                                Console.WriteLine("Les modifications ont été enregistrées avec succès.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Choix invalide.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Le répertoire des mots de passe n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite lors de la lecture des fichiers : " + ex.Message);
            }

            Console.ReadLine();
        }

        static void AjouterMotDePasse()
        {
            Console.Clear();
            Console.Write("Veuillez entrez l'URL du site : ");
            string url = Console.ReadLine();

            Console.Write("Veuillez entrer l'identifiant : ");
            string identifiant = Console.ReadLine();

            Console.Write("Veuillez entrer le mot de passe : ");
            string motDePasse = Console.ReadLine();

            string pathFile = $@"C:\Users\amizeqiri\Desktop\GitHub\P_GestMDP_114\P_GestMDP\password\{url}";

            try
            {
                using (StreamWriter sw = File.AppendText(pathFile))
                {
                    sw.WriteLine("URL : " + url);
                    sw.WriteLine("Identifiant : " + identifiant);
                    sw.WriteLine("Mot de passe : " + motDePasse);
                }

                Console.WriteLine($"\nLe mot de passe a été enregistré dans le fichier '{url}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite lors de l'enregistrement : " + ex.Message);
            }

            Console.ReadLine();
        }

        static void SupprimerMotDePasse()
        {
            Console.Clear();
            string deleteDirectoryPath = @"C:\Users\amizeqiri\Desktop\GitHub\P_GestMDP_114\P_GestMDP\password";

            try
            {
                if (Directory.Exists(deleteDirectoryPath))
                {
                    string[] files = Directory.GetFiles(deleteDirectoryPath, "*");

                    if (files.Length == 0)
                    {
                        Console.WriteLine("Aucun mot de passe trouvé à supprimer.");
                    }
                    else
                    {
                        Console.WriteLine("Vos mots de passe :\n");
                        for (int i = 0; i < files.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                        }

                        Console.Write("\nQuel mot de passe voulez-vous supprimer ? (entrez le numéro) : ");
                        string choix = Console.ReadLine();

                        if (int.TryParse(choix, out int index) && index > 0 && index <= files.Length)
                        {
                            File.Delete(files[index - 1]);
                            Console.WriteLine("Le mot de passe a été supprimé avec succès.");
                        }
                        else
                        {
                            Console.WriteLine("Choix invalide.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Le répertoire des mots de passe n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite lors de la suppression du fichier : " + ex.Message);
            }

            Console.ReadLine();
        }
    }
}
