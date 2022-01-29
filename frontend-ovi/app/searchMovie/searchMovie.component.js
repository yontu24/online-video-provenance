'use strict';

angular.module('searchMovie')
    .component('searchMovieForm', {
        transclude: true,
        controller: searchMovieFormController,
        controllerAs: 'searchmovie',
        templateUrl: 'searchMovie/searchMovie.html'
    });
