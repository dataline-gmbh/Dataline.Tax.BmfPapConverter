using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace Dataline.Tax.BmfEstTestDataGenerator
{
    class Program
    {
        private static readonly Uri Endpoint = new Uri("https://www.bmf-steuerrechner.de/ekst/eingabeformekst.xhtml");
        private const string ResultEndpointSuffix = "?ekst-result=true";
        private static readonly CookieContainer CookieContainer = new CookieContainer();
        private static readonly HttpClient Client = new HttpClient(new HttpClientHandler { CookieContainer = CookieContainer });
        private static readonly CultureInfo German = new CultureInfo("de-DE");
        private const string ResultSuffix = " Euro";

        private static readonly decimal[] TestEinkommenTabelle = CreateTestTable(1000m, 100000m);

        private static void Main(string[] args)
        {
            try
            {
                Task.Run(() => MainAsync(args)).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unerwarteter Fehler:");
                Console.WriteLine(e);

                Console.ReadLine();
            }
        }

        private static async Task MainAsync(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Bitte das Jahr als Parameter angeben");
                return;
            }

            int jahr = int.Parse(args[0]);
            if (jahr < 2013)
            {
                Console.WriteLine("Das Jahr wird nicht unterstützt");
                return;
            }

            var values = new List<Ergebnis>();

            foreach (decimal einkommen in TestEinkommenTabelle)
            {
                values.Add(new Ergebnis(einkommen, false, await EstRequest(einkommen, false, jahr)));
                values.Add(new Ergebnis(einkommen, true, await EstRequest(einkommen, true, jahr)));
            }

            Console.WriteLine();
            Console.WriteLine("Einkommen;Verheiratet;ESt");
            foreach (var value in values)
            {
                Console.WriteLine($"{value.Einkommen:0.00};{(value.Verheiratet ? "1" : "0")};{value.ESt:0.00}");
            }

            Console.WriteLine();
            Console.WriteLine("--- ENDE ---");
            while (true) Console.ReadLine();
        }

        private static async Task<decimal> EstRequest(decimal einkommen, bool verheiratet, int jahr)
        {
            var body = await EstBodyRequest(einkommen, verheiratet, jahr);

            var document = new HtmlDocument();
            document.LoadHtml(body);

            var resultText = document.DocumentNode
                .SelectSingleNode("//td[@headers='ekst_best_table_r_ekst ekst_best_table_betrag']")?
                .InnerText;

            if (string.IsNullOrEmpty(resultText) || !resultText.EndsWith(ResultSuffix))
                throw new Exception("Unerwartete Antwort");

            decimal result = decimal.Parse(resultText.Substring(0, resultText.Length - ResultSuffix.Length), German);
            Console.WriteLine($"Ergebnis: {result:0.00} Euro");

            return result;
        }

        private static async Task<string> EstBodyRequest(decimal einkommen, bool verheiratet, int jahr)
        {
            Console.WriteLine($"Request für {einkommen} {verheiratet} {jahr}");

            string body1;
            using (var response = await Client.GetAsync(Endpoint))
            {
                body1 = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            }

            var document = new HtmlDocument();
            document.LoadHtml(body1);

            var viewState = document.DocumentNode
                .SelectSingleNode("//input[@type='hidden' and @name='javax.faces.ViewState']")
                .Attributes["value"].Value;

            if (string.IsNullOrEmpty(viewState))
                throw new Exception("Kein View State");

            string sessionId = CookieContainer.GetCookies(Endpoint)["JSESSIONID"]?.Value;
            if (sessionId == null)
                throw new Exception("Keine Session-ID");

            using (var formData = new FormUrlEncodedContent(new []
            {
                new KeyValuePair<string, string>("bmf_form_ekst:ekst_zve", einkommen.ToString("0.00", German)),
                new KeyValuePair<string, string>("bmf_form_ekst:ekst_pv", verheiratet ? "true" : "false"),
                new KeyValuePair<string, string>("bmf_form_ekst:ekst_bj", jahr.ToString(German)),
                new KeyValuePair<string, string>("bmf_form_ekst_SUBMIT", "1"),
                new KeyValuePair<string, string>("bmf_form_ekst", "bmf_form_ekst"),
                new KeyValuePair<string, string>("javax.faces.behavior.event", "action"),
                new KeyValuePair<string, string>("javax.faces.partial.event", "click"),
                new KeyValuePair<string, string>("javax.faces.source", "bmf_form_ekst:income_ekst"),
                new KeyValuePair<string, string>("javax.faces.partial.ajax", "true"),
                new KeyValuePair<string, string>("javax.faces.partial.execute", "bmf_form_ekst"),
                new KeyValuePair<string, string>("javax.faces.partial.render", "bmf_form_ekst bmf_infobox bmf_fehlerbox bmf_fehlerbox_ekst_allg bmf_form_ekst:fehlerbox_ekst"),
                new KeyValuePair<string, string>("javax.faces.ViewState", viewState)
            }))
            {
                using (var response = await Client.PostAsync(Endpoint.AbsoluteUri + $";jsessionid={sessionId}", formData))
                {
                    response.EnsureSuccessStatusCode();
                }
            }

            using (var response = await Client.GetAsync(Endpoint.AbsoluteUri + ResultEndpointSuffix))
            {
                return await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            }
        }

        private static decimal[] CreateTestTable(decimal increments, decimal maximum)
        {
            IEnumerable<decimal> CreateTestTableInternal()
            {
                for (decimal i = 0m; i <= maximum; i += increments)
                {
                    yield return i;
                }
            }

            return CreateTestTableInternal().ToArray();
        }

        private readonly struct Ergebnis
        {
            public readonly decimal Einkommen;
            public readonly bool Verheiratet;
            public readonly decimal ESt;

            public Ergebnis(decimal einkommen, bool verheiratet, decimal eSt)
            {
                Einkommen = einkommen;
                Verheiratet = verheiratet;
                ESt = eSt;
            }
        }
    }
}
