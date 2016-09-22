using AutoMapper;
using Bus_Info.Entities;
using Bus_Info.Repositories.Interfaces;
using Bus_Info.Services.Interfaces;
using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Services
{
    public class NewsService: INewsService
    {
        private INewsRepository _INewsRepository;
        public NewsService(INewsRepository INewsRepository)
        {
            _INewsRepository = INewsRepository;
        }

        public List<ViewModelNews> GetAllNews()
        {
            try
            {
                return Mapper.Map<List<News>, List<ViewModelNews>>(_INewsRepository.All.OrderByDescending(x=>x.PublishDate).ToList<News>());
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ViewModelNews GetDetail(int Id)
        {
            try
            {
                return Mapper.Map<News, ViewModelNews>(_INewsRepository.All.Where(x => x.Id == Id).FirstOrDefault());
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void UpdateNews(ViewModelNews News)
        {
            try
            {
                if (News.Publish == true)
                {
                    if (News.PublishDate == null)
                        News.PublishDate = DateTime.Now;
                }
                if (News.CreatedDate == null)
                    News.CreatedDate = DateTime.Now;
                News en = Mapper.Map<ViewModelNews, News>(News);
                _INewsRepository.InsertOrUpdate(en);
                _INewsRepository.Save();
            }
            catch (Exception e)
            {

            }
        }

        public ViewModelNews Search(int Id)
        {
            var result = _INewsRepository.All.Where(x => x.Id == Id).FirstOrDefault();
            return Mapper.Map<News, ViewModelNews>(result);
        }
    }
}
