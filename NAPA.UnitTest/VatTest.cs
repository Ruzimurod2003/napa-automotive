using NAPA.AppService;
using NAPA.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAPA.UnitTest
{
    public class VatTest
    {
        private readonly ApplicationContext context;
        public VatTest()
        {
            string connectionString = "Data Source=d:\\Ruzimurod\\for-github\\napa-automotive\\NAPA.db";
            context = new ApplicationContext(connectionString);
        }
        [Fact]
        public void FirstVatCalculateTestFromDatabase()
        {
            double price = 74.09;
            int quantity = 55;
            double vat = 15.32; //Bu qiymat appsettings.json dan olindi
            //Bu yerda bazadagi qiymat bilan tekshirdim methodni ishlashini
            double? resultFromDatabase = context?.Products?.FirstOrDefault(i => i.Name == "HDD 1TB")?.TotalPrice;
            double result = VAT.GetTotalPrice(price, quantity, vat);
            Assert.NotNull(resultFromDatabase);
            Assert.Equal(resultFromDatabase, result);
        }
        [Fact]
        public void SecondVatCalculateTestFromObject()
        {
            double price = 74.09;
            int quantity = 55;
            double vat = 15.32;
            QQS test = new QQS(74.09, 55, 15.32); //Bu yerda yangi obyekt tuzib o'shani qiymati bilan solishtirdim.
            double result = VAT.GetTotalPrice(price, quantity, vat);
            Assert.NotNull(test.TotalPrice);
            Assert.Equal(test.TotalPrice, result);
        }
    }
    public class QQS
    {
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double VAT { get; set; }
        public double? TotalPrice { get; set; }
        public QQS(double price, int quantity, double vat)
        {
            Price = price;
            Quantity = quantity;
            VAT = vat;
            TotalPrice = price * quantity * (1 + vat);
        }
    }
}