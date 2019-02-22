using NUnit.Framework;
using ZbW.Testing.Dms.Client.ViewModels;

namespace ZbW.Testing.Dms.UnitTest
{
    [TestFixture]
    public class LoginTest
    {
        [Test]
        public void BenutzernameOk_check_meme()
        {
            //arrange
            var Login = new LoginViewModel(null);
            Login.Benutzername = "memi";
            //act
            var r=Login.Benutzername;

            //assert
            Assert.That(r,Is.EqualTo("memi"));

        }
        [Test]
        public void CanLogin_login_true()
        {
            //arrange
            var Login = new LoginViewModel(null);
            Login.Benutzername = "memi";
            //act
            var result = Login.CmdLogin.CanExecute();

            //assert
            Assert.That(result, Is.True);
        }
        [Test]
        public void CanLogin_login_false()
        {
            //arrange
            var Login = new LoginViewModel(null);
            Login.Benutzername = "";
            //act
            var result = Login.CmdLogin.CanExecute();

            //assert
            Assert.That(result, Is.False);
        }
        [Test]
        public void CanclosProgrem_press_true()
        {
            // arrange
            var sut = new LoginViewModel(null);

            //act
            var result = sut.CmdAbbrechen.CanExecute();

            //assert
            Assert.That(result, Is.True);
        }
    }
}