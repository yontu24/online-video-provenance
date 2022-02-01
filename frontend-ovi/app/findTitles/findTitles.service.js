'use strict';

angular.module('findTitles')
    .factory('getRequestTitlesFromAnotherSource', ($http) => {
        function get(title) {
            var url = 'http://localhost:5000/dbpedia/movies/' + encodeURIComponent(title);
            console.log('DBP = ' + url);
            return $http.get(url);
        }
        return {
            get: get
        }
    }).factory('getRequestTitlesFromDataset', ($http) => {
        function get(title) {
            var url = 'http://localhost:5000/movies/titles/' + encodeURIComponent(title);
            console.log('DS = ' + url);
            return $http.get(url);
        }
        return {
            get: get
        }
    }).factory('manipulateData', () => {
        function getTitles(data) {
            return JSON.parse(data);
        }
        return {
            getTitles: getTitles
        }
    });
