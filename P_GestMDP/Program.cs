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

            char Reponse; // Variable pour le recommencement du programme

            do
            {
                Console.Clear();
                Console.Write("*******************************" +
                    "\nSélectionnez une action" +
                    "\n1. Consulter les mots de passe" +
                    "\n2. Ajouter un mot de passe" +
                    "\n3. Supprimer un mot de passe" +
                    "\n4. Quitter le programme" +
                    "\n*******************************" +
                    "\n\nFaites votre choix : ");

                string Choix = Console.ReadLine();

                // Utilisation du switch pour gérer les choix
                switch (Choix)
                {
                    case "1":
                        Console.Clear();
                        string directoryPath = @"C:\Users\ps70dji\Desktop\GitHub\P_GestMDP_114\P_GestMDP\password";

                        try
                        {
                            // Vérifier si le répertoire existe
                            if (Directory.Exists(directoryPath))
                            {
                                // Obtenir tous les fichiers texte dans le répertoire
                                string[] files = Directory.GetFiles(directoryPath, "*");

                                if (files.Length == 0)
                                {
                                    Console.WriteLine("Aucun mot de passe trouvé.");
                                }
                                else
                                {
                                    Console.WriteLine("Vos mots de passe :\n");

                                    // Afficher tous les fichiers disponibles
                                    for (int i = 0; i < files.Length; i++)
                                    {
                                        Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                                    }

                                    Console.Write("\nQuel mot de passe voulez-vous consulter et éventuellement modifier ? (entrez le numéro) : ");
                                    string choix = Console.ReadLine();

                                    // Vérification de la validité du choix
                                    if (int.TryParse(choix, out int index) && index > 0 && index <= files.Length)
                                    {
                                        // Lire le contenu du fichier choisi
                                        string filePath = files[index - 1];
                                        string fileContent = File.ReadAllText(filePath);
                                        Console.WriteLine("\nContenu actuel :\n" + fileContent);

                                        // Demander les nouvelles informations
                                        Console.Write("Veuillez entrer la nouvelle URL (laisser vide pour conserver l'ancienne) : ");
                                        string newUrl = Console.ReadLine();
                                        Console.Write("Veuillez entrer le nouvel identifiant (laisser vide pour conserver l'ancien) : ");
                                        string newIdentifiant = Console.ReadLine();
                                        Console.Write("Veuillez entrer le nouveau mot de passe (laisser vide pour conserver l'ancien) : ");
                                        string newMotDePasse = Console.ReadLine();

                                        // Modifications du contenu
                                        string[] lines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                                        if (!string.IsNullOrWhiteSpace(newUrl))
                                        {
                                            lines[0] = "URL : " + newUrl;

                                            // Créer le nouveau nom de fichier directement à partir de la nouvelle URL
                                            string newFileName = newUrl;

                                            // Déterminer le chemin du nouveau fichier
                                            string newFilePath = Path.Combine(directoryPath, newFileName);

                                            // Supprimer l'ancien fichier
                                            File.Delete(filePath);

                                            // Écrire les modifications dans le nouveau fichier
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

                                            // Écrire les modifications dans le fichier existant
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
                        break;


                    case "2":
                        Console.Clear();
                        Console.Write("Veuillez entrez l'URL du site : ");
                        string url = Console.ReadLine();

                        Console.Write("Veuillez entrer l'identifiant : ");
                        string identifiant = Console.ReadLine();

                        Console.Write("Veuillez entrer le mot de passe : ");
                        string motDePasse = Console.ReadLine();

                        // Chemin du fichier avec le nom basé sur l'URL
                        string pathFile = $@"C:\Users\ps70dji\Desktop\GitHub\P_GestMDP_114\P_GestMDP\password\{url}";

                        try
                        {
                            // Créer ou ajouter au fichier texte
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
                        break;


                    case "3":
                        Console.Clear();
                        string deleteDirectoryPath = @"C:\Users\ps70dji\Desktop\GitHub\P_GestMDP_114\P_GestMDP\password";

                        try
                        {
                            // Vérifier si le répertoire existe
                            if (Directory.Exists(deleteDirectoryPath))
                            {
                                // Obtenir tous les fichiers texte dans le répertoire
                                string[] files = Directory.GetFiles(deleteDirectoryPath, "*");

                                if (files.Length == 0)
                                {
                                    Console.WriteLine("Aucun mot de passe trouvé à supprimer.");
                                }
                                else
                                {
                                    Console.WriteLine("Vos mots de passe :\n");

                                    // Afficher tous les fichiers disponibles
                                    for (int i = 0; i < files.Length; i++)
                                    {
                                        Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                                    }

                                    Console.Write("\nQuel mot de passe voulez-vous supprimer ? (entrez le numéro) : ");
                                    string choix = Console.ReadLine();

                                    // Vérification de la validité du choix
                                    if (int.TryParse(choix, out int index) && index > 0 && index <= files.Length)
                                    {
                                        // Supprimer le fichier correspondant
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
                        break;


                    case "4":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Choix invalide");
                        break;
                }

                // Affichage et saisie pour recommencer
                Console.Write("\nVoulez-vous relancer le programme (o / n): ");
                Reponse = Convert.ToChar(Console.ReadLine());
            }//Fin do

            // Boucle de recommencement
            while (Reponse == 'o' || Reponse == '0');
        }
    }
}
