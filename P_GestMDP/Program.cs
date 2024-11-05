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

        // Vérifie le master password ou le crée s'il n'existe pas
        /// <summary>
        /// Vérifie l'existence du master password et le valide si déjà enregistré. 
        /// Si aucun master password n'est trouvé, permet à l'utilisateur d'en créer un et l'enregistre.
        /// </summary>
        /// <returns>Retourne true si le master password est validé ou créé avec succès, false en cas d'échec de validation.</returns>
        static bool CheckMasterPassword()
        {
            // Vérifie si le fichier contenant le master password existe déjà
            if (File.Exists(masterPasswordFilePath))
            {
                // Demande à l'utilisateur de saisir le master password pour validation
                Console.Write("Veuillez entrer le master password : ");
                string inputMasterPassword = Console.ReadLine();

                // Lit le mot de passe chiffré depuis le fichier et le déchiffre
                string encryptedMasterPassword = File.ReadAllText(masterPasswordFilePath);
                string decryptedMasterPassword = VigenereDecrypt(encryptedMasterPassword, vigenereKey);

                // Vérifie si le mot de passe saisi correspond au mot de passe déchiffré
                if (inputMasterPassword == decryptedMasterPassword)
                {
                    // Accès accordé si le mot de passe est correct
                    Console.WriteLine("Accès accordé.");
                    return true;
                }
                else
                {
                    // Message d'erreur si le mot de passe saisi est incorrect
                    Console.WriteLine("Mot de passe incorrect.");
                    return false;
                }
            }
            else
            {
                // Indique à l'utilisateur qu'aucun master password n'est enregistré
                Console.WriteLine("Aucun master password trouvé.");

                // Demande à l'utilisateur de créer un nouveau master password
                Console.Write("Veuillez créer un master password : ");
                string newMasterPassword = Console.ReadLine();

                // Chiffre le nouveau master password et l'enregistre dans le fichier
                string encryptedMasterPassword = VigenereEncrypt(newMasterPassword, vigenereKey);
                File.WriteAllText(masterPasswordFilePath, encryptedMasterPassword);

                // Confirmation de l'enregistrement du nouveau master password
                Console.WriteLine("Master password enregistré.");
                return true;
            }
        }


        // Fonction pour consulter un mot de passe
        /// <summary>
        /// Affiche la liste des mots de passe chiffrés avec l'algorithme de Vigenère disponibles dans le répertoire spécifié.
        /// Permet à l'utilisateur de sélectionner et de consulter un mot de passe en le déchiffrant.
        /// </summary>
        /// <exception cref="Exception">Lancée si une erreur survient lors de la lecture des fichiers du répertoire.</exception>
        static void ConsulterMotDePasse()
        {
            // Efface le contenu de la console pour une interface plus propre
            Console.Clear();

            // Définition du chemin du répertoire où sont stockés les mots de passe chiffrés
            string directoryPath = @"C:\Users\amizeqiri\Desktop\GitHub\P_GestMDP_114\P_GestMDP\password";

            try
            {
                // Vérifie si le répertoire des mots de passe existe
                if (Directory.Exists(directoryPath))
                {
                    // Récupère tous les fichiers du répertoire
                    string[] files = Directory.GetFiles(directoryPath, "*");

                    // Si aucun fichier n'est trouvé, affiche un message informant l'utilisateur
                    if (files.Length == 0)
                    {
                        Console.WriteLine("Aucun mot de passe trouvé.");
                    }
                    else
                    {
                        // Affiche la liste des mots de passe chiffrés trouvés dans le répertoire
                        Console.WriteLine("Vos mots de passe chiffrés :\n");
                        for (int i = 0; i < files.Length; i++)
                        {
                            // Affiche chaque fichier avec un numéro associé pour faciliter la sélection
                            Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                        }

                        // Demande à l'utilisateur de sélectionner un mot de passe à consulter
                        Console.Write("\nQuel mot de passe voulez-vous consulter ? (entrez le numéro) : ");
                        string choix = Console.ReadLine();

                        // Valide la sélection de l'utilisateur et vérifie que l'index est dans la plage des fichiers
                        if (int.TryParse(choix, out int index) && index > 0 && index <= files.Length)
                        {
                            // Lit le contenu du fichier de mot de passe sélectionné
                            string filePath = files[index - 1];
                            string fileContent = File.ReadAllText(filePath);

                            // Déchiffre le contenu du mot de passe avec la clé Vigenère
                            string decryptedContent = VigenereDecrypt(fileContent, vigenereKey);

                            // Affiche le mot de passe déchiffré à l'utilisateur
                            Console.WriteLine("\n\n" + decryptedContent);
                        }
                        else
                        {
                            // Message d'erreur si la sélection de l'utilisateur est invalide
                            Console.WriteLine("Choix invalide.");
                        }
                    }
                }
                else
                {
                    // Message d'erreur si le répertoire de mots de passe n'existe pas
                    Console.WriteLine("Le répertoire des mots de passe n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                // Gestion des erreurs : affiche un message si une exception se produit pendant la lecture des fichiers
                Console.WriteLine("Une erreur s'est produite lors de la lecture des fichiers : " + ex.Message);
            }

            // Pause pour permettre à l'utilisateur de lire le résultat avant de revenir au menu
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

            // Demande à l'utilisateur d'entrer l'URL du site pour lequel le mot de passe sera enregistré
            Console.Write("Veuillez entrer l'URL du site : ");
            string url = Console.ReadLine();

            // Demande à l'utilisateur d'entrer l'identifiant associé au site
            Console.Write("Veuillez entrer l'identifiant : ");
            string identifiant = Console.ReadLine();

            // Demande à l'utilisateur d'entrer le mot de passe à enregistrer
            Console.Write("Veuillez entrer le mot de passe : ");
            string motDePasse = Console.ReadLine();

            // Chiffre les informations (URL, identifiant et mot de passe) avec l'algorithme de Vigenère
            string encryptedData = VigenereEncrypt($"URL : {url}\nIdentifiant : {identifiant}\nMot de passe : {motDePasse}", vigenereKey);

            // Définit le chemin du fichier de destination en utilisant l'URL comme nom de fichier
            string pathFile = $@"C:\Users\amizeqiri\Desktop\GitHub\P_GestMDP_114\P_GestMDP\password\{url}";

            try
            {
                // Enregistre les données chiffrées dans le fichier spécifié
                File.WriteAllText(pathFile, encryptedData);

                // Confirme que les informations ont été chiffrées et enregistrées
                Console.WriteLine($"\nLe mot de passe a été chiffré et enregistré dans le fichier '{url}'.");
            }
            catch (Exception ex)
            {
                // Affiche un message d'erreur si un problème survient lors de l'enregistrement
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
        // Fonction pour supprimer un mot de passe
        /// <summary>
        /// Affiche la liste des mots de passe disponibles et permet de sélectionner un fichier de mot de passe à supprimer.
        /// Supprime le fichier correspondant du répertoire spécifié.
        /// </summary>
        /// <exception cref="Exception">Lancée si une erreur survient lors de la suppression du fichier.</exception>
        static void SupprimerMotDePasse()
        {
            Console.Clear();

            // Chemin du répertoire contenant les fichiers de mots de passe à supprimer
            string deleteDirectoryPath = @"C:\Users\amizeqiri\Desktop\GitHub\P_GestMDP_114\P_GestMDP\password";

            try
            {
                // Vérifie si le répertoire existe
                if (Directory.Exists(deleteDirectoryPath))
                {
                    // Récupère tous les fichiers du répertoire
                    string[] files = Directory.GetFiles(deleteDirectoryPath, "*");

                    // Si aucun fichier n'est trouvé, informe l'utilisateur
                    if (files.Length == 0)
                    {
                        Console.WriteLine("Aucun mot de passe trouvé à supprimer.");
                    }
                    else
                    {
                        // Affiche la liste des mots de passe disponibles
                        Console.WriteLine("Vos mots de passe :\n");
                        for (int i = 0; i < files.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                        }

                        // Demande à l'utilisateur de choisir un fichier à supprimer en entrant un numéro
                        Console.Write("\nQuel mot de passe voulez-vous supprimer ? (entrez le numéro) : ");
                        string choix = Console.ReadLine();

                        // Vérifie si le choix est valide et dans les limites de la liste
                        if (int.TryParse(choix, out int index) && index > 0 && index <= files.Length)
                        {
                            // Supprime le fichier sélectionné
                            File.Delete(files[index - 1]);
                            Console.WriteLine("Le mot de passe a été supprimé avec succès.");
                        }
                        else
                        {
                            // Informe l'utilisateur si le choix est invalide
                            Console.WriteLine("Choix invalide.");
                        }
                    }
                }
                else
                {
                    // Informe l'utilisateur si le répertoire n'existe pas
                    Console.WriteLine("Le répertoire des mots de passe n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                // Affiche un message d'erreur si un problème survient lors de la suppression
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

            // Chemin du répertoire contenant les fichiers de mots de passe à modifier
            string directoryPath = @"C:\Users\amizeqiri\Desktop\GitHub\P_GestMDP_114\P_GestMDP\password";

            try
            {
                // Vérifie si le répertoire existe
                if (Directory.Exists(directoryPath))
                {
                    // Récupère tous les fichiers du répertoire
                    string[] files = Directory.GetFiles(directoryPath, "*");

                    // Si aucun fichier n'est trouvé, informe l'utilisateur
                    if (files.Length == 0)
                    {
                        Console.WriteLine("Aucun mot de passe trouvé.");
                        return;
                    }

                    // Affiche la liste des mots de passe disponibles
                    Console.WriteLine("Vos mots de passe :\n");
                    for (int i = 0; i < files.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                    }

                    // Demande à l'utilisateur de choisir un fichier à modifier en entrant un numéro
                    Console.Write("\nQuel mot de passe voulez-vous modifier ? (entrez le numéro) : ");
                    string choix = Console.ReadLine();

                    // Vérifie si le choix est valide et dans les limites de la liste
                    if (int.TryParse(choix, out int index) && index > 0 && index <= files.Length)
                    {
                        // Chemin du fichier sélectionné
                        string filePath = files[index - 1];
                        // Lit le contenu chiffré du fichier et le déchiffre
                        string fileContent = File.ReadAllText(filePath);
                        string decryptedContent = VigenereDecrypt(fileContent, vigenereKey);
                        Console.WriteLine("\nMot de passe actuel :\n" + decryptedContent);

                        // Demander les nouvelles informations, en laissant vide pour garder les valeurs actuelles
                        Console.Write("Veuillez entrer la nouvelle URL du site (laisser vide pour ne pas changer) : ");
                        string newUrl = Console.ReadLine();
                        Console.Write("Veuillez entrer le nouvel identifiant (laisser vide pour ne pas changer) : ");
                        string newIdentifiant = Console.ReadLine();
                        Console.Write("Veuillez entrer le nouveau mot de passe (laisser vide pour ne pas changer) : ");
                        string newMotDePasse = Console.ReadLine();

                        // Mettre à jour les informations en utilisant les nouvelles données si elles sont fournies
                        string updatedUrl = string.IsNullOrEmpty(newUrl) ? decryptedContent.Split('\n')[0] : $"URL : {newUrl}";
                        string updatedIdentifiant = string.IsNullOrEmpty(newIdentifiant) ? decryptedContent.Split('\n')[1] : $"Identifiant : {newIdentifiant}";
                        string updatedMotDePasse = string.IsNullOrEmpty(newMotDePasse) ? decryptedContent.Split('\n')[2] : $"Mot de passe : {newMotDePasse}";

                        // Chiffre et enregistre les nouvelles informations dans le même fichier
                        string encryptedData = VigenereEncrypt($"{updatedUrl}\n{updatedIdentifiant}\n{updatedMotDePasse}", vigenereKey);
                        File.WriteAllText(filePath, encryptedData);

                        Console.WriteLine("Le mot de passe a été mis à jour avec succès.");
                    }
                    else
                    {
                        // Informe l'utilisateur si le choix est invalide
                        Console.WriteLine("Choix invalide.");
                    }
                }
                else
                {
                    // Informe l'utilisateur si le répertoire n'existe pas
                    Console.WriteLine("Le répertoire des mots de passe n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                // Affiche un message d'erreur si un problème survient lors de la modification
                Console.WriteLine("Une erreur s'est produite lors de la modification du mot de passe : " + ex.Message);
            }

            Console.ReadLine();
        }


        // Méthode de chiffrement Vigenère du texte avec la clé donnée
        /// <summary>
        /// Chiffre une chaîne de caractères en utilisant la méthode de chiffrement de Vigenère.
        /// Chaque caractère de la chaîne d'entrée est décalé en fonction de la clé fournie.
        /// </summary>
        /// <param name="text">Le texte à chiffrer que l'utilisateur inscrit.</param>
        /// <param name="key">La clé utilisée pour le chiffrement.</param>
        /// <returns>Le texte chiffré.</returns>
        static string VigenereEncrypt(string text, string key)
        {
            // Crée un tableau de caractères pour stocker le texte chiffré
            char[] output = new char[text.Length];

            // Parcourt chaque caractère du texte à chiffrer
            for (int i = 0; i < text.Length; i++)
            {
                // Décale le caractère en fonction du caractère correspondant dans la clé
                // et le limite aux 128 premiers caractères ASCII
                output[i] = (char)(((text[i] + key[i % key.Length]) % 128));
            }

            // Retourne le tableau converti en une chaîne de caractères
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
            // Crée un tableau de caractères pour stocker le texte déchiffré
            char[] output = new char[text.Length];

            // Parcourt chaque caractère du texte chiffré
            for (int i = 0; i < text.Length; i++)
            {
                // Annule le décalage appliqué lors du chiffrement en soustrayant la clé
                // L'opération '+ 128' garantit que le résultat reste positif, même si la soustraction génère un nombre négatif
                output[i] = (char)(((text[i] - key[i % key.Length] + 128) % 128));
            }

            // Retourne le tableau converti en une chaîne de caractères (texte déchiffré)
            return new string(output);
        }
    }
}
