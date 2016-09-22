var app = angular.module('app', ['blockUI']);
app.filter("sanitize", ['$sce', function ($sce) {
    return function (htmlCode) {
        return $sce.trustAsHtml(htmlCode);
    }
}]);
app.controller('HomeCtrl', function ($scope, $timeout, SuggestStation, GetPosition, Search, blockUI, lazyLoadApi) {
    //blockUI.Config.autoInjectBodyBlock = false;
    $scope.rightclick = false;    
    $scope.currentLatLng = null;
    $scope.fromWalk = [];
    $scope.toWalk = [];
    $scope.notifyFlag = false;
    $scope.notifyMess = '';
    $scope.fromSuggestStation = [];
    $scope.toSuggestStation = [];
    $scope.fromPoint = null;
    $scope.toPoint = null;
    $scope.fromCircle = null;
    $scope.toCircle = null;
    $scope.searchResult = [];
    $scope.hasResult = false;
    $scope.tabAni1 = 'treeview active';
    $scope.tabAni2 = 'treeview active';
    $scope.fromLat = 0;
    $scope.fromLng = 0;
    $scope.toLat = 0;
    $scope.toLng = 0;
    $scope.fromLatLng = "";
    $scope.toLatLng = "";
    $scope.colorArray = ['orange', 'yellow', 'blue', 'purple', 'green', 'black', 'orange', 'yellow', 'blue', 'purple', 'green', 'black', 'orange', 'yellow', 'blue', 'purple', 'green', 'black', 'orange', 'yellow', 'blue', 'purple', 'green', 'black'];



    $scope.hideNotify = function () {
        $scope.notifyFlag = false;
    }
    $scope.showNotify = function (mess) {
        $scope.notifyFlag = true;
        $scope.notifyMess = mess;
        $timeout(function () {
            $scope.notifyFlag = false;
        }, 4000);
    }
    $scope.fromMarker = L.ExtraMarkers.icon({
        icon: 'fa-spin fa-spinner',
        prefix: 'fa',
        markerColor: 'blue',
        shape: 'penta',
        iconColor: '#ffffff'
    });
    $scope.toMarker = L.ExtraMarkers.icon({
        icon: 'fa-spin fa-spinner',
        prefix: 'fa',
        markerColor: 'orange',
        shape: 'penta',
        iconColor: '#ffffff'
    });
    $scope.stationToMarker = L.ExtraMarkers.icon({
        icon: 'fa-bus',
        prefix: 'fa',
        markerColor: 'orange',
        shape: 'circle',
        iconColor: '#ffffff'
    });
    $scope.stationFromMarker = L.ExtraMarkers.icon({
        icon: 'fa-bus',
        prefix: 'fa',
        markerColor: 'blue',
        shape: 'circle',
        iconColor: '#ffffff'
    });
    $scope.stationMarker = L.ExtraMarkers.icon({
        icon: 'fa-bus',
        prefix: 'fa',
        markerColor: 'green',
        shape: 'circle',
        iconColor: '#ffffff'
    });
    $scope.stationToMarkerResult = function (route) {
        var marker = L.ExtraMarkers.icon({
            icon: 'fa-number',
            prefix: 'fa',
            markerColor: 'orange',
            shape: 'circle',
            iconColor: '#ffffff',
            number: route
        });
        return marker;
    }  
    $scope.stationFromMarkerResult = function (route) {
        var marker = L.ExtraMarkers.icon({
            icon: 'fa-number',
            prefix: 'fa',
            markerColor: 'blue',
            shape: 'circle',
            iconColor: '#ffffff',
            number: route
        });
        return marker;
    }
    $scope.stationMarkerResult = function(route){
        var marker = L.ExtraMarkers.icon({            
            icon: 'fa-number',
            prefix: 'fa',
            markerColor: 'green',
            shape: 'circle',
            iconColor: '#ffffff',
            number: route
        });
        return marker;
    }
    $scope.stationChangeMarkerResult = function (route) {
        var marker = L.ExtraMarkers.icon({
            icon: 'fa-exchange',
            prefix: 'fa',
            markerColor: 'red',
            shape: 'star',
            iconColor: '#ffffff',
            number: route
        });
        return marker;
    }
    $scope.search = function () {
        if ($scope.fromLatLng != "" && $scope.toLatLng != "") {
            var fromtemp = $scope.fromLatLng.split(";");
            var totemp = $scope.toLatLng.split(";");
            if (fromtemp.length == 2 && totemp.length == 2) {
                if ($.trim(fromtemp[0]) != "" && $.trim(fromtemp[1]) != "" && $.trim(totemp[0]) != "" && $.trim(totemp[1]) != "") {
                    $scope.fromLat = $.trim(fromtemp[0]);
                    $scope.fromLng = $.trim(fromtemp[1]);
                    $scope.fromname = $scope.fromLatLng;
                    $scope.from = $scope.fromLatLng;

                    $scope.toLat = $.trim(totemp[0]);
                    $scope.toLng = $.trim(totemp[1]);
                    $scope.toname = $scope.toLatLng;
                    $scope.to = $scope.toLatLng;

                    $scope.getSuggestStation($scope.fromLat, $scope.fromLng, $scope.fromname, true);
                    $scope.getSuggestStation($scope.toLat, $scope.toLng, $scope.toname, false);
                }
            }
        }
        if ($scope.fromLat == 0) {
            $scope.showNotify('Xin hãy chọn trạm đi!');
            return;
        }
        if ($scope.toLat == 0) {
            $scope.showNotify('Xin hãy chọn trạm tới');
            return;
        }
        $scope.hideNotify();
        $scope.TotalDistance = 0;
        $scope.ChangeRoute = 0;
        $scope.Detail = [];
        
        $scope.SearchResult = Search.search($scope.fromLat, $scope.fromLng, $scope.toLat, $scope.toLng);
        $scope.SearchResult.then(function (data) {
            $scope.tabAni1 = 'treeview';
            $scope.tabAni2 = 'treeview active';
            $scope.hasResult = true;
            $scope.TotalDistance = 0;
            $scope.ChangeRoute = 0;
            var detail = [];
            if (data == '') {
                $scope.showNotify('Hệ thống không tìm thấy đường đi khả thi!')
                return;
            }
            var point = null;
            var pointListFull = [];
            // check route
            for (var i = 0; i < data.length; i++) {                               
                if (i < data.length - 1) {
                    detail.push("<i class='fa fa-bus'></i> &nbsp; Đi xe số <b>" + data[i][data[i].length - 1].Route.Code + "</b> từ <b>" + data[i + 1][0].Station.Address + "</b> tới <b>" + data[i][0].Station.Address + "</b>");
                }
                else {
                    detail.push("<i class='fa fa-bus'></i> &nbsp; Đi xe số <b>" + data[i][0].Route.Code + "</b> từ <b>" + data[i][data[i].length - 1].Station.Address + "</b> tới <b>" + data[i][0].Station.Address + "</b>");
                }
                //connection in route
                for (var j = 0; j < data[i].length; j++) {
                    $scope.TotalDistance = data[i][j].Distance + $scope.TotalDistance;
                    //sink
                    if (i == 0 && j == 0) {
                        point = L.marker([data[i][j].Station.Lat, data[i][j].Station.Lng], { icon: $scope.stationToMarkerResult(data[i][j].Route.Code) }).addTo(map).bindPopup(data[i][j].Station.Address);
                    }
                    //source
                    else if (i == data.length - 1 && j == data[data.length - 1].length - 1) {
                         point = L.marker([data[i][j].Station.Lat, data[i][j].Station.Lng], { icon: $scope.stationFromMarkerResult(data[i][j].Route.Code) }).addTo(map).bindPopup(data[i][j].Station.Address);
                    }
                    else {
                        //change route
                        if (j == 0) {
                            $scope.ChangeRoute = $scope.ChangeRoute + 1;
                            point = L.marker([data[i][j].Station.Lat, data[i][j].Station.Lng], { icon: $scope.stationChangeMarkerResult(data[i][j].Route.Code) }).addTo(map).bindPopup("Chuyển từ xe số <b>" +data[i][j].Route.Code+ "</b> sang <b>"+  data[i - 1][0].Route.Code + "</b> <br\> Tại: <b>" + data[i][j].Station.Address+"</b>");
                        }
                        else {
                            point = L.marker([data[i][j].Station.Lat, data[i][j].Station.Lng], { icon: $scope.stationMarkerResult(data[i][j].Route.Code) }).addTo(map).bindPopup(data[i][j].Station.Address);
                        }
                    }
                    $scope.searchResult.push(point);
                    if (data[i][j].PolyLine != null && data[i][j].PolyLine != '') {
                        var pointList = [];
                        var listDataLatLng = polyline.decode(data[i][j].PolyLine);
                        for (var l = 0; l < listDataLatLng.length; l++) {
                            var pointItemp = new L.LatLng(listDataLatLng[l][1], listDataLatLng[l][0]);  //L.LatLng
                            pointList.push(pointItemp);
                            pointListFull.push(pointItemp);
                        }

                        var polylineRoute = new L.Polyline(pointList, {
                            color: $scope.colorArray[i],
                            weight: 3,
                            opacity: 0.7,
                            smoothFactor: 1
                        });
                        polylineRoute.addTo(map);
                        $scope.searchResult.push(polylineRoute);
                    }                                        
                }
                
            }
            $scope.Detail = detail.reverse();
            map.fitBounds(L.polyline(pointListFull).getBounds());

            // walk from
            var origin = new google.maps.LatLng($scope.fromLat, $scope.fromLng);
            var goal = new google.maps.LatLng(data[data.length - 1][data[data.length - 1].length - 1].Station.Lat, data[data.length - 1][data[data.length - 1].length - 1].Station.Lng);
            $scope.drawWalk(origin, goal);
            //walk to
            var origin2 = new google.maps.LatLng(data[0][0].Station.Lat, data[0][0].Station.Lng);
            var goal2 = new google.maps.LatLng($scope.toLat, $scope.toLng);
            $scope.drawWalk(origin2, goal2);
            
        });
    };
    $scope.drawPath = function (data) {       
        $scope.hideNotify();
        $scope.TotalDistance = 0;
        $scope.ChangeRoute = 0;
        $scope.Detail = [];
      
        $scope.tabAni1 = 'treeview';
        $scope.tabAni2 = 'treeview active';
        $scope.hasResult = true;
        $scope.TotalDistance = 0;
        $scope.ChangeRoute = 0;
        var detail = [];
        if (data == '') {
            $scope.showNotify('Hệ thống không tìm thấy đường đi khả thi!')
            return;
        }
        var point = null;
        var pointListFull = [];
        // check route
        for (var i = 0; i < data.length; i++) {
            if (i < data.length - 1) {
                detail.push("<i class='fa fa-bus'></i> &nbsp; Đi xe số <b>" + data[i][data[i].length - 1].Route.Code + "</b> từ <b>" + data[i + 1][0].Station.Address + "</b> tới <b>" + data[i][0].Station.Address + "</b>");
            }
            else {
                detail.push("<i class='fa fa-bus'></i> &nbsp; Đi xe số <b>" + data[i][0].Route.Code + "</b> từ <b>" + data[i][data[i].length - 1].Station.Address + "</b> tới <b>" + data[i][0].Station.Address + "</b>");
            }
            //connection in route
            for (var j = 0; j < data[i].length; j++) {
                $scope.TotalDistance = data[i][j].Distance + $scope.TotalDistance;
                //sink
                if (i == 0 && j == 0) {
                    point = L.marker([data[i][j].Station.Lat, data[i][j].Station.Lng], { icon: $scope.stationToMarkerResult(data[i][j].Route.Code) }).addTo(map).bindPopup(data[i][j].Station.Address);
                }
                    //source
                else if (i == data.length - 1 && j == data[data.length - 1].length - 1) {
                    point = L.marker([data[i][j].Station.Lat, data[i][j].Station.Lng], { icon: $scope.stationFromMarkerResult(data[i][j].Route.Code) }).addTo(map).bindPopup(data[i][j].Station.Address);
                }
                else {
                    //change route
                    if (j == 0) {
                        $scope.ChangeRoute = $scope.ChangeRoute + 1;
                        point = L.marker([data[i][j].Station.Lat, data[i][j].Station.Lng], { icon: $scope.stationChangeMarkerResult(data[i][j].Route.Code) }).addTo(map).bindPopup("Chuyển từ xe số <b>" + data[i][j].Route.Code + "</b> sang <b>" + data[i - 1][0].Route.Code + "</b> <br\> Tại: <b>" + data[i][j].Station.Address + "</b>");
                    }
                    else {
                        point = L.marker([data[i][j].Station.Lat, data[i][j].Station.Lng], { icon: $scope.stationMarkerResult(data[i][j].Route.Code) }).addTo(map).bindPopup(data[i][j].Station.Address);
                    }
                }
                $scope.searchResult.push(point);
                if (data[i][j].PolyLine != null && data[i][j].PolyLine != '') {
                    var pointList = [];
                    var listDataLatLng = polyline.decode(data[i][j].PolyLine);
                    for (var l = 0; l < listDataLatLng.length; l++) {
                        var pointItemp = new L.LatLng(listDataLatLng[l][1], listDataLatLng[l][0]);  //L.LatLng
                        pointList.push(pointItemp);
                        pointListFull.push(pointItemp);
                    }

                    var polylineRoute = new L.Polyline(pointList, {
                        color: $scope.colorArray[i],
                        weight: 3,
                        opacity: 0.7,
                        smoothFactor: 1
                    });
                    polylineRoute.addTo(map);
                    $scope.searchResult.push(polylineRoute);
                }
            }

        }
        $scope.Detail = detail.reverse();
        map.fitBounds(L.polyline(pointListFull).getBounds());

    };
    $scope.drawWalk = function (origin, goal) {
        var directionsService = new google.maps.DirectionsService;
        directionsService.route({
            origin: origin,
            destination: goal,
            travelMode: google.maps.TravelMode.WALKING
        }, function (response, status) {
            if (status === google.maps.DirectionsStatus.OK) {
                //scope.fromWalk = response.routes[0].overview_path;
                var pointList = [];
                for (var i = 0; i < response.routes[0].overview_path.length; i++) {
                    var pointItemp = new L.LatLng(response.routes[0].overview_path[i].lat(), response.routes[0].overview_path[i].lng());  //L.LatLng
                    pointList.push(pointItemp);
                }

                var polylineRoute = new L.Polyline(pointList, {
                    color: 'red',
                    weight: 3,
                    opacity: 0.5,
                    smoothFactor: 1,
                    dashArray: '20,15',
                    lineJoin: 'round'
                });
                polylineRoute.addTo(map);
                $scope.searchResult.push(polylineRoute);
            } else {
                $scope.showNotify('Hệ thống không tìm thấy đường đi bộ!');
            }
        });
    }

    $scope.getSuggestStation = function (lat, lng, name, from) {
        $scope.cleanSearchResult();
        if (from == true) {
            $scope.fromStationId = 0;
            if ($scope.fromPoint != null) {
                map.removeLayer($scope.fromPoint);
                //map.removeLayer($scope.fromWalk.pop());
            }
            $scope.fromPoint = L.marker([lat, lng], { icon: $scope.fromMarker }).addTo(map).bindPopup(name);
            $scope.fromPoint.openPopup();
            $scope.fromLat = lat;
            $scope.fromLng = lng;          
        }
        else {
            $scope.toStationId = 0;
            if ($scope.toPoint != null) {
                map.removeLayer($scope.toPoint);
                //map.removeLayer($scope.toWalk.pop());
            }
            $scope.toPoint = L.marker([lat, lng], { icon: $scope.toMarker }).addTo(map).bindPopup(name);
            $scope.toPoint.openPopup();
            $scope.toLat = lat;
            $scope.toLng = lng;         
        }       
        
    }

    $scope.chooseFrom = function () {
        var latlng = { lat: parseFloat($scope.currentLatLng.lat), lng: parseFloat($scope.currentLatLng.lng) };
        var geocoder = new google.maps.Geocoder;
        geocoder.geocode({ 'location': latlng }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                if (results[1]) {
                    $timeout(function () {
                        $scope.getSuggestStation($scope.currentLatLng.lat, $scope.currentLatLng.lng, results[1].formatted_address, true);
                        $scope.from = results[1].formatted_address;
                        $scope.rightclick = false;
                    }, 0);
                } else {
                    $scope.showNotify('Hệ thống không tìm được địa điểm này!');
                }
            } else {
                $scope.showNotify('Hệ thống không tìm được địa điểm này!');
            }

        });
    }
    $scope.chooseTo = function () {
        var latlng = { lat: parseFloat($scope.currentLatLng.lat), lng: parseFloat($scope.currentLatLng.lng) };
        var geocoder = new google.maps.Geocoder;
        geocoder.geocode({ 'location': latlng }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                if (results[1]) {
                    $timeout(function () {
                        $scope.getSuggestStation($scope.currentLatLng.lat, $scope.currentLatLng.lng, results[1].formatted_address, false);
                        $scope.to = results[1].formatted_address;
                        $scope.rightclick = false;;
                    }, 0);                    
                } else {
                    $scope.showNotify('Hệ thống không tìm địa điểm này!');
                }
            } else {
                $scope.showNotify('Hệ thống không tìm địa điểm này!');
            }

        });
    }
    map.on('contextmenu', function (e) {
        $scope.currentLatLng = e.latlng;
    });
    $scope.showMenu = function (mouseEvent) {
        var result = GetPosition.getPosition(mouseEvent);
        $scope.myStyle = { 'position': 'absolute', 'width': '130px', 'left': result.x + 'px', 'top': result.y + 'px', 'z-index': '60' }
        $scope.rightclick = true;
    }
    $scope.hideMenu = function () {
        $scope.rightclick = false;
    }
    $scope.cleanSearchResult = function () {
        for (var i = 0; i < $scope.searchResult.length; i++) {
            if ($scope.searchResult[i] != null) {
                map.removeLayer($scope.searchResult[i]);
            }
        }
        $scope.searchResult = [];
    }
});

app.factory('GetPosition', function () {
    var GetPosition = new Object();

    GetPosition.getPosition = function (mouseEvent) {
        var result = {
            x: 0,
            y: 0
        };

        if (!mouseEvent) {
            mouseEvent = window.event;
        }

        if (mouseEvent.pageX || mouseEvent.pageY) {
            result.x = mouseEvent.pageX;
            result.y = mouseEvent.pageY;
        }
        else if (mouseEvent.clientX || mouseEvent.clientY) {
            result.x = mouseEvent.clientX + document.body.scrollLeft +
              document.documentElement.scrollLeft;
            result.y = mouseEvent.clientY + document.body.scrollTop +
              document.documentElement.scrollTop;
        }

        if (mouseEvent.target) {
            var offEl = mouseEvent.target;
            var offX = 0;
            var offY = 0;

            if (typeof (offEl.offsetParent) != "undefined") {
                while (offEl) {
                    offX += offEl.offsetLeft;
                    offY += offEl.offsetTop;

                    offEl = offEl.offsetParent;
                }
            }
            else {
                offX = offEl.x;
                offY = offEl.y;
            }
            //result.x -= offX;
            //result.y -= offY;
        }
        return result;
    }

    return GetPosition;
});

app.factory('Search', function ($http, $q) {
    var Search = new Object();
    Search.search = function (fromLat, fromLng, toLat, toLng) {        
        var seachdata = $q.defer();
        $http.get(baseUrl + "Home/Search?fromLat=" + fromLat + "&fromLng=" + fromLng + "&toLat=" + toLat + "&toLng=" + toLng)
        .then(function (response) {
            seachdata.resolve(response.data);
        });
        return seachdata.promise
    }
    return Search;
});

app.factory('SuggestStation', function ($http, $q) {
    var SuggestStation = new Object();
    SuggestStation.getstations = function (lat, lon) {
        var stationdata = $q.defer();
        $http.get(baseUrl + "Home/SuggestStation?lat=" + lat + "&lng=" + lon)
        .then(function (response) {
            stationdata.resolve(response.data);           
        });
        return stationdata.promise
    }
    return SuggestStation;
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

app.service('lazyLoadApi', function lazyLoadApi($window, $q) {
    function loadScript() {
        console.log('loadScript')
        // use global document since Angular's $document is weak
        var s = document.createElement('script')
        s.src = '//maps.googleapis.com/maps/api/js?key=AIzaSyDRZrUxQikxZwfkMmwy9AqnkxvltaFZYss&libraries=places&callback=initAutocomplete'
        document.body.appendChild(s)
    }
    var deferred = $q.defer()

    $window.initAutocomplete = function () {
        deferred.resolve();
        autocomplete = new google.maps.places.Autocomplete(
        /** @type {!HTMLInputElement} */(document.getElementById('autocomplete')),
        { types: ['geocode'] });

        autocomplete2 = new google.maps.places.Autocomplete(
            /** @type {!HTMLInputElement} */(document.getElementById('autocomplete2')),
            { types: ['geocode'] });

        // When the user selects an address from the dropdown, populate the address
        // fields in the form.
        autocomplete.addListener('place_changed', fillInAddress);
        autocomplete2.addListener('place_changed', fillInAddress2);
    }

    if ($window.attachEvent) {
        $window.attachEvent('onload', loadScript)
    } else {
        $window.addEventListener('load', loadScript, false)
    }

    return deferred.promise
});

// google
var placeSearch, autocomplete, autocomplete2;
var componentForm = {
    street_number: 'short_name',
    route: 'long_name',
    locality: 'long_name',
    administrative_area_level_1: 'short_name',
    country: 'long_name',
    postal_code: 'short_name'
};

function fillInAddress() {
    // Get the place details from the autocomplete object.
    var place = autocomplete.getPlace();
    var fromlat = place.geometry.location.lat();
    var fromlng = place.geometry.location.lng();
    var fromname = place.formatted_address;
    var scope = angular.element($("#searchform")).scope();
    scope.$apply(function () {        
        scope.getSuggestStation(fromlat, fromlng, fromname, true);
    });
   
}

function fillInAddress2() {
    // Get the place details from the autocomplete object.
    var place = autocomplete2.getPlace();
    var tolat = place.geometry.location.lat();
    var tolng = place.geometry.location.lng();
    var toname = place.formatted_address;
    var scope = angular.element($("#searchform")).scope(); 
    scope.$apply(function () {       
        scope.getSuggestStation(tolat, tolng, toname, false);
    });   
}

function fromClick(id,lat,lng) {
    var scope = angular.element($("#searchform")).scope();
    scope.$apply(function () {
        scope.fromStationId = id;
        scope.cleanSuggestStation(true, lat, lng);
        var origin = new google.maps.LatLng(scope.fromPoint._latlng.lat, scope.fromPoint._latlng.lng);
        var goal = new google.maps.LatLng(lat, lng);

        var directionsService = new google.maps.DirectionsService;
        directionsService.route({
            origin: origin,
            destination: goal,
            travelMode: google.maps.TravelMode.WALKING
        }, function (response, status) {
            if (status === google.maps.DirectionsStatus.OK) {
                //scope.fromWalk = response.routes[0].overview_path;
                var pointList = [];
                for (var i = 0; i < response.routes[0].overview_path.length; i++) {
                    var pointItemp = new L.LatLng(response.routes[0].overview_path[i].lat(), response.routes[0].overview_path[i].lng());  //L.LatLng
                    pointList.push(pointItemp);
                }
                
                var polylineRoute = new L.Polyline(pointList, {
                    color: 'red',
                    weight: 3,
                    opacity: 0.5,
                    smoothFactor: 1,
                    dashArray: '10,5',
                    lineJoin: 'round'
                });
                polylineRoute.addTo(map);
                scope.fromWalk.push(polylineRoute);
            } else {
                $scope.showNotify('Hệ thống không tìm thấy đường đi bộ!');
            }
        });
    });
}

function toClick(id,lat,lng) {
    var scope = angular.element($("#searchform")).scope();
    scope.$apply(function () {
        scope.toStationId = id;
        scope.cleanSuggestStation(false, lat, lng);
        var origin = new google.maps.LatLng(scope.toPoint._latlng.lat, scope.toPoint._latlng.lng);
        var goal = new google.maps.LatLng(lat, lng);

        var directionsService = new google.maps.DirectionsService;
        directionsService.route({
            origin: origin,
            destination: goal,
            travelMode: google.maps.TravelMode.WALKING
        }, function (response, status) {
            if (status === google.maps.DirectionsStatus.OK) {
                //scope.fromWalk = response.routes[0].overview_path;
                var pointList = [];
                for (var i = 0; i < response.routes[0].overview_path.length; i++) {
                    var pointItemp = new L.LatLng(response.routes[0].overview_path[i].lat(), response.routes[0].overview_path[i].lng());  //L.LatLng
                    pointList.push(pointItemp);
                }

                var polylineRoute = new L.Polyline(pointList, {
                    color: 'red',
                    weight: 3,
                    opacity: 0.5,
                    smoothFactor: 1,
                    dashArray: '20,15',
                    lineJoin: 'round'
                });
                polylineRoute.addTo(map);
                scope.toWalk.push(polylineRoute);
            } else {
                $scope.showNotify('Hệ thống không tìm thấy đường đi bộ!');
            }
        });

    });
}

$(function () {
    $.get(baseUrl + "home/InitGraph", function () {
    });
    var scope = angular.element($("#searchform")).scope();
    scope.$apply(function () {
        scope.drawPath(model);
    });
})