'use strict';

angular.module('findTitles')
    .component('findTitles', {
        transclude: true,
        controller:
        [
            '$routeParams', 
            '$route',
            '$location',
            '$timeout', 
            'manipulateData', 
            'getRequestTitlesFromDataset', 
            'getRequestTitlesFromAnotherSource',
            findTitlesController
        ],
        controllerAs: 'titlesCtrl',
        templateUrl: 'findTitles/findTitles.html'
    });
