using AutoMapper;
using Bus_Info.Entities;
using Bus_Info.Repositories.Interfaces;
using Bus_Info.Services.Interfaces;
using Bus_Info.ViewModels;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Bus_Info.Services
{
    public class RouteService : IRouteService
    {
        private IRouteRepository _IRouteRepository;
        public RouteService(IRouteRepository IRouteRepository)
        {
            _IRouteRepository = IRouteRepository;
        }

        public List<ViewModelRoute> GetRoute()
        {
            try
            {
                return Mapper.Map<List<Route>, List<ViewModelRoute>>(_IRouteRepository.All.ToList<Route>());
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void UpdateRoute(ViewModelRoute route)
        {
            try
            {
                Route en = Mapper.Map<ViewModelRoute, Route>(route);
                _IRouteRepository.InsertOrUpdate(en);
                _IRouteRepository.Save();
            }
            catch (Exception e)
            {
                
            }
        }

        [Conditional("Test")]
        public void GetInfo()
        {
            var allRoute = _IRouteRepository.All.ToList();
            List<Route> input = new List<Route>();
            foreach (var route in allRoute)
            {
                string URI2 = "http://www.buyttphcm.com.vn/Detail_TTLT.aspx?sl=" + route.Code;
                HttpWebRequest request2 = WebRequest.Create(URI2) as HttpWebRequest;
                request2.Method = "GET";
                WebResponse response2 = request2.GetResponse();
                var stream2 = response2.GetResponseStream();
                HtmlDocument htmlDoc2 = new HtmlAgilityPack.HtmlDocument();
                htmlDoc2.Load(stream2, false);

                if (htmlDoc2.DocumentNode != null)
                {
                    var aTags2 = htmlDoc2.DocumentNode.SelectNodes("//div[@id ='ctl00_ContentPlaceHolder1_UpdatePanel2']/table");
                    if (aTags2.Count == 2)
                    {

                        if (aTags2.LastOrDefault().ChildNodes[9].ChildNodes[1].ChildNodes["ul"] != null)
                        {
                            var table2 = aTags2.LastOrDefault().ChildNodes[9].ChildNodes[1].ChildNodes["ul"].InnerText;
                            var realtext = HttpUtility.HtmlDecode(table2);
                            var items = Regex.Split(realtext, "\r\n").Where(x => x != "").ToList();

                            foreach (var i in items)
                            {
                                if (i.Contains("Loại hình hoạt động:"))
                                {
                                    route.ServiceKind =  ScrubHtml(Regex.Split(i.Trim(), "Loại hình hoạt động:").Where(x => x != "").FirstOrDefault().Trim());
                                }
                                else if (i.Contains("Cự ly:"))
                                {
                                    route.Distance =  ScrubHtml(Regex.Split(i.Trim(), "Cự ly:").Where(x => x != "").FirstOrDefault().Trim());
                                }
                                else if (i.Contains("Số chuyến:"))
                                {
                                    var temp =  Regex.Split(i.Trim(), "Số chuyến:").Where(x => x != "").FirstOrDefault();
                                    if (temp != null)
                                        route.TurnPerDay = ScrubHtml(temp.Trim());
                                }
                                else if (i.Contains("Thời gian chuyến:"))
                                {
                                    route.Duration =  ScrubHtml(Regex.Split(i.Trim(), "Thời gian chuyến:").Where(x => x != "").FirstOrDefault().Trim());
                                }
                                else if (i.Contains("Giãn cách:"))
                                {
                                    route.NextBusTime =  ScrubHtml(Regex.Split(i.Trim(), "Giãn cách:").Where(x => x != "").FirstOrDefault().Trim());
                                }
                                else if (i.Contains("Thời gian hoạt động:"))
                                {
                                    route.WorkTime =  ScrubHtml(Regex.Split(i.Trim(), "Thời gian hoạt động:").Where(x => x != "").FirstOrDefault());
                                }
                                else if (i.Contains("Loại xe:"))
                                {
                                    route.BusKind =  ScrubHtml(Regex.Split(i.Trim(), "Loại xe:").Where(x => x != "").FirstOrDefault().Trim());
                                }
                                else
                                {
                                    route.Note = i.Trim();
                                }
                            }
                            input.Add(route);
                        }
                        else
                        {
                            //manual
                            //var table2 = aTags2.LastOrDefault().ChildNodes[9].InnerText;
                            //var realtext = HttpUtility.HtmlDecode(table2);
                            //var items = Regex.Split(realtext, "\r\n").Where(x => x != "").ToList();
                        }
                    }
                }
            }
            if(input.Count > 0){
                foreach (var rs in input)
                {
                    _IRouteRepository.InsertOrUpdate(rs);
                }
                _IRouteRepository.Save();
            }
        }

        public string ScrubHtml(string value)
        {
            if (value != null)
            {
                var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", " ").Trim();
                var step2 = Regex.Replace(step1, @"\s{2,}", " ");
                return step2;
            }
            return value;
        }
    }

  
}
