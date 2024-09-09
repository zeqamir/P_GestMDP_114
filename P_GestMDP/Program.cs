/// ETML - Vennes
/// Auteur : Amir Zeqiri
/// Date : 26.08.2024 - 28.10.2024
/// Description : Gestionnaire de mot de passe - 114

using System;

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
                        Console.WriteLine("Vos mots de passe : " +
                            "\n\n 1." +
                            "\n URL : www.login.com" +
                            "\n Identifiant : user" +
                            "\n Mot de passe (non chiffré) : password" +
                            "\n\n 2." +
                            "\n URL : www.google.com" +
                            "\n Identifiant : admin" +
                            "\n Mot de passe (non chiffré) : pass" +
                            "\n\n\n Quel mot de passe voulez-vous modifier ? : ");
                        Console.Read();
                        break;

                    case "2":
                        Console.Clear();
                        Console.Write("Veuillez entrez l'URL du site : ");
                        Console.Read();
                        break;

                    case "3":
                        Console.WriteLine("Vos mots de passe : " +
                            "\n\n 1." +
                            "\n URL : www.login.com" +
                            "\n Identifiant : user" +
                            "\n Mot de passe (non chiffré) : password" +
                            "\n\n 2." +
                            "\n URL : www.google.com" +
                            "\n Identifiant : admin" +
                            "\n Mot de passe (non chiffré) : pass" +
                            "\n\n\n Quel mot de passe voulez-vous supprimer ? : ");
                        Console.Read();
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
