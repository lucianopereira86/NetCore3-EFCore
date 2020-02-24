using Base.Test;
using Entidades;
using FluentAssertions;
using Infraestructura.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Infraestructura.Test
{
    [TestClass]
    public class CablemodemRepositoryTestCase : BaseRepositoryTestCase<Cablemodem>
    {
        [TestMethod]
        public void NoCablemodem_Save_SeGuardaClave()
        {
            var cablemodemToPersist = CreateEntity();
            cablemodemToPersist.Ip.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void CablemodemCreado_Search_ObtieneCablemodem()
        {
            var cablemodemToPersist = CreateEntity();
            using (var context = new CablemodemContext(Options))
            {
                ICablemodemRepository reporitory = new CablemodemRepository(context);
                var cablemodem = reporitory.Search(c => c.Ip == cablemodemToPersist.Ip);
                cablemodem.Should().HaveCount(1);
                this.AreEquals(cablemodemToPersist, cablemodem.First());
            };
        }

        public override Cablemodem CreateEntity()
        {
            var cablemodem = new Cablemodem()
            {
                Ip = "1.2.3.4",
                MacAddress = "91:75:1a:ec:9a:c7",
                Fabricante = "Moto",
                Modelo = "modelo",
                VersionSoftware = "1.1.1"
            };

            using (var context = new CablemodemContext(Options))
            {
                ICablemodemRepository reporitory = new CablemodemRepository(context);
                reporitory.Save(cablemodem);
            }

            return cablemodem;
        }
    }
}
