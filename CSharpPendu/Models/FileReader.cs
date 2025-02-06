namespace CSharpPendu.Models;

public struct FileReader
{
    public string[] ReadFile (string path)
    {
        if (!File.Exists (path)) throw new FileNotFoundException ($"Le fichier est introuvable: {path}");
        return File.ReadAllLines (path);
    }
}
