'use strict';

angular.module('titleRecommendation')
    .component('titleRecommendation', {
        bindings: {
            displayTitles: '=',
            movieUri: '<'
        },
        transclude: true,
        replace: true,
        controller:
        [
            'getRequestRecommendedTitles',
            'concatenateUris',
            recommendationController
        ],
        controllerAs: 'recomm',
        templateUrl: 'recommendation/recommendation.html'
    });
