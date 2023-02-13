﻿using NSubstitute;

namespace AccountRefactorWithCSharp.UnitTest;

[TestFixture]
public class BudgetServiceTests
{
    [Test]
    public void OneDayBudget()
    {
        var budgetRepo = Substitute.For<IBudgetRepo>();
        var budgetService = new BudgetService(budgetRepo);
        budgetRepo.GetAll()
                  .Returns(new List<Budget>()
                           {
                               new Budget()
                               {
                                   YearMonth = "202302",
                                   Amount = 2800
                               }
                           });
        var result = budgetService.Query(new DateTime(2023, 2, 2), new DateTime(2023, 2, 2));
        Assert.AreEqual(100, result);
    }

    [Test]
    public void TwoDayBudgetCrossTwoMonths()
    {
        var budgetRepo = Substitute.For<IBudgetRepo>();
        var budgetService = new BudgetService(budgetRepo);
        budgetRepo.GetAll()
                  .Returns(new List<Budget>()
                           {
                               new Budget()
                               {
                                   YearMonth = "202302",
                                   Amount = 2800
                               },
                               new Budget()
                               {
                                   YearMonth = "202303",
                                   Amount = 31000
                               }
                           });
        var result = budgetService.Query(new DateTime(2023, 2, 28), new DateTime(2023, 3, 2));
        Assert.AreEqual(2100, result);
    }

    [Test]
    public void start_without_budget()
    {
        var budgetRepo = Substitute.For<IBudgetRepo>();
        var budgetService = new BudgetService(budgetRepo);
        budgetRepo.GetAll()
                  .Returns(new List<Budget>()
                           {
                               new Budget()
                               {
                                   YearMonth = "202302",
                                   Amount = 2800
                               },
                           });
        var result = budgetService.Query(new DateTime(2023, 1, 28), new DateTime(2023, 2, 2));
        Assert.AreEqual(200, result);
    }

    [Test]
    public void end_without_budget()
    {
        var budgetRepo = Substitute.For<IBudgetRepo>();
        var budgetService = new BudgetService(budgetRepo);
        budgetRepo.GetAll()
                  .Returns(new List<Budget>()
                           {
                               new Budget()
                               {
                                   YearMonth = "202302",
                                   Amount = 2800
                               },
                           });
        var result = budgetService.Query(new DateTime(2023, 2, 26), new DateTime(2023, 3, 2));
        Assert.AreEqual(300, result);
    }

    [Test]
    public void BudgetCrossThreeMonths()
    {
        var budgetRepo = Substitute.For<IBudgetRepo>();
        var budgetService = new BudgetService(budgetRepo);
        budgetRepo.GetAll()
                  .Returns(new List<Budget>()
                           {
                               new Budget()
                               {
                                   YearMonth = "202302",
                                   Amount = 2800
                               },
                               new Budget()
                               {
                                   YearMonth = "202303",
                                   Amount = 31000
                               },
                               new()
                               {
                                   YearMonth = "202304",
                                   Amount = 30
                               }
                           });
        var result = budgetService.Query(new DateTime(2023, 2, 28), new DateTime(2023, 4, 2));
        Assert.AreEqual(31102, result);
    }

    [Test]
    public void OneMonthNoBudget()
    {
        var budgetRepo = Substitute.For<IBudgetRepo>();
        var budgetService = new BudgetService(budgetRepo);
        budgetRepo.GetAll()
                  .Returns(new List<Budget>()
                           {
                               new Budget()
                               {
                                   YearMonth = "202302",
                                   Amount = 2800
                               },
                               new Budget()
                               {
                                   YearMonth = "202303",
                                   Amount = 0
                               },
                               new()
                               {
                                   YearMonth = "202304",
                                   Amount = 30
                               }
                           });
        var result = budgetService.Query(new DateTime(2023, 2, 28), new DateTime(2023, 4, 2));
        Assert.AreEqual(102, result);
    }

    [Test]
    public void BudgetCrossYear()
    {
        var budgetRepo = Substitute.For<IBudgetRepo>();
        var budgetService = new BudgetService(budgetRepo);
        budgetRepo.GetAll()
                  .Returns(new List<Budget>()
                           {
                               new Budget()
                               {
                                   YearMonth = "202311",
                                   Amount = 3000
                               },
                               new Budget()
                               {
                                   YearMonth = "202312",
                                   Amount = 0
                               },
                               new()
                               {
                                   YearMonth = "202401",
                                   Amount = 31
                               }
                           });
        var result = budgetService.Query(new DateTime(2023, 11, 28), new DateTime(2024, 1, 2));
        Assert.AreEqual(302, result);
    }

    [Test]
    public void OneMonth()
    {
        var budgetRepo = Substitute.For<IBudgetRepo>();
        var budgetService = new BudgetService(budgetRepo);
        budgetRepo.GetAll()
                  .Returns(new List<Budget>()
                           {
                               new Budget()
                               {
                                   YearMonth = "202311",
                                   Amount = 3000
                               },
                           });
        var result = budgetService.Query(new DateTime(2023, 11, 28), new DateTime(2023, 11, 30));
        Assert.AreEqual(300, result);
    }
}