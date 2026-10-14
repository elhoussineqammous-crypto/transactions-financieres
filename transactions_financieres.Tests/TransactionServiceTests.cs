using transactions_financieres.Regles;
using transactions_financieres.Repositories;
using transactions_financieres.Services;
using transactions_financieres.Validation;
using static transactions_financieres.Tests.DataMock;
using static transactions_financieres.Tests.FaussesRegles;

namespace transactions_financieres.Tests;

// Le service, testé avec de fausses règles (RG-06, RG-07, RG-08),
// puis une fois avec les vraies règles (scénario de l'énoncé).
public class TransactionServiceTests
{
    private static TransactionService Service(params IValidationRegle[] regles) =>
        new TransactionService(new TransactionRepo(), regles);

    [Fact]
    public void Une_Transaction_Valide_Est_Enregistree_Et_Le_Solde_Mis_A_Jour()
    {
        var service = Service(new RegleToujoursValide());

        service.AjouterTransaction(Credit(300m));
        service.AjouterTransaction(Debit(100m));

        Assert.Equal(2, service.ListerTransactions().Count);
        Assert.Equal(200m, service.ObtenirSolde());
    }

    [Fact]
    public void Une_Transaction_Refusee_N_Est_Pas_Enregistree()
    {
        var service = Service(new RegleToujoursEnEchec("refus"));

        var resultat = service.AjouterTransaction(Credit(300m));

        Assert.False(resultat.EstValide);
        Assert.Empty(service.ListerTransactions());
        Assert.Equal(0m, service.ObtenirSolde());
    }

    [Fact]
    public void Toutes_Les_Erreurs_Sont_Remontees()
    {
        var service = Service(
            new RegleToujoursEnEchec("erreur A"),
            new RegleToujoursValide(),
            new RegleToujoursEnEchec("erreur B"));

        var resultat = service.Valider(Credit(10m));

        Assert.Equal(2, resultat.Erreurs.Count);
        Assert.Equal("erreur A", resultat.Erreurs[0]);
        Assert.Equal("erreur B", resultat.Erreurs[1]);
    }

    [Fact]
    public void Scenario_De_L_Enonce_Avec_Les_Vraies_Regles()
    {
        var service = Service(new MontantMaximumCreditRG(), new SoldeMinimumApresDebitRG());

        Assert.True(service.AjouterTransaction(Credit(2000m)).EstValide);    // accepté
        Assert.False(service.AjouterTransaction(Credit(15000m)).EstValide);  // > 10 000 : refusé
        Assert.True(service.AjouterTransaction(Debit(7000m)).EstValide);     // solde -5 000 pile : accepté
        Assert.False(service.AjouterTransaction(Debit(0.01m)).EstValide);    // < -5 000 : refusé

        Assert.Equal(-5000m, service.ObtenirSolde());
    }
}