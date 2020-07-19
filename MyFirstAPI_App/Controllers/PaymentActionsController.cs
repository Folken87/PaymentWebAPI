using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentWebAPI.PaymentDetail;
using PaymentWebAPI.ViewModels.PaymentTempData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAPI_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

   
    public class PaymentActionsController : ControllerBase
    {
       
        private readonly PaymentContext _context;

        public PaymentActionsController(PaymentContext context)
        {
            _context = context;
        }

        public static Dictionary<string, string> payTmpData = new Dictionary<string, string>();

        // GET: api/PaymentActions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentActions>>> GetPaymentHistory()
        {
            return await _context.PaymentHistory.ToListAsync();
        }

        

        // POST: api/PaymentActions
        [HttpPost]
        public ActionResult<PaymentTempData> GetPaymentSessionId(PaymentTempData paymentTempData)
        {
            HttpContext.Session.SetString("ID", HttpContext.Session.Id);
            var sessi = HttpContext.Session.GetString("ID").ToString();

            payTmpData.Add("sessionKey", sessi.ToString());
            payTmpData.Add("transaction_price", paymentTempData.transaction_price.ToString());
            payTmpData.Add("appointment", paymentTempData.appointment);

            string FirstJSONResponse = "{ \"sessionKey\":\"" + sessi.ToString() + "\"}";

            return Content(FirstJSONResponse, "application/json");
        }

        // POST: api/PaymentActions/details
        [HttpPost("{details}")]
        public async Task<ActionResult<PaymentDetail>> PaymentCheck(PaymentDetail paymentDetail)
        {
            var sessionId = HttpContext.Session.GetString("ID").ToString();

            var tmpdataval = payTmpData["transaction_price"];

            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            else
            {
                if ((paymentDetail.card_number.ToString().Where((e) => e >= '0' && e <= '9')
                            .Reverse()
                            .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                            .Sum((e) => e / 10 + e % 10)) % 10 == 0)
                {
                    _context.PaymentHistory.Add(new PaymentActions
                    {
                        appointment = payTmpData["appointment"],
                        transaction_price = payTmpData["transaction_price"],
                        card_number = paymentDetail.card_number,
                        card_cvv = paymentDetail.card_cvv,
                        card_date = paymentDetail.card_date,
                        date_transaction = DateTime.Now
                    });
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest();
            }
        }
    }
}
