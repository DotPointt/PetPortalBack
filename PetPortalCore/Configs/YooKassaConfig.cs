using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalCore.Configs
{
    public class YooKassaConfig
    {
        public string TestMagazineSecretKey { get; set; } = string.Empty;
        public string ShopId { get; set; } = string.Empty;

        /// <summary>
        /// Фиксированная стоимость размещения проекта.
        /// </summary>
        public decimal PlacementAmount { get; set; } = 199m;

        public string PlacementCurrency { get; set; } = "RUB";

        /// <summary>
        /// URL возврата после оплаты (фронтенд).
        /// </summary>
        public string ReturnUrl { get; set; } = "http://localhost:5173/create-project-success";
    }
}
