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
            'concatenateUris',
            movieInfoController
        ],
        controllerAs: 'info',
        templateUrl: 'movieInfo/movieInfo.html'
    });
