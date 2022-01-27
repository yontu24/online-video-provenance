'use strict';

angular.module('ovi')
    .component('searchMovieForm', {
        transclude: true,
        controller: searchMovieFormController,
        controllerAs: 'searchmovie',
        templateUrl: 'searchMovie/searchMovie.html'
    });
