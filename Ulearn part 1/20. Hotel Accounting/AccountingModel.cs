using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAccounting
{
    public class AccountingModel : ModelBase
    {
        private double price;
        private int nightsCount;
        private double discount;
        private double total;
        public double Price {
            get { return price; }
            set { 
                if (value < 0) throw new ArgumentException();
                if (!value.Equals(price))
                {
                    price = value;
                    Notify("Price");
                    Total = Price * NightsCount * (1 - Discount / 100);
                    //Notify("Total");
                }
            } 
        }

        public int NightsCount { 
            get { return nightsCount; }
            set
            {
                if (value <= 0) throw new ArgumentException();
                if (!value.Equals(nightsCount))
                {
                    nightsCount = value;
                    Notify("NightsCount");
                    Total = Price * NightsCount * (1 - Discount / 100);
                    //Notify("Total");
                }
            }
        }

        public double Discount
        {
            get { return discount; }
            set
            {
                if (!value.Equals(discount))
                {
                    discount = value;
                    Notify("Discount");
                    Total = Price * NightsCount * (1 - Discount / 100);
                    //Notify("Total");
                }
            }
        }

        public double Total
        {
            get { return total; }
            set 
            {
                if (value < 0) throw new ArgumentException();
                if (!value.Equals(total))
                {
                    total = value;
                    Notify("Total");
                    Discount = 100 - (100 * Total) / (Price * NightsCount);
                    //Notify("Discount");
                }
            }
        }
    }
}
