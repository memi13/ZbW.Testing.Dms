using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using ZbW.Testing.Dms.Client.Model;

namespace ZbW.Testing.Dms.UnitTest
{
    [TestFixture]
    public class MetadateItemTest
    {

        [Test]
        public void Guit_check_Guid()
        {
            // arrange //
            var metadateItem = new MetadataItem();
            var Guid = System.Guid.NewGuid().ToString();
            metadateItem.Guid = Guid;
            //act
            var guit = metadateItem.Guid;
            //assert
            Assert.That(guit, Is.EqualTo(Guid));
        }
        [Test]
        public void User_check_mime()
        {
            // arrange 
            var metaItem = new MetadataItem();
            metaItem.User = "mime";
            //act
            var r = metaItem.User;
            //assert
            Assert.That(r, Is.EqualTo("mime"));
        }

        [Test]
        public void FileName_check_hallo_pdf()
        {
            //arrange
            var meta=new MetadataItem();
            meta.FileName = "hallo.pdf";
            //act
            var r = meta.FileName;
            //assert
            Assert.That(r, Is.EqualTo("hallo.pdf"));
        }

        [Test]
        public void CreateDate_check_now()
        {
            //arrange
            var now = DateTime.Now;
            var meta = new MetadataItem();
            meta.CreareDate=now;
          
            //act
            var r = meta.CreareDate;
            //assert
            Assert.That(r,Is.EqualTo(now));
        }

        [Test]
        public void ValueDate_check_now()
        {
            //arrange
            var now = DateTime.Now;
            var meta = new MetadataItem();
            meta.ValueDate = now;

            //act
            var r = meta.ValueDate;
            //assert
            Assert.That(r, Is.EqualTo(now));
        }
        [Test]
        public void Designation_check_Dokument()
        {
            //arrange
            var meta = new MetadataItem();
            meta.Designation = "Dokument";
            //act
            var r = meta.Designation;
            //assert
            Assert.That(r, Is.EqualTo("Dokument"));
        }
        [Test]
        public void Type_check_Vertrag()
        {
            //arrange
            var meta = new MetadataItem();
            meta.Type = "Vertrag";
            //act
            var r = meta.Type;
            //assert
            Assert.That(r, Is.EqualTo("Vertrag"));
        }
        [Test]
        public void keyWort_check_oneItem()
        {
            //arrange
            var meta = new MetadataItem();
            meta.Keywords = new List<string>();
            meta.Keywords.Add("Hallo");
            //act
            var r = meta.Keywords;
            //assert
            Assert.That(r.Count, Is.EqualTo(1));
        }
        [Test]
        public void KesWortasString_check_empty()
        {
            //arrange
            var meta = new MetadataItem();
            meta.Keywords = new List<string>();
            //act
            var r = meta.KeywortsAsString;
            //assert
            Assert.That(r, Is.Empty);
        }
        [Test]
        public void KesWortasString_check_halloDu()
        {
            //arrange
            var meta = new MetadataItem();
            meta.Keywords = new List<string>();
            meta.Keywords.Add("Hallo");
            meta.Keywords.Add("du");
            //act
            var r = meta.KeywortsAsString;
            //assert
            Assert.That(r, Is.EqualTo("Hallo, du, "));
        }
        


    }
}