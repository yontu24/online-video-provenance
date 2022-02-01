'use strict';

angular.module('showTriples')
    .component('showTriples', {
        transclude: true,
        controller:
        [
            '$routeParams', 
            '$location',
            'getRequestTriples',
            'getRequestPerson',
            showTriplesController
        ],
        controllerAs: 'triplesCtrl',
        templateUrl: 'showTriples/showTriples.html'
    });
