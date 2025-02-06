using System.ComponentModel.DataAnnotations;

namespace CSharpPendu.Models;

public struct Pendu
{
    string Mot;
    char[] Lettres;
    bool[] LettresTrouvees;

    List<char> LettresProposees;

    const int MAX_TENTATIVES = 8;
    int NbTentatives;

    private void Initialiser()
    {
        // Récupération des mots présents dans un fichier
        FileReader fileReader = new FileReader();
        string[] mots = fileReader.ReadFile("Data/mots.txt");

        // Initialisation des champs sur base d'un mot sélectionné aléatoirement dans le tableau de mots
        Mot = mots[Random.Shared.Next(mots.Length)].ToUpper();
        Lettres = Mot.ToCharArray();
        LettresTrouvees = new bool[Mot.Length];
        LettresProposees = new List<char>();
        NbTentatives = 0;
    }

    private void AfficherEtat()
    {
        Console.Clear();
        Console.WriteLine($"\n╔══════════════════╗");
        Console.WriteLine($"║ Projet - Pendu   ║");
        Console.WriteLine($"╚══════════════════╝\n");

        AfficherPendu();

        string format = "";
        for (int i = 0; i < Lettres.Length; i++)
        {
            if (LettresTrouvees[i])
            {
                format += Lettres[i];
            }
            else
            {
                format += "-";
            }

            format += " ";
        }

        Console.WriteLine($"\nMot à deviner: {format}");
        Console.WriteLine($"Tentatives restantes: {MAX_TENTATIVES - NbTentatives}");
        Console.WriteLine($"Lettres proposées: [ {string.Join(", ", LettresProposees)} ]");
    }

    public void Jouer()
    {
        bool rejouer = true;
        while (rejouer)
        {
            // Initialisation de la partie
            Initialiser();


            while (!VerifierMotTrouve() && (MAX_TENTATIVES - NbTentatives) > 0)
            {
                AfficherEtat();
                DevinerLettre();
            }

            VerificationVictoire();

            Console.WriteLine($"\nVoulez-vous rejouer ? (true/false)");
            bool valide = false;
            while (!valide)
            {
                valide = bool.TryParse(Console.ReadLine(), out rejouer);
                if (!valide)
                {
                    Console.WriteLine("Entrée invalide. Vous devez entrer 'true' ou 'false'.");
                }
            }

        }
    }

    private void VerificationVictoire()
    {
        Console.Clear();

        if (VerifierMotTrouve())
        {
            Console.WriteLine($"\nFéliciations, vous avez trouvé le mot \"{Mot}\" en {NbTentatives} tentative(s).");
        }
        else
        {
            AfficherPendu();
            Console.WriteLine($"\nPerdu ! Le mot a deviné était {Mot}");
        }
    }

    private void DevinerLettre()
    {
        Console.Write("\nEntrez une lettre: ");
        char lettre = ' ';
        while (!char.IsLetter(lettre) || LettresProposees.Contains(lettre))
        {
            lettre = char.ToUpper(Console.ReadKey().KeyChar);
            if (!char.IsLetter(lettre)) Console.Write("\nCaractère incorrect. Reessayé: ");
            if (LettresProposees.Contains(lettre)) Console.WriteLine($"\nLa lettre a déjà été proposée.");
        }

        lettre = char.ToUpper(lettre);

        if (!Lettres.Contains(lettre))
        {
            LettresProposees.Add(lettre);
            NbTentatives++;
        }
        else
        {
            for (int i = 0; i < Lettres.Length; i++)
            {
                if (Lettres[i] == lettre)
                {
                    LettresTrouvees[i] = true;
                }
            }
        }

    }

    private bool VerifierMotTrouve()
    {
        for (int i = 0; i < LettresTrouvees.Length; i++)
        {
            if (!LettresTrouvees[i]) return false;
        }
        return true;
    }

    private void AfficherPendu()
    {
        string[] pendu = {
        "  _______\n |       |\n |       O\n |      /|\\\n |      / \\\n |\n_|_",
        "  _______\n |       |\n |       O\n |      /|\\\n |      / \n |\n_|_",
        "  _______\n |       |\n |       O\n |      /|\\\n |       \n |\n_|_",
        "  _______\n |       |\n |       O\n |      /|\n |       \n |\n_|_",
        "  _______\n |       |\n |       O\n |       |\n |       \n |\n_|_",
        "  _______\n |       |\n |       O\n |       \n |       \n |\n_|_",
        "  _______\n |       |\n |       \n |       \n |       \n |\n_|_",
        "  _______\n |       \n |       \n |       \n |       \n |\n_|_",
        "  \n |       \n |       \n |       \n |       \n |\n_|_",
        "  \n |       \n |       \n |       \n |       \n \n_|_",
        "  \n \n \n \n \n \n_|_"
        };

        Console.WriteLine(pendu[MAX_TENTATIVES - NbTentatives]);
    }

}
