# Gestion de transactions financières

Application C# (.NET 10) qui enregistre des transactions de crédit et de débit, et les valide selon des règles de gestion. De nouvelles règles peuvent être ajoutées **sans modifier le code existant**.

## Prérequis

- SDK .NET 10
- Visual Studio 2026

## Exécution

```bash
dotnet build      # compiler
dotnet test       # lancer les tests unitaires
dotnet run --project <NomDuProjetConsole>   # lancer la démonstration
```

Dans Visual Studio : ouvrir le fichier `.sln`, puis **F5** pour la démonstration, et **Test → Exécuter tous les tests** pour les tests.

## Règles de gestion

| ID    | Règle                                          | Classe                                               |
|-------|------------------------------------------------|------------------------------------------------------|
| RG-01 | Montant strictement positif                    | `Transaction` (constructeur)                         |
| RG-02 | Description obligatoire                        | `Transaction` (constructeur)                         |
| RG-03 | Date obligatoire                               | `Transaction` (constructeur)                         |
| RG-04 | Un crédit ne peut pas dépasser 10 000          | `MontantMaximumCreditRG`                             |
| RG-05 | Après un débit, le solde reste ≥ -5 000        | `SoldeMinimumApresDebitRG`                           |
| RG-06 | Solde initial à 0, puis somme des transactions | `TransactionService`                                 |
| RG-07 | Une transaction refusée n'est pas enregistrée  | `TransactionService`                                 |
| RG-08 | Toutes les règles sont vérifiées               | `TransactionService`                                 |
| RG-09 | Une règle ne vise que son type de transaction  | `MontantMaximumCreditRG`, `SoldeMinimumApresDebitRG` |
| RG-10 | Nouvelle règle sans modifier le code existant  | `IValidationRegle`                                   |

## Choix techniques

- **Pattern Strategy** : chaque règle est une classe qui implémente `IValidationRegle`. Le `TransactionService` reçoit la liste des règles et ne connaît aucune règle concrète. Ajouter une règle revient à créer une nouvelle classe et à l'ajouter à la liste.
- **Pattern Repository** : le stockage passe par `ITransactionRepo` (implémentation en mémoire), remplaçable par une base de données sans modifier le service.
- **Héritage** : `CreditTransaction` et `DebitTransaction` héritent de la classe abstraite `Transaction`. Chacune définit son effet sur le solde (`ImpactSurSolde`).
- **`ValidationResult`** : un refus renvoie la liste des erreurs plutôt qu'une exception, car c'est un cas métier normal.
- **`decimal`** pour les montants (pas d'erreur d'arrondi), transactions **immuables**.