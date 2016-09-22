var app = angular.module('app', []);
app.controller('StationCtrl', function ($scope, GetStationInfo, SuggestStation) {
    $scope.Lat = 0;
    $scope.Lng = 0;
    $scope.Id = 0;
    $scope.StationInfo = [];
    $scope.stationMarkerResult = function (route, color) {
        var marker = L.ExtraMarkers.icon({
            icon: 'fa-number',
            prefix: 'fa',
            markerColor: color,
            shape: 'circle',
            iconColor: '#ffffff',
            number: route
        });
        return marker;
    }
    $scope.Result = [];
    $scope.hasResult = false;
    $scope.colorArray = ['orange', 'yellow', 'blue', 'purple', 'green', 'black', 'orange', 'yellow', 'blue', 'purple', 'green', 'black', 'orange', 'yellow', 'blue', 'purple', 'green', 'black', 'orange', 'yellow', 'blue', 'purple', 'green', 'black'];
    $scope.Summary = [];
    $scope.currentLatLng = null;
    $scope.SuggestStationResult = [];
    $scope.SuggestStationPoint = [];

    $scope.fromMarker = L.ExtraMarkers.icon({
        icon: 'fa-spin fa-spinner',
        prefix: 'fa',
        markerColor: 'blue',
        shape: 'penta',
        iconColor: '#ffffff'
    });

    $scope.suggestMarker = L.ExtraMarkers.icon({
        icon: 'fa-info',
        prefix: 'fa',
        markerColor: 'yellow',
        shape: 'square',
        iconColor: '#ffffff'
    });

    $scope.cleanResult = function () {
        for (var i = 0; i < $scope.Result.length; i++) {
            if ($scope.Result[i] != null) {
                map.removeLayer($scope.Result[i]);
            }
        }
        $scope.Result = [];
    }

    $scope.search = function () {
        $scope.Summary = [];
        $scope.StationInfo = GetStationInfo.getstationInfo($scope.Id);
        $scope.StationInfo.then(function (data) {
            //alert(JSON.stringify(data));
            var pointListFull = [];
            for (var j = 0; j < data.length; j++) {
                for (var i = 0; i < data[j].length; i++) {
                    if (data[j][i].Station.Lat != $scope.Lat && data[j][i].Station.Lng != $scope.Lng) {
                        var point = L.marker([data[j][i].Station.Lat, data[j][i].Station.Lng], { icon: $scope.stationMarkerResult(data[j][i].Route.Code, $scope.colorArray[j]) }).addTo(map).bindPopup(data[j][i].Station.Address);
                        $scope.Result.push(point);
                    }
                    if (data[j][i].PolyLine != null && data[j][i].PolyLine != '') {
                        var pointList = [];
                        var listDataLatLng = polyline.decode(data[j][i].PolyLine);
                        for (var l = 0; l < listDataLatLng.length; l++) {
                            var pointItemp = new L.LatLng(listDataLatLng[l][1], listDataLatLng[l][0]);  //L.LatLng
                            pointList.push(pointItemp);
                            pointListFull.push(pointItemp);
                        }

                        var polylineRoute = new L.Polyline(pointList, {
                            color: $scope.colorArray[j],
                            weight: 3,
                            opacity: 0.7,
                            smoothFactor: 1
                        });
                        polylineRoute.addTo(map);
                        $scope.Result.push(polylineRoute);
                    }
                }
                var route = new Object();
                route.Route = data[j][1].Route.Code;
                route.Arrive = data[j][1].Arrive == true? 'về' : 'đi';
                $scope.Summary.push(route);
            }
            map.fitBounds(L.polyline(pointListFull).getBounds());
            $scope.hasResult = true;;
        });
    }

    map.on('contextmenu', function (e) {
        $scope.currentLatLng = e.latlng;
    });

    $scope.addLocation = function (event) {
        $scope.cleanResult();
        var point = L.marker([$scope.currentLatLng.lat, $scope.currentLatLng.lng], { icon: $scope.fromMarker }).addTo(map);
        var circle = L.circle([$scope.currentLatLng.lat, $scope.currentLatLng.lng], 450).addTo(map);
        $scope.Result.push(point);
        $scope.Result.push(circle);
        $scope.SuggestStationResult = SuggestStation.getstations($scope.currentLatLng.lat, $scope.currentLatLng.lng);
        $scope.SuggestStationResult.then(function (data) {
            for (var i = 0; i < data.length; i++) {
                var point = L.marker([data[i].Lat, data[i].Lng], { icon: $scope.suggestMarker }).addTo(map);
                point.bindPopup('<span>' + data[i].Name + '</span> &nbsp; &nbsp;<button class="btn btn-success" type="button" onclick="toClick(' + data[i].Id + ', ' + data[i].Lat + ', ' + data[i].Lng + ', \''+ data[i].Address +'\');">Chọn</button>');
                $scope.Result.push(point);
                $scope.SuggestStationPoint.push(point);
            }
        });
    }

    $scope.searchSationByClick = function (id, lat, lng) {
        //clean suggestion station
        for (var i = 0; i < $scope.SuggestStationPoint.length; i++) {
            if ($scope.SuggestStationPoint[i] != null && ($scope.SuggestStationPoint[i].getLatLng().lat != lat || $scope.SuggestStationPoint[i].getLatLng().lng != lng)) {
                map.removeLayer($scope.SuggestStationPoint[i]);
            }
        }
        $scope.Id = id;
        $scope.Lat = lat;
        $scope.Lng = lng;
        $scope.search();
    }
});

app.factory('SuggestStation', function ($http, $q) {
    var SuggestStation = new Object();
    SuggestStation.getstations = function (lat, lon) {
        var stationdata = $q.defer();
        $http.get(baseUrl + "Station/GetSuggestStation?lat=" + lat + "&lng=" + lon)
        .then(function (response) {
            stationdata.resolve(response.data);
        });
        return stationdata.promise
    }
    return SuggestStation;
});

app.factory('GetStationInfo', function ($http, $q) {
    var GetStationInfo = new Object();
    GetStationInfo.getstationInfo = function (Id) {
        var stationdata = $q.defer();
        $http.get(baseUrl + "Station/GetStationInfo?Id=" + Id)
        .then(function (response) {
            stationdata.resolve(response.data);
        });
        return stationdata.promise
    }
    return GetStationInfo;
});

app.directive('ngRightClick', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.ngRightClick);
        element.bind('contextmenu', function (event) {
            scope.$apply(function () {
                event.preventDefault();
                fn(scope, { $event: event });
            });
        });
    };
});

$(function () {
    var fromMarker = L.ExtraMarkers.icon({
        icon: 'fa-spin fa-spinner',
        prefix: 'fa',
        markerColor: 'blue',
        shape: 'penta',
        iconColor: '#ffffff'
    });
    $("#autocomplete").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: baseUrl + "Station/GetStation",
                data: { query: request.term },
                dataType: 'json',
                type: 'GET',
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Address,
                            value: item.Id,
                            Lat: item.Lat,
                            Lng:item.Lng
                        }
                    }));
                }
            })
        },
        select: function (event, ui) {
            $('#autocomplete').val(ui.item.label);
            //$('#Id').val(ui.item.value);
            var point = L.marker([ui.item.Lat, ui.item.Lng], { icon: fromMarker }).addTo(map).bindPopup(ui.item.label);
            map.panTo(new L.LatLng(ui.item.Lat, ui.item.Lng));
            var scope = angular.element($("#searchform")).scope();
            scope.$apply(function () {
                scope.Lat = ui.item.Lat;
                scope.Lng = ui.item.Lng;
                scope.Id = ui.item.value;
                scope.cleanResult();
                scope.Result.push(point);
               
            });
            return false;
        },
        minLength: 2
    });    
});

function toClick(id,lat,lng, address) {
    var scope = angular.element($("#stationfm")).scope();
    scope.$apply(function () {
        scope.searchSationByClick(id, lat, lng);        
        $('#autocomplete').val(address);
    });
}