'use strict';

angular.module('findTitles')
    .factory('getRequestTitlesFromAnotherSource', ($http) => {
        function get(title) {
            return $http.get('http://localhost:5000/dbpedia/movies/' + title);
        }
        return {
            get: get
        }
    }).factory('getRequestTitlesFromDataset', ($http) => {
        function get(title) {
            return $http.get('http://localhost:5000/movies/titles/' + title);
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
