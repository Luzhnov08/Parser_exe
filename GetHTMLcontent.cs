using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace ParserTask
{
    //использование abstract классов
    public static class GetHTMLcontent //получить содержимое веб-страницы
    {
        public static string GetHTMLtext(string url) //статический класс и метод экземпляр которого не нужно создавать
        {
            var doc = new HtmlDocument();
            HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Empty; //<?>
            doc.OptionWriteEmptyNodes = true;
            
            try
            {                
                var webRequest = HttpWebRequest.Create(url);

                CookieContainer gaCookies = new CookieContainer(); //Добавить куки
                Uri target = new Uri(url);
                gaCookies.Add(new Cookie("statix", "statixOK") { Domain = target.Host });
                
                Stream stream = webRequest.GetResponse().GetResponseStream(); //обернуть в using GetResponse потому что Disposable
                doc.Load(stream);
                stream.Close();
            }
            catch (UriFormatException uex)
            {
                Fatale("Ошибка в формате строки url: " + url, uex);
                throw;
            }
            catch (WebException wex)
            {
                Fatale("Ошибка при попытке открыть страницу с url: " + url, wex);
                throw;
            }

            string EncodedString = RemoveStyleAttributes(doc);
            return EncodedString;
        }

        public static void Fatale(string message, Exception exception)
        {
            using (StreamWriter sw = new StreamWriter(ParserTask.StartProgram.writePath()))
            {
                sw.WriteLine(message + exception.InnerException);
            }
        }

        public static string RemoveStyleAttributes(HtmlDocument doc) //Удаление стилей и скриптов
        {
            string result = "";
            IEnumerable<HtmlNode> nodes = doc.DocumentNode.Descendants().Where(n =>
            n.NodeType == HtmlNodeType.Text &&
            n.ParentNode.Name != "script" &&
            n.ParentNode.Name != "style");

            foreach (HtmlNode node in nodes)
            {
                result += node.InnerText.ToString();
            }
            return result;
        }
    }
}