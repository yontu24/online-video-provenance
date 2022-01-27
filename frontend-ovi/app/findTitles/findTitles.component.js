'use strict';

angular.module('findTitles')
    .component('findTitles', {
        bindings: {
            titles: '='
        },
        transclude: true,
        controller: findTitlesController,
        controllerAs: 'titlesCtrl',
        templateUrl: 'findTitles/findTitles.html'
    });
