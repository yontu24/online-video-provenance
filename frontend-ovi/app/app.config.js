angular.module('ovi').
    config(
    [
        '$routeProvider', 
        function config($routeProvider) {
            $routeProvider.
                when('/search', {
                    template: '<search-movie-form class="center" ng-transclude></search-movie-form>'
                }).
                when('/findTitle/:titleId', {
                    template: '<find-titles></find-titles>'
                }).
                when('/movieInfo/:title', {
                    template: '<movie-info></movie-info>'
                }).
                // te trimite pe uri (prop = uri)
                when('/wade-ovi.org/:property', {
                    template: '<show-triples></show-triples>'
                }).
                otherwise('/search');
        }
    ]
);
