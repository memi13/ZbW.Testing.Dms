using System;
using System.Collections.Generic;
using NUnit.Framework;
using ZbW.Testing.Dms.Client.ViewModels;

namespace ZbW.Testing.Dms.UnitTest
{
    [TestFixture]
    public class DocumentDetailViewModelUnitTest
    {
        [Test]
        public void Benutzer_check_mime()
        {
            // arrange
            var sut = new DocumentDetailViewModel();
            sut.Benutzer = "mime";

            //act
            var result = sut.Benutzer;


            //assert
            Assert.That(result, Is.EqualTo("mime"));
        }

        [Test]
        public void IsRemoveFileEnabled_check_true()
        {
            //arrange
            var dokumentDetail=new DocumentDetailViewModel();
            dokumentDetail.IsRemoveFileEnabled = true;
            //act
            var result = dokumentDetail.IsRemoveFileEnabled;
            //assert
            Assert.That(result,Is.True);
        }
        //             
        [Test]
        public void Stichwoerter_check_Wichtig()
        {
            //arrange
            var dokumentDetail = new DocumentDetailViewModel();
            dokumentDetail.Stichwoerter = "wichtig";
            //act
            var result = dokumentDetail.Stichwoerter;
            //assert
            Assert.That(result, Is.EqualTo("wichtig"));
        }
        [Test]
        public void Bezeichnung_check_Restaurant()
        {
            //arrange
            var dokumentDetail = new DocumentDetailViewModel();
            dokumentDetail.Bezeichnung = "Restaurant";
            //act
            var result = dokumentDetail.Bezeichnung;
            //assert
            Assert.That(result, Is.EqualTo("Restaurant"));
        }
        [Test]
        public void TypItems_check_OneItem()
        {
            //arrange
            var dokumentDetail = new DocumentDetailViewModel();
            dokumentDetail.TypItems = new List<string>();
            dokumentDetail.TypItems.Add("H");
            //act
            var result = dokumentDetail.TypItems;
            //assert
            Assert.That(result.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void Erfassungsdatum_check_now()
        {
            //arrange
            var now = DateTime.Now;
            var dokumentDetail = new DocumentDetailViewModel();
            dokumentDetail.Erfassungsdatum = now;
            //act
            var result = dokumentDetail.Erfassungsdatum;
            //assert
            Assert.That(result, Is.EqualTo(now));
        }
        [Test]
        public void ValutaDatum_check_now()
        {
            //arrange
            var now = DateTime.Now;
            var dokumentDetail = new DocumentDetailViewModel();
            dokumentDetail.ValutaDatum = now;
            //act
            var result = dokumentDetail.ValutaDatum;
            //assert
            Assert.That(result, Is.EqualTo(now));
        }
        [Test]
        public void TypItems_check_FristItem()
        {
            //arrange
            var dokumentDetail = new DocumentDetailViewModel();
            dokumentDetail.TypItems = new List<string>();
            dokumentDetail.TypItems.Add("H");
            //act
            var result = dokumentDetail.TypItems;
            //assert
            Assert.That(result[0], Is.EqualTo("H"));
        }
    }
}