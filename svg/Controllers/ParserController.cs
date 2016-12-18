using HtmlAgilityPack;
using svg;
using svg.Models;
using svgProject.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace svgProject.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ParserController : Controller
    {
        private SvgManager _manager = new SvgManager();

        // GET: Parser
        public async Task<Int32> Parse(string href, int pageStart, int pageFinish)
        {
            var domain = "https://openclipart.org/";
            var page = pageStart == 0 ? 1 : pageStart;
            string url;
            string htmlText;

            bool objectsExist = true;

            List<HtmlNode> divs = new List<HtmlNode>();
            HtmlDocument result = new HtmlDocument();
            List<SvgObject> objs = new List<SvgObject>();

            var notification = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            //получаем divs с ссылкой на detail
            while (objectsExist)
            {
                if (pageFinish != 0 && page > pageFinish)
                    break;

                url = domain + href + "?page=" + page;
                htmlText = await GetHtmlTextForPage(url);
                result = new HtmlDocument();
                result.LoadHtml(htmlText);
                var divsOnPage = result.DocumentNode.Descendants("div")
                    .Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("r-img underline"));

                if (divsOnPage.Count() == 0)
                {
                    objectsExist = false;
                    continue;
                }
                else
                    divs.AddRange(divsOnPage);

                notification.Clients.All.sendPageCount(page);
                page++;             
            }

            notification.Clients.All.sendCountImages(divs.Count);

            List<String> linksToDetail = new List<String>();
            //получаем ссылки на detail
            foreach (var div in divs)
            {
                var h4 = div.Descendants("h4").First();
                var a = h4.Descendants("a").Where(_ => _.Attributes.Contains("href")).FirstOrDefault();
                linksToDetail.Add(a.Attributes["href"].Value);
            }

            List<String> linksToDownload = new List<String>();
            int count = 0;
            //получаем ссылки на загрузки изображений
            foreach (var link in linksToDetail)
            {
                url = domain + link;
                htmlText = await GetHtmlTextForPage(url);
                result.LoadHtml(htmlText);
                var downloadLink = result.DocumentNode.Descendants("a")
                    .Where(_ => _.Attributes.Contains("class") && _.Attributes["class"].Value == "clipart-source")
                    .First().Attributes["href"].Value;
                linksToDownload.Add(downloadLink);
                notification.Clients.All.sendUrlCount(++count);
            }

            notification.Clients.All.sendDownloadImageStarted();

            var addedCount = 0;
            //загружаем изображения
            foreach (var link in linksToDownload)
            {
                url = domain + link;
                htmlText = await GetHtmlTextForPage(url);
                if (!_manager.SvgObjectExist(htmlText))
                {
                    objs.Add(new SvgObject()
                    {
                        Id = Guid.NewGuid(),
                        Name = GetNameFromLink(url),
                        Value = htmlText
                    });
                }
                addedCount++;
                notification.Clients.All.sendCurrent(addedCount);
            }

            notification.Clients.All.parseCompleted();
            _manager.AddSvgObjects(objs);

            return objs.Count;
        }

        private async Task<String> GetHtmlTextForPage(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetByteArrayAsync(url);
            string source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            return WebUtility.HtmlDecode(source);
        }

        private string GetNameFromLink(string link)
        {
            var startIndex = link.LastIndexOf('/') + 1;
            return link.Substring(startIndex, link.Length - startIndex - 4);
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> StartParse(string href, int pageStart, int pageFinish)
        {
            int count = await Parse(href, pageStart, pageFinish);
            return Content(count.ToString());
        }
    }
}