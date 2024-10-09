using System;
using System.IO;

namespace P_GestMDP
{
    class Program
    {
        static void Main(string[] args)
        {
            // Efface l'écran
            Console.Clear();

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
                    case "1":  // Cas où l'utilisateur veut consulter/modifier un mot de passe
                        Console.Clear();

                        // Chemin du répertoire où sont stockés les fichiers de mots de passe
                        string directoryPath = @"C:\Users\ps70dji\Desktop\GitHub\P_GestMDP_114\P_GestMDP\password";

                        try
                        {
                            // Vérifie si le répertoire existe
                            if (Directory.Exists(directoryPath))
                            {
                                // Récupère tous les fichiers dans ce répertoire
                                string[] files = Directory.GetFiles(directoryPath, "*");

                                if (files.Length == 0)
                                {
                                    // Si aucun fichier n'est trouvé, affiche un message
                                    Console.WriteLine("Aucun mot de passe trouvé.");
                                }
                                else
                                {
                                    // Affiche les fichiers disponibles
                                    Console.WriteLine("Vos mots de passe :\n");
                                    for (int i = 0; i < files.Length; i++)
                                    {
                                        Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                                    }

                                    // Demande à l'utilisateur quel fichier il souhaite consulter/modifier
                                    Console.Write("\nQuel mot de passe voulez-vous consulter et éventuellement modifier ? (entrez le numéro) : ");
                                    string choix = Console.ReadLine();

                                    // Vérifie que le choix est valide
                                    if (int.TryParse(choix, out int index) && index > 0 && index <= files.Length)
                                    {
                                        // Lit le contenu du fichier choisi
                                        string filePath = files[index - 1];
                                        string fileContent = File.ReadAllText(filePath);
                                        Console.WriteLine("\nContenu actuel :\n" + fileContent);

                                        // Demande les nouvelles informations à l'utilisateur (laisser vide pour conserver l'ancienne)
                                        Console.Write("Veuillez entrer la nouvelle URL (laisser vide pour conserver l'ancienne) : ");
                                        string newUrl = Console.ReadLine();
                                        Console.Write("Veuillez entrer le nouvel identifiant (laisser vide pour conserver l'ancien) : ");
                                        string newIdentifiant = Console.ReadLine();
                                        Console.Write("Veuillez entrer le nouveau mot de passe (laisser vide pour conserver l'ancien) : ");
                                        string newMotDePasse = Console.ReadLine();

                                        // Modifie le contenu du fichier en fonction des nouvelles entrées
                                        string[] lines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                                        if (!string.IsNullOrWhiteSpace(newUrl))
                                        {
                                            lines[0] = "URL : " + newUrl;

                                            // Crée un nouveau fichier avec la nouvelle URL comme nom
                                            string newFileName = newUrl;
                                            string newFilePath = Path.Combine(directoryPath, newFileName);

                                            // Supprime l'ancien fichier et enregistre les modifications dans le nouveau fichier
                                            File.Delete(filePath);
                                            File.WriteAllLines(newFilePath, lines);
                                            Console.WriteLine("Les modifications ont été enregistrées avec succès sous le nouveau nom.");
                                        }
                                        else
                                        {
                                            // Si l'URL n'a pas changé, on vérifie les autres champs (identifiant et mot de passe)
                                            if (!string.IsNullOrWhiteSpace(newIdentifiant))
                                            {
                                                lines[1] = "Identifiant : " + newIdentifiant;
                                            }
                                            if (!string.IsNullOrWhiteSpace(newMotDePasse))
                                            {
                                                lines[2] = "Mot de passe : " + newMotDePasse;
                                            }

                                            // Enregistre les modifications dans le fichier existant
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
                                // Si le répertoire n'existe pas
                                Console.WriteLine("Le répertoire des mots de passe n'existe pas.");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Gère les erreurs lors de l'accès au répertoire
                            Console.WriteLine("Une erreur s'est produite lors de la lecture des fichiers : " + ex.Message);
                        }

                        Console.ReadLine();
                        break;

                    case "2":  // Cas où l'utilisateur veut ajouter un mot de passe
                        Console.Clear();

                        // Demande les informations de l'utilisateur pour le mot de passe à ajouter
                        Console.Write("Veuillez entrez l'URL du site : ");
                        string url = Console.ReadLine();

                        Console.Write("Veuillez entrer l'identifiant : ");
                        string identifiant = Console.ReadLine();

                        Console.Write("Veuillez entrer le mot de passe : ");
                        string motDePasse = Console.ReadLine();

                        // Crée un chemin de fichier basé sur l'URL
                        string pathFile = $@"C:\Users\ps70dji\Desktop\GitHub\P_GestMDP_114\P_GestMDP\password\{url}";

                        try
                        {
                            // Ajoute ou crée un nouveau fichier texte contenant les informations
                            using (StreamWriter sw = File.AppendText(pathFile))
                            {
                                sw.WriteLine("URL : " + url);
                                sw.WriteLine("Identifiant : " + identifiant);
                                sw.WriteLine("Mot de passe : " + motDePasse);
                            }

                            // Confirmation de l'enregistrement
                            Console.WriteLine($"\nLe mot de passe a été enregistré dans le fichier '{url}'.");
                        }
                        catch (Exception ex)
                        {
                            // Gère les erreurs lors de l'enregistrement
                            Console.WriteLine("Une erreur s'est produite lors de l'enregistrement : " + ex.Message);
                        }

                        Console.ReadLine();
                        break;

                    case "3":  // Cas où l'utilisateur veut supprimer un mot de passe
                        Console.Clear();
                        string deleteDirectoryPath = @"C:\Users\ps70dji\Desktop\GitHub\P_GestMDP_114\P_GestMDP\password";

                        try
                        {
                            // Vérifie si le répertoire existe
                            if (Directory.Exists(deleteDirectoryPath))
                            {
                                // Récupère tous les fichiers du répertoire
                                string[] files = Directory.GetFiles(deleteDirectoryPath, "*");

                                if (files.Length == 0)
                                {
                                    // Si aucun fichier n'est trouvé
                                    Console.WriteLine("Aucun mot de passe trouvé à supprimer.");
                                }
                                else
                                {
                                    // Affiche les fichiers disponibles
                                    Console.WriteLine("Vos mots de passe :\n");
                                    for (int i = 0; i < files.Length; i++)
                                    {
                                        Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                                    }

                                    // Demande à l'utilisateur quel fichier il veut supprimer
                                    Console.Write("\nQuel mot de passe voulez-vous supprimer ? (entrez le numéro) : ");
                                    string choix = Console.ReadLine();

                                    // Vérifie que le choix est valide
                                    if (int.TryParse(choix, out int index) && index > 0 && index <= files.Length)
                                    {
                                        // Supprime le fichier sélectionné
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
                                // Si le répertoire n'existe pas
                                Console.WriteLine("Le répertoire des mots de passe n'existe pas.");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Gère les erreurs lors de la suppression du fichier
                            Console.WriteLine("Une erreur s'est produite lors de la suppression du fichier : " + ex.Message);
                        }

                        Console.ReadLine();
                        break;

                    case "4":  // Cas où l'utilisateur veut quitter le programme
                        Environment.Exit(0);
                        break;

                    default:  // Cas où l'utilisateur entre une option invalide
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
    }
}
