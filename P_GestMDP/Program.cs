///ETML 
///Auteur : Amir Zeqiri
///Date : 30.10.2024 
///Description : Gestionnaire de mot de passe avec un algorithme de chiffrage en Vigenère


using System;
using System.IO;

namespace P_GestMDP
{
    class Program
    {
        // Chemin du fichier où le master password chiffré est stocké
        static string masterPasswordFilePath = @"C:\Users\amizeqiri\Desktop\GitHub\P_GestMDP_114\P_GestMDP\MasterPassword\masterpassword";
        // Clé de chiffrement utilisée par l'algorithme de Vigenère pour chiffrer et déchiffrer le master password et les mots de passe
        static string vigenereKey = "MACLEDECHIFFREMENT";

        static void Main(string[] args)
        {
            Console.Clear();

            // Si la fonction de vérification du Master Passsword n'est pas 'true' alors ça retourne rien
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
                    "\n4. Modifier un mot de passe" +
                    "\n5. Quitter le programme" +
                    "\n*******************************" +
                    "\n\nFaites votre choix : ");

                string Choix = Console.ReadLine();

                // Switch pour gérer chaque option
                switch (Choix)
                {
                    case "1":
                        ConsulterMotDePasse(); // Appelle la fonction de consultation
                        break;

                    case "2":
                        AjouterMotDePasse(); // Appelle la fonction d'ajout
                        break;

                    case "3":
                        SupprimerMotDePasse(); // Appelle la fonction de supression
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

        // Vérifie le master password ou le crée si il n'existe pas
        /// <summary>
        /// Vérifie l'existence du master password et le valide si déjà enregistré. 
        /// Si aucun master password n'est trouvé, permet à l'utilisateur d'en créer un et l'enregistre.
        /// </summary>
        /// <returns>Retourne true si le master password est validé ou créé avec succès, false en cas d'échec de validation.</returns>
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
        /// <summary>
        /// Affiche la liste des mots de passe chiffrés avec Vigenère disponibles dans le répertoire spécifié.
        /// Permet de sélectionner et de consulter un mot de passe en le déchiffrant.
        /// </summary>
        /// <exception cref="Exception">Lancée si une erreur survient lors de la lecture des fichiers du répertoire.</exception>
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
        /// <summary>
        /// Ajoute un nouveau mot de passe en demandant à l'utilisateur l'URL, l'identifiant, et le mot de passe associé.
        /// Chiffre les informations et les enregistre dans un fichier portant le nom de l'URL dans le répertoire spécifié.
        /// </summary>
        /// <exception cref="Exception">Lancée si une erreur survient lors de l'enregistrement du fichier.</exception>
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

            string pathFile = $@"C:\Users\amizeqiri\Desktop\GitHub\P_GestMDP_114\P_GestMDP\password\{url}";

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
        /// <summary>
        /// Affiche la liste des mots de passe disponibles et permet de sélectionner un fichier de mot de passe à supprimer.
        /// Supprime le fichier correspondant du répertoire spécifié.
        /// </summary>
        /// <exception cref="Exception">Lancée si une erreur survient lors de la suppression du fichier.</exception>
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

        // Fonction pour modifier un mot de passe existant
        /// <summary>
        /// Permet à l'utilisateur de modifier un mot de passe existant en affichant la liste des mots de passe disponibles.
        /// Affiche le mot de passe actuel et demande de nouvelles informations (URL, identifiant, mot de passe) à l'utilisateur.
        /// Chiffre les nouvelles informations et les enregistre dans le même fichier.
        /// </summary>
        /// <exception cref="Exception">Lancée si une erreur survient lors de la modification ou de l'enregistrement du mot de passe.</exception>
        static void ModifierMotDePasse()
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
        /// <summary>
        /// Chiffre une chaîne de caractères en utilisant la méthode de chiffrement de Vigenère.
        /// Chaque caractère de la chaîne d'entrée est décalé en fonction de la clé fournie.
        /// </summary>
        /// <param name="text">Le texte à chiffrer que l'utilisateur inscrit.</param>
        /// <param name="key">La clé utilisée pour le chiffrement.</param>
        /// <returns>Le texte chiffré.</returns>
        static string VigenereEncrypt(string text, string key)
        {
            char[] output = new char[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                output[i] = (char)(((text[i] + key[i % key.Length]) % 128));
            }
            return new string(output);
        }

        // Méthode de déchiffrement Vigenère du texte avec la clé donnée
        /// <summary>
        /// Déchiffre une chaîne de caractères en utilisant la méthode de déchiffrement de Vigenère.
        /// Chaque caractère du texte chiffré est décalé en fonction de la clé pour retrouver le texte original.
        /// </summary>
        /// <param name="text">Le texte chiffré à déchiffrer.</param>
        /// <param name="key">La clé utilisée pour le déchiffrement.</param>
        /// <returns>Le texte déchiffré.</returns>
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
