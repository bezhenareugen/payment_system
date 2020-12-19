using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PaymentSystem.Server.Application.Wallets.Commands;
using PaymentSystem.Server.Data;
using PaymentSystem.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentSystem.Server.UnitTests
{
    class CreateWalletCommandHandlerTests
    {
        private ApplicationDbContext context;

        [SetUp]
        public void Setup()
        {      
            context = new ApplicationDbContext(
             new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseSqlite("Filename=Test.db")
                 .Options, Microsoft.Extensions.Options.Options.Create(new OperationalStoreOptions()));

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new ApplicationUser
            {
                Id = "test_user_id",
                Wallets = new List<Wallet>
                {
                    new Wallet
                    {
                        Amount = 100,
                        Currency = "EC"
                    }
                }
            };

            context.Add(user);

            context.SaveChanges();
        }

        [Test]
        public async Task CreateWalletSuccessful()
        {
            var sut = new CreateWalletCommandHandler(context);

            var command = new CreateWalletCommand
            {
                UserId = "test_user_id",
                Currency = "EUR"
            };

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public async Task CreateWalletInvalidCurrency()
        {
            var sut = new CreateWalletCommandHandler(context);

            var command = new CreateWalletCommand
            {
                UserId = "test_user_id",
                Currency = "RUB"
            };

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.IsSuccessful);
                Assert.AreEqual("INVALID_CURRENCY", result.FailureReason);
            });
        }

    }
}
