namespace transactions_financieres.Validation
{
    // Résultat d'une validation : la liste des erreurs (vide = valide).
    // Choix : plutôt qu'un bool (on veut savoir pourquoi) ou une exception (un refus est normal).
    public sealed class ValidationResult
    {
        

        // Les erreurs, en lecture seule : personne ne peut les modifier.
        public IReadOnlyList<string> Erreurs { get; }

        // Valide s'il n'y a aucune erreur.
        public bool EstValide => Erreurs.Count == 0;

        // Reçoit les erreurs et en fait une copie.
        public ValidationResult(IEnumerable<string> erreurs) => Erreurs = erreurs.ToList();

        // Crée un résultat sans erreur.
        public static ValidationResult Succes() => new(Array.Empty<string>());

        // Crée un résultat avec une erreur.
        public static ValidationResult Echec(string message) => new(new[] { message });
    }
}