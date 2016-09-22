using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Services.Interfaces
{
    public interface IPathSearchService
    {
        List<List<ViewModelConnection>> LabelingSetting(List<int> sourceIdList, List<int> sinkIdList);
        List<List<ViewModelNewConnection>> WalkingLabelingSetting(List<int> sourceIdList, List<int> sinkIdList);
        List<List<ViewModelConnection>> ChangeNewConnectionToOld(List<ViewModelNewConnection> newConnectionList);
    }
}
