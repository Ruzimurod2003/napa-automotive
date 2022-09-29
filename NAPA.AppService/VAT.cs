using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAPA.AppService
{
    public static class VAT
    {
        public static double GetTotalPrice(double price, int quantity, double vat)
        {
            double result = 0;
            result = price * quantity * (1 + vat);
            return result;
        }
    }
}
