'use strict';

angular.module('showPersons')
    .component('showPersons', {
        transclude: true,
        controller:
        [
            '$routeParams', 
            '$location',
            '$timeout',
            'getRequestPerson',
            showPersonsController
        ],
        controllerAs: 'personsCtrl',
        templateUrl: 'showPersons/showPersons.html'
    });
