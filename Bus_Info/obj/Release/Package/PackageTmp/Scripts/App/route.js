var app = angular.module('app', []);

app.factory('RouteRetriever', function ($http, $q, $timeout) {
    var RouteRetriever = new Object();
    RouteRetriever.getroutes = function () {
        var routedata = $q.defer();
        $http.get(baseUrl + "Route/GetRoute")
        .then(function (response) {
            routedata.resolve(response.data);
        });      
        return routedata.promise
    }
    return RouteRetriever;
});

app.factory('StopRetriever', function ($http, $q, $timeout) {
    var StopRetriever = new Object();

    StopRetriever.getstops = function (id, check) {
        var stopdata = $q.defer();

        $http.get(baseUrl + "Route/GetRouteConnection?id_route=" + id + "&is_checked=" + check)
        .then(function (response) {
            stopdata.resolve(response.data);
        });

        return stopdata.promise
    }

    return StopRetriever;
});

app.controller('MyCtrl', function ($scope, RouteRetriever, StopRetriever) {
    $scope.selectedItem = null;
    $scope.routes = RouteRetriever.getroutes();
    $scope.routes.then(function (data) {
        $scope.routes = data;
        $scope.selectedItem = $scope.routes[0];
        $scope.update($scope.selectedItem.Id, 0);
    });    
    $scope.stops = [];
    $scope.routeInfo = null;
    $scope.update = function (Id, turnback) {
        if (turnback == false) {
            $scope.stops = StopRetriever.getstops(Id, 0);
        }
        else {
            $scope.stops = StopRetriever.getstops(Id, 1);
        }
        $scope.stops.then(function (data) {
            $scope.cleanallpath();
            $scope.cleanallmarker();
            $scope.stops = data;
            $scope.showRouteOfBus(data);
        });
        for(var i = 0; i < $scope.routes.length; i++){
            if ($scope.routes[i].Id == Id) {
                $scope.routeInfo = $scope.routes[i];
                break;
            }
        }
    }
    $scope.$watch('turnback', function (newValue, oldValue) { if ($scope.selectedItem != null) $scope.update($scope.selectedItem.Id, newValue); }, true);
    $scope.listMarker = [];
    $scope.listPolyline = [];
    $scope.showRouteOfBus = function (data) {
        var pointListFull = [];
        for (var j = 0; j < data.length; j++) {           
            var pointList = [];
            var customMarker = L.ExtraMarkers.icon({
                icon: 'fa-number',
                prefix: 'fa',
                markerColor: 'blue',
                shape: 'square',
                iconColor: '#ffffff',
                number: data[j].Order + 1  //must use icon fa-number for the number icon
            });
            var fromPoint = L.marker([data[j].Station.Lat, data[j].Station.Lng], { icon: customMarker }).addTo(map).bindPopup('['+ (data[j].Order + 1) + '] ' + data[j].Station.Address);
            $scope.listMarker.push(fromPoint);
            if (data[j].PolyLine != null && data[j].PolyLine != '') {
                var listDataLatLng = polyline.decode(data[j].PolyLine);
                for (var i = 0; i < listDataLatLng.length; i++) {
                    var pointItemp = new L.LatLng(listDataLatLng[i][1], listDataLatLng[i][0]);  //L.LatLng
                    pointList.push(pointItemp);
                    pointListFull.push(pointItemp);
                }
            }
            var polylineRoute = new L.Polyline(pointList, {
                color: 'red',
                weight: 3,
                opacity: 0.5,
                smoothFactor: 1
            });
            polylineRoute.addTo(map);
            $scope.listPolyline.push(polylineRoute);
        }       
        map.fitBounds(L.polyline(pointListFull).getBounds());
        //var currentLevel = map.getZoom();
        //map.setZoom(currentLevel);      
    }

    $scope.cleanallpath = function () {
        if ($scope.listPolyline.length > 0) {
            for (var i = 0; i < $scope.listPolyline.length; i++) {
                if ($scope.listPolyline[i] != null) {
                    map.removeLayer($scope.listPolyline[i]);
                }
            }
            $scope.listPolyline = [];
        }
    }

    $scope.cleanallmarker = function () {

        for (var i = 0; i < $scope.listMarker.length; i++) {
            if ($scope.listMarker[i] != null) {
                map.removeLayer($scope.listMarker[i]);
            }
        }
        $scope.listMarker = [];
    }

    $scope.moveto = function (lat,lon, index) {
        map.setView([lat, lon], 14, { animation: true });
        $scope.listMarker[index].openPopup();
        return false;
    }
});