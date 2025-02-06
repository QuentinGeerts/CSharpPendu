using CSharpPendu.Enums;

namespace CSharpPendu.Models;

public struct Pendu
{
    // Champs
    string Mot;
    char[] Lettres;
    bool[] LettresTrouvees;

    int NbPartiesGagnees;
    int NbPartiesPerdues;

    List<char> LettresProposees;
    const int MAX_TENTATIVES = 8;
    int NbTentatives;

    EtatPartie Etat;
    CategorieMot Categorie;

    // Méthodes
    public void Jouer()
    {
        NbPartiesGagnees = 0;
        NbPartiesPerdues = 0;

        bool rejouer = true;
        while (rejouer)
        {
            // Initialisation de la partie
            Initialiser();

            while (Etat == EtatPartie.EnCours)
            {
                AfficherEtat();
                DevinerLettre();
                VerificationVictoire();
            }

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

    private void Initialiser()
    {
        Console.Clear();

        Console.WriteLine("\nChoisissez une catégorie :");
        foreach (object? categorie in Enum.GetValues(typeof(CategorieMot)))
        {
            Console.WriteLine($"- {categorie}");
        }

        bool valide = false;
        while (!valide)
        {
            string choix = Console.ReadLine();
            valide = Enum.TryParse(choix, true, out Categorie);
            if (!valide) Console.WriteLine("Catégorie invalide. Essayez encore.");
        }

        // Récupération des mots présents dans un fichier
        FileReader fileReader = new FileReader();
        string chemin = $"Data/{Categorie.ToString().ToLower()}.txt";
        string[] mots = fileReader.ReadFile(chemin);

        // Initialisation des champs sur base d'un mot sélectionné aléatoirement dans le tableau de mots
        Mot = mots[Random.Shared.Next(mots.Length)].ToUpper();
        Lettres = Mot.ToCharArray();
        LettresTrouvees = new bool[Mot.Length];
        LettresProposees = new List<char>();
        NbTentatives = 0;

        Etat = EtatPartie.EnCours;
    }

    private void AfficherEtat()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine($"╔════════════════════╗");
        Console.WriteLine($"║   Projet - Pendu   ║");
        Console.WriteLine($"╚════════════════════╝");
        Console.WriteLine();

        Console.WriteLine($"Catégorie sélectionnée: {Categorie}");

        Console.WriteLine($"Parties gagnées: {NbPartiesGagnees}");
        Console.WriteLine($"Parties perdues: {NbPartiesPerdues}");

        Console.WriteLine($"Tentatives restantes: {MAX_TENTATIVES - NbTentatives}");

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

        Console.WriteLine($"Mot à deviner: {format}");
        Console.WriteLine($"Lettres proposées: [ {string.Join(", ", LettresProposees)} ]");
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

        LettresProposees.Add(lettre);

    }

    private void VerificationVictoire()
    {
        if (!VerifierMotTrouve() && (MAX_TENTATIVES - NbTentatives) > 0) return;

        Console.Clear();

        if (VerifierMotTrouve())
        {
            Etat = EtatPartie.Gagnee;
            NbPartiesGagnees++;
            Console.WriteLine($"\nFélicitations, vous avez trouvé le mot \"{Mot}\" en {NbTentatives} tentative(s) infructueuse(s).");

        }
        else
        {
            Etat = EtatPartie.Perdue;
            NbPartiesPerdues++;
            AfficherPendu();
            Console.WriteLine($"\nPerdu ! Le mot à deviner était \"{Mot}\".");
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
        "  _______" +
        "\n |       |" +
        "\n |       O" +
        "\n |      /|\\" +
        "\n |      / \\" +
        "\n |" +
        "\n_|_",

        "  _______" +
        "\n |       |" +
        "\n |       O" +
        "\n |      /|\\" +
        "\n |      / " +
        "\n |" +
        "\n_|_",

        "  _______" +
        "\n |       |" +
        "\n |       O" +
        "\n |      /|\\" +
        "\n |       " +
        "\n |" +
        "\n_|_",

        "  _______" +
        "\n |       |" +
        "\n |       O" +
        "\n |      /|" +
        "\n |       " +
        "\n |" +
        "\n_|_",

        "  _______" +
        "\n |       |" +
        "\n |       O" +
        "\n |       |" +
        "\n |       " +
        "\n |" +
        "\n_|_",

        "  _______" +
        "\n |       |" +
        "\n |       O" +
        "\n |       " +
        "\n |       " +
        "\n |" +
        "\n_|_",

        "  _______" +
        "\n |       " +
        "|\n |       " +
        "\n |       " +
        "\n |       " +
        "\n |" +
        "\n_|_",
        "  _______" +
        "\n |       " +
        "\n |       " +
        "\n |       " +
        "\n |       " +
        "\n |" +
        "\n_|_",

        "  " +
        "\n |       " +
        "\n |       " +
        "\n |       " +
        "\n |       " +
        "\n |" +
        "\n_|_",

        "  " +
        "\n |       " +
        "\n |       " +
        "\n |       " +
        "\n |       " +
        "\n " +
        "\n_|_",

        "  " +
        "\n " +
        "\n " +
        "\n " +
        "\n " +
        "\n " +
        "\n_|_"
        };

        Console.WriteLine($"\n{pendu[MAX_TENTATIVES - NbTentatives]}\n");
    }
}
