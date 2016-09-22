
var app = angular.module("app", ['ngRoute', 'blockUI', 'ngSanitize']);
var defaultTab = '';
app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
      .when('/Member/RouteView', {
          templateUrl: baseUrl+'Member/RouteView',
          controller: 'RouteController'
      })
    .when('/Member/Logout', {
        templateUrl: baseUrl + 'Member/Logout',
        controller: 'UserController'
    })
    .when('/Member/AddNews',{
        templateUrl: baseUrl + 'Member/AddNews',
        controller: 'NewsController'
    })
    .when('/Member/EditNews', {
        templateUrl: baseUrl + 'Member/EditNews',
        controller: 'NewsController'
    })
    .otherwise({
        redirectTo: defaultTab
    });;
}]);

app.controller('NewsController', ['$scope', '$rootScope', '$location', '$routeParams', '$routeParams', 'fileUpload', 'NewsService', function ($scope, $rootScope, $location, $routeParams, $routeParams, fileUpload, NewsService) {
    $scope.Id = null;
    $scope.Title = null;
    $scope.Description = null;
    $scope.Body = null;
    $scope.TopImage = null;
    $scope.TopDetailImage = null;
    $scope.Publish = null;
    $scope.Tag = null;
    $scope.Genre = null;
    $scope.New = null;

    $scope.searchNews = function () {
        var news = NewsService.SearchNews($scope.Id);
        news.then(function (data) {
            $scope.Id = data.Id;
            $scope.Title = data.Title;
            $scope.Description = data.Description;
            $scope.Body = data.Body;
            $scope.TopImage = data.TopImage;
            $scope.TopDetailImage = data.TopDetailImage;
            $scope.Publish = data.Publish;
            $scope.Tag = data.Tag;
            $scope.Genre = data.Genre;
            $scope.New = data.New;
            tinyMCE.get('editArea').setContent(data.Description);
            tinyMCE.get('editArea2').setContent(data.Body);
            //tinyMCE.activeEditor.setContent(data.Description);
            //tinyMCE.activeEditor.setContent(data.Body);
        });
    }

    $scope.submitNews = function () {
        $scope.Description = tinyMCE.get('editArea').getContent();
        $scope.Body = tinyMCE.get('editArea2').getContent();
        var data = new Object();
        data.Title = $scope.Title;
        data.Description = $scope.Description;
        data.Body = $scope.Body;
        data.TopImage = $scope.TopImage;
        data.TopDetailImage = $scope.TopDetailImage;
        data.Publish = $scope.Publish;
        data.Tag = $scope.Tag;
        data.Genre = $scope.Genre;
        data.New = $scope.New;
        data.Id = $scope.Id;
        NewsService.Add(data);
    }
    $scope.uploadFile = function () {
        var file = $scope.myFile;

        var uploadUrl = baseUrl + "Member/FileUpload";
        var result = fileUpload.uploadFileToUrl(file, uploadUrl);
    };
}]);

app.controller('UserController', ['$scope', '$rootScope', '$location', '$routeParams', '$routeParams', 'RouteService', function ($scope, $rootScope, $location, $routeParams, $routeParams, RouteService) { 
    RouteService.logout();
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

    Route.logout = function () {
        var seachdata = $q.defer();
        $http.get(baseUrl + "Member/Logout")
        .then(function (response) {
            window.location.href = baseUrl + "Member/Index";
        });
    }

    Route.getview = function () {
        var seachdata = $q.defer();
        $http.get(baseUrl + "Member/RouteView")
        .then(function (response) {
            seachdata.resolve(response.data);
        });
        return seachdata.promise
    }

    Route.getRoutes = function () {
        var seachdata = $q.defer();
        $http.get(baseUrl + "Member/GetRoute")
        .then(function (response) {
            seachdata.resolve(response.data);
        });
        return seachdata.promise
    }

    Route.postRoutes = function (data) {
        var seachdata = $q.defer();
        $http.post(baseUrl + "Member/PostRoute",data)
        .then(function (response) {
            seachdata.resolve(response.data);
        });
        return seachdata.promise
    }

    return Route;
});

app.service('fileUpload', ['$http', function ($http) {
    this.uploadFileToUrl = function (file, uploadUrl) {
        var fd = new FormData();
        fd.append('file', file);

        $http.post(uploadUrl, fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })

        .success(function (data) {
            if (data == true) {
                $('#uploadfileurl').text( baseUrl + 'NewsImages/' + file.name);
            }
        })

        .error(function () {
        });
    }
}]);

app.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;

            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}]);

app.service('NewsService', ['$http','$q', function ($http,$q) {
    var News = new Object();
    News.Add = function (data) {
        $http.post(baseUrl + "Member/SubmitNews",data)
        .then(function (response) {
            alert("successful");
        });
    }
    
    News.SearchNews = function (id) {
        var seachdata = $q.defer();
        $http.get(baseUrl + "Member/SearchNews?Id=" + id)
        .then(function (response) {
            seachdata.resolve(response.data);
        });
        return seachdata.promise
    }
    return News;

}]);

