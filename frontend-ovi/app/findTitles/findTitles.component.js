'use strict';

angular.module('findTitles')
    .component('findTitles', {
        transclude: true,
        controller:
        [
            '$routeParams', 
            '$location',
            '$interval', 
            'manipulateData', 
            'getRequestTitlesFromDataset', 
            'getRequestTitlesFromAnotherSource',
            findTitlesController
        ],
        controllerAs: 'titlesCtrl',
        templateUrl: 'findTitles/findTitles.html'
    });
