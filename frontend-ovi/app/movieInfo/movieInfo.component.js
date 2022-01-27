'use strict';

angular.module('movieInfo')
    .component('movieInfo', {
        bindings: {
            movies: '='
        },
        transclude: true,
        controller: movieInfoController,
        controllerAs: 'info',
        templateUrl: 'movieInfo/movieInfo.html'
    });
