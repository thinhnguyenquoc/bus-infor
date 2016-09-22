using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Services.Interfaces
{
    public interface INewsService
    {
        List<ViewModelNews> GetAllNews();
        void UpdateNews(ViewModelNews News);
        ViewModelNews GetDetail(int Id);
        ViewModelNews Search(int Id);
    }
}
