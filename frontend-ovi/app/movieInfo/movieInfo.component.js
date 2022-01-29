'use strict';

angular.module('movieInfo')
    .component('movieInfo', {
        transclude: true,
        controller:
        [
            '$routeParams',
            '$location',
            'getRequestMovieInfo', 
            'processMovieInfo',
            movieInfoController
        ],
        controllerAs: 'info',
        templateUrl: 'movieInfo/movieInfo.html'
    });
