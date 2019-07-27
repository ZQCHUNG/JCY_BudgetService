
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Appointments;
using Windows.Devices.SmartCards;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CashTest2
{
    [TestClass]
    public class BudgetServiceTest
    {
        [TestMethod]
        public void SingleDate()
        {
            //Arrange
            var budget = new List<Budget>()
            {
                new Budget()
                {
                    Amount = 3100,
                   YearMonth = "201901"
                },
                new Budget()
                {
                   Amount = 2800,
                   YearMonth = "201902"
                },
            };

            IBudgetRepo budgetRepo= Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(budget);
            var budgetService = new BudgetService(budgetRepo);
            Assert.AreEqual(100, budgetService.Query(new DateTime(2019, 1, 1), new DateTime(2019,1, 1)));

        }
    }

    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }

    public class BudgetService {
        private List<Budget> _budget = new List<Budget>();
        private IBudgetRepo _repo;

        public BudgetService(IBudgetRepo repo)
        {
            _repo = repo;
        }

        public double Query(DateTime start, DateTime end)
        {
            var allBudget = this._repo.GetAll();
            var days = DateTime.DaysInMonth(start.Year, start.Month);
            var key = start.Year.ToString() + start.Month.ToString();
            var amount = allBudget.SingleOrDefault(x => x.YearMonth == key)?.Amount??0;
            return (double)(amount / days);
        }
    }

    public class Budget
    {
        public int Amount { get; set; }
        public string YearMonth { get; set; }
    }
}
