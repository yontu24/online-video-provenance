'use strict';

angular.module('ovi')
    .factory('getRequestTitle', ($http) => {
        function get(title) {
            return $http.get('_responses/response_1643045839773-titles.json');
            // return $http.get('http://localhost:58397/getTitles/' + title);
        }
        return {
            get: get
        }
    }).factory('manipulateData', () => {
        function getMovieInfo(data) {
            return JSON.parse(data);
        }
        return {
            getMovieInfo: getMovieInfo
        }
    });
