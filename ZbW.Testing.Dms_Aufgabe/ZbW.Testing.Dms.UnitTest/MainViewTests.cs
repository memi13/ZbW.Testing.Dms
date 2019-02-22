using System;
using System.Windows.Controls;
using FakeItEasy;
using NUnit.Framework;
using ZbW.Testing.Dms.Client.ViewModels;
using ZbW.Testing.Dms.Client.Views;

namespace ZbW.Testing.Dms.UnitTest
{
    [TestFixture]
    public class MainViewTests
    {
        [Test]
        public void Benutzername_check_mime()
        {
            // arrange //
            var mainView = new MainViewModel("mime");
            //act
            var benutzeranme = mainView.Benutzer;
            //assert
            Assert.That(benutzeranme, Is.EqualTo("mime"));
        }
        [Test]
        public void UserComtrol_check_null()
        {
            // arrange //
            var mainView = new MainViewModel("mime");
            UserControl content = null;
            mainView.Content = content;
            //act
            var result = mainView.Content;
            //assert
            Assert.That(result, Is.EqualTo(content));
        }
    }
}