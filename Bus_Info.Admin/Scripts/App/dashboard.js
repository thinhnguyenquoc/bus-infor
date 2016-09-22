
var app = angular.module("app", ['ngRoute', 'blockUI', 'ngSanitize']);
var defaultTab = '';
app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
      .when('/Home/RouteView', {
          templateUrl: baseUrl+'Home/RouteView',
          controller: 'RouteController'
      })
    .otherwise({
        redirectTo: defaultTab
    });;
}]);

app.controller('RouteController', ['$scope', '$rootScope', '$location', '$routeParams', '$routeParams', 'RouteService', function ($scope, $rootScope, $location, $routeParams, $routeParams, RouteService) {
    $rootScope.ChosenRoute = null;
    $rootScope.repeatSelect = null;
    $rootScope.Routes = [];
    var getRoute = RouteService.getRoutes();
    getRoute.then(function (data) {
        $rootScope.Routes = data;
        if (data.length > 0) {
            $rootScope.repeatSelect = data[0].Code;
        }
    });
    $scope.$watch(
        "repeatSelect", function (newValue) {
            var data = $rootScope.Routes;
            for (var i = 0; i < data.length; i++) {
                if (data[i].Code == newValue) {
                    $rootScope.ChosenRoute = data[i];
                    break;
                }
            }
        }
    );

    $scope.submitRoute = function () {
        RouteService.postRoutes($rootScope.ChosenRoute);
    }
}]);

app.factory('RouteService', function ($http, $q) {
    var Route = new Object();
    Route.getview = function () {
        var seachdata = $q.defer();
        $http.get(baseUrl + "Home/RouteView")
        .then(function (response) {
            seachdata.resolve(response.data);
        });
        return seachdata.promise
    }

    Route.getRoutes = function () {
        var seachdata = $q.defer();
        $http.get(baseUrl + "Home/GetRoute")
        .then(function (response) {
            seachdata.resolve(response.data);
        });
        return seachdata.promise
    }

    Route.postRoutes = function (data) {
        var seachdata = $q.defer();
        $http.post(baseUrl + "Home/PostRoute",data)
        .then(function (response) {
            seachdata.resolve(response.data);
        });
        return seachdata.promise
    }

    return Route;
});

