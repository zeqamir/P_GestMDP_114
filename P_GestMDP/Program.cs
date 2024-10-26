using System;
using System.IO;

namespace P_GestMDP
{
    class Program
    {
        // Chemin du fichier où le master password chiffré est stocké
        static string masterPasswordFilePath = @"C:\Users\tvami\OneDrive\Bureau\GitHub\P_GestMDP_114\P_GestMDP\MasterPassword\masterpassword";
        // Clé de chiffrement utilisée par l'algorithme de Vigenère pour chiffrer et déchiffrer le master password et les mots de passe
        static string vigenereKey = "maCleDeChiffrement";

        static void Main(string[] args)
        {
            Console.Clear();

            // Vérifie si le master password est correct
            if (!CheckMasterPassword())
            {
                return;
            }

            char Reponse;

            do
            {
                Console.Clear();

                // Menu principal d'options
                Console.Write("*******************************" +
                    "\nSélectionnez une action" +
                    "\n1. Consulter les mots de passe" +
                    "\n2. Ajouter un mot de passe" +
                    "\n3. Supprimer un mot de passe" +
                    "\n4. Modifier un mot de passe" + // Nouvelle option
                    "\n5. Quitter le programme" +
                    "\n*******************************" +
                    "\n\nFaites votre choix : ");

                string Choix = Console.ReadLine();

                // Switch pour gérer chaque option
                switch (Choix)
                {
                    case "1":
                        ConsulterMotDePasse();
                        break;

                    case "2":
                        AjouterMotDePasse();
                        break;

                    case "3":
                        SupprimerMotDePasse();
                        break;

                    case "4":
                        ModifierMotDePasse(); // Appelle la fonction de modification
                        break;

                    case "5":
                        Environment.Exit(0); // Ferme le programme
                        break;

                    default:
                        Console.WriteLine("Choix invalide");
                        break;
                }

                Console.Write("\nVoulez-vous relancer le programme (o / n): ");
                Reponse = Convert.ToChar(Console.ReadLine());
            }
            while (Reponse == 'o' || Reponse == '0');
        }

        // Vérifie le master password ou le crée s'il n'existe pas
        static bool CheckMasterPassword()
        {
            if (File.Exists(masterPasswordFilePath))
            {
                Console.Write("Veuillez entrer le master password : ");
                string inputMasterPassword = Console.ReadLine();

                string encryptedMasterPassword = File.ReadAllText(masterPasswordFilePath);
                string decryptedMasterPassword = VigenereDecrypt(encryptedMasterPassword, vigenereKey);

                if (inputMasterPassword == decryptedMasterPassword)
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
                Console.WriteLine("Aucun master password trouvé.");
                Console.Write("Veuillez créer un master password : ");
                string newMasterPassword = Console.ReadLine();

                string encryptedMasterPassword = VigenereEncrypt(newMasterPassword, vigenereKey);
                File.WriteAllText(masterPasswordFilePath, encryptedMasterPassword);

                Console.WriteLine("Master password enregistré.");
                return true;
            }
        }

        // Fonction pour consulter un mot de passe
        static void ConsulterMotDePasse()
        {
            Console.Clear();
            string directoryPath = @"C:\Users\tvami\OneDrive\Bureau\GitHub\P_GestMDP_114\P_GestMDP\password";

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
                        Console.WriteLine("Vos mots de passe chiffrés :\n");
                        for (int i = 0; i < files.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                        }

                        Console.Write("\nQuel mot de passe voulez-vous consulter ? (entrez le numéro) : ");
                        string choix = Console.ReadLine();

                        if (int.TryParse(choix, out int index) && index > 0 && index <= files.Length)
                        {
                            string filePath = files[index - 1];
                            string fileContent = File.ReadAllText(filePath);

                            string decryptedContent = VigenereDecrypt(fileContent, vigenereKey);
                            Console.WriteLine("\n\n" + decryptedContent);
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

        // Fonction pour ajouter un mot de passe
        static void AjouterMotDePasse()
        {
            Console.Clear();
            Console.Write("Veuillez entrer l'URL du site : ");
            string url = Console.ReadLine();

            Console.Write("Veuillez entrer l'identifiant : ");
            string identifiant = Console.ReadLine();

            Console.Write("Veuillez entrer le mot de passe : ");
            string motDePasse = Console.ReadLine();

            string encryptedData = VigenereEncrypt($"URL : {url}\nIdentifiant : {identifiant}\nMot de passe : {motDePasse}", vigenereKey);
            string pathFile = $@"C:\Users\tvami\OneDrive\Bureau\GitHub\P_GestMDP_114\P_GestMDP\password\{url}";

            try
            {
                File.WriteAllText(pathFile, encryptedData);
                Console.WriteLine($"\nLe mot de passe a été chiffré et enregistré dans le fichier '{url}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite lors de l'enregistrement : " + ex.Message);
            }

            Console.ReadLine();
        }

        // Fonction pour supprimer un mot de passe
        static void SupprimerMotDePasse()
        {
            Console.Clear();
            string deleteDirectoryPath = @"C:\Users\tvami\OneDrive\Bureau\GitHub\P_GestMDP_114\P_GestMDP\password";

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

        // Méthode pour modifier un mot de passe existant
        static void ModifierMotDePasse()
        {
            Console.Clear();
            string directoryPath = @"C:\Users\tvami\OneDrive\Bureau\GitHub\P_GestMDP_114\P_GestMDP\password";

            try
            {
                if (Directory.Exists(directoryPath))
                {
                    string[] files = Directory.GetFiles(directoryPath, "*");
                    if (files.Length == 0)
                    {
                        Console.WriteLine("Aucun mot de passe trouvé.");
                        return;
                    }

                    Console.WriteLine("Vos mots de passe :\n");
                    for (int i = 0; i < files.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                    }

                    Console.Write("\nQuel mot de passe voulez-vous modifier ? (entrez le numéro) : ");
                    string choix = Console.ReadLine();

                    if (int.TryParse(choix, out int index) && index > 0 && index <= files.Length)
                    {
                        string filePath = files[index - 1];
                        string fileContent = File.ReadAllText(filePath);

                        string decryptedContent = VigenereDecrypt(fileContent, vigenereKey);
                        Console.WriteLine("\nMot de passe actuel :\n" + decryptedContent);

                        // Demander les nouvelles informations
                        Console.Write("Veuillez entrer la nouvelle URL du site (laisser vide pour ne pas changer) : ");
                        string newUrl = Console.ReadLine();
                        Console.Write("Veuillez entrer le nouvel identifiant (laisser vide pour ne pas changer) : ");
                        string newIdentifiant = Console.ReadLine();
                        Console.Write("Veuillez entrer le nouveau mot de passe (laisser vide pour ne pas changer) : ");
                        string newMotDePasse = Console.ReadLine();

                        // Mettre à jour les informations
                        string updatedUrl = string.IsNullOrEmpty(newUrl) ? decryptedContent.Split('\n')[0] : $"URL : {newUrl}";
                        string updatedIdentifiant = string.IsNullOrEmpty(newIdentifiant) ? decryptedContent.Split('\n')[1] : $"Identifiant : {newIdentifiant}";
                        string updatedMotDePasse = string.IsNullOrEmpty(newMotDePasse) ? decryptedContent.Split('\n')[2] : $"Mot de passe : {newMotDePasse}";

                        // Chiffre et enregistre les nouvelles informations
                        string encryptedData = VigenereEncrypt($"{updatedUrl}\n{updatedIdentifiant}\n{updatedMotDePasse}", vigenereKey);
                        File.WriteAllText(filePath, encryptedData);

                        Console.WriteLine("Le mot de passe a été mis à jour avec succès.");
                    }
                    else
                    {
                        Console.WriteLine("Choix invalide.");
                    }
                }
                else
                {
                    Console.WriteLine("Le répertoire des mots de passe n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite lors de la modification du mot de passe : " + ex.Message);
            }

            Console.ReadLine();
        }

        // Méthode de chiffrement Vigenère
        static string VigenereEncrypt(string text, string key)
        {
            char[] output = new char[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                output[i] = (char)(((text[i] + key[i % key.Length]) % 128));
            }
            return new string(output);
        }

        // Déchiffrement Vigenère du texte avec la clé donnée
        static string VigenereDecrypt(string text, string key)
        {
            char[] output = new char[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                output[i] = (char)(((text[i] - key[i % key.Length] + 128) % 128));
            }
            return new string(output);
        }
    }
}
